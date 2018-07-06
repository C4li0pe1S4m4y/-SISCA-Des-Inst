using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Controladores
{
    public class cFuente
    {
        DBConexion conectar = new DBConexion(); //OK
        public DataTable dropAcciones()
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select * from sgc_ccl_accion_generada;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public mFuente ObtenerFuente(int idFuente) //OK -- agregar switch por tipo de fuente 
        {
            mFuente informe = new mFuente();
            try
            {
                string query = string.Format("SELECT * FROM sgc_fuente WHERE id_fuente = '{0}';", idFuente);
                conectar.AbrirConexion();
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader dr = cmd.ExecuteReader();
                //
                while (dr.Read())
                {
                    informe.id_fuente = int.Parse(dr.GetString("id_fuente"));
                    informe.anio = int.Parse(dr.GetString("anio"));
                    DateTime fecha = DateTime.Parse(dr.GetString("fecha"));
                        informe.fecha = fecha.ToString("yyyy-MM-dd");
                    informe.id_status = int.Parse(dr.GetString("id_status"));
                    informe.id_tipo_fuente = int.Parse(dr.GetString("id_tipo_fuente"));
                    if (!dr.IsDBNull(dr.GetOrdinal("id_fadn")))
                        informe.id_fadn = int.Parse(dr.GetString("id_fadn"));
                    if (!dr.IsDBNull(dr.GetOrdinal("id_periodo")))
                        informe.id_periodo = int.Parse(dr.GetString("id_periodo"));
                    if (!dr.IsDBNull(dr.GetOrdinal("id_indicador")))
                        informe.id_indicador = int.Parse(dr.GetString("id_indicador"));
                    if (!dr.IsDBNull(dr.GetOrdinal("id_ind_satisfaccion")))
                        informe.id_ind_satisfaccion = int.Parse(dr.GetString("id_ind_satisfaccion"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_informe_ei")))
                        informe.no_fuente = int.Parse(dr.GetString("no_informe_ei"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_informe_ee")))
                        informe.no_fuente = int.Parse(dr.GetString("no_informe_ee"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_queja")))
                        informe.no_fuente = int.Parse(dr.GetString("no_queja"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_medicion_ind")))
                        informe.no_fuente = int.Parse(dr.GetString("no_medicion_ind"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_iniciativa_pro")))
                        informe.no_fuente = int.Parse(dr.GetString("no_iniciativa_pro"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_medicion_satisfaccion")))
                        informe.no_fuente = int.Parse(dr.GetString("no_medicion_satisfaccion"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_minuta_rev_ad")))
                        informe.no_fuente = int.Parse(dr.GetString("no_minuta_rev_ad"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_salida_no_conforme")))
                        informe.no_fuente = int.Parse(dr.GetString("no_salida_no_conforme"));
                    if (!dr.IsDBNull(dr.GetOrdinal("no_ineficacia")))
                        informe.no_fuente = int.Parse(dr.GetString("no_ineficacia"));
                }
                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                throw;
            }
            return informe;
        }

        public int AlmacenarEncabezado(mFuente mInforme) //OK -- agregar switch por tipo de fuente ok
        {
            string tipoFuente = "";
            switch(mInforme.id_tipo_fuente)
            {
                case 1: tipoFuente = "no_informe_ei"; break;
                case 2: tipoFuente = "no_informe_ee"; break;
                case 3: tipoFuente = "no_queja"; break;
                case 4: tipoFuente = "no_iniciativa_pro"; break;
                case 5: tipoFuente = "no_medicion_ind"; break;
                case 6: tipoFuente = "no_medicion_satisfaccion"; break;
                case 7: tipoFuente = "no_minuta_rev_ad"; break;
                case 8: tipoFuente = "no_salida_no_conforme"; break;
                case 9: tipoFuente = "no_ineficacia"; break;
            }

            int result = 0;
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_fuente(anio, {4}, fecha, id_status, id_tipo_fuente) " +
                    "VALUES('{0}', '{1}', '{2}', 0, '{3}'); ",
                    mInforme.anio, mInforme.no_fuente, mInforme.fecha, mInforme.id_tipo_fuente,tipoFuente);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();
                query = string.Format("SELECT * from sgc_fuente WHERE anio = '{0}' ORDER BY {1} DESC LIMIT 1; ", mInforme.anio,tipoFuente);
                cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = int.Parse(reader[0].ToString());
                }
                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return -10;
            }
            return result;
        }

        public string nombreFuente(string idAccion) //OK -- agregar switch por tipo de fuente
        {
            string nombre = "";
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("SELECT "+
                    "CASE f.id_tipo_fuente " +
                        "WHEN 1 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_informe_ei, ')') " +
                        "WHEN 2 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_informe_ee, ')') " +
                        "WHEN 3 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_queja, ')') " +
                        "WHEN 4 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_iniciativa_pro, ')') " +
                        "WHEN 5 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_medicion_ind, ')') " +
                        "WHEN 6 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_medicion_satisfaccion, ')') " +
                        "WHEN 7 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_minuta_rev_ad, ')') " +
                        "WHEN 8 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_salida_no_conforme, ')') " +
                        "WHEN 9 THEN CONCAT(tf.nombre, ' (', f.anio, '-', f.no_ineficacia, ')') " +
                    "END AS fuente " +
                "FROM sgc_fuente f " +
                    "INNER JOIN sgc_accion_generada ag ON f.id_fuente = ag.id_fuente " +
                    "INNER JOIN sgc_tipo_fuente tf ON f.id_tipo_fuente = tf.id_tipo_fuente " +
                "WHERE ag.id_accion_generada = '{0}'; ", idAccion);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    nombre = reader["fuente"].ToString();

                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return "";
            }
            return nombre;
        }

        public int ultimoInforme(string anio, string idTipoFuente) //OK -- agregar switch por tipo de fuente ok
        {
            string tipoFuente = "";
            switch (idTipoFuente)
            {
                case "1": tipoFuente = "no_informe_ei"; break;
                case "2": tipoFuente = "no_informe_ee"; break;
                case "3": tipoFuente = "no_queja"; break;
                case "4": tipoFuente = "no_iniciativa_pro"; break;
                case "5": tipoFuente = "no_medicion_ind"; break;
                case "6": tipoFuente = "no_medicion_satisfaccion"; break;
                case "7": tipoFuente = "no_minuta_rev_ad"; break;
                case "8": tipoFuente = "no_salida_no_conforme"; break;
                case "9": tipoFuente = "no_ineficacia"; break;
            }

            int result = 0;
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("SELECT * from sgc_fuente WHERE anio = '{0}' AND id_tipo_fuente = '{1}' ORDER BY {2} DESC LIMIT 1; ", anio, idTipoFuente,tipoFuente);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    result = int.Parse(reader[tipoFuente].ToString());

                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return -10;
            }
            return result+1;
        }        

        public mFuente BuscarEncabezado(string noInforme, int anio, string idTipoFuente) //OK -- agregar switch por tipo de fuente 
        {
            string tipoFuente = "";
            switch (idTipoFuente)
            {
                case "1": tipoFuente = "no_informe_ei"; break;
                case "2": tipoFuente = "no_informe_ee"; break;
                case "3": tipoFuente = "no_queja"; break;
                case "4": tipoFuente = "no_iniciativa_pro"; break;
                case "5": tipoFuente = "no_medicion_ind"; break;
                case "6": tipoFuente = "no_medicion_satisfaccion"; break;
                case "7": tipoFuente = "no_minuta_rev_ad"; break;
                case "8": tipoFuente = "no_salida_no_conforme"; break;
                case "9": tipoFuente = "no_ineficacia"; break;
            }

            mFuente informe = new mFuente();
            try
            {
                DataSet tabla = new DataSet();
                conectar.AbrirConexion();
                string query = string.Format("Select id_fuente,Date_format(fecha,'%Y-%m-%d') fecha, id_status from sgc_fuente where {2} = '{0}' AND anio = {1};", noInforme, anio, tipoFuente);
                MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
                consulta.Fill(tabla);
                conectar.CerrarConexion();
                if (tabla.Tables[0].Rows.Count > 0)
                {
                    informe.id_fuente = int.Parse(tabla.Tables[0].Rows[0]["id_fuente"].ToString());
                    informe.anio = anio;
                    informe.no_fuente = int.Parse(noInforme);
                    informe.fecha = (tabla.Tables[0].Rows[0]["fecha"].ToString());
                    informe.id_status = int.Parse(tabla.Tables[0].Rows[0]["id_status"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return informe;
        }        

        public DataSet ListadoAcciones(int idFuente, int status, string aprobacion) //ok -- agregar switch para el select con las acciones
        {
            string aprob = "";
            switch(aprobacion)
            {
                case "aprobado":
                    aprob = "AND ag.aprobado = 2";
                    break;

                case "rechazado":
                    aprob = "AND ag.aprobado = -2";
                    break;

                case "todos":
                    aprob = "";
                    break;
            }

            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("select ag.id_accion_generada as 'id',ca.Accion as 'Acción',ag.correlativo_hallazgo as 'Correlativo',ag.norma as 'Punto de Norma',  sag.nombre as 'Status', " +
                "p.Proceso,u.Unidad,d.Unidad Dependencia,ag.descripcion as 'Descripción', ee.Nombre Enlace, " +
                "ea.Nombre Analista, Date_format(ag.fecha,'%d/%m/%Y') as 'Fecha', ta.accion as 'Tipo Acción', ag.aprobado " +

                        "from sgc_accion_generada ag inner join sgc_ccl_accion_generada ca on ca.id_acciones = ag.id_ccl_accion_generada " +
                        "inner join sgc_proceso p on p.id_proceso = ag.id_proceso " +
                        "inner join sgc_unidad u on u.id_unidad = ag.id_unidad " +
                        "inner join sgc_unidad d on d.id_unidad = ag.id_dependencia  " +
                        "inner join sgc_empleados ea on ea.id_empleado = ag.id_analista " +
                        "inner join sgc_empleados el on el.id_empleado = ag.id_lider " +
                        "inner join sgc_empleados ee on ee.id_empleado = ag.id_enlace " +
                        "inner join sgc_tipo_accion ta on ta.id_tipo_accion = ag.id_tipo_accion " +
                        "left join sgc_status_accion_generada sag on sag.id_status = ag.id_status " +

                        "where ag.id_fuente = '{0}' {1}; ", idFuente, aprob);

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataSet ListadoFuentes(int status, int tipoFuente) //ok
        {
            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("SELECT * FROM sgc_fuente f LEFT JOIN sgc_status_fuente sf ON sf.id_status = f.id_status WHERE id_tipo_fuente = '{0}' AND f.id_status = '{1}';", tipoFuente, status);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        
        public void actualizarInforme(int idFuente, int status) //ok
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_fuente SET id_status = '{1}' WHERE id_fuente = '{0}'; ",
                idFuente, status);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
                //return true;
            }
            catch (Exception ex)
            {
                try
                {
                    transaccion.Rollback();
                }
                catch
                { };
                conectar.CerrarConexion();
                //return false;
            };
        }

        
    }
}
