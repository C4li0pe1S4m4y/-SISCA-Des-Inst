using Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.InformeResultados
{
    public partial class ListadoAcciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cInformeResultados informe = new cInformeResultados();
            if (!IsPostBack)
            {
                gvListadoAcciones.DataSource = informe.ListadoAcciones(Convert.ToInt16(Request.QueryString["idInforme"]));
                gvListadoAcciones.DataBind();
            }
           
        }
    }
}