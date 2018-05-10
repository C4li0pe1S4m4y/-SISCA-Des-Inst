﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Controladores;

namespace SistemaGdC
{
    public partial class _Default : System.Web.UI.Page
    {
        cDashboard dasboard = new cDashboard();
        cPlanAcion cPlanAccion = new cPlanAcion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //lblUser.Text = Session["Usuario"].ToString().ToLower();
                    gvListadoPlanes.DataSource = cPlanAccion.ListadoPlanesAccion("todos");                    
                    gvListadoPlanes.DataBind();
                    gvListadoPlanes.Columns[0].Visible = false;

                    lblUser.Text = Session["Usuario"].ToString().ToLower();
                    //lblId.Text = Session["idUsuario"].ToString();

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

        

        protected void gvListadoAcciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string valor = DataBinder.Eval(e.Row.DataItem, "Progreso").ToString();
                HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("progbarA");
                if (int.Parse(valor) < 45) div.Attributes["class"] = "progress-bar progress-bar-danger";
                else if (int.Parse(valor) < 85) div.Attributes["class"] = "progress-bar progress-bar-warning";
                else div.Attributes["class"] = "progress-bar progress-bar-success";

                valor = valor + "%";
                div.Style.Add("width", valor);
                div.InnerText = valor;
            }
        }

        

        /*protected void gvListadoAcciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string valor = DataBinder.Eval(e.Row.DataItem, "Progreso").ToString();
                HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("progbarA");
                if (int.Parse(valor) < 30) div.Attributes["class"] = "progress-bar progress-bar-danger";
                else if (int.Parse(valor) < 60) div.Attributes["class"] = "progress-bar progress-bar-warning";
                else div.Attributes["class"] = "progress-bar progress-bar-success";

                valor = valor + "%";
                div.Style.Add("width", valor);
                div.InnerText = valor;
            }
        }*/

        protected void gvListadoInformes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string valor = DataBinder.Eval(e.Row.DataItem, "Progreso").ToString();
                HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("progbarI");
                if (int.Parse(valor) < 45) div.Attributes["class"] = "progress-bar progress-bar-danger";
                else if (int.Parse(valor) < 85) div.Attributes["class"] = "progress-bar progress-bar-warning";
                else div.Attributes["class"] = "progress-bar progress-bar-success";

                valor = valor + "%";
                div.Style.Add("width", valor);
                div.InnerText = valor;
            }
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
            strDatos = "[['Unidad','Abierta',{ role: 'annotation' },'Cerrada',{ role: 'annotation' },'Cerrada por Transición',{ role: 'annotation' } ],";
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

                for(int i=1;i<=3;i++)
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
                     
            this.Session["pagina"] = e.NewPageIndex;
            gvListadoPlanes.PageIndex = e.NewPageIndex;

            gvListadoPlanes.DataSource = cPlanAccion.ListadoPlanesAccion("todos");
            gvListadoPlanes.DataBind();

            gvListadoPlanes.Columns[0].Visible = false;
        }

        protected void gvListadoPlanes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = int.Parse(gvListadoPlanes.SelectedValue.ToString());
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
    }
}

