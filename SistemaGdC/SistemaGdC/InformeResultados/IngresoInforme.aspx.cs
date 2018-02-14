using Controladores;
using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Informe
{
    public partial class IngresoInforme : System.Web.UI.Page
    {
        cInformeResultados cResultados;
        cGeneral cGen;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtanio.Text = "2017";
                cResultados = new cInformeResultados();
                ddlAccionGenerada.ClearSelection();
                ddlAccionGenerada.Items.Clear();
                ddlAccionGenerada.AppendDataBoundItems = true;
                ddlAccionGenerada.Items.Add("<< Elija Accion >>");
                ddlAccionGenerada.Items[0].Value = "0";

                ddlAccionGenerada.DataSource = cResultados.dropAcciones();
                ddlAccionGenerada.DataTextField = "Accion";
                ddlAccionGenerada.DataValueField = "id_acciones";
                ddlAccionGenerada.DataBind();
                cResultados.dropProceso(ddlProceso);
                cResultados.dropUnidad(ddlUnidad);
                cResultados.dropTipoAccion(dllTipoAccion);
                cGen = new cGeneral();
                ddlResponsable.ClearSelection();
                ddlResponsable.Items.Clear();
                ddlResponsable.AppendDataBoundItems = true;
                ddlResponsable.Items.Add("<< Elija Empleado >>");
                ddlResponsable.Items[0].Value = "0";
                ddlResponsable.DataSource = cGen.dropEmpleados();
                ddlResponsable.DataTextField = "texto";
                ddlResponsable.DataValueField = "id";
                ddlResponsable.DataBind();

                ddlAnalista.ClearSelection();
                ddlAnalista.Items.Clear();
                ddlAnalista.AppendDataBoundItems = true;
                ddlAnalista.Items.Add("<< Elija Empleado >>");
                ddlAnalista.Items[0].Value = "0";
                ddlAnalista.DataSource = cGen.dropEmpleados();
                ddlAnalista.DataTextField = "texto";
                ddlAnalista.DataValueField = "id";
                ddlAnalista.DataBind();

            }
           


        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cResultados = new cInformeResultados();
            mAccionesGeneradas acciones = new mAccionesGeneradas();
            acciones.id_informe = int.Parse(lblCorrelativo.Text);
            acciones.id_accion = int.Parse(ddlAccionGenerada.SelectedValue);
            acciones.no_correlativo_accion = int.Parse(txtNoPlanAccion.Text);
            acciones.norma = txtPuntoNorma.Text;
            acciones.id_unidad = int.Parse(ddlUnidad.SelectedValue);
            acciones.id_dependecia = int.Parse(ddlDependencia.SelectedValue);
            acciones.descripcion = txtDescripcion.Text;
            acciones.id_enlace = int.Parse(ddlResponsable.SelectedValue);
            acciones.id_analista = int.Parse(ddlAnalista.SelectedValue);
           
            acciones.fecha_recepcion = txtFechaRecepcion.Text;
            acciones.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
            acciones.id_proceso = int.Parse(ddlProceso.SelectedValue);
            if (cResultados.IngresarInforme(acciones))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Mensaje", "alert('ALMACENADO/MODIFICADO exitosamente!');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "Mensaje", "alert('Error al ingresar');", true);
            }
        }

        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlUnidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlUnidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                cResultados = new cInformeResultados();
                cResultados.dllDependencia(ddlDependencia, idUnidad);
            }
        }

        protected void btnGuardarEncabezado_Click(object sender, EventArgs e)
        {
            mInformeCorrecion mInforme = new mInformeCorrecion();
            mInforme.anio_inicio = int.Parse(txtanio.Text);
            mInforme.no_informe = txtInforme.Text;
            mInforme.fecha_informe = txtFechaInforme.Text;
            cResultados = new cInformeResultados();
            int resultado = cResultados.AlmacenarEncabezado(mInforme);
            if (resultado>0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "alert('ALMACENADO/MODIFICADO exitosamente!');", true);
                lblCorrelativo.Text = resultado.ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "alert('Error al ingresar');", true);
            }
        }

        protected void btnBuscarEncabezado_Click(object sender, EventArgs e)
        {
            cResultados = new cInformeResultados();
            mInformeCorrecion mInforme = cResultados.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));
            if (mInforme != null )
            {
                lblCorrelativo.Text = mInforme.id_correlativo.ToString();
                txtFechaInforme.Text = mInforme.fecha_informe;
            }
        }

        protected void btNuevo_Click(object sender, EventArgs e)
        {
            ddlAccionGenerada.SelectedIndex = 0;
            txtHallazgo.Text = "";
            txtPuntoNorma.Text = "";
            ddlProceso.SelectedIndex = 0;
            ddlUnidad.SelectedIndex = 0;
            ddlDependencia.SelectedIndex = 0;
            txtDescripcion.Text = "";
            ddlResponsable.SelectedIndex = 0;
            ddlAnalista.SelectedIndex = 0;
            txtFechaInforme.Text = "";
            dllTipoAccion.SelectedIndex = 0;
            txtFechaRecepcion.Text = "";
            txtNoPlanAccion.Text = "";
        }

        protected void btnListado_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InformeResultados/ListadoAcciones.aspx?idInforme=" + lblCorrelativo.Text);
        }
    }
}