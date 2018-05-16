using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Controladores
{
    
    public class cUsuarios
    {
        DBConexion conectar;
        public bool login(string usuario, string pass)
        {
            conectar = new DBConexion();

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


        public bool IngresoNuevoUsuario(string usuario, string contra, string id_empleado, string tipo_usuario, string correo)
        {
            try
            {
                conectar = new DBConexion();

                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_usuario (usuario, contrasena,  Habilitado, id_empleado, id_tipo_usuario)" +
                    "VALUES ('{0}', AES_ENCRYPT('{1}', 'SCOGA'), 1, '{2}', '{3}'); ", usuario, contra, id_empleado, tipo_usuario);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();

                /////////////////////////////////////////////////////

                MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
                MySqlCommand command = conectar.conectar.CreateCommand();
                command.Transaction = transaccion;
                try
                {
                    command.CommandText = string.Format("UPDATE sgc_empleados SET email = '{1}' WHERE id_empleado = '{0}'; ",
                    id_empleado, correo);
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
            conectar = new DBConexion();
            string query = string.Format(" select * from sgc_usuario u "+
                "inner join sgc_empleados e on e.id_empleado = u.id_empleado "+
                "where usuario = '{0}'; "
            , usuario);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objUsuario.Usuario = dr.GetString("usuario");
                objUsuario.idUsuario = int.Parse(dr.GetString("idusuario"));
                objUsuario.id_empleado = int.Parse(dr.GetString("id_empleado"));
                objUsuario.id_tipo_usuario = int.Parse(dr.GetString("id_tipo_usuario"));
                objUsuario.nombre = dr.GetString("Nombre");
                //objUsuario.correo = dr.GetString("email");
            }
            return objUsuario;
        }

        public mUsuario Obtner_UsuarioID(int id)
        {
            mUsuario objUsuario = new mUsuario();
            conectar = new DBConexion();
            string query = string.Format("select * from sgc_usuario u " +
                "inner join sgc_empleados e on e.id_empleado = u.id_empleado " +
                "where u.idusuario = '{0}'; "
            , id);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objUsuario.Usuario = dr.GetString("usuario");
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
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT id_tipo_usuario id, nombre " +
                "FROM sgc_tipo_usuario;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }
    }
}
