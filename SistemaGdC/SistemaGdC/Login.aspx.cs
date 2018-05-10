using Controladores;
using System;
using System.Web.Security;
using Modelos;



namespace SistemaGdC
{
    public partial class Login : System.Web.UI.Page
    {
        cUsuarios ContUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtusuario.Focus();
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {


            ContUsuario = new cUsuarios();
            mUsuario objUsuario = new mUsuario();
            ContUsuario.login(txtusuario.Text, txtpassword.Text);
            if (ContUsuario.login(txtusuario.Text, txtpassword.Text))
            {

                objUsuario = ContUsuario.Obtner_Usuario(txtusuario.Text);

                //redirect
                this.Session["Usuario"] = txtusuario.Text;
                this.Session["id_empleado"] = objUsuario.id_empleado;
                this.Session["id_tipo_usuario"] = objUsuario.id_tipo_usuario;
                FormsAuthentication.RedirectFromLoginPage(this.txtusuario.Text, false);
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                lblError.Text = "Usuario o Contraseña Incorrectos";
            }
        }
    }
}