using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.InformeResultados.Acciones
{
    public partial class ListadoAcciones : System.Web.UI.Page
    {
        //cInformeResultados cResultados;
        cFuente cFuente = new cFuente();
        cAcciones cAcciones = new cAcciones();
        cEmpleado cEmpleado = new cEmpleado();
        //cDashboard dasboard = new cDashboard();
        cGeneral cGen = new cGeneral();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                lblAcciones.InnerText = "control de acciones";

                this.Session["pagina"] = 0;
                panel1.Visible = false;

                
                gvListadoAcciones.DataSource = cAcciones.ListadoAcciones(int.Parse(Session["id_empleado"].ToString()),"2", "accionesEnlace");
                gvListadoAcciones.DataBind();
                ddlAccionGenerada.ClearSelection();
                ddlAccionGenerada.Items.Clear();
                ddlAccionGenerada.AppendDataBoundItems = true;
                ddlAccionGenerada.Items.Add("<< Elija Accion >>");
                ddlAccionGenerada.Items[0].Value = "0";

                ddlAccionGenerada.DataSource = cFuente.dropAcciones();
                ddlAccionGenerada.DataTextField = "Accion";
                ddlAccionGenerada.DataValueField = "id_acciones";
                ddlAccionGenerada.DataBind();
                cAcciones.dropProceso(ddlProceso);
                cAcciones.dropUnidad(ddlUnidad);
                cAcciones.dropTipoAccion(dllTipoAccion);               
                pn1.Visible = true;
            }
        }

        protected void btnAccion_Click(object sender, EventArgs e) 
        {            
            Response.Redirect("~/InformeResultados/PlanAccion.aspx");
        }

        protected void btnInformeCO_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/InformeResultados/InformeCO.aspx");            
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
                cEmpleado.dllEmpleado(ddlEnlace, idUnidad);
                cEmpleado.dllEmpleado(ddlLider, idUnidad);
                cAcciones.dllDependencia(ddlDependencia, idUnidad);
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
                mAccionG = cAcciones.Obtner_AccionGenerada(int.Parse(selectedRow.Cells[0].Text));

                Session["noAccion"]= selectedRow.Cells[0].Text;
                Session["noHallazgo"] = mAccionG.correlativo_hallazgo.ToString();

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

                int statusA = mAccionG.id_status;

                if (mAccionG.id_status == 1|| mAccionG.id_status == -1) dllTipoAccion.Enabled = false;
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