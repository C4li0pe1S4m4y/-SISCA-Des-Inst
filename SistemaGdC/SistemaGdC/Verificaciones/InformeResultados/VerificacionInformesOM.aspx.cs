using Controladores;
using Modelos;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SistemaGdC.Verificaciones.InformeResultados
{
    public partial class VerificacionInformesOM : System.Web.UI.Page
    {
        cPlanAcion cPlanAccion = new cPlanAcion();
        cAcciones cAcciones = new cAcciones();

        cFuente cFuente = new cFuente();
        mInformeOM mInformeOM = new mInformeOM();
        cEmpleado cEmpleado = new cEmpleado();
        cCorreo cCorreo = new cCorreo();
        
        cGeneral cGen = new cGeneral();
        cInformeOM cInformeOM = new cInformeOM();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
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
                panel4.Visible = false;

                mAccionG = cAcciones.Obtner_AccionGenerada(40);
                id_enlace = mAccionG.id_enlace;
            }
        }

        protected string tipoConsulta()
        {
            string tipoConsulta = "";
            switch (Session["id_tipo_usuario"].ToString())
            {
                case "1":
                    tipoConsulta = "validarInformeOMDirector";
                    break;

                case "3":
                    tipoConsulta = "validarInformeOMAnalista";
                    break;

                case "4":
                    tipoConsulta = "validarInformeOMLider";
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
            if (e.CommandName == "Ver")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = true;
                panel4.Visible = true;

                mAccionG = new mAccionesGeneradas();
                
                int index = Convert.ToInt16(e.CommandArgument);
                GridViewRow selectedRow = gvListadoAcciones.Rows[index];

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                lblFuente.InnerText = cFuente.nombreFuente(mAccionG.id_accion_generada.ToString());
                ///////////////////////////////////////////////////////////////////////
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();

                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                //////////////////////////////////////////////////////////////////////
                this.Session["noAccion"] = mAccionG.id_accion_generada;                
                
                mInformeOM = cInformeOM.Obtner_InformeOM(int.Parse(Session["noAccion"].ToString()));
                this.Session["id_informe_correccion"] = mInformeOM.id_informe_om.ToString();
                cInformeOM.ddlEstadoInforme(ddlEstado);
                ddlEstado.SelectedValue = mInformeOM.estado.ToString();
                txtAccionRealizada.Text = mInformeOM.descripcion_accion;
                txtDesEvidencia.Text = mInformeOM.descripcion_evidencia;
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
                case 4: //Líder
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 31);
                    break;

                case 3: //Analista
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 32);
                    break;

                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 33);
                    break;

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Plan de Acción', '', 'warning');", true);
                    break;
            }
            gvListadoAcciones.DataSource = cPlanAccion.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", tipoConsulta());
            gvListadoAcciones.DataBind();
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));
            mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_enlace, "enlace");

            switch (int.Parse(Session["id_tipo_usuario"].ToString()))
            {
                case 4: //Líder
                case 3: //Analista
                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -3);
                    cInformeOM.actualizarStatus_InformeOM(int.Parse(Session["noAccion"].ToString()), -1);
                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Informe de Oportunidad de Mejora", txtRechazo.Text);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionInformesOM.aspx");
                    break;                    

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Plan de Acción', '', 'warning');", true);
                    break;
            }
            gvListadoAcciones.DataSource = cPlanAccion.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", tipoConsulta());
            gvListadoAcciones.DataBind();
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string filename = Session["id_informe_correccion"].ToString() + ".pdf";
            string folder = "Archivos\\InformeOM\\";

            string filepath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + folder + filename;
            FileInfo file = new FileInfo(filepath);

            if (file.Exists)
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