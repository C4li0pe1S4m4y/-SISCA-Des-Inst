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
    public partial class PlanAccion : System.Web.UI.Page
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
                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);
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

                ddlResponsable.ClearSelection();
                ddlResponsable.Items.Clear();
                ddlResponsable.AppendDataBoundItems = true;
                ddlResponsable.Items.Add("<< Elija Empleado >>");
                ddlResponsable.Items[0].Value = "0";
                ddlResponsable.DataSource = cGen.dropEmpleados();
                ddlResponsable.DataTextField = "texto";
                ddlResponsable.DataValueField = "id";
                ddlResponsable.DataBind();
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
                cInfoCorrec.ddlInformeResultados(lblEvaluacion);
            }
        }

        protected void lblEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedValue;
            string id_dependecia = ddldependencia.SelectedValue;
            string id_informe = lblEvaluacion.SelectedValue;
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

        protected void btnGuardarCausa_Click(object sender, EventArgs e)
        {
            mPlanAccion plan = new mPlanAccion();
            plan.id_accion = int.Parse(ddlHallazgo.SelectedValue);
            plan.tecnica_analisis = ddlTecnicaAnalisis.SelectedValue;
            plan.causa_raiz = txtCausa.Text;
            plan.id_lider = int.Parse(ddlLider.SelectedValue);
            plan.usuario_ing = Session["usuario"].ToString();
            cPlanAcion cpAccion = new cPlanAcion();
            int result = cpAccion.IngresraInforme(plan);
            if (result>0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Mensaje", "alert('ALMACENADO exitosamente!');", true);
                lblPlan.Text = result.ToString();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            mAccionesRealizar mAcciones = new mAccionesRealizar();
            mAcciones.id_plan = int.Parse(lblPlan.Text);
            mAcciones.accion = txtAccionRealizar.Text;
            mAcciones.responsable = ddlResponsable.SelectedItem.ToString();
            mAcciones.fechaInicio = txtFechaInicio.Text;
            mAcciones.fechaFinal = txtFechaFin.Text;
            mAcciones.observaciones = txtObservaciones.Text;
            cPlanAcion cpAccion = new cPlanAcion();
            int result = cpAccion.IngresarAccionRealizar(mAcciones);
            if (result == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Mensaje", "alert('ALMACENADO exitosamente!');", true);
                gvListado.DataSource = cpAccion.ListadoAccionesRealizar(int.Parse(lblPlan.Text));
                gvListado.DataBind();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            txtAccionRealizar.Text = "";
            ddlResponsable.SelectedIndex = 0;
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtObservaciones.Text = "";
        }

        protected void gvListado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}