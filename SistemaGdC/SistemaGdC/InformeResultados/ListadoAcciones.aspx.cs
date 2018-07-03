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
            cFuente informe = new cFuente();
            if (!IsPostBack)
            {
                string fullname2 = Request["Correlativo"];
                //Response.Redirect("~/InformeResultados/ListadoAcciones.aspx?idInforme=" + lblCorr.Text);
            }
           
        }
    }
}