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
    public class cInformeCorrecion
    {
        DBConexion conectar;
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
            ddl.Items[0].Value = "Atendido";
            ddl.Items.Add("No Atendido");
            ddl.Items[0].Value = "No Atendido";
            ddl.Items.Add("Se Atendera");
            ddl.Items[0].Value = "Se Atendera";
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
            ddl.Items[0].Value = "No aplica";
            ddl.Items.Add("5 porque");
            ddl.Items[0].Value = "5 porque";
            ddl.Items.Add("Pareto");
            ddl.Items[0].Value = "Pareto";
            ddl.Items.Add("Causa Efecto");
            ddl.Items[0].Value = "Causa Efecto";
            ddl.Items.Add("Otra");
            ddl.Items[0].Value = "Otra";
            ddl.DataBind();
        }

        public int IngresraInforme(mInformeResult obj)
        {
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format("Insert Into sgc_informe_correcion(id_accion_generada,observacion,descripcion_evidencia,evidencia,id_lider,usur_ingresa,fecha)  "+
                    "Values({0},'{1}','{2}','{3}',{4},'{5}',now())",obj.id_accion_generada,obj.observacion,obj.Descripcion_evidencia,obj.evidencia,obj.id_lider,obj.usuario_ingresa);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                resultado = cmd.ExecuteNonQuery();
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
