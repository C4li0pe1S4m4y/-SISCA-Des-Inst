using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class mInformeResult
    {
        public int id_informe_correcion { get; set; }
        public int id_accion_generada { get; set; }
        public string observacion { get; set; }
        public string Descripcion_evidencia { get; set; }
        public string evidencia { get; set; }
        public int id_lider { get; set; }
        public string usuario_ingresa { get; set; }
        public string fecha_ingreso { get; set; }
    }
}
