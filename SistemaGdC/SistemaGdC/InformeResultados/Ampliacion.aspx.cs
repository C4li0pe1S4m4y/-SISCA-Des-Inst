using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SistemaGdC.InformeResultados
{
    public partial class Ampliacion : System.Web.UI.Page
    {
        cFuente cFuente = new cFuente();
        cGeneral cGen = new cGeneral();
        cInformeCO cInfoCorrec = new cInformeCO();
        cAcciones cAcciones = new cAcciones();
        cPlanAcion cPlanAccion = new cPlanAcion();
        cActividades cActividades = new cActividades();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        mPlanAccion mPlanAccion = new mPlanAccion();        
        mActividad mActividad = new mActividad();
        mFuente mFuente = new mFuente();

        int id_enlace;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblFuente.InnerText = cFuente.nombreFuente(Session["noAccion"].ToString());
                //this.Session["noPlanAccion"] = 0;

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));
                mFuente = cFuente.ObtenerFuente(mAccionG.id_fuente);

                id_enlace = mAccionG.id_enlace;
                visibleAdjuntar(false);
                visibleActividad(false);
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();

                txtanio.Text = mFuente.anio.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();
                txtEvaluacion.Text = Session["noAccion"].ToString();
                
                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);

                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();                

                enabledPlan(false);
                //if (mAccionG.id_status == 1)
                        mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada);
                        ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                        txtCausa.Text = mPlanAccion.causa_raiz;

                        enabledCausaRaiz(false);
                        enabledPlan(true);
                        visibleAdjuntar(false);
                        this.Session["noPlanAccion"] = mPlanAccion.id_plan;

                gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));                
                gvListado.DataBind();

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

                cAcciones.dllDependencia(ddldependencia, idUnidad);
                //cInfoCorrec = new cInformeCorrecion();
                //cInfoCorrec.ddlInformeResultados(ddlEvaluacion);
            }
        }

        protected void lblEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedValue;
            string id_dependecia = ddldependencia.SelectedValue;
            string id_informe = txtEvaluacion.Text;
            //cInfoCorrec = new cInformeCorrecion();
            //cInfoCorrec.ddlHallazgo(txtHallazgo, id_unidad, id_dependecia, id_informe);
        }

        protected void ddlHallazgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DataSet tabla = cInfoCorrec.InformacionInformeResultados(txtHallazgo.Text);
            ddlTipoAccionInforme.SelectedIndex = int.Parse(tabla.Tables[0].Rows[0]["id_tipo_accion"].ToString());
            txtDescripcion.Text = tabla.Tables[0].Rows[0]["descripcion"].ToString();
        }

        protected void btnGuardarActividad_Click(object sender, EventArgs e)
        {
            mActividad.id_plan = int.Parse(Session["noPlanAccion"].ToString());
            mActividad.accion = txtAccionRealizar.Text;
            mActividad.responsable = txtResponsable.Text;
            mActividad.fecha_inicio = txtFechaInicio.Text;
            mActividad.fecha_fin = txtFechaFin.Text;

            int result = cPlanAccion.IngresarAccionRealizar(mActividad);
            if (result == 1)
            {
                gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
                gvListado.DataBind();
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Actividad almacenada exitosamente!', '', 'success');", true);
                limpiarActividad();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            btnGuardar.Visible = true;
            visibleActividad(false);
            limpiarActividad();
        }

        protected void limpiarActividad()
        {
            txtAccionRealizar.Text = "";
            txtResponsable.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
        }

        protected void gvListado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                mPlanAccion = cPlanAccion.Obtner_PlanAccion(int.Parse(Session["noAccion"].ToString()));
                cPlanAccion.actualizar_statusPlan(mPlanAccion.id_plan, 1);
                cPlanAccion.agregar_Ampliacion(mPlanAccion.id_plan, mPlanAccion.no_ampliacion);
                Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
            }

            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible finalizar el Plan de Acción', 'Intente de nuevo', 'error');", true);
            }
            
        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            
            if (FileEvidencia.HasFile)
            {
                int tam = FileEvidencia.FileBytes.Length;
                string ext = Path.GetExtension(FileEvidencia.FileName);                
                if (ext == ".pdf")
                {
                    if (tam <= 1048576)
                    {
                        FileEvidencia.PostedFile.SaveAs(Server.MapPath("~/Archivos/CausaRaiz/") + Session["noPlanAccion"].ToString() + ".pdf");
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La evidencia se ha cargado exitosamente', '', 'success');", true);                        
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El tamaño de archivo debe ser menor a 1MB', 'info');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El archivo debe ser extensión PDF', 'info');", true);
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'Por favor seleccione un archivo PDF', 'info');", true);
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

        void enabledPlan(bool en)
        {
            txtAccionRealizar.Enabled = en;
            //txtAccionRealizar.Enabled = en;
            ddlResponsable.Enabled = en;
            txtResponsable.Enabled = en;
            txtFechaInicio.Enabled = en;
            txtFechaFin.Enabled = en;
            btnGuardar.Enabled = en;
            btnNuevo.Enabled = en;
        }

        void enabledCausaRaiz(bool en)
        {
            ddlTecnicaAnalisis.Enabled = en;
            txtCausa.Enabled = en;
        }

        void visibleAdjuntar(bool vis)
        {
            FileEvidencia.Visible = vis;
        }

        void visibleActividad(bool vis)
        {
            btnEditar.Visible = vis;
            btnEliminar.Visible = vis;
        }

        protected void ddlTecnicaAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTecnicaAnalisis.SelectedItem.Value == "No aplica")
                visibleAdjuntar(false);
            else visibleAdjuntar(true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
        }

        protected void gvListado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                btnGuardar.Visible = false;
                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina"]);
                int psize = gvListado.PageSize;

                GridViewRow selectedRow = gvListado.Rows[index];
                this.Session["Actividad"] = selectedRow.Cells[1].Text;

                mActividad = cActividades.Obtner_Actividad(int.Parse(selectedRow.Cells[1].Text));
                txtAccionRealizar.Text = mActividad.accion;
                txtResponsable.Text = mActividad.responsable;
                txtFechaInicio.Text = mActividad.fecha_inicio;
                txtFechaFin.Text = mActividad.fecha_fin;

                visibleActividad(true);
                //cActividades.EliminarAccion(int.Parse(selectedRow.Cells[1].Text));
                //gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
                //gvListado.DataBind();
                //ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Registro eliminado exitosamente!', '', 'error');", true);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            cActividades.EliminarAccion(int.Parse(Session["Actividad"].ToString()));
            btnGuardar.Visible = true;
            limpiarActividad();
            visibleActividad(false);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Registro eliminado exitosamente!', '', 'error');", true);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            mActividad.id_accion_realizar = int.Parse(Session["Actividad"].ToString());
            mActividad.accion = txtAccionRealizar.Text;
            mActividad.responsable = txtResponsable.Text;
            mActividad.fecha_inicio = txtFechaInicio.Text;
            mActividad.fecha_fin = txtFechaFin.Text;
            cActividades.actualizarActividad(mActividad);
            cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 6);
            btnGuardar.Visible = true;
            limpiarActividad();
            visibleActividad(false);

            gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
            gvListado.DataBind();
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Actividad actualizada exitosamente!', '', 'success');", true);
            
        }
    }
}