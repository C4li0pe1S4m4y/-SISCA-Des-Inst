﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Controladores;
using Modelos;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaGdC
{
    public partial class _Default : System.Web.UI.Page
    {
        cGeneral cGen = new cGeneral();
        cDashboard dasboard = new cDashboard();
        cPlanAcion cPlanAccion = new cPlanAcion();
        cInformeCO cInformeCO = new cInformeCO();
        cInformeOM cInformeOM = new cInformeOM();

        mInformeCO mInformeCO = new mInformeCO();
        mInformeOM mInformeOM = new mInformeOM();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Thread informesVencidos = new Thread(cInformeOM.buscarIOMvencidos);
                    informesVencidos.Start();


                    gvListadoPlanes.DataSource = cPlanAccion.ListadoPlanesAccion("todos");
                    gvListadoInformesCO.DataSource = cInformeCO.ListadoInformesCO("todos");
                    gvListadoInformesOM.DataSource = cInformeOM.ListadoInformesOM("todos");
                    gvListadoPlanes.DataBind();
                    gvListadoInformesCO.DataBind();
                    gvListadoInformesOM.DataBind();
                    gvListadoPlanes.Columns[0].Visible = false;
                    gvListadoInformesCO.Columns[0].Visible = false;
                    gvListadoInformesOM.Columns[0].Visible = false;

                    lblUser.Text = Session["Usuario"].ToString().ToLower();

                    foreach (DataRow dr in dasboard.graficaPlanesAccionAbiertos().Rows)
                        checkboxPAccionAbiertos.Items.Add(new ListItem(dr[0].ToString()));

                    foreach (DataRow dr in dasboard.graficaPlanesAccion().Rows)
                        checkboxPAccion.Items.Add(new ListItem(dr[0].ToString()));

                    foreach (ListItem itemActual in checkboxPAccionAbiertos.Items)
                        itemActual.Selected = true;

                    foreach (ListItem itemActual in checkboxPAccion.Items)
                        itemActual.Selected = true;

                    checkboxPAccionAbiertos.DataBind();
                    checkboxPAccion.DataBind();
                }
                catch
                {
                    Response.Redirect("~/login.aspx");
                }

            }
        }

        protected void buscarInformesVencidos()
        {
            cInformeOM.buscarIOMvencidos();
        }

        protected string graficaAcciones()
        {
            string usuarioN = Session["Usuario"].ToString().ToLower();

            DataTable Datos = dasboard.GraficaAcciones();

            string strDatos;
            strDatos = "[['Tipo','Total'],";
            foreach (DataRow dr in Datos.Rows)
            {
                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + " (" + dr[1] + ")'" + "," + dr[1];
                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";
            return strDatos;
        }

        protected string graficaPlanesAccion()
        {
            DataTable Datos = dasboard.graficaPlanesAccion();

            string strDatos;
            //strDatos = "[['Unidad','Abierta',{ role: 'annotation' },'Cerrada',{ role: 'annotation' },'Cerrada por Transición',{ role: 'annotation' } ],";
            strDatos = "[['Unidad','Abierta',{ role: 'annotation' },'Cerrada',{ role: 'annotation' } ],";
            foreach (DataRow dr in Datos.Rows)
            {
                bool quitar = false;
                foreach (ListItem itemActual in checkboxPAccion.Items)
                {
                    if ((itemActual.Selected == false) &&
                        (itemActual.Value == dr[0].ToString()))
                    {
                        quitar = true;
                        continue;
                    }
                }
                if (quitar) continue;

                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + "'" + ",";

                //for(int i=1;i<=3;i++)
                for (int i = 1; i <= 2; i++)
                {
                    if (dr[i].ToString() == "0") strDatos = strDatos + dr[i] + ",'',";
                    else strDatos = strDatos + dr[i] + ",'" + dr[i] + "',";
                }

                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";
            return strDatos;
        }

        protected string graficaPlanesAccionAbiertos()
        {
            DataTable Datos = dasboard.graficaPlanesAccionAbiertos();

            string strDatos;
            strDatos = "[['Unidad','Fuera de tiempo','En tiempo'],";
            foreach (DataRow dr in Datos.Rows)
            {
                bool quitar = false;
                foreach (ListItem itemActual in checkboxPAccionAbiertos.Items)
                {
                    if ((itemActual.Selected == false) &&
                        (itemActual.Value == dr[0].ToString()))
                    {
                        quitar = true;
                        continue;
                    }
                }
                if (quitar) continue;

                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + "'" + "," + dr[1] + "," + dr[2];
                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";
            return strDatos;
        }

        protected string graficaAccionesInformes()
        {
            DataTable nDatos = dasboard.graficaAccionesInformes();

            string strDatos;
            strDatos = "[";
            foreach (DataRow dr in nDatos.Rows)
            {
                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + "'" + "," + dr[1] + "," + dr[2];
                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";
            return strDatos;
        }

        protected void gvListadoPlanes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListadoPlanes.Columns[0].Visible = true;

            //this.Session["pagina"] = e.NewPageIndex;
            gvListadoPlanes.PageIndex = e.NewPageIndex;

            gvListadoPlanes.DataSource = cPlanAccion.ListadoPlanesAccion("todos");
            gvListadoPlanes.DataBind();

            gvListadoPlanes.Columns[0].Visible = false; /////////
        }

        protected void gvListadoPlanes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Session["noAccion"] = int.Parse(gvListadoPlanes.SelectedValue.ToString());
            Response.Redirect("~/Visualizar/VerPlanAccion.aspx");
        }

        protected void gvListadoPlanes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[0].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string valor = DataBinder.Eval(e.Row.DataItem, "Progreso").ToString();
                HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("progbarP");
                if (int.Parse(valor) < 45) div.Attributes["class"] = "progress-bar progress-bar-danger";
                else if (int.Parse(valor) < 85) div.Attributes["class"] = "progress-bar progress-bar-warning";
                else div.Attributes["class"] = "progress-bar progress-bar-success";

                valor = valor + "%";
                div.Style.Add("width", valor);
                div.InnerText = valor;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected void gvListadoInformesCO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListadoInformesCO.Columns[0].Visible = true;

            //this.Session["pagina"] = e.NewPageIndex;
            gvListadoInformesCO.PageIndex = e.NewPageIndex;

            gvListadoInformesCO.DataSource = cInformeCO.ListadoInformesCO("todos");
            gvListadoInformesCO.DataBind();

            gvListadoInformesCO.Columns[0].Visible = false; /////////
        }

        protected void gvListadoInformesCO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string valor = DataBinder.Eval(e.Row.DataItem, "Progreso").ToString();
                if (int.Parse(valor) < 0) valor = "10";
                HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("progbarICO");
                if (int.Parse(valor) < 45) div.Attributes["class"] = "progress-bar progress-bar-danger";
                else if (int.Parse(valor) < 85) div.Attributes["class"] = "progress-bar progress-bar-warning";
                else div.Attributes["class"] = "progress-bar progress-bar-success";

                valor = valor + "%";
                div.Style.Add("width", valor);
                div.InnerText = valor;
            }
        }

        protected void gvListadoInformesCO_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Session["noAccion"] = int.Parse(gvListadoInformesCO.SelectedValue.ToString());
            Response.Redirect("~/Visualizar/VerInformeCO.aspx");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected void gvListadoInformesOM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListadoInformesOM.Columns[0].Visible = true;

            //this.Session["pagina"] = e.NewPageIndex;
            gvListadoInformesOM.PageIndex = e.NewPageIndex;

            gvListadoInformesOM.DataSource = cInformeOM.ListadoInformesOM("todos");
            gvListadoInformesOM.DataBind();

            gvListadoInformesOM.Columns[0].Visible = false; /////////
        }

        protected void gvListadoInformesOM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string valor = DataBinder.Eval(e.Row.DataItem, "Progreso").ToString();
                if (int.Parse(valor) < 0) valor = "10";
                HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("progbarIOM");
                if (int.Parse(valor) < 45) div.Attributes["class"] = "progress-bar progress-bar-danger";
                else if (int.Parse(valor) < 85) div.Attributes["class"] = "progress-bar progress-bar-warning";
                else div.Attributes["class"] = "progress-bar progress-bar-success";

                valor = valor + "%";
                div.Style.Add("width", valor);
                div.InnerText = valor;
            }
        }

        protected void gvListadoInformesOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Session["noAccion"] = int.Parse(gvListadoInformesOM.SelectedValue.ToString());
            Response.Redirect("~/Visualizar/VerInformeOM.aspx");
        }

        protected void actualizarCkeckbox_Click(object sender, EventArgs e)
        {
            checkboxPAccion.DataBind();
            checkboxPAccionAbiertos.DataBind();


            /*string script = @"<script type='text/javascript'>
                            drawChart3();
                            </script>";*/

            ScriptManager.RegisterStartupScript(this, typeof(string), "grafica", "drawChart3();", true);
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "grafica", script, true);
        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt = cGen.informacionGeneral();
            ExportToExcel(dt);
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
    }
}

