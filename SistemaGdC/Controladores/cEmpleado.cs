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
    public class cEmpleado
    {
        DBConexion conectar;
        public mEmpleado Obtner_Empleado(int id)
        {
            mEmpleado mEmpleado = new mEmpleado();
            conectar = new DBConexion();
            string query = string.Format(" select * from sgc_empleados " +                
                "where id_empleado = '{0}'; "
            , id);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                mEmpleado.id_empleado = int.Parse(dr.GetString("id_empleado"));
                mEmpleado.Nombre = dr.GetString("Nombre");

                if (!dr.IsDBNull(dr.GetOrdinal("email")))
                    mEmpleado.email = dr.GetString("email");
            }
            return mEmpleado;
        }
    }
}
