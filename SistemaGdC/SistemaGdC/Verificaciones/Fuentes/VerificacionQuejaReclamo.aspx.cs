using Controladores;
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
        mAccionesGeneradas mAccionG = new mAccionesGeneradas();
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

                cAcciones.dropProceso(ddlProceso);
                cAcciones.dropUnidad(ddlUnidad);
                cAcciones.dropTipoAccion(dllTipoAccion);
                cAcciones.dropFadn(ddlFadn);

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
                panel4.Visible = true;

                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina2"]);
                int psize = gvListadoAcciones.PageSize;

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

                int colDescrip = 7;
                e.Row.Cells[colDescrip].Text =
                    e.Row.Cells[colDescrip].Text.Length > 50 ?
                    (e.Row.Cells[colDescrip].Text.Substring(0, 50) + "...") :
                    e.Row.Cells[colDescrip].Text;
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

                GridViewRow selectedRow = gvListadoInformes.Rows[index - (pag * psize)];

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
            gvListadoAcciones.Columns[2].Visible = ver;
            gvListadoAcciones.Columns[10].Visible = ver;
        }

        protected void actualizarListadoAcciones() //carga y actualiza el Listado de Acciones
        {
            verColumnas(true);
            gvListadoAcciones.DataSource = cQuejaReclamo.ListadoAcciones(int.Parse(Session["idFuente"].ToString()), 0, "todos", 3);
            gvListadoAcciones.DataBind();
            verColumnas(false);
        }

        protected void ValidarAccion(string idAccion)
        {
            mostrarBotones(false);
            cAcciones.aprobar_Accion(int.Parse(idAccion), 2);

            actualizarListadoAcciones();

            int totalAprobados = 0;
            DataTable aprobados = new DataTable();
            aprobados = cAcciones.ListadoAccionesAprob(Session["idFuente"].ToString());
            foreach (DataRow row in aprobados.Rows)
                if (row["aprobado"].ToString() == "2") totalAprobados += 1;
            if (aprobados.Rows.Count == totalAprobados)
                cQuejaReclamo.actualizarStatusFuente(int.Parse(Session["idFuente"].ToString()), 2);

            botonesTodos();            
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
                {
                    ValidarAccion(Session["Accion"].ToString());

                    mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));
                    mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_analista, "analista");

                    string fuente = cQuejaReclamo.nombreFuenteA(Session["Accion"].ToString());
                    string asunto = "Acción Asignada (" + Session["Accion"].ToString() + "), " + fuente;

                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, mAccionG.descripcion);

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción validada correctamente', '', 'success');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar acciones', '', 'warning');", true);                
            }
            catch
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
                    ListadoAcciones = cQuejaReclamo.ListadoAcciones(int.Parse(Session["idFuente"].ToString()), 0, "todos", 3);

                    foreach (DataRow Row in ListadoAcciones.Rows)
                        if (Row["aprobado"].ToString() != "2" && Row["aprobado"].ToString() != "-2")
                        {
                            ValidarAccion(Row[0].ToString());

                            mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Row[0].ToString()));
                            mEmpleado = cEmpleado.Obtner_Empleado(mAccionG.id_enlace, "enlace");

                            string fuente = cQuejaReclamo.nombreFuenteA(Row[0].ToString());
                            string asunto = "Acción Asignada (" + Row[0].ToString() + "), " + fuente;

                            if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, mAccionG.descripcion);
                        }

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acciones validadas correctamente', '', 'success');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Acciones', '', 'warning');", true);
            }
            catch { }
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
                mEmpleado = cEmpleado.Obtner_Empleado(0, "administrador");

                string fuente = cQuejaReclamo.nombreFuenteA(Session["Accion"].ToString());
                string asunto = "Rechazo de Acción (" + Session["Accion"].ToString() + "), " + fuente;

                RechazarAccion(Session["Accion"].ToString());               

                if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, txtRechazoAccion.Text);
                txtRechazoAccion.Text = "";

                actualizarListadoAcciones();

                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acción rechazada correctamente', '', 'error');", true);

                botonesTodos();
            }

            else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Informe', '', 'warning');", true);
        }

        protected void btnRechazarTodo_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
                {
                    DataTable ListadoAcciones = new DataTable();
                    ListadoAcciones = cQuejaReclamo.ListadoAcciones(int.Parse(Session["idFuente"].ToString()), 0, "todos", 3);

                    string asunto = "Rechazo de acciones (";
                    foreach (DataRow Row in ListadoAcciones.Rows)
                        if (Row["aprobado"].ToString() != "2" && Row["aprobado"].ToString() != "-2")
                        {
                            RechazarAccion(Row[0].ToString());
                            asunto += Row[0].ToString() + ", ";
                        }

                    mEmpleado = cEmpleado.Obtner_Empleado(0, "administrador");

                    string fuente = cQuejaReclamo.nombreFuenteA(Session["Accion"].ToString());
                    asunto = asunto.Remove(asunto.Length - 2);
                    asunto += "), " + fuente;

                    if (mEmpleado.email != null) cCorreo.enviarCorreo(mEmpleado.email, asunto, txtRechazoAccionTodo.Text);
                    txtRechazoAccionTodo.Text = "";

                    ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Acciones rechazadas correctamente', '', 'error');", true);
                }
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para rechazar Acciones', '', 'warning');", true);
            }
            catch { }            
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {

                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(Session["Accion"].ToString()));

                bool editada = false;

                mAccionesGeneradas ag = new mAccionesGeneradas();

                mAccionG.id_fuente = mQuejaReclamo.id_fuente;
                mAccionG.instalacion = txtInstalacion.Text;
                mAccionG.id_fadn = int.Parse(ddlFadn.SelectedValue);
                mAccionG.descripcion = txtDescripcion.Text;
                mAccionG.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                mAccionG.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                mAccionG.id_proceso = int.Parse(ddlProceso.SelectedValue);
                mAccionG.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);
                mAccionG.id_lider = int.Parse(ddlLider.SelectedValue);
                mAccionG.id_enlace = int.Parse(ddlEnlace.SelectedValue);

                ag.id_accion_generada = int.Parse(Session["fAccion"].ToString());
                ag.instalacion = txtInstalacion.Text;
                ag.id_fadn = int.Parse(ddlFadn.SelectedValue);
                ag.descripcion = txtDescripcion.Text;
                ag.id_lider = int.Parse(ddlLider.SelectedValue);
                ag.id_enlace = int.Parse(ddlEnlace.SelectedValue);
                ag.id_unidad = int.Parse(ddlUnidad.SelectedValue);
                ag.id_dependencia = int.Parse(ddlDependencia.SelectedValue);
                ag.id_proceso = int.Parse(ddlProceso.SelectedValue);
                ag.id_tipo_accion = int.Parse(dllTipoAccion.SelectedValue);

                editada = cAcciones.actualizar_Accion(ag);
                cAcciones.aprobar_Accion(ag.id_accion_generada, 0);
                verColumnas(true);
                mQuejaReclamo = cQuejaReclamo.BuscarEncabezado(txtInforme.Text, int.Parse(txtanio.Text), "3");
                gvListadoAcciones.DataSource = cQuejaReclamo.ListadoAcciones(mQuejaReclamo.id_fuente, 0, "todos", 3);
                gvListadoAcciones.DataBind();
                verColumnas(false);

                if (editada) ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('La Acción ha sido actualizada correctamente', '', 'success');", true);
                else ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No fue posible actualizar Acción', 'Intente de nuevo', 'error');", true);

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