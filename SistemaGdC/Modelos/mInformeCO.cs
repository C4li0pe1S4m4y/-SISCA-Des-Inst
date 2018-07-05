using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mInformeCO
    {
        public int id_informe_correccion { get; set; }
        public string estado { get; set; }
        public string descripcion_accion { get; set; }
        public string descripcion_evidencia { get; set; }
        public string fecha { get; set; }
        public int id_accion_generada { get; set; }
        public int id_status { get; set; }
    }
}
