using System;
using Controladores;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SistemaGdC
{
    public partial class SiteMaster : MasterPage
    {
        cMenu obMenu;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblUsuario.Text = Session["Usuario"].ToString().ToLower();
                if (!Page.IsPostBack)
                {
                    llenarMenu();
                }
            }
            catch
            {
                Response.Redirect("~/Login");
            }
                     
        }

        public void llenarMenu()
        {
            DataTable dtMenuItems = new DataTable();
            obMenu = new cMenu();
            dtMenuItems = obMenu.LlenarDoctosUsuarios(Session["id_tipo_usuario"].ToString());

            foreach (DataRow drMenuItem in dtMenuItems.Rows)
            {
                if (drMenuItem["id_menu"].Equals(drMenuItem["PadreId"]))
                {
                    MenuItem mnuMenuItem = new MenuItem();

                    mnuMenuItem.Value = drMenuItem["id_menu"].ToString();
                    mnuMenuItem.Text = drMenuItem["descripcion"].ToString();
                    //mnuMenuItem.NavigateUrl = drMenuItem["Url"].ToString();
                    MenuP.Items.Add(mnuMenuItem);
                 
                    agregarMenuItem(mnuMenuItem, dtMenuItems);
                }
            }
        }

        public void agregarMenuItem(MenuItem mnuMenuItem, DataTable dtMenuItems)
        {
            foreach (DataRow drMenuItem in dtMenuItems.Rows)
            {
                if ((drMenuItem["PadreId"].ToString().Equals(mnuMenuItem.Value)) && !(drMenuItem["id_menu"].Equals(drMenuItem["PadreId"])))
                {
                    MenuItem mnuNewMenuItem = new MenuItem();

                    mnuNewMenuItem.Value = drMenuItem["id_menu"].ToString();
                    mnuNewMenuItem.Text = drMenuItem["descripcion"].ToString();
                    mnuNewMenuItem.NavigateUrl = drMenuItem["Url"].ToString();
                    
                    mnuMenuItem.ChildItems.Add(mnuNewMenuItem);
                    agregarMenuItem(mnuNewMenuItem, dtMenuItems);
                }
            }
        }
    }
}