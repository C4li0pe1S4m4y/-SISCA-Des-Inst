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
        public int id_accion { get; set; }
        public string tecnica_analisis { get; set; }
        public string causa_raiz { get; set; }
        public int id_lider { get; set; }
        public string usuario_ing { get; set; }
        public string fecha { get; set; }
    }
}
