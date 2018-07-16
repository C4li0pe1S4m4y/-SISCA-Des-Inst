using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Controladores
{
    public class cEmpleado
    {
        DBConexion conectar = new DBConexion();
        mEmpleado mEmpleado = new mEmpleado();

        public mEmpleado Obtner_Empleado(int id, string tipo_usuario)
        {
            string consulta = "";
            switch(tipo_usuario)
            {
                case "enlace":
                case "lider":
                case "analista":
                    consulta = "e.id_empleado = " + id.ToString();
                    break;

                case "administrador":
                    consulta = "u.habilitado = 1 AND u.id_tipo_usuario = 2";
                    break;

                case "director":
                    consulta = "u.habilitado = 1 AND u.id_tipo_usuario = 1";
                    break;
            }
            //conectar = new DBConexion();
            string query = string.Format("SELECT e.id_empleado, e.nombre, e.email " +
                "FROM sgc_empleados e INNER JOIN sgc_usuario u ON e.id_empleado = u.id_empleado " +
                "WHERE {0};"                
            , consulta);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                mEmpleado.id_empleado = int.Parse(dr.GetString("id_empleado"));
                mEmpleado.Nombre = dr.GetString("nombre");

                if (!dr.IsDBNull(dr.GetOrdinal("email")))
                    mEmpleado.email = dr.GetString("email");
            }
            return mEmpleado;
        }

        public void dllEmpleado(DropDownList ddl, int unidad) //OK
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("select id_empleado,nombre from sgc_empleados where id_unidad = {0};", unidad);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Empleado >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "nombre";
            ddl.DataValueField = "id_empleado";
            ddl.DataBind();
        }

        public string ObtenerAnalistaUnidad(int idUnidad)
        {
            string analista = "";
            //conectar = new DBConexion();
            string query = string.Format("SELECT e.nombre FROM sgc_unidad u "+
                    "INNER JOIN sgc_empleados e ON u.id_analista = e.id_empleado "+
                "WHERE u.id_unidad = '{0}'; "
            , idUnidad);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                analista = dr.GetString("nombre");

            return analista;
        }
    }
}
