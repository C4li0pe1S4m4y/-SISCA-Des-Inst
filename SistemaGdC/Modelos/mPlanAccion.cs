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
        public int id_accion_generada { get; set; }
        public int id_status { get; set; }
    }
}
