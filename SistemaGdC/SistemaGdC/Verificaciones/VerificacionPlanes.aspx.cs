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

namespace SistemaGdC.Verificaciones
{
    public partial class VerificacionPlanes : System.Web.UI.Page
    {
        cPlanAcion cPlanAccion = new cPlanAcion();
        cAcciones cAcciones = new cAcciones();


        cInformeEI cResultados = new cInformeEI();
        
        cGeneral cGen = new cGeneral();
        cInformeCO cInfoCorrec = new cInformeCO();
        mPlanAccion mPlanAccion = new mPlanAccion();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
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

                mAccionG = cResultados.Obtner_AccionGenerada(40);
                id_enlace = mAccionG.id_enlace;

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
                cResultados = new cInformeEI();
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


                mAccionG = new mAccionesGeneradas();
                mPlanAccion = new mPlanAccion();

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
                txtEvaluacion.Text = mAccionG.no_informe_ei.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();

                cResultados.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                //////////////////////////////////////////////////////////////////////

                //cInfoCorrec = new cInformeCorrecion();
                cInfoCorrec.ddlTecnicaAnalisis(ddlTecnicaAnalisis);
                ddlLider.DataSource = cGen.dropEmpleados();
                ddlLider.DataTextField = "texto";
                ddlLider.DataValueField = "id";
                ddlLider.DataBind();

                mPlanAccion = cPlanAccion.Obtner_PlanAccion(mAccionG.id_accion_generada);
                ddlTecnicaAnalisis.SelectedValue = mPlanAccion.tecnica_analisis;
                ddlLider.SelectedValue = mPlanAccion.id_lider.ToString();
                txtCausa.Text = mPlanAccion.causa_raiz;

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                ddlTecnicaAnalisis.Enabled = false;
                ddlLider.Enabled = false;
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
                    Response.Redirect("~/Verificaciones/VerificacionPlanes.aspx");
                    break;

                case 3: //Analista
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 13);
                    Response.Redirect("~/Verificaciones/VerificacionPlanes.aspx");
                    break;

                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 14);
                    Response.Redirect("~/Verificaciones/VerificacionPlanes.aspx");
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Plan de Acción', '', 'warning');", true);
                    break;
            }            
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 4: //Líder
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -1);
                    Response.Redirect("~/Verificaciones/VerificacionPlanes.aspx");
                    break;

                case 3: //Analista
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -1);
                    Response.Redirect("~/Verificaciones/VerificacionPlanes.aspx");
                    break;

                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -1);
                    Response.Redirect("~/Verificaciones/VerificacionPlanes.aspx");
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

            if (file.Exists)
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();

                //Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", file.Name));
                //Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/pdf";
                //Response.Write("<script>window.open('" + file.FullName + "','_newtab');</script>");
                Response.TransmitFile(file.FullName);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay evidencia adjunta', '', 'info');", true);
            }
        }
    }
}