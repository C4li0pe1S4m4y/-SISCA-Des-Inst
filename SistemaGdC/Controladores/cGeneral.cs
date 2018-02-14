using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                "FROM dbcdagsgc.sgc_empleados se WHERE estado = 1  ORDER BY se.nombre;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }
    }
}
