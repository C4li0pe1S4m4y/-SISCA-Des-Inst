using Controladores;
using Modelos;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaGdC.DataSet;
using SistemaGdC.Reportes;
using MySql.Data.MySqlClient;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using System.IO;

namespace SistemaGdC.InformeResultados.Acciones
{
    public partial class ListadoUsuarios : System.Web.UI.Page
    {
        //cInformeResultados cResultados;
        cFuente cFuente = new cFuente();
        cAcciones cAcciones = new cAcciones();
        cEmpleado cEmpleado = new cEmpleado();
        cUsuarios cUsuarios = new cUsuarios();
        //cDashboard dasboard = new cDashboard();
        cGeneral cGen = new cGeneral();
        DBConexion conectar = new DBConexion();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {




                this.Session["pagina"] = 0;

                gvListadoUsuarios.DataSource = cUsuarios.ListadoUsuarios("todos");
                gvListadoUsuarios.DataBind();
            }
        }



        protected void gvListadoUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                int index = Convert.ToInt16(e.CommandArgument);
                int pag = Convert.ToInt16(Session["pagina"]);
                int psize = gvListadoUsuarios.PageSize;

                GridViewRow selectedRow = gvListadoUsuarios.Rows[index - (pag * psize)];
                mUsuario mUsuario = new mUsuario();
                mUsuario = cUsuarios.Obtner_UsuarioID(int.Parse(selectedRow.Cells[0].Text));


            }
        }

        protected void gvListadoUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        protected void gvListadoUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.Session["pagina"] = e.NewPageIndex;
            gvListadoUsuarios.PageIndex = e.NewPageIndex;

            gvListadoUsuarios.DataSource = cUsuarios.ListadoUsuarios("todos");
            gvListadoUsuarios.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        public void ExportToExcel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string filename = "Reporte.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                dgGrid.RenderControl(hw);

                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = cGen.informacionGeneral();
            ExportToExcel(dt);
        }
    }
}