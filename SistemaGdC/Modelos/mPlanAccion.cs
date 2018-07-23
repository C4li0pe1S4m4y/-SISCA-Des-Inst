using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mPlanAccion
    {
        public int id_plan { get; set; }
        public string tecnica_analisis { get; set; }
        public string causa_raiz { get; set; }
        public string usuario_ingreso { get; set; }
        public string fecha_ingreso { get; set; }
        public string fecha_recepcion { get; set; }
        public int id_accion_generada { get; set; }
        public int id_status { get; set; }
        public int no_ampliacion { get; set; }
        public string inicio_actividades { get; set; }
        public string final_actividades { get; set; }
    }
}
