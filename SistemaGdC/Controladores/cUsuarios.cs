using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    
    public class cUsuarios
    {
        DBConexion conectar;
        public bool login(string usuario, string pass)
        {
            conectar = new DBConexion();

            conectar.AbrirConexion();
            string query = string.Format("select a.usuario from (select usuario, CAST(AES_DECRYPT(Contrasena, 'SCOGA') AS CHAR(50)) as Contrasena from dbcdagsgc.sgc_usuario where habilitado = 1) as a where a.usuario = '{0}' and a.Contrasena = '{1}'; ",
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


        public bool IngresoNuevoUsuario(string usuario,string contra,string id_empleado)
        {
            try
            {
                conectar = new DBConexion();

                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO dbcdagsgc.sgc_usuario (usuario, contrasena,  Habilitado, id_empleado)" +
                    "VALUES ('{0}', AES_ENCRYPT('{1}', 'SCOGA'), 1, {2}); ", usuario, contra, id_empleado);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();
                conectar.CerrarConexion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
              
            }
            

        }
    }
}
