﻿using Controladores;
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
    public partial class InformeCO : System.Web.UI.Page
    {
        cFuente cFuente = new cFuente();
        cGeneral cGen = new cGeneral();
        
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();

        cAcciones cAcciones = new cAcciones();
        cInformeCO cInfoCorrec = new cInformeCO();
        mInformeCO informeCO = new mInformeCO();

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
                cInfoCorrec = new cInformeCO();
                //cInfoCorrec.ddlEstadoInforme(ddlEstado);
                cAcciones.dropTipoAccion(ddlTipoAccionInforme);
                ddlTipoAccionInforme.SelectedValue = mAccionG.id_tipo_accion.ToString();
                cGen = new cGeneral();
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
            cInfoCorrec = new cInformeCO();
        }

        protected void ddlHallazgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cInfoCorrec = new cInformeCO();
            System.Data.DataSet tabla = cInfoCorrec.InformacionInformeResultados(txtHallazgo.Text);
            ddlTipoAccionInforme.SelectedIndex = int.Parse(tabla.Tables[0].Rows[0]["id_tipo_accion"].ToString());
            txtDescripcion.Text = tabla.Tables[0].Rows[0]["descripcion"].ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            informeCO = cInfoCorrec.Obtner_InformeCorreccion(int.Parse(Session["noAccion"].ToString()));
            bool actualizar = false;
            int informe = 0;
            if (informeCO.id_status == -1) actualizar = true;


            string elempleado = Session["id_empleado"].ToString();

            informeCO.id_accion_generada = int.Parse(Session["noAccion"].ToString());
            //informeCO.id_enlace = int.Parse(Session["id_empleado"].ToString());
            //informeCO.id_lider = int.Parse(ddlLider.SelectedValue);
            informeCO.descripcion_evidencia = txtDesEvidencia.Text;
            informeCO.descripcion_accion = txtAccionRealizada.Text;
            informeCO.estado = "Atendido";            

            //////////////////////////////////////////////////////////////////

            if (FileEvidencia.HasFile)
            {
                int tam = FileEvidencia.FileBytes.Length;
                string ext = Path.GetExtension(FileEvidencia.FileName);
                if (ext == ".pdf")
                {
                    if (tam <= 1048576)
                    {
                        if (actualizar) informe = cInfoCorrec.actualizarInforme(informeCO);
                        else informe = cInfoCorrec.IngresraInforme(informeCO);

                        if (informe > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe de Correción ingresado exitosamente!', '', 'success');", true);
                            cAcciones.actualizarStatus_Accion(int.Parse(Session["noAccion"].ToString()), 2);
                            cAcciones.ingresarFecha_Solicitud(int.Parse(Session["noAccion"].ToString()));
                            FileEvidencia.PostedFile.SaveAs(Server.MapPath("~/Archivos/InformeCorreccion/") + informe.ToString() + ".pdf");                            

                            Response.Redirect("~/InformeResultados/Acciones/ListadoAcciones.aspx");
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No ha sido posible ingresar Informe!', 'Intente de nuevo!', 'warning');", true);                        
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