using Controladores;
using Modelos;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Informe
{
    public partial class InformeEvaluacionInterna : System.Web.UI.Page
    {
        cFuente cInformeEI = new cFuente();
        cGeneral cGen = new cGeneral();
        cCorreo cCorreo = new cCorreo();
        cUsuarios cUsuario = new cUsuarios(); ////////
        cEmpleado cEmpleado = new cEmpleado();
        cAcciones cAcciones = new cAcciones();
        mUsuario mUsuario = new mUsuario(); /////
        mFuente mInformeEI = new mFuente();
        mEmpleado mEmpleado = new mEmpleado();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["pagina"] = 0;
                string anio = DateTime.Today.ToString("yyyy");
                txtanio.Text = anio;
                txtInforme.Text = cInformeEI.ultimoInforme(anio,"1").ToString();

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
            ddlAccionGenerada.DataSource = cInformeEI.dropAcciones();
            ddlAccionGenerada.DataTextField = "Accion";
            ddlAccionGenerada.DataValueField = "id_acciones";
            ddlAccionGenerada.DataBind();

            cAcciones.dropProceso(ddlProceso);
            cAcciones.dropUnidad(ddlUnidad);
            cAcciones.dropTipoAccion(dllTipoAccion);
        }

        protected void btnGuardar_Click(object sender, EventArgs e) //ok
        {           
            bool existeHallazgo = false;

            verColumnas(true);

            foreach (GridViewRow Row in gvListadoAcciones.Rows)        
                if (Row.Cells[3].Text == txtHallazgo.Text) existeHallazgo = true;                         

            if(!existeHallazgo)
            {
                mInformeEI = cInformeEI.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));

                mAccionG.id_fuente = mInformeEI.id_fuente;
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

                if (mInformeEI.id_status==0)
                {
                    if (cAcciones.ingresarAcción(mAccionG))
                    {
                        gvListadoAcciones.DataSource = cInformeEI.ListadoAcciones(mInformeEI.id_fuente, 0, "todos");
                        gvListadoAcciones.DataBind();
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción generada exitosamente!', '', 'success');", true);
                        btnFinalizar.Visible = true;
                        limpiarAccion();
                    }
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Error al ingresar!', 'Intente de nuevo', 'error');", true);
                }                    
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No es posible agregar más Acciones!', 'El Informe ya ha sido finalizado', 'warning');", true);
            }
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
            txtInforme.Text = cInformeEI.ultimoInforme(txtanio.Text,"1").ToString();
        }

        protected void btnGuardarEncabezado_Click(object sender, EventArgs e)
        {
            mInformeEI.anio = int.Parse(txtanio.Text);
            mInformeEI.no_fuente = int.Parse(txtInforme.Text);
            mInformeEI.fecha = txtFechaInforme.Text;
            mInformeEI.id_tipo_fuente = 1;          
            
            int resultado = cInformeEI.AlmacenarEncabezado(mInformeEI);
            if (resultado > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe creado exitosamente!', '', 'success');", true);
                btnFinalizar.Visible = false;
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
            mInformeEI = cInformeEI.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));
            if (mInformeEI.no_informe_ei != 0)
            {
                lblCorrelativo.Text = mInformeEI.id_fuente.ToString();
                txtFechaInforme.Text = mInformeEI.fecha;

                switch (mInformeEI.id_status)
                {
                    case 0:
                    case -2:
                    case 1:
                        pn1.Visible = true;
                        btnFinalizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnGuardar.Visible = false;
                        btNuevo.Visible = false;

                        if (mInformeEI.id_status == 0)
                        {
                            btnGuardar.Visible = true;
                            btNuevo.Visible = true;
                        }

                        gvListadoAcciones.DataSource = cInformeEI.ListadoAcciones(mInformeEI.id_fuente, 0, "todos");
                        gvListadoAcciones.DataBind();
                        if (gvListadoAcciones.Rows.Count > 0)
                        {
                            pn1.Visible = true;
                            btnFinalizar.Visible = false;
                            if (mInformeEI.id_status == 0)
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
            mInformeEI = cInformeEI.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));

            mostrarCampos(true);
            btnGuardar.Visible = false;
            btNuevo.Visible = false;
            btnEditar.Visible = false;            
            btnEliminar.Visible = false;

            if(mInformeEI.id_status==0)
            {
                btnGuardar.Visible = true;
                btNuevo.Visible = true;
            }

            int hallazgo = 1;

            verColumnas(true);
            foreach (GridViewRow Row in gvListadoAcciones.Rows)            
                if (int.Parse(Row.Cells[3].Text) >= hallazgo) hallazgo = int.Parse(Row.Cells[3].Text)+1;
            verColumnas(false);

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
                ddlEnlace.SelectedIndex = 0;
                ddlLider.SelectedIndex = 0;
                txtAnalista.Text = "";
                dllTipoAccion.SelectedIndex = 0;
                //txtFechaRecepcion.Text = "";
                txtNoPlanAccion.Text = "";
            }

            catch
            {

            }
            

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
                txtNoPlanAccion.Text = "";

                this.Session["noAccion"] = mAccionG.id_accion_generada;

                string aprob = selectedRow.Cells[11].Text;
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
            }
        }

        protected void gvListadoAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina"] = e.NewPageIndex;
            mInformeEI = cInformeEI.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            gvListadoAcciones.DataSource = cInformeEI.ListadoAcciones(mInformeEI.id_fuente, 0, "todos");
            gvListadoAcciones.DataBind();
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            cInformeEI.actualizarInforme(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 1);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ha finalizado correctamente el Informe', '', 'success');", true);
            Response.Redirect("~/Fuentes/InformeEvaluacionInterna.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                verColumnas(true);
                cAcciones.EliminarAccion(int.Parse(Session["noAccion"].ToString()));
                mInformeEI = cInformeEI.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));
                gvListadoAcciones.DataSource = cInformeEI.ListadoAcciones(mInformeEI.id_fuente, 0, "todos");
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

                bool existeHallazgo = false;
                bool editada = false;

                verColumnas(true);
                foreach (GridViewRow Row in gvListadoAcciones.Rows)
                    if (Row.Cells[3].Text == txtHallazgo.Text) existeHallazgo = true;
                verColumnas(false);

                if (!existeHallazgo
                    || mAccionG.correlativo_hallazgo == int.Parse(txtHallazgo.Text))
                {
                    mAccionesGeneradas ag = new mAccionesGeneradas();

                    mAccionG.id_fuente = mInformeEI.id_fuente;
                    mAccionG.correlativo_hallazgo = int.Parse(txtHallazgo.Text);
                    mAccionG.norma = txtPuntoNorma.Text;
                    mAccionG.descripcion = txtDescripcion.Text;
                    //mAccionG.anio_informe_ei = int.Parse(txtanio.Text);
                    //mAccionG.no_informe_ei = int.Parse(txtInforme.Text);
                    mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    mAccionG.id_ccl_accion_generada = int.Parse(ddlAccionGenerada.SelectedValue);
                    mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                    mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                    mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);

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

                    editada = cAcciones.actualizar_Accion(ag);
                    cAcciones.aprobar_Accion(ag.id_accion_generada,0);
                    //verColumnas(true);
                    mInformeEI = cInformeEI.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text));
                    gvListadoAcciones.DataSource = cInformeEI.ListadoAcciones(mInformeEI.id_fuente, 0, "todos");
                    gvListadoAcciones.DataBind();
                    //verColumnas(false);

                    if(editada) ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La Acción ha sido actualizada correctamente', '', 'success');", true);
                    else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Hallazgo!', 'Intente con otro número', 'warning');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
            }
        }
    }

}