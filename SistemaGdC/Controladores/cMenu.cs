using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class cMenu
    {
        DBConexion conectar;
        public DataTable LlenarDoctosUsuarios(string tipo_usuario)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("select m.id_menu, m.Descripcion,m.PadreId,m.URL " +
                "from sgc_menu m " +
                "inner join sgc_menu_usuarios mu on mu.id_menu = m.id_menu" +
                 " inner join sgc_tipo_usuario tu on tu.id_tipo_usuario = mu.id_tipo_usuario " +
                 "where m.habilitado = 1 and mu.id_tipo_usuario = '{0}'", tipo_usuario);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }
    }
}
