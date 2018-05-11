using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Acciones
{
    public partial class ListadoAcciones : System.Web.UI.Page
    {
        //cInformeResultados cResultados;
        cInformeEI cResultados = new cInformeEI();
        cAcciones cAcciones = new cAcciones();
        //cDashboard dasboard = new cDashboard();
        cGeneral cGen = new cGeneral();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                this.Session["pagina"] = 0;
                panel1.Visible = false;

                
                gvListadoAcciones.DataSource = cAcciones.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()),"2", "accionesEnlace");
                gvListadoAcciones.DataBind();
                //txtanio.Text = DateTime.Today.ToString("yyyy");
                //cResultados = new cInformeResultados();
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
                //cGen = new cGeneral();
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

        protected void btnAccion_Click(object sender, EventArgs e) 
        {            
            Response.Redirect("~/InformeResultados/PlanAccion.aspx");
        }

        protected void btnInformeCO_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/InformeResultados/InformeCorrecion.aspx");            
        }

        protected void btnInformeOM_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InformeResultados/InformeOM.aspx");
        }

        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_unidad = ddlUnidad.SelectedItem.Value;

            int idUnidad = 0;
            int.TryParse(ddlUnidad.SelectedValue, out idUnidad);
            if (idUnidad > 0)
            {
                //cResultados = new cInformeResultados();
                cResultados.dllDependencia(ddlDependencia, idUnidad);
            }
        }    

        protected void gvListadoAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                panel1.Visible = true;
                btnActualizar.Visible = false;

                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina"]);
                int psize = gvListadoAcciones.PageSize;

                GridViewRow selectedRow = gvListadoAcciones.Rows[index - (pag * psize)];
                mAccionesGeneradas mAccionG = new mAccionesGeneradas();
                mAccionG = cResultados.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                Session["noAccion"]= selectedRow.Cells[0].Text;
                Session["noHallazgo"] = mAccionG.correlativo_hallazgo.ToString();

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

                int statusA = mAccionG.id_status;

                if (mAccionG.id_status == 1) dllTipoAccion.Enabled = false;
                else dllTipoAccion.Enabled = true;

                switch (int.Parse(mAccionG.id_tipo_accion.ToString()))
                {
                    case 1:
                    case 2:
                        btnAccion.Visible = true;
                        btnInformeCO.Visible = false;
                        btnInformeOM.Visible = false;
                        break;

                    case 4:
                    case 5:
                        btnAccion.Visible = false;
                        btnInformeCO.Visible = true;
                        btnInformeOM.Visible = true;
                        break;
                }
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

            gvListadoAcciones.DataSource = cAcciones.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", "accionesEnlace");
            gvListadoAcciones.DataBind();
        }

        protected void dllTipoAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAccion.Visible = false;
            btnInformeCO.Visible = false;
            btnInformeOM.Visible = false;
            btnActualizar.Visible = true;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            cAcciones.actualizarTipoAccion(int.Parse(Session["noAccion"].ToString()), int.Parse(dllTipoAccion.SelectedValue));
            
            gvListadoAcciones.DataSource = cAcciones.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()), "2", "accionesEnlace");
            gvListadoAcciones.DataBind();
            btnActualizar.Visible = false;

            switch (int.Parse(dllTipoAccion.SelectedValue))
            {
                case 1:
                case 2:
                    btnAccion.Visible = true;
                    btnInformeCO.Visible = false;
                    btnInformeOM.Visible = false;
                    break;

                case 4:
                case 5:
                    btnAccion.Visible = false;
                    btnInformeCO.Visible = true;
                    btnInformeOM.Visible = true;
                    break;
            }
        }
    }
}