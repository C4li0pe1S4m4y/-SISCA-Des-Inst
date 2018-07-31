using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mAccionesGeneradas
    {
        public int id_accion_generada { get; set; }
        public int correlativo_hallazgo { get; set; }
        public string norma { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public int id_status { get; set; }
        public int id_fuente { get; set; }
        public int id_analista { get; set; }
        public int id_lider { get; set; }
        public int id_enlace { get; set; }
        public int id_unidad { get; set; }
        public int id_dependencia { get; set; }
        public int id_ccl_accion_generada { get; set; }
        public int id_proceso { get; set; }
        public int id_tipo_accion { get; set; }
        public int id_accion_anual { get; set; }
        public int aprobado { get; set; }
        public int id_fadn { get; set; }
        public int id_periodo { get; set; }
        public string instalacion { get; set; }
        public int correlativo_compromiso { get; set; }
    }
}
