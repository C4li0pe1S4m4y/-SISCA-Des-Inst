using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Controladores
{
    
    public class cUsuarios
    {
        DBConexion conectar = new DBConexion();
        mUsuario mUsuario = new mUsuario();
        public bool login(string usuario, string pass)
        {
            conectar.AbrirConexion();
            string query = string.Format("select a.usuario from (select usuario, CAST(AES_DECRYPT(Contrasena, 'SCOGA') AS CHAR(50)) as Contrasena from sgc_usuario where habilitado = 1) as a where a.usuario = '{0}' and a.Contrasena = '{1}'; ",
                usuario, pass);
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (!string.IsNullOrEmpty(dr.GetString("usuario")))
                {
                    conectar.CerrarConexion();
                    return true;
                }
            }

            conectar.CerrarConexion();
            return false;
        }


        public bool IngresoNuevoUsuario(mUsuario mUsuario, string contra)
        {
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_usuario (usuario, contrasena,  Habilitado, id_empleado, id_tipo_usuario)" +
                    "VALUES ('{0}', AES_ENCRYPT('{1}', 'SCOGA'), 1, '{2}', '{3}'); ", mUsuario.usuario, contra, mUsuario.id_empleado, mUsuario.id_tipo_usuario);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();

                /////////////////////////////////////////////////////

                MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
                MySqlCommand command = conectar.conectar.CreateCommand();
                command.Transaction = transaccion;
                try
                {
                    command.CommandText = string.Format("UPDATE sgc_empleados SET email = '{1}' WHERE id_empleado = '{0}'; ",
                    mUsuario.id_empleado, mUsuario.correo);
                    command.ExecuteNonQuery();
                    transaccion.Commit();
                    conectar.CerrarConexion();
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaccion.Rollback();
                    }
                    catch
                    { };
                    
                    conectar.CerrarConexion();
                    return false;
                };

                /////////////////////////////////////////////////////

                conectar.CerrarConexion();
                return true;
            }
            catch (Exception ex)
            {
                return false;              
            }            
        }

        public mUsuario Obtner_Usuario(string usuario)
        {
            mUsuario objUsuario = new mUsuario();
            string query = string.Format(" select * from sgc_usuario u "+
                "inner join sgc_empleados e on e.id_empleado = u.id_empleado "+
                "where usuario = '{0}'; "
            , usuario);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objUsuario.usuario = dr.GetString("usuario");
                objUsuario.idUsuario = int.Parse(dr.GetString("idusuario"));
                objUsuario.id_empleado = int.Parse(dr.GetString("id_empleado"));
                objUsuario.id_tipo_usuario = int.Parse(dr.GetString("id_tipo_usuario"));
                objUsuario.nombre = dr.GetString("Nombre");
                //objUsuario.correo = dr.GetString("email");
            }
            return objUsuario;
        }

        public mUsuario Obtner_UsuarioID(int idUsuario)
        {
            mUsuario objUsuario = new mUsuario();
            string query = string.Format("select * from sgc_usuario u " +
                "inner join sgc_empleados e on e.id_empleado = u.id_empleado " +
                "where u.idusuario = '{0}'; "
            , idUsuario);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objUsuario.usuario = dr.GetString("usuario");
                objUsuario.idUsuario = int.Parse(dr.GetString("idusuario"));
                objUsuario.id_empleado = int.Parse(dr.GetString("id_empleado"));
                objUsuario.id_tipo_usuario = int.Parse(dr.GetString("id_tipo_usuario"));
                objUsuario.nombre = dr.GetString("Nombre");
                objUsuario.correo = dr.GetString("correo");
            }
            return objUsuario;
        }

        public DataTable dropTipoUsuario()
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT id_tipo_usuario id, nombre " +
                "FROM sgc_tipo_usuario;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public DataTable ListadoUsuarios(string tipoUsuario) //crear consulta por tipo de fuente
        {
            switch (tipoUsuario)
            {
                case "todos":
                    tipoUsuario = "";
                    break;

                case "enlaces":
                    tipoUsuario = "WHERE u.id_tipo_usuario = 5";
                    break;
            }

            DataTable result = new DataTable();
            conectar.AbrirConexion();
            string query2 = string.Format("SELECT u.idusuario id, u.usuario 'Usuario', e.nombre 'Nombre', tu.nombre 'Tipo Usuario', e.email 'Correo' " +
                "FROM sgc_usuario u " +
                    "INNER JOIN sgc_empleados e ON e.id_empleado = u.id_empleado " +
                    "INNER JOIN sgc_tipo_usuario tu ON tu.id_tipo_usuario = u.id_tipo_usuario; ");

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataSet ListadoUsuariosDS(string tipoUsuario) //crear consulta por tipo de fuente
        {
            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("SELECT u.idusuario id, u.usuario 'Usuario', e.nombre 'Nombre', tu.nombre 'Tipo Usuario', e.email 'Correo' " +
                "FROM sgc_usuario u " +
                    "INNER JOIN sgc_empleados e ON e.id_empleado = u.id_empleado " +
                    "INNER JOIN sgc_tipo_usuario tu ON tu.id_tipo_usuario = u.id_tipo_usuario; ");

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result,"Reporte");
            conectar.CerrarConexion();
            return result;
        }
    }
}
