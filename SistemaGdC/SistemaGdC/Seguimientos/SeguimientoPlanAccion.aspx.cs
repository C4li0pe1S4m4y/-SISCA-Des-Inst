﻿using Controladores;
using Modelos;
using System;
using System.Collections.Generic;
//using System.Data;
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
        cFuente cFuente = new cFuente();
        cEmpleado cEmpleado = new cEmpleado();
        cCorreo cCorreo = new cCorreo();        
        
        mPlanAccion mPlanAccion = new mPlanAccion();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        mActividad mAccionesRealizar = new mActividad();
        mEmpleado mEmpleado = new mEmpleado();
        mFuente mIneficacia = new mFuente();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (!IsPostBack)
            {
                this.Session["noPlanAccion"] = 0;
                
                btnFinalizar.Visible = false;
                panelBtEnlace.Visible = false;
                panelBtAnalista.Visible = false;
                txtObservacionAct.Enabled = false;

                gvListadoAcciones.DataSource = cPlanAccion.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", tipoConsulta());
                gvListadoAcciones.DataBind();

                panel1.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;

                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);

                ddlTecnicaAnalisis.Enabled = false;
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

                cAcciones.dllDependencia(ddldependencia, idUnidad);
            }
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt16(e.CommandArgument);
            GridViewRow selectedRow = gvListadoAcciones.Rows[index];

            mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

            if (e.CommandName == "Ver")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = true;
                panel4.Visible = true;

                

                

                ///////////////////////////////////////////////////////////////////////
                //txtanio.Text = mAccionG.anio_informe_ei.ToString();
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                txtEvaluacion.Text = " "; //pendiente revisar
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();

                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                //////////////////////////////////////////////////////////////////////

                mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada);
                ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                //ddlLider.SelectedValue = mPlanAccion.id_lider.ToString();
                txtCausa.Text = mPlanAccion.causa_raiz;

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                ////////////////////////////////////////////////
                switch (Session["id_tipo_usuario"].ToString())
                {
                    case "1": //director
                    case "4": //lídeer
                        btnDescargarEficacia.Visible = true;
                        FileEficacia.Visible = false;
                        btnAdjuntarEficacia.Visible = false;
                        btnValidarEficacia.Visible = true;
                        btnRechazarEficacia.Visible = true;
                        break;

                    case "5": //enlace
                        if(mPlanAccion.id_status==6) Response.Redirect("~/InformeResultados/Ampliacion.aspx");
                        FileEficacia.Visible = false;
                        btnAdjuntarEficacia.Visible = false;
                        btnDescargarEficacia.Visible = false;
                        btnValidarEficacia.Visible = false;
                        btnRechazarEficacia.Visible = false;
                        btnAmpliacion.Visible = true;
                        break;

                    case "3": //analista
                        FileEficacia.Visible = true;
                        btnAdjuntarEficacia.Visible = true;
                        btnDescargarEficacia.Visible = false;
                        btnValidarEficacia.Visible = false;
                        btnRechazarEficacia.Visible = false;
                        break;
                }
                ////////////////////////////////////////////////


                ddlTecnicaAnalisis.Enabled = false;
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

        protected int finPlan()
        {
            int finPlan = int.Parse(gvListadoActividadesPendientes.Rows.Count.ToString());
            finPlan += int.Parse(gvListadoActividadesTerminadas.Rows.Count.ToString());
            finPlan += int.Parse(gvListadoActividadesRechazadas.Rows.Count.ToString());

            return finPlan;
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 3: //Analista
                    cActividades.actualizarStatusActividad(int.Parse(Session["idActividad"].ToString()), 2);
                    actualizarListadosActiviades();                

                    if (finPlan()==0)
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
            mAccionG = cAcciones.Obtner_AccionGenerada(mPlanAccion.id_accion_generada);
            mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_enlace, "enlace");            

            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 3: //Analista
                    cActividades.actualizarStatusActividad(int.Parse(Session["idActividad"].ToString()), -2);
                    actualizarListadosActiviades();

                    string fuente = cFuente.nombreFuenteA(Session["noAccion"].ToString());
                    string asunto = "ACTIVIDAD RECHAZADA (" + Session["idActividad"].ToString() + "), " + fuente;
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, txtRechazo.Text);
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
                    txtObservacionAct.Enabled = true;
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
                    txtObservacionAct.Enabled = false;
                    break;

                case "VerValidadas":
                    GridViewRow selectedRowV = gvListadoActividadesValidadas.Rows[index];
                    mAccionesRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRowV.Cells[0].Text));
                    btnValidar.Visible = false;
                    btnRechazar.Visible = false;
                    btnFinalizar.Visible = false;
                    FileEvidencia.Visible = false;
                    txtObservacionAct.Enabled = false;
                    break;

                case "VerRechazadas":
                    GridViewRow selectedRowR = gvListadoActividadesRechazadas.Rows[index];
                    mAccionesRealizar = cActividades.Obtner_Actividad(int.Parse(selectedRowR.Cells[0].Text));
                    btnValidar.Visible = false;
                    btnRechazar.Visible = false;
                    break;
            }
                txtIdActividad.Text = mAccionesRealizar.id_accion_realizar.ToString();
                txtResponsableAct.Text = mAccionesRealizar.responsable.ToString();
                txtActividad.Text = mAccionesRealizar.accion.ToString();
                txtFechaInicioActividad.Text = mAccionesRealizar.fecha_inicio.ToString();
                txtFechaFinActividad.Text = mAccionesRealizar.fecha_fin.ToString();
                txtObservacionAct.Text = mAccionesRealizar.observaciones.ToString();

                this.Session["idActividad"]=mAccionesRealizar.id_accion_realizar;            
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
                            cActividades.actualizarStatusActividad(int.Parse(Session["idActividad"].ToString()), 1);
                            cActividades.actualizarObsActividad(int.Parse(Session["idActividad"].ToString()), txtObservacionAct.Text);
                            actualizarListadosActiviades();

                            if(int.Parse(gvListadoActividadesPendientes.Rows.Count.ToString())==0)
                            {
                                cPlanAccion.finalizarPlan(int.Parse(Session["noPlanAccion"].ToString()));
                            }

                            FileEvidencia.PostedFile.SaveAs(Server.MapPath("~/Archivos/EvidenciasPlanesAccion/") + Session["idActividad"].ToString() + ".pdf");
                            btnFinalizar.Visible = false;
                            FileEvidencia.Visible = false;
                            txtObservacionAct.Enabled = false;
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
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.WriteFile(file.FullName);
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
                            txtObservacionAct.Enabled = false;
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
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(file.FullName);
                    Response.End();
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay eficacia adjunta', '', 'info');", true);
            }
        }

        protected void btnValidarEficacia_Click(object sender, EventArgs e)
        {
            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {                
                //case 4: //Lider
                //    cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 4);
                //    Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
                //    break;

                case 1: //Director
                    //cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 5);
                    cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), 4);
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
            //mEmpleado mAnalista = new mEmpleado();
            mEmpleado mEnlace = new mEmpleado();

            mPlanAccion = cPlanAccion.Obtner_PlanAccion(int.Parse(Session["noAccion"].ToString()));
            mAccionG = cAcciones.Obtner_AccionGenerada(mPlanAccion.id_accion_generada);
            //mAnalista = cEmpleado.Obtner_Empleado(mAccionG.id_analista, "analista");
            mEnlace = cEmpleado.Obtner_Empleado(mAccionG.id_enlace, "enlace");

            string fuente = cFuente.nombreFuenteA(Session["noAccion"].ToString());
            string asunto = "Plan de Acción RECHAZADO (" + Session["noAccion"].ToString() + "), " + fuente;

            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 1: //Director
                    mIneficacia = cFuente.ObtenerFuente(mAccionG.id_fuente);
                    mIneficacia.no_fuente = int.Parse(Session["noAccion"].ToString());
                    mIneficacia.fecha = DateTime.Today.ToString("yyyy-MM-dd");
                    mIneficacia.id_tipo_fuente = 9;
                    int idIneficacia = cFuente.AlmacenarEncabezado(mIneficacia);
                    mAccionG.id_fuente = idIneficacia;
                    mAccionG.aprobado = 2;
                    cAcciones.ingresarAccion(mAccionG);

                    cPlanAccion.actualizar_statusPlan(int.Parse(Session["noPlanAccion"].ToString()), -4);
                    if (mEnlace.email != null) cCorreo.enviarCorreo(mEnlace.email, asunto, txtRechazOEficacia.Text);
                    Response.Redirect("~/Seguimientos/SeguimientoPlanAccion.aspx");
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Actividad', '', 'warning');", true);
                    break;
            }
        }

        protected void btnAmpliacion_Click(object sender, EventArgs e)
        {
            mPlanAccion = cPlanAccion.Obtner_PlanAccion(int.Parse(Session["noAccion"].ToString()));
            if(mPlanAccion.no_ampliacion==2)
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No es posible realizar la ampliación del Plan', 'Ha excedido el máximo de ampliaciones', 'warning');", true);
            else Response.Redirect("~/InformeResultados/Ampliacion.aspx");
        }

        protected void gvListadoAcciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int colDescrip = 5;
                e.Row.Cells[colDescrip].Text =
                    e.Row.Cells[colDescrip].Text.Length > 50 ?
                    (e.Row.Cells[colDescrip].Text.Substring(0, 50) + "...") :
                    e.Row.Cells[colDescrip].Text;
            }                
        }
    }
}