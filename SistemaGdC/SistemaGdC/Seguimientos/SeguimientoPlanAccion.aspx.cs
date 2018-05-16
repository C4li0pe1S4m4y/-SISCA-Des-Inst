using Controladores;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Seguimientos
{
    public partial class SeguimientoPlanAccion : System.Web.UI.Page
    {
        cPlanAcion cPlanAccion = new cPlanAcion();
        cAcciones cAcciones = new cAcciones();
        cActividades cActividades = new cActividades();
        cGeneral cGen = new cGeneral();
        cInformeCO cInfoCorrec = new cInformeCO();
        cInformeEI cResultados = new cInformeEI();
        cEmpleado cEmpleado = new cEmpleado();
        cCorreo cCorreo = new cCorreo();

        mPlanAccion mPlanAccion = new mPlanAccion();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        mAccionesRealizar mAccionesRealizar = new mAccionesRealizar();
        mEmpleado mEmpleado = new mEmpleado();
        int id_enlace;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (!IsPostBack)
            {
                this.Session["noPlanAccion"] = 0;
                
                btnFinalizar.Visible = false;
                panelBtEnlace.Visible = false;
                panelBtAnalista.Visible = false;

                gvListadoAcciones.DataSource = cPlanAccion.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", tipoConsulta());
                gvListadoAcciones.DataBind();

                panel1.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;

                mAccionG = cResultados.Obtner_AccionGenerada(40);
                id_enlace = mAccionG.id_enlace;

                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);

                ddlLider.DataSource = cGen.dropEmpleados();
                ddlLider.DataTextField = "texto";
                ddlLider.DataValueField = "id";
                ddlLider.DataBind();

                ddlTecnicaAnalisis.Enabled = false;
                ddlLider.Enabled = false;
                txtCausa.Enabled = false;
            }
        }

        protected string tipoConsulta()
        {
            string tipoConsulta = "";
            switch (Session["id_tipo_usuario"].ToString())
            {
                case "5":
                    tipoConsulta = "seguimientoEnlace";
                    break;

                case "3":
                    tipoConsulta = "seguimientoAnalista";
                    break;

                case "1":
                    tipoConsulta = "seguimientoDirector";
                    break;

                case "4":
                    tipoConsulta = "seguimientoLider";
                    break;

            }
            return tipoConsulta;
        }

        protected void ddlunidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlunidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {

                cResultados.dllDependencia(ddldependencia, idUnidad);
            }
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = true;
                panel4.Visible = true;

                switch (Session["id_tipo_usuario"].ToString())
                {
                    case "1":
                    case "4":
                        btnDescargarEficacia.Visible = true;
                        FileEficacia.Visible = false;
                        btnAdjuntarEficacia.Visible = false;
                        btnValidarEficacia.Visible = true;
                        btnRechazarEficacia.Visible = true;
                        break;

                    case "5":
                        FileEficacia.Visible = false;
                        btnAdjuntarEficacia.Visible = false;
                        btnDescargarEficacia.Visible = false;
                        btnValidarEficacia.Visible = false;
                        btnRechazarEficacia.Visible = false;
                        break;

                    case "3":
                        FileEficacia.Visible = true;
                        btnAdjuntarEficacia.Visible = true;
                        btnDescargarEficacia.Visible = false;
                        btnValidarEficacia.Visible = false;
                        btnRechazarEficacia.Visible = false;
                        break;
                }

                int index = Convert.ToInt16(e.CommandArgument);
                GridViewRow selectedRow = gvListadoAcciones.Rows[index];

                mAccionG = cResultados.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                ///////////////////////////////////////////////////////////////////////
                txtanio.Text = mAccionG.anio_informe_ei.ToString();
                cResultados.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cResultados.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                txtEvaluacion.Text = " "; //pendiente revisar
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();

                cResultados.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                //////////////////////////////////////////////////////////////////////

                mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada);
                ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                ddlLider.SelectedValue = mPlanAccion.id_lider.ToString();
                txtCausa.Text = mPlanAccion.causa_raiz;

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                ddlTecnicaAnalisis.Enabled = false;
                ddlLider.Enabled = false;
                txtCausa.Enabled = false;

                this.Session["noPlanAccion"] = mPlanAccion.id_plan;
                actualizarListadosActiviades();

                if(Session["id_tipo_usuario"].ToString() == "3")
                if (mPlanAccion.id_status == 2 || mPlanAccion.id_status == -3)
                {
                    FileEficacia.Visible = true;
                    btnAdjuntarEficacia.Visible = true;
                }
                else
                {
                    FileEficacia.Visible = false;
                    btnAdjuntarEficacia.Visible = true;
                    btnAdjuntarEficacia.Enabled = false;
                }
                    
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 3: //Analista
                    cActividades.actualizarActividad(int.Parse(Session["idActividad"].ToString()), 2);
                    actualizarListadosActiviades();

                    int finPlan = int.Parse(gvListadoActividadesPendientes.Rows.Count.ToString());
                    finPlan += int.Parse(gvListadoActividadesTerminadas.Rows.Count.ToString());
                    finPlan += int.Parse(gvListadoActividadesRechazadas.Rows.Count.ToString());                    

                    if (finPlan==0)
                    {
                        FileEficacia.Visible = true;
                        FileEficacia.Enabled = true;
                        btnAdjuntarEficacia.Visible = true;
                        btnAdjuntarEficacia.Enabled = true;        
                        cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 2);
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Plan terminado correctamente', 'Puede presentar Eficacia del Plan de Acción', 'success');", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Plan terminado');", true);
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Actividad validada correctamente', '', 'success');", true);
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No tiene permisos para validar Actividad');", true);
                    break;
            }
            btnValidar.Visible = false;
            btnRechazar.Visible = false;
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            mPlanAccion = cPlanAccion.Obtner_PlanAccion(int.Parse(Session["noAccion"].ToString()));
            mAccionG = cResultados.Obtner_AccionGenerada(mPlanAccion.id_accion_generada);
            mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_enlace);            

            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 3: //Analista
                    cActividades.actualizarActividad(int.Parse(Session["idActividad"].ToString()), -2);
                    actualizarListadosActiviades();
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Actividad", txtRechazo.Text);
                    txtRechazo.Text = "";
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Actividad rechazada correctamente', '', 'error');", true);
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Actividad', '', 'warning');", true);
                    break;
            }
            btnValidar.Visible = false;
            btnRechazar.Visible = false;
        }

        protected void gvListadoActividades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 5:
                    panelBtEnlace.Visible = true;
                    btnFinalizar.Visible = true;
                    FileEvidencia.Visible = true;
                    break;

                case 3:
                    panelBtAnalista.Visible = true;
                    btnValidar.Visible = true;
                    btnRechazar.Visible = true;
                    break;
            }

            int index = Convert.ToInt16(e.CommandArgument);
            switch(e.CommandName)
            {
                case "VerPendientes":
                    GridViewRow selectedRowP = gvListadoActividadesPendientes.Rows[index];
                    mAccionesRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRowP.Cells[0].Text));
                    btnValidar.Visible = false;
                    btnRechazar.Visible = false;
                    break;

                case "VerTerminadas":
                    GridViewRow selectedRowT = gvListadoActividadesTerminadas.Rows[index];
                    mAccionesRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRowT.Cells[0].Text));
                    btnFinalizar.Visible = false;
                    FileEvidencia.Visible = false;
                    break;

                case "VerValidadas":
                    GridViewRow selectedRowV = gvListadoActividadesValidadas.Rows[index];
                    mAccionesRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRowV.Cells[0].Text));
                    btnValidar.Visible = false;
                    btnRechazar.Visible = false;
                    btnFinalizar.Visible = false;
                    FileEvidencia.Visible = false;
                    break;

                case "VerRechazadas":
                    GridViewRow selectedRowR = gvListadoActividadesRechazadas.Rows[index];
                    mAccionesRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRowR.Cells[0].Text));
                    btnValidar.Visible = false;
                    btnRechazar.Visible = false;
                    break;
            }
                txtIdActividad.Text = mAccionesRealizar.id_accion_reallizar.ToString();
                txtResponsableAct.Text = mAccionesRealizar.responsable.ToString();
                txtActividad.Text = mAccionesRealizar.accion.ToString();
                txtFechaInicioActividad.Text = mAccionesRealizar.fecha_inicio.ToString();
                txtFechaFinActividad.Text = mAccionesRealizar.fecha_fin.ToString();
                txtObservacionAct.Text = mAccionesRealizar.observaciones.ToString();

                this.Session["idActividad"]=mAccionesRealizar.id_accion_reallizar;            
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (FileEvidencia.HasFile)
            {
                int tam = FileEvidencia.FileBytes.Length;
                string ext = Path.GetExtension(FileEvidencia.FileName);
                if (ext == ".pdf")
                {
                    if (tam <= 1048576)
                    {
                        if (int.Parse(Session["id_tipo_usuario"].ToString()) == 5)
                        {
                            cActividades.actualizarActividad(int.Parse(Session["idActividad"].ToString()), 1);
                            actualizarListadosActiviades();

                            FileEvidencia.PostedFile.SaveAs(Server.MapPath("~/Archivos/EvidenciasPlanesAccion/") + Session["idActividad"].ToString() + ".pdf");
                            btnFinalizar.Visible = false;
                            FileEvidencia.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Actividad finalizada correctamente', '', 'success');", true);                            
                        }
                        else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El tamaño de archivo debe ser menor a 1MB', 'info');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El archivo debe ser extensión PDF', 'info');", true);
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'Por favor seleccione un archivo PDF', 'info');", true);            
        }        

        protected void btnDescargar_Click(object sender, EventArgs e)
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
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay evidencia adjunta', '', 'info');", true);            
        }
        
        void actualizarListadosActiviades()
        {
            gvListadoActividadesPendientes.DataSource = cActividades.ListadoActividades(int.Parse(Session["noPlanAccion"].ToString()), "actPendientes");  //pendientes
            gvListadoActividadesTerminadas.DataSource = cActividades.ListadoActividades(int.Parse(Session["noPlanAccion"].ToString()), "actTerminadas");  //terminadas
            gvListadoActividadesValidadas.DataSource = cActividades.ListadoActividades(int.Parse(Session["noPlanAccion"].ToString()), "actValidadas");  //validadas
            gvListadoActividadesRechazadas.DataSource = cActividades.ListadoActividades(int.Parse(Session["noPlanAccion"].ToString()), "actRechazadas");  //rechazadas
            gvListadoActividadesPendientes.DataBind();
            gvListadoActividadesTerminadas.DataBind();
            gvListadoActividadesValidadas.DataBind();
            gvListadoActividadesRechazadas.DataBind();
        }

        protected void btnAdjuntarEficacia_Click(object sender, EventArgs e)
        {
            if (FileEficacia.HasFile)
            {
                int tam = FileEficacia.FileBytes.Length;
                string ext = Path.GetExtension(FileEficacia.FileName);
                if (ext == ".pdf")
                {
                    if (tam <= 1048576)
                    {
                        if (int.Parse(Session["id_tipo_usuario"].ToString()) == 3)
                        {
                            cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 3);

                            FileEficacia.PostedFile.SaveAs(Server.MapPath("~/Archivos/EficaciaPlanAccion/") + Session["noPlanAccion"].ToString() + ".pdf");
                            btnFinalizar.Visible = false;
                            FileEficacia.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Eficacia agregada exitosamente!', '', 'success');", true);
                            Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
                        }
                        else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El tamaño de archivo debe ser menor a 1MB', 'info');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El archivo debe ser extensión PDF', 'info');", true);
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'Por favor seleccione un archivo PDF', 'info');", true);
        }

        protected void btnDescargarEficacia_Click(object sender, EventArgs e)
        {
            if((Session["id_tipo_usuario"].ToString() == "1")||
               (Session["id_tipo_usuario"].ToString() == "4"))
            { 
                string filename = Session["noPlanAccion"].ToString() + ".pdf";
                string folder = "Archivos\\EficaciaPlanAccion\\";

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
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay eficacia adjunta', '', 'info');", true);
            }
        }

        protected void btnValidarEficacia_Click(object sender, EventArgs e)
        {
            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {                
                case 4: //Lider
                    cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 4);
                    Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
                    break;

                case 1: //Director
                    cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 5);
                    cAcciones.actualizarStatus_Accion(int.Parse(this.Session["noAccion"].ToString()), 15);
                    //cambiar status acción
                    Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
                    break;
            }
        }

        protected void btnRechazarEficacia_Click(object sender, EventArgs e)
        {
            mPlanAccion = cPlanAccion.Obtner_PlanAccion(int.Parse(Session["noAccion"].ToString()));
            mAccionG = cResultados.Obtner_AccionGenerada(mPlanAccion.id_accion_generada);
            mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_analista);


            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 1: //Director
                case 4: //Lider
                    cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), -3);
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Eficacia", txtRechazOEficacia.Text);
                    Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
                    break;
            }
        }
    }
}