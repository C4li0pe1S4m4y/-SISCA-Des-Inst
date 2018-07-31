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

namespace SistemaGdC.InformeResultados
{
    public partial class InformeOM : System.Web.UI.Page
    {
        cFuente cFuente = new cFuente();
        cGeneral cGen = new cGeneral();
        cInformeOM cInformeOM = new cInformeOM();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        cAcciones cAcciones = new cAcciones();
        mInformeOM mInformeOM = new mInformeOM();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));

                lblFuente.InnerText = cFuente.nombreFuenteA(Session["noAccion"].ToString());
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
                //cInfoCorrec = new cInformeCorreccion();
                cInformeOM.ddlEstadoInforme(ddlEstado);
                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
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

        protected void ddlEvaluacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlunidad.SelectedValue;
            string id_dependecia = ddldependencia.SelectedValue;
            //cInformeOM = new cInformeCorreccion();
        }

        protected void ddlHallazgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cInformeOM = new cInformeOM();
            System.Data.DataSet tabla = cInformeOM.InformacionInformeResultados(txtHallazgo.Text);
            ddlTipoAccionInforme.SelectedIndex = int.Parse(tabla.Tables[0].Rows[0]["id_tipo_accion"].ToString());
            txtDescripcion.Text = tabla.Tables[0].Rows[0]["descripcion"].ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            mInformeOM = cInformeOM.Obtner_InformeOM(int.Parse(Session["noAccion"].ToString()));
            bool actualizar = false;
            int informe = 0;
            if (mInformeOM.id_status == -1) actualizar = true;

            mInformeOM.id_accion_generada = int.Parse(Session["noAccion"].ToString());
            mInformeOM.descripcion_evidencia = txtDesEvidencia.Text;
            mInformeOM.descripcion_accion = txtAccionRealizada.Text;
            mInformeOM.estado = ddlEstado.SelectedValue;

            //////////////////////////////////////////////////////////////////

            if (FileEvidencia.HasFile)
            {
                int tam = FileEvidencia.FileBytes.Length;
                string ext = Path.GetExtension(FileEvidencia.FileName);
                if (ext == ".pdf")
                {
                    if (tam <= 1048576)
                    {
                        if (actualizar) informe = cInformeOM.actualizarInforme(mInformeOM);
                        else informe = cInformeOM.IngresraInforme(mInformeOM);

                        if (informe > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe de Oportunidad de Mejora ingresado exitosamente', '', 'success');", true);                            
                            cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 3);
                            cAcciones.ingresarFecha_Solicitud(int.Parse(Session["noAccion"].ToString()));
                            FileEvidencia.PostedFile.SaveAs(Server.MapPath("~/Archivos/InformeOM/") + informe.ToString() + ".pdf");                            

                            Response.Redirect("~/InformeResultados/Acciones/ListadoAcciones.aspx");
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No ha sido posible ingresar Informe', 'Intente de nuevo', 'error');", true);
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El tamaño de archivo debe ser menor a 1MB', 'info');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'El archivo debe ser extensión PDF', 'info');", true);
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible cargar el archivo', 'Por favor seleccione un archivo PDF', 'info');", true);
        }
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InformeResultados/Acciones/ListadoAcciones.aspx");
        }
    }
}