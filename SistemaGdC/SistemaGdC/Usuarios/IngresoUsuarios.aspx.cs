using Controladores;
using Modelos;
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
        cGeneral cgDatos = new cGeneral();
        cUsuarios cUsuarios = new cUsuarios();

        mUsuario mUsuario = new mUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //txtNombre.Text = "";
                limpiarCampos();
            }
        }

        void limpiarCampos()
        {
            txtUsuario.Text = "";
            ddlEmpleado.ClearSelection();
            ddlEmpleado.Items.Clear();
            ddlEmpleado.AppendDataBoundItems = true;
            ddlEmpleado.Items.Add("<< Elija Empleado >>");
            ddlEmpleado.Items[0].Value = "0";
            ddlEmpleado.DataSource = cgDatos.dropEmpleados();
            ddlEmpleado.DataTextField = "texto";
            ddlEmpleado.DataValueField = "id";
            ddlEmpleado.DataBind();

            ddlTipoUsuario.ClearSelection();
            ddlTipoUsuario.Items.Clear();
            ddlTipoUsuario.AppendDataBoundItems = true;
            ddlTipoUsuario.Items.Add("<< Elita Tipo de Usuario >>");
            ddlTipoUsuario.Items[0].Value = "0";
            ddlTipoUsuario.DataSource = cUsuarios.dropTipoUsuario();
            ddlTipoUsuario.DataTextField = "nombre";
            ddlTipoUsuario.DataValueField = "id";
            ddlTipoUsuario.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //cUsuarios = new cUsuarios();
            if (ddlEmpleado.SelectedValue !="0")
            {
                if (txtUsuario.Text !="")
                {
                    mUsuario.usuario = txtUsuario.Text;
                    mUsuario.id_empleado = int.Parse(ddlEmpleado.SelectedValue);
                    mUsuario.id_tipo_usuario = int.Parse(ddlTipoUsuario.SelectedValue);
                    mUsuario.correo = txtCorreo.Text;

                    // objUsuarios = new cUsuarios();
                    if (cUsuarios.IngresoNuevoUsuario(mUsuario, txtcontra.Text))
                    {
                        limpiarCampos();
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Usuario ingresado exitosamente!', '', 'success');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "Mensaje", "swal('Ya existe usuario', 'Por favor intente con otro nombre de usuario', 'warning');", true);
                    }                    
                }
            }
        }
    }
}