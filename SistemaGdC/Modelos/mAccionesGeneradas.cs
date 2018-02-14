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
        public int id_informe { get; set; }
        public int id_accion { get; set; }
        public int no_correlativo_accion { get; set; }
        public string norma { get; set; }
        public int id_unidad { get; set; }
        public int id_dependecia { get; set; }
        public string descripcion { get; set; }
        public int id_enlace { get; set; }
        public int id_analista { get; set; }
        public string fecha_accion { get; set; }
        public int id_tipo_accion { get; set; }
        public string fecha_recepcion { get; set; }
        public int id_proceso { get; set; }

    }
}
