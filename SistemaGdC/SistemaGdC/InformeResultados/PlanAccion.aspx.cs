using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SistemaGdC.InformeResultados
{
    public partial class PlanAccion : System.Web.UI.Page
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

        int id_enlace;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["noPlanAccion"] = 0;

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));
                id_enlace = mAccionG.id_enlace;
                visibleAdjuntar(false);
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                //txtEvaluacion.Text = mAccionG.no_informe_ei.ToString();
                //txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();
                
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

                enabledPlan(false);
                //if (mAccionG.id_status == 1)
                switch(mAccionG.id_status)
                {
                    case 1: //pendiente de finalizar
                    case -1: //rechazado
                        mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada);
                        ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                        ddlLider.SelectedValue = mPlanAccion.id_lider.ToString();
                        txtCausa.Text = mPlanAccion.causa_raiz;

                        enabledCausaRaiz(false);
                        enabledPlan(true);
                        visibleAdjuntar(false);
                        this.Session["noPlanAccion"] = mPlanAccion.id_plan;
                        break;
                }
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

        protected void btnGuardarCausa_Click(object sender, EventArgs e)
        {       
            mPlanAccion.tecnica_analisis = ddlTecnicaAnalisis.SelectedValue;
            mPlanAccion.causa_raiz = txtCausa.Text;
            mPlanAccion.id_lider = int.Parse(ddlLider.SelectedValue);
            mPlanAccion.usuario_ingreso = Session["usuario"].ToString();
            mPlanAccion.id_accion_generada = int.Parse(Session["noAccion"].ToString());

            if (ddlTecnicaAnalisis.SelectedValue == "No aplica")
            {
                int result = cPlanAccion.IngresraCausaRaiz(mPlanAccion);
                if (int.Parse(Session["noPlanAccion"].ToString()) == 0)
                {
                    cAcciones.validarCausaRaiz_Accion(mPlanAccion.id_accion_generada, 1);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Causa Raíz almacenada exitosamente!', '', 'success');", true);

                    this.Session["noPlanAccion"] = result;

                    enabledCausaRaiz(false);
                    enabledPlan(true);
                }
            }
            else if (ddlTecnicaAnalisis.SelectedValue != "0")
            {
                if (FileEvidencia.HasFile)
                {
                    int tam = FileEvidencia.FileBytes.Length;
                    string ext = Path.GetExtension(FileEvidencia.FileName);
                    if (ext == ".pdf")
                    {
                        if (tam <= 1048576)
                        {
                            int result = cPlanAccion.IngresraCausaRaiz(mPlanAccion);
                            if (int.Parse(Session["noPlanAccion"].ToString()) == 0)
                            {
                                cAcciones.validarCausaRaiz_Accion(mPlanAccion.id_accion_generada, 1);
                                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Causa almacenada exitosamente!', '', 'success');", true);

                                this.Session["noPlanAccion"] = result;
                                FileEvidencia.PostedFile.SaveAs(Server.MapPath("~/Archivos/CausaRaiz/") + Session["noPlanAccion"].ToString() + ".pdf");

                                enabledCausaRaiz(false);
                                enabledPlan(true);
                                visibleAdjuntar(false);
                            }
                        }
                        else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El tamaño de archivo debe ser menor a 1MB', 'info');", true);
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El archivo debe ser extensión PDF', 'info');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'Por favor seleccione un archivo PDF', 'info');", true);
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
        }

        protected void btnGuardarActividad_Click(object sender, EventArgs e)
        {
            mAccionRealizar.id_plan = int.Parse(Session["noPlanAccion"].ToString());
            mAccionRealizar.accion = txtAccionRealizar.Text;
            mAccionRealizar.responsable = txtResponsable.Text;
            mAccionRealizar.fecha_inicio = txtFechaInicio.Text;
            mAccionRealizar.fecha_fin = txtFechaFin.Text;

            int result = cPlanAccion.IngresarAccionRealizar(mAccionRealizar);
            if (result == 1)
            {
                gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
                gvListado.DataBind();
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Actividad almacenada exitosamente!', '', 'success');", true);                
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            txtAccionRealizar.Text = "";
            //ddlResponsable.SelectedIndex = 0;
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
                cPlanAccion.fechaRecepcion_plan(int.Parse(Session["noPlanAccion"].ToString()));
                cAcciones.validarCausaRaiz_Accion(int.Parse(Session["noAccion"].ToString()), 11);
                Response.Redirect("~/Acciones/ListadoAcciones.aspx");
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
            ddlLider.Enabled = en;
            txtCausa.Enabled = en;
            btnGuardarCausa.Enabled = en;
        }

        void visibleAdjuntar(bool vis)
        {
            FileEvidencia.Visible = vis;
        }

        protected void ddlTecnicaAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTecnicaAnalisis.SelectedItem.Value == "No aplica")
                visibleAdjuntar(false);
            else visibleAdjuntar(true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Acciones/ListadoAcciones.aspx");
        }

        protected void gvListado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina"]);
                int psize = gvListado.PageSize;

                GridViewRow selectedRow = gvListado.Rows[index];

                mAccionRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRow.Cells[1].Text));
                cActividades.EliminarAccion(int.Parse(selectedRow.Cells[1].Text));
                gvListado.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
                gvListado.DataBind();

                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Registro eliminado exitosamente!', '', 'error');", true);
            }
        }
    }
}