using Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.Usuarios
{
    public partial class IngresoUsuarios : System.Web.UI.Page
    {
        cGeneral cgDatos;
        cUsuarios objUsuarios;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNombre.Text = "";
                txtUsuario.Text = "";
                ddlEmpleado.ClearSelection();
                ddlEmpleado.Items.Clear();
                ddlEmpleado.AppendDataBoundItems = true;
                ddlEmpleado.Items.Add("<< Elija Empleado >>");
                ddlEmpleado.Items[0].Value = "0";
                cgDatos = new  cGeneral();
                ddlEmpleado.DataSource = cgDatos.dropEmpleados();
                ddlEmpleado.DataTextField = "texto";
                ddlEmpleado.DataValueField = "id";
                ddlEmpleado.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            objUsuarios = new cUsuarios();
            if (txtNombre.Text !="")
            {
                if (txtUsuario.Text !="")
                {
                    objUsuarios = new cUsuarios();
                   if( objUsuarios.IngresoNuevoUsuario(txtUsuario.Text, txtcontra.Text, ddlEmpleado.SelectedValue))
                    {
                        lblResultado.Visible = true;
                        lblResultado.Text = "Ingresado Correctamente";
                        txtNombre.Text = "";
                        txtUsuario.Text = "";
                        ddlEmpleado.Items[0].Value = "0";
                    }
                    else
                    {
                        lblResultado.Visible = true;
                        lblResultado.Text = "Error al inseretar";
                    }
                    
                }
            }
        }
    }
}