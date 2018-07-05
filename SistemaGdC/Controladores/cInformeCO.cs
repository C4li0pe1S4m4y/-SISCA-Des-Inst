using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Modelos;

namespace Controladores
{
    public class cInformeCO
    {
        DBConexion conectar = new DBConexion();
        public void ddlInformeResultados(DropDownList ddl)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select id_correlativo,no_informe from sgc_informe_resultados;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Informe >>");
            ddl.Items[0].Value = "0";

            ddl.DataSource = tabla;
            ddl.DataTextField = "no_informe";
            ddl.DataValueField = "id_correlativo";
            ddl.DataBind();
        }

        public void ddlHallazgo(DropDownList ddl,string unidad,string dependencia,string informe)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("select id_accion_generada,no_correlativo_hallazgo from sgc_acciones_generadas where id_informe = {0} and id_unidad = {1} and id_dependencia = {2};",
                informe,unidad,dependencia);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Informe >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "no_correlativo_hallazgo";
            ddl.DataValueField = "id_accion_generada";
            ddl.DataBind();
        }

        public DataSet InformacionInformeResultados(string id_accion)
        {
            conectar = new DBConexion();
            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query = string.Format("select id_tipo_accion,descripcion from sgc_acciones_generadas where id_accion_generada={0};", id_accion);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public void ddlEstadoInforme(DropDownList ddl)
        {
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Estado >>");
            ddl.Items[0].Value = "0";
            ddl.Items.Add("Atendido");
            ddl.Items[1].Value = "1";
            ddl.Items.Add("No Atendido");
            ddl.Items[2].Value = "2";
            ddl.Items.Add("Se Atenderá");
            ddl.Items[3].Value = "3";
            ddl.DataBind();
        }
        public void ddlTecnicaAnalisis(DropDownList ddl)
        {
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Tecnica >>");
            ddl.Items[0].Value = "0";
            ddl.Items.Add("No aplica");
            ddl.Items[1].Value = "No aplica";
            ddl.Items.Add("5 porque");
            ddl.Items[2].Value = "5 porque";
            ddl.Items.Add("Pareto");
            ddl.Items[3].Value = "Pareto";
            ddl.Items.Add("Causa Efecto");
            ddl.Items[4].Value = "Causa Efecto";
            ddl.Items.Add("Otra");
            ddl.Items[5].Value = "Otra";
            ddl.DataBind();
        }

        public int IngresraInforme(mInformeCO obj) //ok
        {
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format("Insert Into sgc_informe_co(id_accion_generada,descripcion_accion,descripcion_evidencia,fecha,estado,id_status) "+
                    "Values('{0}','{1}','{2}',now(),'{3}', 1)",obj.id_accion_generada,obj.descripcion_accion,obj.descripcion_evidencia,obj.estado);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                resultado = cmd.ExecuteNonQuery();
                query = "select @@IDENTITY;";
                cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    resultado = int.Parse(reader[0].ToString());
                }
                conectar.CerrarConexion();
                return resultado;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int actualizarInforme(mInformeCO obj)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_informe_co SET descripcion_accion = '{0}', descripcion_evidencia = '{1}', " +
                    "estado = '{2}', id_status = 1 WHERE id_accion_generada = '{3}'; "
                    , obj.descripcion_accion, obj.descripcion_evidencia, obj.estado, obj.id_accion_generada);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
                return obj.id_informe_correccion;
            }
            catch (Exception ex)
            {
                try
                {
                    transaccion.Rollback();
                }
                catch
                {
                    
                };                
                conectar.CerrarConexion();
                return 0;
            };
        }

        public void actualizarStatus_InformeCO(int id, int status)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_informe_co SET id_status = '{1}' WHERE id_accion_generada = '{0}'; ",
                id, status);
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

        public mInformeCO Obtner_InformeCorreccion(int id)
        {
            mInformeCO mInformeCo = new mInformeCO();
            string query = string.Format("SELECT * FROM sgc_informe_co WHERE id_accion_generada = {0}; "
            , id);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                mInformeCo.id_informe_correccion = int.Parse(dr.GetString("id_informe_correcion"));
                mInformeCo.estado = dr.GetString("estado");
                mInformeCo.id_accion_generada = int.Parse(dr.GetString("id_accion_generada"));
                mInformeCo.descripcion_accion = dr.GetString("descripcion_accion");
                mInformeCo.descripcion_evidencia = dr.GetString("descripcion_evidencia");
                DateTime fecha = DateTime.Parse(dr.GetString("fecha"));
                    mInformeCo.fecha = fecha.ToString("yyyy-MM-dd");
                //mInformeCo.id_enlace = int.Parse(dr.GetString("id_enlace"));
                //mInformeCo.id_lider = int.Parse(dr.GetString("id_lider"));
                mInformeCo.id_status = int.Parse(dr.GetString("id_status"));
            }
            conectar.CerrarConexion();
            return mInformeCo;
        }

        public DataSet ListadoInformesCO(string status)
        {
            //public DataSet ListadoAccionesRealizar(int id_plan, string status)
            //string status = "";
            if (status == "todos")
            {
                status = "";
            }
            else status = "AND id_status=" + status;

            DataSet result = new DataSet();
            conectar = new DBConexion();
            conectar.AbrirConexion();
            string query = string.Format("SELECT ico.id_accion_generada ID, ico.descripcion_accion Descripción, TRUNCATE((((ag.id_status-20)*100)/3),0) Progreso " +
                "FROM sgc_informe_co ico INNER JOIN sgc_accion_generada ag ON ico.id_accion_generada = ag.id_accion_generada;");
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }
    }
}
