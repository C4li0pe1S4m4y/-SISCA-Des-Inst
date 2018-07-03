using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Controladores
{
    public class cDashboard
    {
        DBConexion conectar = new DBConexion();

        public DataTable GraficaAcciones()
        {
            DataTable result = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("SELECT ta.abreviacion AS 'Tipo', COUNT(ag.id_tipo_accion) AS 'Total' " +
                "FROM sgc_accion_generada ag LEFT JOIN sgc_tipo_accion ta ON ag.id_tipo_accion = ta.id_tipo_accion "+
                "WHERE ag.id_status != 0 "+
                "GROUP BY ag.id_tipo_accion;");
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataTable graficaPlanesAccion()
        {
            DataTable result = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("SELECT u.unidad Unidad, IFNULL(a.abiertas,0) Abierta, IFNULL(c.cerradas,0) Cerrada " +
                "FROM sgc_unidad u " +
                
                "LEFT JOIN(SELECT u.id_unidad, pa.id_status, COUNT(pa.id_status) abiertas " +
                    "FROM sgc_plan_accion pa INNER JOIN sgc_accion_generada ag ON pa.id_accion_generada = ag.id_accion_generada " +
                        "INNER JOIN sgc_unidad u ON u.id_unidad = ag.id_unidad " +
                    "WHERE pa.id_status != 5 " +
                    "GROUP BY u.unidad) a ON u.id_unidad = a.id_unidad " +
                
                "LEFT JOIN(SELECT u.id_unidad, pa.id_status, COUNT(pa.id_status) cerradas " +
                    "FROM sgc_plan_accion pa INNER JOIN sgc_accion_generada ag ON pa.id_accion_generada = ag.id_accion_generada " +
                        "INNER JOIN sgc_unidad u ON u.id_unidad = ag.id_unidad " +
                    "WHERE pa.id_status = 5 " +
                    "GROUP BY u.unidad) c ON u.id_unidad = c.id_unidad " +

                "WHERE a.abiertas > 0 OR c.cerradas > 0; ");
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataTable graficaPlanesAccionOld()
        {
            //DATOS DE PRUEBA GRAFICA
            DataTable Datos = new DataTable();
            Datos.Columns.Add(new DataColumn("Unidad", typeof(string)));
            Datos.Columns.Add(new DataColumn("Abierta", typeof(string)));
            Datos.Columns.Add(new DataColumn("Cerrada", typeof(string)));
            Datos.Columns.Add(new DataColumn("CerradaTransición", typeof(string)));

            Datos.Rows.Add(new Object[] { "Todas las Unidades", 1, 2, 0 });
            Datos.Rows.Add(new Object[] { "Subgerencia Técnica", 3, 5, 3 });
            Datos.Rows.Add(new Object[] { "Subgerencia de Infraestructura", 4, 20, 14 });
            Datos.Rows.Add(new Object[] { "Subgerencia Financiera", 3, 5, 3 });
            Datos.Rows.Add(new Object[] { "Subgerencia Desarrollo Institucional", 10, 23, 7 });
            Datos.Rows.Add(new Object[] { "Subgerencia Desarrollo Humano", 4, 8, 1 });
            Datos.Rows.Add(new Object[] { "Subgerencia de Gestión Nacional", 1, 1, 0 });
            Datos.Rows.Add(new Object[] { "Subgerenciia Administrativa", 0, 8, 0 });
            Datos.Rows.Add(new Object[] { "Secretaría General", 1, 0, 0 });
            Datos.Rows.Add(new Object[] { "Gestión del MEGD", 2, 0, 1 });
            Datos.Rows.Add(new Object[] { "Ciencias Aplicadas", 1, 4, 1 });
            Datos.Rows.Add(new Object[] { "Atención a FADN", 1, 4, 3 });
            Datos.Rows.Add(new Object[] { "Alta Dirección", 0, 2, 0 });

            return Datos;            
        }

        /*public DataTable graficaPlanesAccionAbiertos()
        {
            //DATOS DE PRUEBA GRAFICA
            DataTable Datos = new DataTable();
            Datos.Columns.Add(new DataColumn("Unidad", typeof(string)));
            Datos.Columns.Add(new DataColumn("FuertaTiempo", typeof(string)));
            Datos.Columns.Add(new DataColumn("EnTiempo", typeof(string)));

            Datos.Rows.Add(new Object[] { "Todas las Unidades", 0, 1 });
            Datos.Rows.Add(new Object[] { "Subgerencia Técnica", 2, 1 });
            Datos.Rows.Add(new Object[] { "Subgerencia de Infraestructura", 0, 4 });
            Datos.Rows.Add(new Object[] { "Subgerencia Financiera", 1, 2 });
            Datos.Rows.Add(new Object[] { "Subgerencia Desarrollo Institucional", 2, 7 });
            Datos.Rows.Add(new Object[] { "Subgerencia Desarrollo Humano", 4, 0 });
            Datos.Rows.Add(new Object[] { "Subgerencia de Gestión Nacional", 0, 1 });
            Datos.Rows.Add(new Object[] { "Secretaría General", 0, 1 });
            Datos.Rows.Add(new Object[] { "Ciencias Aplicadas", 0, 1 });
            Datos.Rows.Add(new Object[] { "Atención a FADN", 0, 1 });

            return Datos;
        }*/

        public DataTable graficaPlanesAccionAbiertos()
        {
            DataTable result = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("SELECT u.unidad Unidad, "+

                "IF((SELECT MIN(ar.fecha_fin) "+
                    "FROM sgc_accion_realizar ar "+
                    "WHERE ar.id_plan = pa.id_plan) < now(), 1, 0) "+
                "AS FueraTiempo, "+

                "IF((SELECT MIN(ar.fecha_fin) "+
                    "FROM sgc_accion_realizar ar "+
                    "WHERE ar.id_plan = pa.id_plan) < now(), 0, 1) "+
                "AS EnTiempo "+

                "FROM sgc_plan_accion pa "+
                    "INNER JOIN sgc_accion_generada ag ON ag.id_accion_generada = pa.id_accion_generada "+
                    "INNER JOIN sgc_unidad u ON ag.id_unidad = u.id_unidad; ");
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataTable graficaAccionesInformes()
        {
            //DATOS DE PUREBA GRAFICA
            DataTable Datos = new DataTable();
            Datos.Columns.Add(new DataColumn("Fecha", typeof(string)));
            Datos.Columns.Add(new DataColumn("Acciones", typeof(Int16)));
            Datos.Columns.Add(new DataColumn("Informes", typeof(Int16)));

            Datos.Rows.Add(new Object[] { "2017-01", 15, 20 });
            Datos.Rows.Add(new Object[] { "2017-02", 30, 65 });
            Datos.Rows.Add(new Object[] { "2017-03", 25, 57 });
            Datos.Rows.Add(new Object[] { "2017-04", 45, 18 });
            Datos.Rows.Add(new Object[] { "2017-05", 50, 75 });
            Datos.Rows.Add(new Object[] { "2017-06", 35, 43 });
            Datos.Rows.Add(new Object[] { "2017-07", 90, 55 });
            Datos.Rows.Add(new Object[] { "2017-08", 12, 29 });
            Datos.Rows.Add(new Object[] { "2017-01", 30, 50 });
            Datos.Rows.Add(new Object[] { "2017-09", 37, 72 });
            Datos.Rows.Add(new Object[] { "2017-10", 25, 75 });
            Datos.Rows.Add(new Object[] { "2017-11", 40, 37 });
            Datos.Rows.Add(new Object[] { "2017-12", 54, 19 });

            DataTable nDatos = Datos.AsEnumerable()
              .GroupBy(r => r.Field<string>("Fecha"))
              .Select(g =>
              {
                  var row = Datos.NewRow();
                  row["Fecha"] = g.Key;
                  row["Acciones"] = g.Sum(r => r.Field<Int16>("Acciones"));
                  row["Informes"] = g.Sum(r => r.Field<Int16>("Informes"));
                  return row;
              }).CopyToDataTable();

            return nDatos;
        }
    }
}
