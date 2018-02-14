using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mAccionesRealizar
    {
        public int id_accion_reallizar { get; set; }
        public int id_plan { get; set; }
        public string accion { get; set; }
        public string responsable { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public string observaciones { get; set; }

    }
}
