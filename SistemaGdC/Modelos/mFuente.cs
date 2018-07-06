using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mFuente
    {
        public int id_fuente { get; set; }
        public int anio { get; set; }
        public string fecha { get; set; }
        public int id_status { get; set; }
        public int id_tipo_fuente { get; set; }
        public int id_fadn { get; set; }
        public int id_periodo { get; set; }
        public int id_indicador { get; set; }
        public int id_ind_satisfaccion { get; set; }
        public int no_fuente { get; set; } //solo para guardar encabezado
        public int no_informe_ei { get; set; }
        public int no_informe_ee { get; set; }
        public int no_queja { get; set; }
        public int no_medicion_ind { get; set; }
        public int no_iniciativa_pro { get; set; }
        public int no_medicion_satisfaccion { get; set; }
        public int no_minuta_rev_ad { get; set; }
        public int no_salida_no_conforme { get; set; }
        public int no_ineficacia { get; set; }
    }
}
