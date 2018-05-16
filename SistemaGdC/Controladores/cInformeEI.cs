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
    public class cInformeEI
    {
        DBConexion conectar = new DBConexion();
        public DataTable dropAcciones()
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select * from sgc_ccl_accion_generada;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public void dropProceso(DropDownList ddl)
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT * FROM sgc_proceso;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Proceso >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "Proceso";
            ddl.DataValueField = "id_proceso";
            ddl.DataBind();

        }

        public void dropOpcionIngreso(DropDownList ddl)
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT * FROM sgc_tipo_solicitud;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Proceso >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "descripcion";
            ddl.DataValueField = "id_tipo";
            ddl.DataBind();
        }

        public void dropUnidad(DropDownList ddl)
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select id_unidad,Unidad from sgc_unidad where id_unidad = id_padre;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Unidad >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "Unidad";
            ddl.DataValueField = "id_unidad";
            ddl.DataBind();
        }

        public void dllDependencia(DropDownList ddl, int unidad)
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("select id_unidad,Unidad from sgc_unidad where id_padre = {0};", unidad);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Dependencia >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "Unidad";
            ddl.DataValueField = "id_unidad";
            ddl.DataBind();
        }

        public void dropTipoAccion(DropDownList ddl)
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select * from sgc_tipo_accion;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Tipo Accion >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "accion";
            ddl.DataValueField = "id_tipo_accion";
            ddl.DataBind();
        }


        public bool IngresarInforme(mAccionesGeneradas accion)
        {
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_accion_generada (" +
                "correlativo_hallazgo, norma, descripcion, fecha, id_status, " +
                "anio_informe_ei, no_informe_ei, id_analista, id_enlace, id_unidad, " +
                "id_dependencia, id_ccl_accion_generada, id_proceso, id_tipo_accion) " +

                "VALUES({0},'{1}','{2}','{3}',0,{4},{5},{6},{7},{8},{9},{10},{11},{12});",

                accion.correlativo_hallazgo, accion.norma, accion.descripcion, accion.fecha,
                accion.anio_informe_ei, accion.no_informe_ei, accion.id_analista, accion.id_enlace, accion.id_unidad,
                accion.id_dependencia, accion.id_ccl_accion_generada, accion.id_proceso, accion.id_tipo_accion);

                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

                cmd.ExecuteNonQuery();
                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public int AlmacenarEncabezado(mInformeEI mInforme)
        {
            int result = 0;
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("SET @id = (SELECT MAX(id_informe_ei)+1 FROM sgc_informe_ei); " +
                    "INSERT INTO sgc_informe_ei(id_informe_ei, anio, no_informe, fecha, id_status) " +
                    "VALUES(IF(@id, @id, 1), {0}, {1}, '{2}', 0); ",
                    mInforme.anio, mInforme.no_informe, mInforme.fecha);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();
                query = "Select * from sgc_informe_ei order by id_informe_ei DESC LIMIT 1";
                cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = int.Parse(reader[0].ToString());
                }
                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return -10;
            }
            return result;
        }

        public mInformeEI BuscarEncabezado(string noInforme, int anio)
        {
            mInformeEI informe = new mInformeEI();
            try
            {
                DataSet tabla = new DataSet();
                conectar.AbrirConexion();
                string query = string.Format("Select id_informe_ei,Date_format(fecha,'%Y-%m-%d') fecha, id_status from sgc_informe_ei where no_informe ='{0}' AND anio={1};", noInforme, anio);
                MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
                consulta.Fill(tabla);
                conectar.CerrarConexion();
                if (tabla.Tables[0].Rows.Count > 0)
                {
                    informe.id_informe_ei = int.Parse(tabla.Tables[0].Rows[0]["id_informe_ei"].ToString());
                    informe.anio = anio;
                    informe.no_informe = noInforme;
                    informe.fecha = (tabla.Tables[0].Rows[0]["fecha"].ToString());
                    informe.id_status = int.Parse(tabla.Tables[0].Rows[0]["id_status"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return informe;
        }

        public DataSet ListadoAcciones(int anio, int id, int status)
        {
            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("select ag.id_accion_generada as 'id',ca.Accion as 'Acción',ag.correlativo_hallazgo as 'Correlativo',ag.norma as 'Punto de Norma',  sag.nombre as 'Status', " +
                "p.Proceso,u.Unidad,d.Unidad Dependencia,ag.descripcion as 'Descripción', ee.Nombre Enlace, " +
                "ea.Nombre Analista, Date_format(ag.fecha,'%d/%m/%Y') as 'Fecha', ta.accion as 'Tipo Acción' " +

                        "from sgc_accion_generada ag inner join sgc_ccl_accion_generada ca on ca.id_acciones = ag.id_ccl_accion_generada " +
                        "inner join sgc_proceso p on p.id_proceso = ag.id_proceso " +
                        "inner join sgc_unidad u on u.id_unidad = ag.id_unidad " +
                        "inner join sgc_unidad d on d.id_unidad = ag.id_dependencia  " +
                        "inner join sgc_empleados ea on ea.id_empleado = ag.id_analista " +
                        "inner join sgc_empleados ee on ee.id_empleado = ag.id_enlace " +
                        "inner join sgc_tipo_accion ta on ta.id_tipo_accion = ag.id_tipo_accion " +
                        "left join sgc_status_accion_generada sag on sag.id_status = ag.id_status " +

                        "where ag.anio_informe_ei = '{0}' AND ag.no_informe_ei = '{1}' AND ag.id_status = '{2}'; ", anio, id, status);

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataSet ListadoInformes(int status)
        {
            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("SELECT * FROM sgc_informe_ei iei LEFT JOIN sgc_status_informe_ei siei ON siei.id_status = iei.id_status  WHERE iei.id_status = '{0}';", status);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public mAccionesGeneradas Obtner_AccionGenerada(int id)
        {
            mAccionesGeneradas objAccionGenerada = new mAccionesGeneradas();
            conectar = new DBConexion();
            string query = string.Format(" select * from sgc_accion_generada where id_accion_generada = {0}; "
            , id);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objAccionGenerada.id_accion_generada = int.Parse(dr.GetString("id_accion_generada"));
                objAccionGenerada.anio_informe_ei = int.Parse(dr.GetString("anio_informe_ei"));
                objAccionGenerada.no_informe_ei = int.Parse(dr.GetString("no_informe_ei"));
                objAccionGenerada.id_ccl_accion_generada = int.Parse(dr.GetString("id_ccl_accion_generada"));
                objAccionGenerada.correlativo_hallazgo = int.Parse(dr.GetString("correlativo_hallazgo"));
                objAccionGenerada.norma = dr.GetString("norma");
                objAccionGenerada.id_proceso = int.Parse(dr.GetString("id_proceso"));
                objAccionGenerada.id_unidad = int.Parse(dr.GetString("id_unidad"));
                objAccionGenerada.id_dependencia = int.Parse(dr.GetString("id_dependencia"));
                objAccionGenerada.descripcion = dr.GetString("descripcion");
                objAccionGenerada.id_analista = int.Parse(dr.GetString("id_analista"));
                objAccionGenerada.id_enlace = int.Parse(dr.GetString("id_enlace"));
                DateTime fecha = DateTime.Parse(dr.GetString("fecha"));
                objAccionGenerada.fecha = fecha.ToString("yyyy-MM-dd");
                objAccionGenerada.id_tipo_accion = int.Parse(dr.GetString("id_tipo_accion"));
                objAccionGenerada.id_status = int.Parse(dr.GetString("id_status"));
            }
            return objAccionGenerada;
        }

        public void actualizarInforme(int anio, int noInforme, int status)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_informe_ei SET id_status = '{2}' WHERE anio = '{0}' AND no_informe = '{1}'; ",
                anio, noInforme, status);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
                //return true;
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
                //return false;
            };
        }

        public void EliminarAccion(int id)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("DELETE FROM sgc_accion_generada WHERE id_accion_generada = {0};",
                id);
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
            };
        }
    }
}
