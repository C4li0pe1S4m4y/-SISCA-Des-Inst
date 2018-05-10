using MySql.Data.MySqlClient;
using System.Data;

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
            string query = "SELECT se.id_empleado id,COALESCE(CONCAT(se.id_empleado, ' - ', se.nombre, ' '), 'S/D') texto "+
                "FROM dbcdagsgc2.sgc_empleados se ORDER BY se.id_empleado;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }
    }
}
