﻿using Controladores;
using Modelos;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace SistemaGdC.Fuentes
{
    public partial class InformeEvaluacionExterna : System.Web.UI.Page
    {
        cFuente cInformeEE = new cFuente();
        cGeneral cGen = new cGeneral();
        cCorreo cCorreo = new cCorreo();
        cUsuarios cUsuario = new cUsuarios(); ////////
        cEmpleado cEmpleado = new cEmpleado();
        cAcciones cAcciones = new cAcciones();
        mUsuario mUsuario = new mUsuario(); /////
        mFuente mInformeEE = new mFuente();
        mEmpleado mEmpleado = new mEmpleado();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["pagina"] = 0;
                string anio = DateTime.Today.ToString("yyyy");
                txtanio.Text = anio;
                txtInforme.Text = cInformeEE.ultimoInforme(anio, "2").ToString();

                cargarDropLists();

                btnGuardar.Visible = true;
                btNuevo.Visible = true;
                btnEditar.Visible = false;
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
            ddlAccionGenerada.DataSource = cInformeEE.dropAcciones();
            ddlAccionGenerada.DataTextField = "Accion";
            ddlAccionGenerada.DataValueField = "id_acciones";
            ddlAccionGenerada.DataBind();

            cAcciones.dropProceso(ddlProceso);
            cAcciones.dropUnidad(ddlUnidad);
            cAcciones.dropTipoAccion(dllTipoAccion);

            cEmpleado.dllEmpleado(ddlEnlace, 0);
            cEmpleado.dllEmpleado(ddlLider, 0);
            cAcciones.dllDependencia(ddlDependencia, 0);
        }

        protected bool existeHallazgo(string numHallazgo)
        {
            bool existeHallazgo = false;
            verColumnas(true);

            foreach (GridViewRow Row in gvListadoAcciones.Rows)
                if (Row.Cells[3].Text == numHallazgo) existeHallazgo = true;

            verColumnas(false);
            return existeHallazgo;
        }

        protected bool existeNumAccion()
        {
            bool existeNumAccion = false;
            DataTable correlativo = new DataTable();
            correlativo = cAcciones.ListadoCorrelativoAcciones(txtanio.Text, dllTipoAccion.SelectedValue);

            foreach (DataRow Row in correlativo.Rows)
                if (Row["correlativo"].ToString() == txtNoAccion.Text)
                    existeNumAccion = true;

            return existeNumAccion;
        }

        protected void btnGuardar_Click(object sender, EventArgs e) //ok
        {

            if (!existeHallazgo(txtHallazgo.Text))
                if (!existeNumAccion())
                {
                    verColumnas(true);
                    mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");

                    mAccionG.id_fuente = mInformeEE.id_fuente;
                    mAccionG.correlativo_hallazgo = int.Parse(txtHallazgo.Text);
                    mAccionG.norma = txtPuntoNorma.Text;
                    mAccionG.descripcion = txtDescripcion.Text;
                    mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    mAccionG.id_ccl_accion_generada = int.Parse(ddlAccionGenerada.SelectedValue);
                    mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                    mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                    mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);
                    mAccionG.id_accion_anual = int.Parse(txtNoAccion.Text);

                    if (mInformeEE.id_status == 0)
                    {
                        if (cAcciones.ingresarAccion(mAccionG))
                        {
                            gvListadoAcciones.DataSource = cInformeEE.ListadoAcciones(mInformeEE.id_fuente, 0, "todos", 2);
                            gvListadoAcciones.DataBind();
                            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción generada exitosamente!', '', 'success');", true);
                            btnFinalizar.Visible = true;
                            limpiarAccion();
                        }
                        else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Error al ingresar!', 'Intente de nuevo', 'error');", true);
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No es posible agregar más Acciones!', 'El Informe ya ha sido finalizado', 'warning');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Acción!', 'Intente con otro número', 'warning');", true);
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Hallazgo!', 'Intente con otro número', 'warning');", true);

            verColumnas(false);
        }

        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idUnidad = 0;
            int.TryParse(ddlUnidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                cEmpleado.dllEmpleado(ddlEnlace, idUnidad);
                cEmpleado.dllEmpleado(ddlLider, idUnidad);
                cAcciones.dllDependencia(ddlDependencia, idUnidad);
            }

        }

        protected void ddlDependencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idDependencia = 0;
            int.TryParse(ddlDependencia.SelectedValue, out idDependencia);
            if (idDependencia > 0)
                txtAnalista.Text = cEmpleado.ObtenerAnalistaUnidad(idDependencia);
        }

        protected void txtanio_TextChanged(object sender, EventArgs e)
        {
            txtInforme.Text = cInformeEE.ultimoInforme(txtanio.Text, "2").ToString();
        }

        protected void btnGuardarEncabezado_Click(object sender, EventArgs e)
        {
            mInformeEE.anio = int.Parse(txtanio.Text);
            mInformeEE.no_fuente = int.Parse(txtInforme.Text);
            mInformeEE.fecha = txtFechaInforme.Text;
            mInformeEE.id_tipo_fuente = 2;

            int resultado = cInformeEE.AlmacenarEncabezado(mInformeEE);
            if (resultado > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe creado exitosamente!', '', 'success');", true);
                btnFinalizar.Visible = false;
                this.Session["idFuente"] = resultado.ToString();
                lblCorrelativo.Text = resultado.ToString();

                pn1.Visible = true;

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

        protected void btnBuscarEncabezado_Click(object sender, EventArgs e) //OK
        {
            mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");
            if (mInformeEE.no_fuente != 0)
            {
                this.Session["idFuente"] = mInformeEE.id_fuente.ToString();
                lblCorrelativo.Text = mInformeEE.id_fuente.ToString();
                txtFechaInforme.Text = mInformeEE.fecha;

                switch (mInformeEE.id_status)
                {
                    case 0:
                    case -2:
                    case 1:
                        pn1.Visible = true;
                        btnFinalizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnGuardar.Visible = false;
                        btNuevo.Visible = false;

                        if (mInformeEE.id_status == 0)
                        {
                            btnGuardar.Visible = true;
                            btNuevo.Visible = true;
                        }

                        gvListadoAcciones.DataSource = cInformeEE.ListadoAcciones(mInformeEE.id_fuente, 0, "todos", 2);
                        gvListadoAcciones.DataBind();
                        if (gvListadoAcciones.Rows.Count > 0)
                        {
                            pn1.Visible = true;
                            btnFinalizar.Visible = false;
                            if (mInformeEE.id_status == 0)
                            {
                                btnFinalizar.Visible = true;
                            }
                            limpiarAccion();
                        }
                        else txtHallazgo.Text = "1";

                        verColumnas(false);

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
        }

        void limpiarAccion()
        {
            mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");

            mostrarCampos(true);
            btnGuardar.Visible = false;
            btNuevo.Visible = false;
            btnEditar.Visible = false;
            btnEliminar.Visible = false;

            if (mInformeEE.id_status == 0)
            {
                btnGuardar.Visible = true;
                btNuevo.Visible = true;
            }

            int hallazgo = 1;

            verColumnas(true);
            foreach (GridViewRow Row in gvListadoAcciones.Rows)
                if (int.Parse(Row.Cells[3].Text) >= hallazgo) hallazgo = int.Parse(Row.Cells[3].Text) + 1;
            verColumnas(false);

            cargarDropLists();


            ddlAccionGenerada.SelectedIndex = 0;
            txtHallazgo.Text = hallazgo.ToString();
            txtPuntoNorma.Text = "";
            ddlProceso.SelectedIndex = 0;
            ddlUnidad.SelectedIndex = 0;
            ddlDependencia.SelectedIndex = 0;
            txtDescripcion.Text = "";
            ddlEnlace.SelectedIndex = 0;
            ddlLider.SelectedIndex = 0;
            txtAnalista.Text = "";
            dllTipoAccion.SelectedIndex = 0;
            txtNoAccion.Text = "";

            btnEliminar.Visible = false;
        }

        protected void btnListado_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InformeResultados/Acciones/ListadoAcciones.aspx?idInforme=" + lblCorrelativo.Text);
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e) //ok
        {
            if (e.CommandName == "Ver")
            {
                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina"]);
                int psize = gvListadoAcciones.PageSize;

                verColumnas(true);
                GridViewRow selectedRow = gvListadoAcciones.Rows[index - (pag * psize)];

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                ddlAccionGenerada.SelectedValue = mAccionG.id_ccl_accion_generada.ToString();
                txtHallazgo.Text = mAccionG.correlativo_hallazgo.ToString();
                txtPuntoNorma.Text = mAccionG.norma.ToString();
                ddlProceso.SelectedValue = mAccionG.id_proceso.ToString();
                ddlUnidad.SelectedValue = mAccionG.id_unidad.ToString();
                cAcciones.dllDependencia(ddlDependencia, mAccionG.id_unidad);
                ddlDependencia.SelectedValue = mAccionG.id_dependencia.ToString();
                txtDescripcion.Text = mAccionG.descripcion.ToString();
                cEmpleado.dllEmpleado(ddlEnlace, mAccionG.id_unidad);
                ddlEnlace.SelectedValue = mAccionG.id_enlace.ToString();
                cEmpleado.dllEmpleado(ddlLider, mAccionG.id_unidad);
                ddlLider.SelectedValue = mAccionG.id_lider.ToString();
                txtAnalista.Text = cEmpleado.ObtenerAnalistaUnidad(mAccionG.id_dependencia);
                dllTipoAccion.SelectedValue = mAccionG.id_tipo_accion.ToString();
                txtNoAccion.Text = mAccionG.id_accion_anual.ToString();

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                string aprob = selectedRow.Cells[11].Text;
                verColumnas(false);

                switch (aprob)
                {
                    case "2":
                        btnGuardar.Visible = false;
                        btnEditar.Visible = false;
                        btnEliminar.Visible = false;
                        mostrarCampos(false);
                        break;

                    case "-2":
                        btnGuardar.Visible = false;
                        btnEditar.Visible = true;
                        btnEliminar.Visible = true;
                        mostrarCampos(true);
                        break;

                    default:
                        btnGuardar.Visible = false;
                        btnEditar.Visible = true;
                        btnEliminar.Visible = true;
                        mostrarCampos(true);
                        break;
                }
            }
        }

        protected void verColumnas(bool ver)
        {
            gvListadoAcciones.Columns[4].Visible = ver;
            gvListadoAcciones.Columns[11].Visible = ver;
        }

        protected void mostrarCampos(bool ver)
        {
            ddlAccionGenerada.Enabled = ver;
            txtHallazgo.Enabled = ver;
            txtPuntoNorma.Enabled = ver;
            ddlProceso.Enabled = ver;
            ddlUnidad.Enabled = ver;
            ddlDependencia.Enabled = ver;
            ddlEnlace.Enabled = ver;
            ddlLider.Enabled = ver;
            dllTipoAccion.Enabled = ver;
            txtDescripcion.Enabled = ver;
        }

        protected void gvListado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string aprobado = DataBinder.Eval(e.Row.DataItem, "Aprobado").ToString();
                if (aprobado == "2") (e.Row.FindControl("btIngresar") as Button).ControlStyle.CssClass = "btn btn-success";
                else if (aprobado == "-2") (e.Row.FindControl("btIngresar") as Button).ControlStyle.CssClass = "btn btn-danger";
                else (e.Row.FindControl("btIngresar") as Button).ControlStyle.CssClass = "btn btn-info";

                int colDescrip = 8;
                e.Row.Cells[colDescrip].Text =
                    e.Row.Cells[colDescrip].Text.Length > 50 ?
                    (e.Row.Cells[colDescrip].Text.Substring(0, 50) + "...") :
                    e.Row.Cells[colDescrip].Text;
            }
        }

        protected void gvListadoAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            verColumnas(true); /////////////////
            this.Session["pagina"] = e.NewPageIndex;
            mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            gvListadoAcciones.DataSource = cInformeEE.ListadoAcciones(mInformeEE.id_fuente, 0, "todos", 2);
            gvListadoAcciones.DataBind();
            verColumnas(true); /////////////////
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            mEmpleado = cEmpleado.Obtner_Empleado(1, "director");
            string fuente = cInformeEE.nombreFuenteF(Session["idFuente"].ToString());
            string asunto = "NUEVA FUENTE: " + fuente;
            string descripcion = "Se ha creado una neuva fuenta en la cual deberá revisar y validar todas sus acciones.";
            if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, descripcion);

            cInformeEE.actualizarStatusFuente(int.Parse(Session["idFuente"].ToString()), 1);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ha finalizado correctamente el Informe', '', 'success');", true);
            Response.Redirect("~/Fuentes/InformeEvaluacionExterna.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            verColumnas(true);
            cAcciones.EliminarAccion(int.Parse(Session["noAccion"].ToString()));
            mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");
            gvListadoAcciones.DataSource = cInformeEE.ListadoAcciones(mInformeEE.id_fuente, 0, "todos", 2);
            gvListadoAcciones.DataBind();
            limpiarAccion();
            btnEliminar.Visible = false;
            verColumnas(false);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Registro eliminado correctamente', '', 'error');", true);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));

            bool editada = false;

            if (!existeHallazgo(txtHallazgo.Text)
                || mAccionG.correlativo_hallazgo == int.Parse(txtHallazgo.Text))
                if (!existeNumAccion()
                    || mAccionG.id_tipo_accion == int.Parse(ddlAccionGenerada.Text))
                {
                    mAccionesGeneradas ag = new mAccionesGeneradas();

                    mAccionG.id_fuente = mInformeEE.id_fuente;
                    mAccionG.correlativo_hallazgo = int.Parse(txtHallazgo.Text);
                    mAccionG.norma = txtPuntoNorma.Text;
                    mAccionG.descripcion = txtDescripcion.Text;
                    mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    mAccionG.id_ccl_accion_generada = int.Parse(ddlAccionGenerada.SelectedValue);
                    mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                    mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                    mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);
                    mAccionG.id_accion_anual = int.Parse(txtNoAccion.Text);

                    ag.id_accion_generada = int.Parse(Session["noAccion"].ToString());
                    ag.norma = txtPuntoNorma.Text;
                    ag.descripcion = txtDescripcion.Text;
                    ag.id_lider = int.Parse(ddlLider.SelectedValue);
                    ag.id_enlace = int.Parse(ddlEnlace.SelectedValue);
                    ag.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    ag.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    ag.id_ccl_accion_generada = int.Parse(ddlAccionGenerada.SelectedValue);
                    ag.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    ag.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                    ag.correlativo_hallazgo = int.Parse(txtHallazgo.Text);
                    ag.id_accion_anual = int.Parse(txtNoAccion.Text);

                    editada = cAcciones.actualizar_Accion(ag);
                    cAcciones.aprobar_Accion(ag.id_accion_generada, 0);
                    verColumnas(true); ///////////
                    mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");
                    gvListadoAcciones.DataSource = cInformeEE.ListadoAcciones(mInformeEE.id_fuente, 0, "todos", 2);
                    gvListadoAcciones.DataBind();
                    verColumnas(false); ///////////

                    if (editada)
                    {
                        if (mAccionG.aprobado == -2)
                        {
                            mEmpleado = cEmpleado.Obtner_Empleado(1, "director");
                            string fuente = cInformeEE.nombreFuenteF(Session["idFuente"].ToString());
                            string asunto = "ACCIÓN CORREGIDA: " + mAccionG.id_accion_generada + " " + fuente;
                            string descripcion = "Se corrigió la acción para nueva revisión. Por favor revisar.";
                            if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, descripcion);
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La Acción ha sido actualizada correctamente', '', 'success');", true);
                    }

                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Acción!', 'Intente con otro número', 'warning');", true);
            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Hallazgo!', 'Intente con otro número', 'warning');", true);
        }

        protected void dllTipoAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable correlativo = new DataTable();
            correlativo = cAcciones.ListadoCorrelativoAcciones(txtanio.Text, dllTipoAccion.SelectedValue);

            if (correlativo.Rows.Count > 0)
            {
                int mayor = Convert.ToInt32(correlativo.Compute("max([correlativo])", string.Empty));
                txtNoAccion.Text = (mayor + 1).ToString();
            }

            else txtNoAccion.Text = "1";
        }
    }

}