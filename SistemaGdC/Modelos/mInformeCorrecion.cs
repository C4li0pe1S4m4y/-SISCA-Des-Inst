using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mInformeCorrecion
    {
        public int id_correlativo { get; set; }
        public int anio_inicio { get; set; }
        public string no_informe { get; set; }
        public string fecha_informe { get; set; }
        public int cantidad_acciones { get; set; }
        public int acciones_finalizadas { get; set; }
    }
}
