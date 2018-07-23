using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace Controladores
{
    public class cGeneral
    {
        DBConexion conectar;
        public DataTable dropEmpleados()
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT se.id_empleado id,COALESCE(CONCAT(se.nombre), 'S/D') texto "+
                "FROM dbcdagsgc2.sgc_empleados se ORDER BY se.nombre;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public int rangoFechas(string fecha_inicio, string fecha_final, bool fin_semana)
        {
            string consulta = "";
            if (fin_semana)
                consulta = "SET @FromDate	= '{0}'; " +
                "SET @ToDate	= '{1}'; " +
                "SET @Domingo = 1; " +
                "SET @Sabado = 7; " +
                "SET @Minimo = DAYOFWEEK(@FromDate); " +
                "SET @Maximo = IF(DAYOFWEEK(@ToDate) = 7, 1, 0); " +

                "SELECT datediff(@ToDate, @FromDate) +1 - " +
                    "(datediff(@ToDate, @FromDate) DIV 7 " +
                    "+ (CASE WHEN @Minimo = @Domingo THEN 1 ELSE 0 END)) - " +
                    "(datediff(@ToDate, @FromDate) DIV 7 " +
                    "+ (CASE WHEN @Minimo = @Sabado THEN 1 ELSE 0 END)) - @Maximo " +
                "AS 'dias';";
            else
                consulta = "SELECT datediff('{1}', '{0}') AS 'dias';";
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format(consulta, fecha_inicio, fecha_final);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    resultado = int.Parse(reader.GetString("dias"));

                conectar.CerrarConexion();
                return resultado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
