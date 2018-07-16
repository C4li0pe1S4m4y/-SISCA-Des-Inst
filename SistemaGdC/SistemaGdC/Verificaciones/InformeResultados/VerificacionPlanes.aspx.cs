using Controladores;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SistemaGdC.Verificaciones.InformeResultados
{
    public partial class VerificacionPlanes : System.Web.UI.Page
    {
        cPlanAcion cPlanAccion = new cPlanAcion();
        cAcciones cAcciones = new cAcciones();
        cCorreo cCorreo = new cCorreo();
        cFuente cResultados = new cFuente();
        cUsuarios cUsuario = new cUsuarios();
        cGeneral cGen = new cGeneral();
        cInformeCO cInfoCorrec = new cInformeCO();
        cEmpleado cEmpleado = new cEmpleado();
        mPlanAccion mPlanAccion = new mPlanAccion();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        mUsuario mUsuario = new mUsuario();
        mEmpleado mEmpleado = new mEmpleado();
        int id_enlace;

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                this.Session["noPlanAccion"] = 0;

                gvListadoAcciones.DataSource = cPlanAccion.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", tipoConsulta());
                gvListadoAcciones.DataBind();

                panel1.Visible = false;
                panel3.Visible = false;

                mAccionG = cAcciones.Obtner_AccionGenerada(40);
                id_enlace = mAccionG.id_enlace;

                ddlTecnicaAnalisis.Enabled = false;
                //ddlLider.Enabled = false;
                txtCausa.Enabled = false;                              
            }
        }

        protected string tipoConsulta()
        {
            string tipoConsulta = "";
            switch (Session["id_tipo_usuario"].ToString())
            {
                case "1":
                    tipoConsulta = "validarDirector";
                    break;

                case "3":
                    tipoConsulta = "validarAnalista";
                    break;

                case "4":
                    tipoConsulta = "validarLider";
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
                cResultados = new cFuente();
                cAcciones.dllDependencia(ddldependencia, idUnidad);
            }
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = true;


                mAccionG = new mAccionesGeneradas();
                mPlanAccion = new mPlanAccion();

                int index = Convert.ToInt16(e.CommandArgument);
                GridViewRow selectedRow = gvListadoAcciones.Rows[index];

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                ///////////////////////////////////////////////////////////////////////
                //txtanio.Text = mAccionG.anio_informe_ei.ToString();
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();              
                //txtEvaluacion.Text = mAccionG.no_informe_ei.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();

                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                //////////////////////////////////////////////////////////////////////

                //cInfoCorrec = new cInformeCorrecion();
                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);

                mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada);
                ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                txtCausa.Text = mPlanAccion.causa_raiz;

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                ddlTecnicaAnalisis.Enabled = false;
                //ddlLider.Enabled = false;
                txtCausa.Enabled = false;
              
                this.Session["noPlanAccion"] = mPlanAccion.id_plan;

                gvListadoActividades.DataSource = cPlanAccion.ListadoAccionesRealizar(int.Parse(Session["noPlanAccion"].ToString()));
                gvListadoActividades.DataBind();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            switch(int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 4: //Líder
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 12);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionPlanes.aspx");
                    break;

                case 3: //Analista
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 13);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionPlanes.aspx");
                    break;

                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 14);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionPlanes.aspx");
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Plan de Acción', '', 'warning');", true);
                    break;
            }            
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            //string rechazo = txtRechazo.Text;

            mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));
            mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_enlace, "enlace");
            

            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 4: //Líder
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -1);
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Plan de Acción", txtRechazo.Text);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionPlanes.aspx");
                    break;

                case 3: //Analista
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -1);
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Plan de Acción", txtRechazo.Text);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionPlanes.aspx");
                    break;

                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -1);
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Plan de Acción", txtRechazo.Text);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionPlanes.aspx");
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Plan de Acción', '', 'warning');", true);
                    
                    break;
            }
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string filename = Session["noPlanAccion"].ToString() + ".pdf";
            string folder = "Archivos\\CausaRaiz\\";

            string ruta = "~/Archivos/CausaRaiz/" + filename;

            string filepath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + folder + filename;
            FileInfo file = new FileInfo(filepath);

            if (file.Exists) //abre el documento pdf en navegador. Se necesita javascript para abrirlo en nueva pestaña
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay evidencia adjunta', '', 'info');", true);
            }
        }
    }
}