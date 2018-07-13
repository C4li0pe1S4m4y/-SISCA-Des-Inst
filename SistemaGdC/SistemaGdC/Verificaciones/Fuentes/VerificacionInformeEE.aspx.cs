using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace SistemaGdC.Verificaciones.Fuentes
{    
    public partial class VerificacionInformeEE : System.Web.UI.Page
    {
        cFuente cInformeEE = new cFuente();
        cGeneral cGen = new cGeneral();
        cAcciones cAcciones = new cAcciones();
        cEmpleado cEmpleado = new cEmpleado();
        cCorreo cCorreo = new cCorreo();

        mFuente mInformeEE = new mFuente();
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
                gvListadoInformes.DataSource = cInformeEE.ListadoFuentes(1,2);
                gvListadoInformes.DataBind();
                gvListadoInformes.Columns[0].Visible = false;
                
                //cResultados = new cInformeEI();
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
                string aprob = selectedRow.Cells[11].Text;
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
            }
        }

        protected void mostrarBotones(bool ver)
        {
            btnActualizar.Visible = ver;
            btnValidar.Visible = ver;
            btnRechazar.Visible = ver;

            
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
                if (aprobado == "2") (e.Row.FindControl("btnRevisar") as Button).ControlStyle.CssClass = "btn btn-success";
                else if (aprobado == "-2") (e.Row.FindControl("btnRevisar") as Button).ControlStyle.CssClass = "btn btn-danger";
                else (e.Row.FindControl("btnRevisar") as Button).ControlStyle.CssClass = "btn btn-info";
            }
        }


        protected void OnPaging(Object sender, GridViewPageEventArgs e)
        {
            gvListadoInformes.PageIndex = e.NewPageIndex;
            gvListadoInformes.Columns[0].Visible = true;
            gvListadoInformes.DataSource = cInformeEE.ListadoFuentes(1,1);
            gvListadoInformes.DataBind();
            gvListadoInformes.Columns[0].Visible = false;
        }

        protected void gvListadoInformes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina1"] = e.NewPageIndex;
            gvListadoInformes.PageIndex = e.NewPageIndex;
            gvListadoInformes.Columns[0].Visible = true;
            gvListadoInformes.DataSource = cInformeEE.ListadoFuentes(1,2);
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
            mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");
            DataTable todos = cInformeEE.ListadoAcciones(mInformeEE.id_fuente, 0, "todos", 2);
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
            gvListadoAcciones.Columns[4].Visible = ver;
            gvListadoAcciones.Columns[11].Visible = ver;
        }

        protected void actualizarListadoAcciones() //carga y actualiza el Listado de Acciones
        {
            verColumnas(true);
            gvListadoAcciones.DataSource = cInformeEE.ListadoAcciones(int.Parse(Session["idFuente"].ToString()), 0, "todos", 2);
            gvListadoAcciones.DataBind();
            verColumnas(false);
        }

        protected void ValidarAccion(string idAccion)
        {
            mostrarBotones(false);
            cAcciones.aprobar_Accion(int.Parse(idAccion), 2);

            actualizarListadoAcciones();
            botonesTodos();            
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
                {
                    ValidarAccion(Session["Accion"].ToString());

                    mAccionGenerada = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));
                    mEmpleado = cEmpleado.Obtner_Empleado(mAccionGenerada.id_analista);

                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Acción Asignada", "Descripción de la acción");
                    txtRechazoAccion.Text = "";

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción validada correctamente', '', 'success');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar acciones', '', 'warning');", true);                
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Hubo un problema al validar Acción', 'Intente de nuevo', 'warning');", true);
            }           
        }

        protected void btnValidarTodo_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
                {
                    DataTable ListadoAcciones = new DataTable();
                    ListadoAcciones = cInformeEE.ListadoAcciones(int.Parse(Session["idFuente"].ToString()), 0, "todos", 2);

                    foreach (DataRow Row in ListadoAcciones.Rows)
                    {
                        if (Row[13].ToString() != "2" && Row[13].ToString() != "-2")
                        {
                            ValidarAccion(Row[0].ToString());

                            mAccionGenerada = cAcciones.Obtner_AccionGenerada(int.Parse(Row[0].ToString()));
                            mEmpleado = cEmpleado.Obtner_Empleado(mAccionGenerada.id_enlace);
                            if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Acción Asignada", "Descripción de la acción");
                            txtRechazoAccion.Text = "";
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acciones validadas correctamente', '', 'success');", true);

                    //botonesTodos();
                    //actualizarListadoAcciones();
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Acciones', '', 'warning');", true);
            }
            finally { }
        }

        protected void RechazarAccion(string idAccion)
        {
                mostrarBotones(false);
                cAcciones.aprobar_Accion(int.Parse(idAccion), -2);

                actualizarListadoAcciones();
                botonesTodos();
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
            {
                mostrarBotones(false);
                mAccionGenerada = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));
                mEmpleado = cEmpleado.Obtner_Empleado(mAccionGenerada.id_analista);

                cAcciones.aprobar_Accion(int.Parse(Session["Accion"].ToString()), -2);

                if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Accion", txtRechazoAccion.Text);
                txtRechazoAccion.Text = "";

                actualizarListadoAcciones();

                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción rechazada correctamente', '', 'error');", true);

                botonesTodos();
            }

            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Informe', '', 'warning');", true);
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                mAccionGenerada = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));

                bool existeHallazgo = false;
                foreach (GridViewRow Row in gvListadoAcciones.Rows)
                    if (Row.Cells[3].Text == txtHallazgo.Text) existeHallazgo = true;

                if (!existeHallazgo
                    ||mAccionGenerada.correlativo_hallazgo==int.Parse(txtHallazgo.Text))
                {
                    mAccionesGeneradas ag = new mAccionesGeneradas();
                    ag.id_accion_generada = int.Parse(Session["Accion"].ToString());
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

                    cAcciones.actualizar_Accion(ag);

                    actualizarListadoAcciones();

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La Acción ha sido actualizada correctamente', '', 'success');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe Número de Hallazgo!', 'Intente con otro número', 'warning');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);
            }            
        }

        

        protected void btnRechazarTodo_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
                {
                    mostrarBotones(false);
                    mInformeEE = cInformeEE.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "2");
                    cAcciones.aprobarTodo_Accion(mInformeEE.id_fuente, "rechazado");
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acciones rechazadas correctamente', '', 'error');", true);

                    botonesTodos();

                    //if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, "Rechazo de Varias Acciones", txtRechazoAccionTodo.Text);
                    //txtRechazoAccionTodo.Text = "";

                    actualizarListadoAcciones();
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Acciones', '', 'warning');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ha ocurrido un error', 'Intente de nuevo', 'error');", true);
            }
            
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