using Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaGdC.InformeResultados
{
    public partial class IngresoFuente : System.Web.UI.Page
    {
        cInformeResultados cResultados;
        cGeneral cGen;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cResultados = new cInformeResultados();
                cResultados.dropProceso(ddlProceso);
                cResultados.dropUnidad(ddlUnidad);
                cResultados.dropOpcionIngreso(ddlOpcion);
            }
        }

        protected void ddlOpcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (int.Parse(ddlOpcion.SelectedValue))
            {
                case 1:
                    pSatisfaccionCliente.Visible = false;
                    pRevAltaDirec.Visible = false;
                    pSNC.Visible = false;
                    pQuejas.Visible = false;
                    break;
                case 2:
                    pSatisfaccionCliente.Visible = false;
                    pRevAltaDirec.Visible = false;
                    pSNC.Visible = false;
                    pQuejas.Visible = false;
                    break;
                case 3:
                    pSatisfaccionCliente.Visible = true;
                    pRevAltaDirec.Visible = false;
                    pSNC.Visible = false;
                    pQuejas.Visible = false;
                    break;
                case 4 :
                    pSatisfaccionCliente.Visible = false;
                    pRevAltaDirec.Visible = true;
                    pSNC.Visible = false;
                    pQuejas.Visible = false;
                    break;
                case 5:
                    pSatisfaccionCliente.Visible = false;
                    pRevAltaDirec.Visible = false;
                    pSNC.Visible = true;
                    pQuejas.Visible = false;
                    break;
                case 6:
                    pSatisfaccionCliente.Visible = false;
                    pRevAltaDirec.Visible = false;
                    pSNC.Visible = false;
                    pQuejas.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
}