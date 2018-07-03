using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.IO;

namespace SistemaGdC.Visualizar
{
    public partial class VerPlanAccion : System.Web.UI.Page
    {
        cFuente cResultados = new cFuente();
        cGeneral cGen = new cGeneral();
        cInformeCO cInfoCorrec = new cInformeCO();
        cAcciones cAcciones = new cAcciones();
        cPlanAcion cPlanAccion = new cPlanAcion();
        cActividades cActividades = new cActividades();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        mPlanAccion mPlanAccion = new mPlanAccion();
        mAccionesRealizar mAccionRealizar = new mAccionesRealizar();
        mActividad mActividad = new mActividad();

        int id_enlace;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["noPlanAccion"] = 0;

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));
                id_enlace = mAccionG.id_enlace;
                //txtanio.Text = mAccionG.anio_informe_ei.ToString();
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                //txtEvaluacion.Text = mAccionG.no_informe_ei.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();
                
                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);

                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                
                ddlLider.ClearSelection();
                ddlLider.Items.Clear();
                ddlLider.AppendDataBoundItems = true;
                ddlLider.Items.Add("<< Elija Empleado >>");
                ddlLider.Items[0].Value = "0";
                ddlLider.DataSource = cGen.dropEmpleados();
                ddlLider.DataTextField = "texto";
                ddlLider.DataValueField = "id";
                ddlLider.DataBind();

                mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada); this.Session["noPlanAccion"] = mPlanAccion.id_plan;

                ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                ddlLider.SelectedValue = mPlanAccion.id_lider.ToString();
                txtCausa.Value = mPlanAccion.causa_raiz;

                gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
                gvListado.DataBind();

                enabledCausaRaiz(false);
                enabledActividad(false);

            }
        }

        protected void ddlunidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlunidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                cAcciones.dllDependencia(ddldependencia, idUnidad);
            }
        }

        protected void lblEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedValue;
            string id_dependecia = ddldependencia.SelectedValue;
            string id_informe = txtEvaluacion.Text;
        }

        protected void ddlHallazgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DataSet tabla = cInfoCorrec.InformacionInformeResultados(txtHallazgo.Text);
            ddlTipoAccionInforme.SelectedIndex = int.Parse(tabla.Tables[0].Rows[0]["id_tipo_accion"].ToString());
            txtDescripcion.Text = tabla.Tables[0].Rows[0]["descripcion"].ToString();
        }       

        protected void gvListado_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Session["idActividad"] = gvListado.SelectedValue.ToString();
            mAccionRealizar = cActividades.Obtner_Actividad(int.Parse(gvListado.SelectedValue.ToString().ToString()));

            txtAccionRealizar.Value = mAccionRealizar.accion.ToString();
            txtResponsable.Text = mAccionRealizar.responsable;
            txtFechaInicio.Text = mAccionRealizar.fecha_inicio.ToString();
            txtFechaFin.Text = mAccionRealizar.fecha_fin.ToString();
            txtObservaciones.Value = mAccionRealizar.observaciones.ToString();

            btnDescargarEvidencia.Visible = true;

        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            cAcciones.validarCausaRaiz_Accion(int.Parse(Session["noAccion"].ToString()), 11);
            Response.Redirect("~/Acciones/ListadoAcciones.aspx");
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {       
            string filename = Session["noPlanAccion"].ToString() + ".pdf";
            string folder = "Archivos\\CausaRaiz\\";

            string filepath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + folder + filename;
            FileInfo file = new FileInfo(filepath);

            if (file.Exists)
            {
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", file.Name));
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/pdf";
                Response.TransmitFile(file.FullName);
                Response.End();
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay evidencia adjunta', '', 'info');", true);            
        }

        void enabledCausaRaiz(bool en)
        {
            ddlTecnicaAnalisis.Enabled = en;
            ddlLider.Enabled = en;
            txtCausa.Disabled = !en;
        }

        void enabledActividad(bool en)
        {
            txtAccionRealizar.Disabled = !en;
            txtResponsable.Enabled = en;
            txtFechaInicio.Enabled = en;
            txtFechaFin.Enabled = en;
            txtObservaciones.Disabled = !en;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnDescargarEvidencia_Click(object sender, EventArgs e)
        {
            string filename = Session["idActividad"].ToString() + ".pdf";
            string folder = "Archivos\\EvidenciasPlanesAccion\\";

            string filepath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + folder + filename;
            FileInfo file = new FileInfo(filepath);

            if (file.Exists)
            {
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", file.Name));
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/pdf";
                Response.TransmitFile(file.FullName);
                Response.End();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "__doPostBack(sender, e)", true);
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay evidencia adjunta', '', 'info');", true);
        }
    }
}