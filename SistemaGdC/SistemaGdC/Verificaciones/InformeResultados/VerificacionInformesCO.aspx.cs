using Controladores;
using Modelos;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SistemaGdC.Verificaciones.InformeResultados
{
    public partial class VerificacionInformesCO : System.Web.UI.Page
    {
        cPlanAcion cPlanAccion = new cPlanAcion();
        cAcciones cAcciones = new cAcciones();
        cCorreo cCorreo = new cCorreo();
        cEmpleado cEmpleado = new cEmpleado();
        cFuente cResultados = new cFuente();

        cGeneral cGen = new cGeneral();
        cInformeCO cInfoCorrec = new cInformeCO();
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
                    tipoConsulta = "validarInformeCoDirector";
                    break;

                case "3":
                    tipoConsulta = "validarInformeCoAnalista";
                    break;

                case "4":
                    tipoConsulta = "validarInformeCoLider";
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
                panel4.Visible = true;

                mAccionG = new mAccionesGeneradas();
                
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
                //ddlLider.DataSource = cGen.dropEmpleados();
                //ddlLider.DataTextField = "texto";
                //ddlLider.DataValueField = "id";
                //ddlLider.DataBind();

                this.Session["noAccion"] = mAccionG.id_accion_generada;                

                mInformeCO mInformeCo = new mInformeCO();
                cInformeCO cInformeCo = new cInformeCO();
                mInformeCo = cInformeCo.Obtner_InformeCorreccion(int.Parse(Session["noAccion"].ToString()));
                this.Session["id_informe_correccion"] = mInformeCo.id_informe_correccion.ToString();
                //cInfoCorrec.ddlEstadoInforme(ddlEstado);
                //string prueba = ddlEstado.SelectedValue;
                //ddlEstado.SelectedValue = mInformeCo.estado.ToString();
                //ddlEstado.SelectedValue = "Se Atenderá";
                //ddlLider.SelectedValue = mInformeCo.id_lider.ToString();
                txtAccionRealizada.Text = mInformeCo.descripcion_accion;
                txtDesEvidencia.Text = mInformeCo.descripcion_evidencia;

                //ddlLider.Enabled = false;
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
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 21);
                    break;

                case 3: //Analista
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 22);
                    break;

                case 1: //Director
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 23);
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
                    cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), -2);
                    cInfoCorrec.actualizarStatus_InformeCO(int.Parse(Session["noAccion"].ToString()), -1);
                    if (mEmpleado.email != null)  cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Informe de Corrección", txtRechazo.Text);
                    Response.Redirect("~/Verificaciones/InformeResultados/VerificacionInformesCO.aspx");
                    break;                    

                default:
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Informe de Corrección', '', 'warning');", true);
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
            string folder = "Archivos\\InformeCorreccion\\";

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
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No hay evidencia adjunta', '', 'info');", true);
            }
        }
    }
}