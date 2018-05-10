using Controladores;
using Modelos;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Informe
{
    public partial class IngresoInforme : System.Web.UI.Page
    {
        cInformeEI cResultados = new cInformeEI();
        cGeneral cGen = new cGeneral();
        cCorreo cCorreo = new cCorreo();
        cUsuarios cUsuario = new cUsuarios(); ////////
        mUsuario mUsuario = new mUsuario(); /////
        mInformeEI mInformeEI = new mInformeEI();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.Session["pagina"] = 0;
                txtanio.Text = DateTime.Today.ToString("yyyy");

                cargarDropLists();

                btnEliminar.Visible = false;

                pn1.Visible = false;
            }
        }

        protected void cargarDropLists()
        {
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

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            

            bool existeHallazgo = false;
            foreach (GridViewRow Row in gvListadoAcciones.Rows)        
                if (Row.Cells[3].Text == txtHallazgo.Text) existeHallazgo = true;

            if(!existeHallazgo)
            {
                mAccionG.correlativo_hallazgo = int.Parse(txtHallazgo.Text);
                mAccionG.norma = txtPuntoNorma.Text;
                mAccionG.descripcion = txtDescripcion.Text;
                mAccionG.fecha = txtFechaRecepcion.Text;
                mAccionG.anio_informe_ei = int.Parse(txtanio.Text);
                mAccionG.no_informe_ei = int.Parse(txtInforme.Text);
                mAccionG.id_analista = int.Parse(ddlAnalista.SelectedValue);
                mAccionG.id_enlace = int.Parse(ddlResponsable.SelectedValue);
                mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                mAccionG.id_ccl_accion_generada = int.Parse(ddlAccionGenerada.SelectedValue);
                mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);

                if (cResultados.IngresarInforme(mAccionG))
                {
                    gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 0);
                    gvListadoAcciones.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción generada exitosamente!', '', 'success');", true);
                    limpiarAccion();                    
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Error al ingresar!', 'Intente de nuevo', 'error');", true);
                }
            }
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Hallazgo!', 'Intente con otro número', 'warning');", true);
        }

        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlUnidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlUnidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                cResultados.dllDependencia(ddlDependencia, idUnidad);
            }
        }

        protected void btnGuardarEncabezado_Click(object sender, EventArgs e)
        {

            mInformeEI.anio = int.Parse(txtanio.Text);
            mInformeEI.no_informe = txtInforme.Text;
            mInformeEI.fecha = txtFechaInforme.Text;
            
            int resultado = cResultados.AlmacenarEncabezado(mInformeEI);
            if (resultado > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe creado exitosamente!', '', 'success');", true);
                lblCorrelativo.Text = resultado.ToString();

                pn1.Visible = true;
                gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 0);
                gvListadoAcciones.DataBind();
                btnFinalizar.Visible = true;

                txtHallazgo.Text = "1";
            }
            else if (resultado == -10)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No es posible crear Informe', 'Ya existe Informe ó falta fecha de Informe', 'warning');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Error al ingresar', 'Intente de nuevo', 'error');", true);
            }
        }

        protected void btnBuscarEncabezado_Click(object sender, EventArgs e)
        {
            mInformeEI mInformeEI = cResultados.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));
            if (mInformeEI.no_informe != null)
            {
                lblCorrelativo.Text = mInformeEI.id_informe_ei.ToString();
                txtFechaInforme.Text = mInformeEI.fecha;

                switch (mInformeEI.id_status)
                {
                    case 0:
                    case -2:
                        pn1.Visible = true;
                        btnFinalizar.Visible = true;
                        btnEliminar.Visible = false;

                        gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 0);
                        gvListadoAcciones.DataBind();
                        if (gvListadoAcciones.Rows.Count > 0)
                        {
                            pn1.Visible = true;
                            btnFinalizar.Visible = true;
                            limpiarAccion();
                        }
                        else txtHallazgo.Text = "1";
                        break;

                    case 1:
                        pn1.Visible = false;
                        btnFinalizar.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('El informe está pendiente de Validación por el Director', '', 'info');", true);
                        break;

                    case 2:
                        pn1.Visible = false;
                        btnFinalizar.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('El informe está en ejecución', '', 'info');", true);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                pn1.Visible = false;
                btnFinalizar.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No existe Informe', '', 'warning');", true);
            }
        }

        protected void btNuevo_Click(object sender, EventArgs e)
        {
            limpiarAccion();
            

            /*
            mUsuario = cUsuario.Obtner_Usuario(Session["Usuario"].ToString());
            bool correo = cCorreo.enviarCorreo(mUsuario.correo, "nueva prueba", "usando controlador");
            if(correo) ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "alert('Se ha enviado notificación correctamente');", true);
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "alert('Hubo un problema al enviar notificación');", true);
            */
        }

        void limpiarAccion()
        {
            int hallazgo = 1;
            foreach (GridViewRow Row in gvListadoAcciones.Rows)            
                if (int.Parse(Row.Cells[3].Text) >= hallazgo) hallazgo = int.Parse(Row.Cells[3].Text)+1;

            cargarDropLists();

            try
            {
                ddlAccionGenerada.SelectedIndex = 0;
                txtHallazgo.Text = hallazgo.ToString();
                txtPuntoNorma.Text = "";
                ddlProceso.SelectedIndex = 0;
                ddlUnidad.SelectedIndex = 0;
                ddlDependencia.SelectedIndex = 0;
                txtDescripcion.Text = "";
                ddlResponsable.SelectedIndex = 0;
                ddlAnalista.SelectedIndex = 0;
                dllTipoAccion.SelectedIndex = 0;
                txtFechaRecepcion.Text = "";
                txtNoPlanAccion.Text = "";
            }

            catch
            {

            }
            

            btnEliminar.Visible = false;
        }

        protected void btnListado_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InformeResultados/ListadoAcciones.aspx?idInforme=" + lblCorrelativo.Text);
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina"]);
                int psize = gvListadoAcciones.PageSize;

                GridViewRow selectedRow = gvListadoAcciones.Rows[index - (pag * psize)];
                
                mAccionG = cResultados.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                ddlAccionGenerada.SelectedValue = mAccionG.id_ccl_accion_generada.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();
                txtPuntoNorma.Text = mAccionG.norma.ToString();
                ddlProceso.SelectedValue = mAccionG.id_proceso.ToString();
                ddlUnidad.SelectedValue = mAccionG.id_unidad.ToString();
                cResultados.dllDependencia(ddlDependencia, mAccionG.id_unidad);
                ddlDependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                ddlResponsable.SelectedValue = mAccionG.id_enlace.ToString();
                ddlAnalista.SelectedValue = mAccionG.id_analista.ToString();
                dllTipoAccion.SelectedValue = mAccionG.id_tipo_accion.ToString();
                txtFechaRecepcion.Text = mAccionG.fecha.ToString();
                txtNoPlanAccion.Text = "";

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                btnEliminar.Visible = true;
            }
        }

        protected void gvListadoAcciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        protected void gvListadoAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina"] = e.NewPageIndex;
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 0);
            gvListadoAcciones.DataBind();
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            cResultados.actualizarInforme(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 1);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ha finalizado correctamente el Informe', '', 'success');", true);
            Response.Redirect("~/InformeResultados/IngresoInforme.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                cResultados.EliminarAccion(int.Parse(Session["noAccion"].ToString()));
                gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 0);
                gvListadoAcciones.DataBind();
                limpiarAccion();
                btnEliminar.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Registro eliminado correctamente', '', 'error');", true);
            }
            catch
            {

            }

        }
    }

}