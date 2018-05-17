using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mAccionesRealizar
    {
        public int id_accion_realizar { get; set; }
        public int id_plan { get; set; }
        public string accion { get; set; }
        public string responsable { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string observaciones { get; set; }
        public int id_status { get; set; }

    }
}
