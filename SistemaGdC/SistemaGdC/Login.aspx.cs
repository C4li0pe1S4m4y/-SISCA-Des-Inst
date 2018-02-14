using Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC
{
    public partial class Login : System.Web.UI.Page
    {
        cUsuarios ContralodrUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtusuario.Focus();
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            ContralodrUsuario = new cUsuarios();
            ContralodrUsuario.login(txtusuario.Text, txtpassword.Text);
            if (ContralodrUsuario.login(txtusuario.Text, txtpassword.Text))
            {
                //redirect
                this.Session["Usuario"] = txtusuario.Text;
                FormsAuthentication.RedirectFromLoginPage(this.txtusuario.Text, false);
                Response.Redirect("~/default.aspx");
            }
            else
            {
                lblError.Text = "Usuario o Contraña Incorectos";
            }
        }
    }
}