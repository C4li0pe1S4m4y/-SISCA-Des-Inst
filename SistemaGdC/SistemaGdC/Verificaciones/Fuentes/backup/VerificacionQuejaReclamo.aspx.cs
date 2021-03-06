﻿using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace SistemaGdC.Verificaciones.Fuentes
{
    public partial class VerificacionQuejaReclamo : System.Web.UI.Page
    {
        cFuente cQuejaReclamo = new cFuente();
        cGeneral cGen = new cGeneral();
        cAcciones cAcciones = new cAcciones();
        cEmpleado cEmpleado = new cEmpleado();
        cCorreo cCorreo = new cCorreo();

        mFuente mQuejaReclamo = new mFuente();
        mAccionesGeneradas mAccionGenerada = new mAccionesGeneradas();
        mEmpleado mEmpleado = new mEmpleado();
        mAprobados aprobados = new mAprobados();
        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                mostrarBotones(false);

                this.Session["pagina1"] = 0;
                this.Session["pagina2"] = 0;
                gvListadoInformes.Columns[0].Visible = true;
                gvListadoInformes.DataSource = cQuejaReclamo.ListadoFuentes(1,3);
                gvListadoInformes.DataBind();
                gvListadoInformes.Columns[0].Visible = false;
                
                //cResultados = new cInformeEI();

                cAcciones.dropProceso(ddlProceso);
                cAcciones.dropUnidad(ddlUnidad);
                cAcciones.dropTipoAccion(dllTipoAccion);
                cAcciones.dropFadn(ddlFadn);
                //cGen = new cGeneral();
                

                pn1.Visible = true;
            }
        }

        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlUnidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlUnidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                cEmpleado.dllEmpleado(ddlEnlace, idUnidad);
                cEmpleado.dllEmpleado(ddlLider, idUnidad);
                cAcciones.dllDependencia(ddlDependencia, idUnidad);
            }
                
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                mAccionesGeneradas mAccionG = new mAccionesGeneradas();

                panel4.Visible = true;

                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina2"]);
                int psize = gvListadoAcciones.PageSize;

                //string prueba = gvListadoAcciones.Rows[index].Cells[0].Text;


                GridViewRow selectedRow = gvListadoAcciones.Rows[index - (pag * psize)];

                verColumnas(true);
                string aprob = selectedRow.Cells[8].Text;
                verColumnas(false);

                switch(aprob)
                {
                    case "2":
                    case "-2":
                        mostrarBotones(false);
                        break;

                    default:
                        mostrarBotones(true);
                        break;
                }
                this.Session["Accion"] = gvListadoAcciones.Rows[index - (pag * psize)].Cells[0].Text;

                mAccionG = cAcciones.Obtner_AccionGenerada((int.Parse(Session["Accion"].ToString())));

                ddlFadn.SelectedValue = mAccionG.id_fadn.ToString();
                txtInstalacion.Text = mAccionG.instalacion.ToString();
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
            }
        }

        protected void mostrarBotones(bool ver)
        {
            btnActualizar.Visible = ver;
            btnValidar.Visible = ver;
            btnRechazar.Visible = ver;

            
            ddlFadn.Enabled = ver;
            txtInstalacion.Enabled = ver;
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
                if (aprobado == "2") (e.Row.FindControl("btnRevisar") as Button).ControlStyle.CssClass = "btn btn-success";
                else if (aprobado == "-2") (e.Row.FindControl("btnRevisar") as Button).ControlStyle.CssClass = "btn btn-danger";
                else (e.Row.FindControl("btnRevisar") as Button).ControlStyle.CssClass = "btn btn-info";
            }
        }


        protected void OnPaging(Object sender, GridViewPageEventArgs e)
        {
            gvListadoInformes.PageIndex = e.NewPageIndex;
            gvListadoInformes.Columns[0].Visible = true;
            gvListadoInformes.DataSource = cQuejaReclamo.ListadoFuentes(1,3);
            gvListadoInformes.DataBind();
            gvListadoInformes.Columns[0].Visible = false;
        }

        protected void gvListadoInformes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina1"] = e.NewPageIndex;
            gvListadoInformes.PageIndex = e.NewPageIndex;
            gvListadoInformes.Columns[0].Visible = true;
            gvListadoInformes.DataSource = cQuejaReclamo.ListadoFuentes(1,3);
            gvListadoInformes.DataBind();
            gvListadoInformes.Columns[0].Visible = false;
        }

        protected void gvListadoInformes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Revisar")
            {                                           
                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina1"]);
                int psize = gvListadoInformes.PageSize;

                GridViewRow selectedRow = gvListadoInformes.Rows[index-(pag*psize)];

                lblCorrelativo.Text = selectedRow.Cells[0].Text;
                txtanio.Text = selectedRow.Cells[2].Text;
                txtInforme.Text = selectedRow.Cells[3].Text;
                DateTime fecha = DateTime.Parse(selectedRow.Cells[4].Text);             
                    txtFechaInforme.Text = fecha.ToString("yyyy-MM-dd");

                this.Session["idFuente"] = selectedRow.Cells[0].Text;

                actualizarListadoAcciones();

                botonesTodos();

                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = true;
                panel4.Visible = false;
                mostrarBotones(false);
            }
        }

        protected void botonesTodos()
        {
            aprobados = totalAcciones();
            if (aprobados.pend == 0)
            {
                btnRechazarTodo.Visible = false;
                btnValidarTodo.Visible = false;
            }
            else
            {
                btnRechazarTodo.Visible = true;
                btnValidarTodo.Visible = true;
            }
        }

        protected mAprobados totalAcciones()
        {
            mAprobados aprobados = new mAprobados();
            aprobados.aprob = 0;
            aprobados.rech = 0;
            aprobados.pend = 0;
            mQuejaReclamo = cQuejaReclamo.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "3");
            DataTable todos = cQuejaReclamo.ListadoAcciones(mQuejaReclamo.id_fuente, 0, "todos", 3);
            foreach (DataRow row in todos.Rows)
                switch(row["aprobado"].ToString())
                {
                    case "2":
                        aprobados.aprob++;
                        break;

                    case "-2":
                        aprobados.rech++;
                        break;

                    default:
                        aprobados.pend++;
                        break;
                }
            return aprobados;
        }

        protected void gvListadoAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina2"] = e.NewPageIndex;
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            actualizarListadoAcciones();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        protected void verColumnas(bool ver)
        {
            //gvListadoAcciones.Columns[0].Visible = ver;
            gvListadoAcciones.Columns[2].Visible = ver;
            gvListadoAcciones.Columns[8].Visible = ver;
        }

        protected void actualizarListadoAcciones() //carga y actualiza el Listado de Acciones
        {
            verColumnas(true);
            gvListadoAcciones.DataSource = cQuejaReclamo.ListadoAcciones(int.Parse(Session["idFuente"].ToString()), 0, "todos", 3);
            gvListadoAcciones.DataBind();
            verColumnas(false);
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            
            if(int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
            {
                mostrarBotones(false);
                cAcciones.aprobar_Accion(int.Parse(Session["Accion"].ToString()), 2);
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción validada correctamente', '', 'success');", true);

                actualizarListadoAcciones();

                botonesTodos();
            }

            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Informe', '', 'warning');", true);                      
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
                {
                    mostrarBotones(false);
                    mAccionGenerada = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));
                    mEmpleado = cEmpleado.Obtner_Empleado(mAccionGenerada.id_analista, "analista");

                    cAcciones.aprobar_Accion(int.Parse(Session["Accion"].ToString()), -2);

                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Accion", txtRechazoAccion.Text);
                    txtRechazoAccion.Text = "";

                    actualizarListadoAcciones();

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción rechazada correctamente', '', 'error');", true);

                    botonesTodos();
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Informe', '', 'warning');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Hubo un problema', 'Por favor intente de nuevo', 'warning');", true);
            }
            
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                mAccionGenerada = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));

                    mAccionesGeneradas ag = new mAccionesGeneradas();
                    ag.id_accion_generada = int.Parse(Session["Accion"].ToString());
                    ag.descripcion = txtDescripcion.Text;
                    ag.id_lider = int.Parse(ddlLider.SelectedValue);
                    ag.id_enlace = int.Parse(ddlEnlace.SelectedValue);
                    ag.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                    ag.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                    ag.id_fadn = int.Parse(ddlFadn.SelectedValue);
                    ag.id_proceso = int.Parse(ddlProceso.SelectedValue);
                    ag.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                    ag.instalacion = txtInstalacion.Text;

                    cAcciones.actualizar_Accion(ag);

                    actualizarListadoAcciones();

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La Acción ha sido actualizada correctamente', '', 'success');", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
            }            
        }

        protected void btnValidarTodo_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
            {
                mostrarBotones(false);
                mQuejaReclamo = cQuejaReclamo.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "3");
                cAcciones.aprobarTodo_Accion(mQuejaReclamo.id_fuente, "aprobado");
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acciones validadas correctamente', '', 'success');", true);

                botonesTodos();

                actualizarListadoAcciones();
            }

            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Acciones', '', 'warning');", true);
        }

        protected void btnRechazarTodo_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
            {
                mostrarBotones(false);
                mQuejaReclamo = cQuejaReclamo.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "3");
                cAcciones.aprobarTodo_Accion(mQuejaReclamo.id_fuente, "rechazado");
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acciones rechazadas correctamente', '', 'error');", true);

                botonesTodos();

                actualizarListadoAcciones();
            }

            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Acciones', '', 'warning');", true);
        }

        protected void ddlDependencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idDependencia = 0;
            int.TryParse(ddlDependencia.SelectedValue, out idDependencia);
            if (idDependencia > 0)
            {
                txtAnalista.Text = cEmpleado.ObtenerAnalistaUnidad(idDependencia);
            }

        }
    }
}