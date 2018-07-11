using Controladores;
using Modelos;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Fuentes
{
    public partial class MinutaRevisionAD : System.Web.UI.Page
    {
        cFuente cMinutaRevisionAD = new cFuente();
        cGeneral cGen = new cGeneral();
        cCorreo cCorreo = new cCorreo();
        cUsuarios cUsuario = new cUsuarios(); ////////
        cEmpleado cEmpleado = new cEmpleado();
        cAcciones cAcciones = new cAcciones();
        mUsuario mUsuario = new mUsuario(); /////
        mFuente mMinutaRevisionAD = new mFuente();
        mEmpleado mEmpleado = new mEmpleado();
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Session["pagina"] = 0;
                string anio = DateTime.Today.ToString("yyyy");
                txtanio.Text = anio;
                txtInforme.Text = cMinutaRevisionAD.ultimoInforme(anio,"7").ToString();

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
            cAcciones.dropProceso(ddlProceso);
            cAcciones.dropUnidad(ddlUnidad);
            cAcciones.dropTipoAccion(dllTipoAccion);
        }

        protected void btnGuardar_Click(object sender, EventArgs e) //ok
        {           
            bool existeCompromiso = false;

            verColumnas(true);

            foreach (GridViewRow Row in gvListadoAcciones.Rows)        
                if (Row.Cells[2].Text == txtCompromiso.Text) existeCompromiso = true;                         

            if(!existeCompromiso)
            {
                mMinutaRevisionAD = cMinutaRevisionAD.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "7");

                mAccionG.id_fuente = mMinutaRevisionAD.id_fuente;
                mAccionG.correlativo_compromiso = int.Parse(txtCompromiso.Text);
                mAccionG.descripcion = txtDescripcion.Text;
                mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);                               
                mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);

                if (mMinutaRevisionAD.id_status==0)
                {
                    if (cAcciones.ingresarAcción(mAccionG))
                    {
                        gvListadoAcciones.DataSource = cMinutaRevisionAD.ListadoAcciones(mMinutaRevisionAD.id_fuente, 0, "todos", 7);
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
            txtInforme.Text = cMinutaRevisionAD.ultimoInforme(txtanio.Text,"7").ToString();
        }

        protected void btnGuardarEncabezado_Click(object sender, EventArgs e)
        {
            mMinutaRevisionAD.anio = int.Parse(txtanio.Text);
            mMinutaRevisionAD.no_fuente = int.Parse(txtInforme.Text);
            mMinutaRevisionAD.fecha = txtFechaInforme.Text;
            mMinutaRevisionAD.id_tipo_fuente = 7;          
            
            int resultado = cMinutaRevisionAD.AlmacenarEncabezado(mMinutaRevisionAD);
            if (resultado > 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Informe creado exitosamente!', '', 'success');", true);
                btnFinalizar.Visible = false;
                this.Session["idFuente"]= resultado.ToString();
                lblCorrelativo.Text = resultado.ToString();

                pn1.Visible = true;

                txtCompromiso.Text = "1";
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
            mMinutaRevisionAD = cMinutaRevisionAD.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "7");
            if (mMinutaRevisionAD.no_fuente != 0)
            {
                this.Session["idFuente"] = mMinutaRevisionAD.id_fuente.ToString();
                lblCorrelativo.Text = mMinutaRevisionAD.id_fuente.ToString();
                txtFechaInforme.Text = mMinutaRevisionAD.fecha;

                switch (mMinutaRevisionAD.id_status)
                {
                    case 0:
                    case -2:
                    case 1:
                        pn1.Visible = true;
                        btnFinalizar.Visible = false;
                        btnEliminar.Visible = false;
                        btnGuardar.Visible = false;
                        btNuevo.Visible = false;

                        if (mMinutaRevisionAD.id_status == 0)
                        {
                            btnGuardar.Visible = true;
                            btNuevo.Visible = true;
                        }

                        gvListadoAcciones.DataSource = cMinutaRevisionAD.ListadoAcciones(mMinutaRevisionAD.id_fuente, 0, "todos", 7);
                        gvListadoAcciones.DataBind();
                        if (gvListadoAcciones.Rows.Count > 0)
                        {
                            pn1.Visible = true;
                            btnFinalizar.Visible = false;
                            if (mMinutaRevisionAD.id_status == 0)
                            {
                                btnFinalizar.Visible = true;
                            } 
                            limpiarAccion();
                        }
                        else txtCompromiso.Text = "1";

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
            mMinutaRevisionAD = cMinutaRevisionAD.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "7");

            mostrarCampos(true);
            btnGuardar.Visible = false;
            btNuevo.Visible = false;
            btnEditar.Visible = false;            
            btnEliminar.Visible = false;

            if(mMinutaRevisionAD.id_status==0)
            {
                btnGuardar.Visible = true;
                btNuevo.Visible = true;
            }

            int compromiso = 1;

            verColumnas(true);
            foreach (GridViewRow Row in gvListadoAcciones.Rows)            
                if (int.Parse(Row.Cells[2].Text) >= compromiso) compromiso = int.Parse(Row.Cells[2].Text)+1;
            verColumnas(false);

            cargarDropLists();

            try
            {
                txtCompromiso.Text = compromiso.ToString();
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

                txtCompromiso.Text = mAccionG.correlativo_compromiso.ToString();
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

                string aprob = selectedRow.Cells[9].Text;
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
            gvListadoAcciones.Columns[3].Visible = ver; //status
            gvListadoAcciones.Columns[9].Visible = ver; //aprobado
        }

        protected void mostrarCampos(bool ver)
        {
            txtCompromiso.Enabled = ver;
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
            mMinutaRevisionAD = cMinutaRevisionAD.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "7");
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            gvListadoAcciones.DataSource = cMinutaRevisionAD.ListadoAcciones(mMinutaRevisionAD.id_fuente, 0, "todos", 7);
            gvListadoAcciones.DataBind();
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            cMinutaRevisionAD.actualizarInforme(int.Parse(Session["idFuente"].ToString()), 7);
            ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ha finalizado correctamente el Informe', '', 'success');", true);
            Response.Redirect("~/Fuentes/MinutaRevisionAD.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                verColumnas(true);
                cAcciones.EliminarAccion(int.Parse(Session["noAccion"].ToString()));
                mMinutaRevisionAD = cMinutaRevisionAD.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "7");
                gvListadoAcciones.DataSource = cMinutaRevisionAD.ListadoAcciones(mMinutaRevisionAD.id_fuente, 0, "todos", 7);
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

                bool existeCompromiso = false;
                bool editada = false;

                verColumnas(true);
                foreach (GridViewRow Row in gvListadoAcciones.Rows)
                    if (Row.Cells[2].Text == txtCompromiso.Text) existeCompromiso = true;
                verColumnas(false);

                if (!existeCompromiso
                    || mAccionG.correlativo_compromiso == int.Parse(txtCompromiso.Text))
                {
                    mAccionesGeneradas ag = new mAccionesGeneradas();

                    mAccionG.id_fuente = mMinutaRevisionAD.id_fuente;
                    mAccionG.correlativo_compromiso = int.Parse(txtCompromiso.Text);
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
                    ag.correlativo_compromiso = int.Parse(txtCompromiso.Text);

                    editada = cAcciones.actualizar_Accion(ag);
                    cAcciones.aprobar_Accion(ag.id_accion_generada,0);
                    //verColumnas(true);
                    mMinutaRevisionAD = cMinutaRevisionAD.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "7");
                    gvListadoAcciones.DataSource = cMinutaRevisionAD.ListadoAcciones(mMinutaRevisionAD.id_fuente, 0, "todos", 7);
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