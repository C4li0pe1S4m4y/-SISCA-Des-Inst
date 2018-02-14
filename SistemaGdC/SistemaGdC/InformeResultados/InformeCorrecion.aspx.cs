using Controladores;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.InformeResultados
{
    public partial class InformeCorrecion : System.Web.UI.Page
    {
        cInformeResultados cResultados;
        cGeneral cGen;
        cInformeCorrecion cInfoCorrec;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cResultados = new cInformeResultados();
                txtanio.Text = "2017";
                cResultados.dropUnidad(ddlunidad);
                cInfoCorrec = new cInformeCorrecion();
                cInfoCorrec.ddlEstadoInforme(ddlEstado);
                cResultados.dropTipoAccion(ddlTipoAccionInforme);
                cGen = new cGeneral();
                ddlLider.ClearSelection();
                ddlLider.Items.Clear();
                ddlLider.AppendDataBoundItems = true;
                ddlLider.Items.Add("<< Elija Empleado >>");
                ddlLider.Items[0].Value = "0";
                ddlLider.DataSource = cGen.dropEmpleados();
                ddlLider.DataTextField = "texto";
                ddlLider.DataValueField = "id";
                ddlLider.DataBind();
            }
        }

        protected void ddlunidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlunidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                cResultados = new cInformeResultados();
                cResultados.dllDependencia(ddldependencia, idUnidad);
                cInfoCorrec = new cInformeCorrecion();
                cInfoCorrec.ddlInformeResultados(ddlEvaluacion);
            }
        }

        protected void ddlEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedValue;
            string id_dependecia = ddldependencia.SelectedValue;
            string id_informe = ddlEvaluacion.SelectedValue;
            cInfoCorrec = new cInformeCorrecion();
            cInfoCorrec.ddlHallazgo(ddlHallazgo, id_unidad, id_dependecia, id_informe);
        }

        protected void ddlHallazgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cInfoCorrec = new cInformeCorrecion();
            DataSet tabla = cInfoCorrec.InformacionInformeResultados(ddlHallazgo.SelectedValue);
            ddlTipoAccionInforme.SelectedIndex = int.Parse(tabla.Tables[0].Rows[0]["id_tipo_accion"].ToString());
            txtDescripcion.Text = tabla.Tables[0].Rows[0]["descripcion"].ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            mInformeResult obj = new mInformeResult();
            obj.id_accion_generada = int.Parse(ddlHallazgo.SelectedValue);
            obj.observacion = txtOportunidades.Text;
            obj.Descripcion_evidencia = txtDesEvidencia.Text;
            obj.evidencia = txtRutaEvidencia.Text;
            obj.id_lider = int.Parse(ddlLider.SelectedValue);
            obj.usuario_ingresa = Session["usuario"].ToString(); 
            cInfoCorrec = new cInformeCorrecion();
            if (cInfoCorrec.IngresraInforme(obj) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Mensaje", "alert('ALMACENADO exitosamente!');", true);
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ddlEvaluacion.SelectedIndex = 0;
            ddlHallazgo.SelectedIndex = 0;
            ddlTipoAccionInforme.SelectedIndex = 0;
            txtDescripcion.Text = "";
            ddlEstado.SelectedIndex = 0;
            txtOportunidades.Text = "";
            txtDesEvidencia.Text = "";
            txtRutaEvidencia.Text = "";
            ddlLider.SelectedIndex = 0;
        }
    }
}