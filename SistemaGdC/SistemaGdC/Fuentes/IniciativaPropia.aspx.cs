﻿using Controladores;
using Modelos;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Fuentes
{
    public partial class IniciativaPropia : System.Web.UI.Page
    {
        cFuente cIniciativaPropia = new cFuente();
        cGeneral cGen = new cGeneral();
        cCorreo cCorreo = new cCorreo();
        cUsuarios cUsuario = new cUsuarios(); ////////
        cEmpleado cEmpleado = new cEmpleado();
        cAcciones cAcciones = new cAcciones();
        mUsuario mUsuario = new mUsuario(); /////
        mFuente mIniciativaPropia = new mFuente();
        mEmpleado mEmpleado = new mEmpleado();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["pagina"] = 0;
                string anio = DateTime.Today.ToString("yyyy");
                txtanio.Text = anio;
                txtInforme.Text = cIniciativaPropia.ultimoInforme(anio,"4").ToString();

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
            cIniciativaPropia.dropMejora(ddlTipoMejora);
            cAcciones.dropProceso(ddlProceso);
            cAcciones.dropUnidad(ddlUnidad);
            cAcciones.dropTipoAccion(dllTipoAccion);
        }

        protected void btnGuardarEncabezado_Click(object sender, EventArgs e)
        {
            mIniciativaPropia.anio = int.Parse(txtanio.Text);
            mIniciativaPropia.no_fuente = int.Parse(txtInforme.Text);
            mIniciativaPropia.fecha = txtFechaInforme.Text;
            mIniciativaPropia.id_tipo_mejora = int.Parse(ddlTipoMejora.SelectedValue);
            mIniciativaPropia.id_tipo_fuente = 4;

            int resultado = cIniciativaPropia.AlmacenarEncabezado(mIniciativaPropia);
            if (resultado > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe creado exitosamente!', '', 'success');", true);
                btnFinalizar.Visible = false;
                this.Session["idFuente"] = resultado.ToString();
                lblCorrelativo.Text = resultado.ToString();

                pn1.Visible = true;

                //txtHallazgo.Text = "1";
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
            mIniciativaPropia = cIniciativaPropia.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "4");
            if (mIniciativaPropia.no_fuente != 0)
            {
                this.Session["idFuente"] = mIniciativaPropia.id_fuente.ToString();
                lblCorrelativo.Text = mIniciativaPropia.id_fuente.ToString();
                txtFechaInforme.Text = mIniciativaPropia.fecha;
                cIniciativaPropia.dropMejora(ddlTipoMejora);
                ddlTipoMejora.SelectedValue = mIniciativaPropia.id_tipo_mejora.ToString();

                switch (mIniciativaPropia.id_status)
                {
                    case 0:
                    case -2:
                    case 1:
                        pn1.Visible = true;
                        btnFinalizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnGuardar.Visible = false;
                        btNuevo.Visible = false;

                        if (mIniciativaPropia.id_status == 0)
                        {
                            btnGuardar.Visible = true;
                            btNuevo.Visible = true;
                        }

                        gvListadoAcciones.DataSource = cIniciativaPropia.ListadoAcciones(mIniciativaPropia.id_fuente, 0, "todos", 4);
                        gvListadoAcciones.DataBind();
                        if (gvListadoAcciones.Rows.Count > 0)
                        {
                            pn1.Visible = true;
                            btnFinalizar.Visible = false;
                            if (mIniciativaPropia.id_status == 0)
                            {
                                btnFinalizar.Visible = true;
                            }
                            limpiarAccion();
                        }
                        //else txtHallazgo.Text = "1";

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

        protected void btnGuardar_Click(object sender, EventArgs e) //ok
        {           
            verColumnas(true);

            mIniciativaPropia = cIniciativaPropia.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "4");

                mAccionG.id_fuente = mIniciativaPropia.id_fuente;
                mAccionG.descripcion = txtDescripcion.Text;
                mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);                               
                mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);

                if (mIniciativaPropia.id_status==0)
                {
                    if (cAcciones.ingresarAccion(mAccionG))
                    {
                        gvListadoAcciones.DataSource = cIniciativaPropia.ListadoAcciones(mIniciativaPropia.id_fuente, 0, "todos", 4);
                        gvListadoAcciones.DataBind();
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción generada exitosamente!', '', 'success');", true);
                        btnFinalizar.Visible = true;
                        limpiarAccion();
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Error al ingresar!', 'Intente de nuevo', 'error');", true);
                }                    
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No es posible agregar más Acciones!', 'El Informe ya ha sido finalizado', 'warning');", true);

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
            txtInforme.Text = cIniciativaPropia.ultimoInforme(txtanio.Text,"4").ToString();
        }        

        protected void btNuevo_Click(object sender, EventArgs e)
        {            
            limpiarAccion();
        }

        void limpiarAccion()
        {
            mIniciativaPropia = cIniciativaPropia.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "4");

            mostrarCampos(true);
            btnGuardar.Visible = false;
            btNuevo.Visible = false;
            btnEditar.Visible = false;            
            btnEliminar.Visible = false;

            if(mIniciativaPropia.id_status==0)
            {
                btnGuardar.Visible = true;
                btNuevo.Visible = true;
            }

            cargarDropLists();

            try
            {
                ddlProceso.SelectedIndex = 0;
                ddlUnidad.SelectedIndex = 0;
                ddlDependencia.SelectedIndex = 0;
                txtDescripcion.Text = "";
                ddlEnlace.SelectedIndex = 0;
                ddlLider.SelectedIndex = 0;
                txtAnalista.Text = "";
                dllTipoAccion.SelectedIndex = 0;
                txtNoPlanAccion.Text = "";
            }

            catch
            {

            }
            

            btnEliminar.Visible = false;
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
                txtNoPlanAccion.Text = "";

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                string aprob = selectedRow.Cells[8].Text;
                verColumnas(false);

                switch(aprob)
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
            gvListadoAcciones.Columns[2].Visible = ver; //status
            gvListadoAcciones.Columns[8].Visible = ver; //aprobado
        }

        protected void mostrarCampos(bool ver)
        {
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

                int colDescrip = 5;
                e.Row.Cells[colDescrip].Text =
                    e.Row.Cells[colDescrip].Text.Length > 50 ?
                    (e.Row.Cells[colDescrip].Text.Substring(0, 50) + "...") :
                    e.Row.Cells[colDescrip].Text;
            }
        }

        protected void gvListadoAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina"] = e.NewPageIndex;
            mIniciativaPropia = cIniciativaPropia.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "4");
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            gvListadoAcciones.DataSource = cIniciativaPropia.ListadoAcciones(mIniciativaPropia.id_fuente, 0, "todos", 4);
            gvListadoAcciones.DataBind();
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            cIniciativaPropia.actualizarStatusFuente(int.Parse(Session["idFuente"].ToString()), 1);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ha finalizado correctamente el Informe', '', 'success');", true);
            Response.Redirect("~/Fuentes/IniciativaPropia.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                verColumnas(true);
                cAcciones.EliminarAccion(int.Parse(Session["noAccion"].ToString()));
                mIniciativaPropia = cIniciativaPropia.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "4");
                gvListadoAcciones.DataSource = cIniciativaPropia.ListadoAcciones(mIniciativaPropia.id_fuente, 0, "todos", 4);
                gvListadoAcciones.DataBind();
                limpiarAccion();
                btnEliminar.Visible = false;
                verColumnas(false);
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Registro eliminado correctamente', '', 'error');", true);
            }
            catch
            {

            }

        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["noAccion"].ToString()));               

                bool editada = false;

                    mAccionesGeneradas ag = new mAccionesGeneradas();

                    mAccionG.id_fuente = mIniciativaPropia.id_fuente;
                    mAccionG.descripcion = txtDescripcion.Text;
                    mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                    mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                    mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);

                    ag.id_accion_generada = int.Parse(Session["noAccion"].ToString());
                    ag.descripcion = txtDescripcion.Text;
                    ag.id_lider = int.Parse(ddlLider.SelectedValue);
                    ag.id_enlace = int.Parse(ddlEnlace.SelectedValue);
                    ag.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    ag.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    ag.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    ag.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);

                    editada = cAcciones.actualizar_Accion(ag);
                    cAcciones.aprobar_Accion(ag.id_accion_generada,0);
                    verColumnas(true);
                    mIniciativaPropia = cIniciativaPropia.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "4");
                    gvListadoAcciones.DataSource = cIniciativaPropia.ListadoAcciones(mIniciativaPropia.id_fuente, 0, "todos", 4);
                    gvListadoAcciones.DataBind();
                    verColumnas(false);

                if (editada)
                {
                    if (mAccionG.aprobado == -2)
                    {
                        mEmpleado = cEmpleado.Obtner_Empleado(1, "director");
                        string fuente = cIniciativaPropia.nombreFuenteF(Session["idFuente"].ToString());
                        string asunto = "ACCIÓN CORREGIDA: " + mAccionG.id_accion_generada + " " + fuente;
                        string descripcion = "Se corrigió la acción para nueva revisión. Por favor revisar.";
                        if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, descripcion);
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La Acción ha sido actualizada correctamente', '', 'success');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
            }
        }
    }

}