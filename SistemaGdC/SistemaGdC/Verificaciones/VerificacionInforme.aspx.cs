using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace SistemaGdC.Verificaciones
{
    public partial class VerificacionInforme : System.Web.UI.Page
    {
        cInformeEI cResultados = new cInformeEI();
        cGeneral cGen;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                panel2.Visible = false;
                panel3.Visible = false;

                this.Session["pagina1"] = 0;
                this.Session["pagina2"] = 0;
                gvListadoInformes.Columns[0].Visible = true;
                gvListadoInformes.DataSource = cResultados.ListadoInformes(1);
                gvListadoInformes.DataBind();
                gvListadoInformes.Columns[0].Visible = false;
                

                cResultados = new cInformeEI();
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
                cResultados.dllDependencia(ddlDependencia, idUnidad);
            }
        }

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                mAccionesGeneradas mAccionG = new mAccionesGeneradas();

                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina2"]);
                int psize = gvListadoAcciones.PageSize;

                //string prueba = gvListadoAcciones.Rows[index].Cells[0].Text;


                GridViewRow selectedRow = gvListadoAcciones.Rows[index - (pag * psize)];
                string prueba = selectedRow.Cells[0].Text;

                //mAccionG = cResultados.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));
                mAccionG = cResultados.Obtner_AccionGenerada((int.Parse(gvListadoAcciones.Rows[index - (pag * psize)].Cells[0].Text)));

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
            }
        }


        protected void OnPaging(Object sender, GridViewPageEventArgs e)
        {
            gvListadoInformes.PageIndex = e.NewPageIndex;
            gvListadoInformes.Columns[0].Visible = true;
            gvListadoInformes.DataSource = cResultados.ListadoInformes(1);
            gvListadoInformes.DataBind();
            gvListadoInformes.Columns[0].Visible = false;
        }

        protected void gvListadoInformes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina1"] = e.NewPageIndex;
            gvListadoInformes.PageIndex = e.NewPageIndex;
            gvListadoInformes.Columns[0].Visible = true;
            gvListadoInformes.DataSource = cResultados.ListadoInformes(1);
            gvListadoInformes.DataBind();
            gvListadoInformes.Columns[0].Visible = false;
        }

        protected void gvListadoInformes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Revisar")
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = true;

                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina1"]);
                int psize = gvListadoInformes.PageSize;

                GridViewRow selectedRow = gvListadoInformes.Rows[index-(pag*psize)];

                lblCorrelativo.Text = selectedRow.Cells[0].Text;
                txtanio.Text = selectedRow.Cells[2].Text;
                txtInforme.Text = selectedRow.Cells[3].Text;
                DateTime fecha = DateTime.Parse(selectedRow.Cells[4].Text);             
                    txtFechaInforme.Text = fecha.ToString("yyyy-MM-dd");

                gvListadoAcciones.Columns[0].Visible = true;
                gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(selectedRow.Cells[2].Text), int.Parse(selectedRow.Cells[3].Text),0);                
                gvListadoAcciones.DataBind();
                gvListadoAcciones.Columns[0].Visible = false;
                //string dato = selectedRow.Cells[2].Text + "-" + selectedRow.Cells[3].Text;
            }
        }

        protected void gvListadoAcciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina2"] = e.NewPageIndex;
            gvListadoAcciones.PageIndex = e.NewPageIndex;
            gvListadoAcciones.Columns[0].Visible = true;
            gvListadoAcciones.DataSource = cResultados.ListadoAcciones(int.Parse(txtanio.Text), int.Parse(txtInforme.Text),0);
            gvListadoAcciones.DataBind();
            gvListadoAcciones.Columns[0].Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {       
            if(int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;

                cResultados.actualizarInforme(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), 2);
                gvListadoInformes.Columns[0].Visible = true;
                gvListadoInformes.DataSource = cResultados.ListadoInformes(1);
                gvListadoInformes.DataBind();
                gvListadoInformes.Columns[0].Visible = false;
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('No tiene permisos para validar Informe', '', 'warning');", true);
            }
                      
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["id_tipo_usuario"].ToString()) == 1)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;

                cResultados.actualizarInforme(int.Parse(txtanio.Text), int.Parse(txtInforme.Text), -2);
                gvListadoInformes.Columns[0].Visible = true;
                gvListadoInformes.DataSource = cResultados.ListadoInformes(1);
                gvListadoInformes.DataBind();
                gvListadoInformes.Columns[0].Visible = false;
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No tiene permisos para rechazar Informe');", true);
            }

        }
    }
}