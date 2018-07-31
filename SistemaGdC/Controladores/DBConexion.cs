using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class DBConexion
    {
        private String contenido = "server=localhost; database =dbcdagsgc3;user=root; password =123456; Allow User Variables=True";
        //private String contenido = "server=localhost; database =dbcdagsgc3;user=root; password =1234; Allow User Variables=True";
        
        public MySqlConnection conectar = new MySqlConnection();
        public MySqlDataAdapter adaptador = new MySqlDataAdapter();
        public DataTable tabla = new DataTable();

        public void AbrirConexion()
        {
            string sConn;
            sConn = contenido;
            conectar = new MySqlConnection();
            conectar.ConnectionString = sConn;

            try
            {
                conectar.Open();
                Console.WriteLine("Conexión Exitosa");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Falló en la Conexión");
                throw;
            }
        }

        public void CerrarConexion()
        {
            if (conectar.State == System.Data.ConnectionState.Open)
            {
                conectar.Close();
            }
        }
    }
}
