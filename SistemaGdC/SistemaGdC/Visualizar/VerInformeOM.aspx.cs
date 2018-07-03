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

namespace SistemaGdC.Visualizar
{
    public partial class VerInformeOM : System.Web.UI.Page
    {
        cFuente cResultados = new cFuente();
        cGeneral cGen = new cGeneral();
        
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();

        cAcciones cAcciones = new cAcciones();
        cInformeOM cInfoOM = new cInformeOM();
        mInformeOM informeOM = new mInformeOM();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));

                //txtanio.Text = mAccionG.anio_informe_ei.ToString();
                cAcciones.dropUnidad(ddlunidad);
                ddlunidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddldependencia, mAccionG.id_unidad);
                ddldependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                txtEvaluacion.Text = Session["noAccion"].ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();

                txtanio.Text = "2018";
                cAcciones.dropUnidad(ddlunidad);
                cInfoOM.ddlEstadoInforme(ddlEstado);
                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
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

                informeOM = cInfoOM.Obtner_InformeOM(mAccionG.id_accion_generada);
                ddlEstado.SelectedValue = informeOM.estado;
                ddlLider.SelectedValue = informeOM.id_lider.ToString();
                txtAccionRealizada.Text = informeOM.descripcion_accion;                
                txtDesEvidencia.Text = informeOM.descripcion_evidencia;
                this.Session["id_informe_om"] = informeOM.id_informe_om;
            }
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

        protected void ddlEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedValue;
            string id_dependecia = ddldependencia.SelectedValue;
        }

        protected void ddlHallazgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet tabla = cInfoOM.InformacionInformeResultados(txtHallazgo.Text);
            ddlTipoAccionInforme.SelectedIndex = int.Parse(tabla.Tables[0].Rows[0]["id_tipo_accion"].ToString());
            txtDescripcion.Text = tabla.Tables[0].Rows[0]["descripcion"].ToString();
        }

        
                
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnDescargarEvidencia_Click(object sender, EventArgs e)
        {
            string filename = Session["id_informe_om"].ToString() + ".pdf";
            string folder = "Archivos\\InformeOM\\";

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