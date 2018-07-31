using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace Controladores
{
    public class cGeneral
    {
        DBConexion conectar;
        public DataTable dropEmpleados()
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT se.id_empleado id,COALESCE(CONCAT(se.nombre), 'S/D') texto " +
                "FROM dbcdagsgc2.sgc_empleados se ORDER BY se.nombre;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public DataTable informacionGeneral()
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SET lc_time_names = 'es_MX';" +

                "SELECT ag.id_accion_generada id, f.anio, " +

                "CASE f.id_tipo_fuente " +
                    "WHEN 1 THEN CONCAT('EI-', f.anio, '-', f.no_informe_ei)  " +
                    "WHEN 2 THEN CONCAT('EE-', f.anio, '-', f.no_informe_ee) " +
                    "WHEN 3 THEN CONCAT('Q-', f.anio, '-', f.no_queja) " +
                    "WHEN 4 THEN CONCAT('IP-', f.anio, '-', f.no_iniciativa_pro) " +
                    "WHEN 5 THEN CONCAT('MI-', f.anio, '-', f.no_medicion_ind) " +
                    "WHEN 6 THEN CONCAT('MSC-', f.anio, '-', f.no_medicion_satisfaccion) " +
                    "WHEN 7 THEN CONCAT('MRAD-', f.anio, '-', f.no_minuta_rev_ad) " +
                    "WHEN 8 THEN CONCAT('SNC-', f.anio, '-', f.no_salida_no_conforme) " +
                    "WHEN 9 THEN CONCAT('I-', f.anio, '-', f.no_ineficacia) " +
                "END AS informe,  " +
                "f.Fecha, IFNULL(tag.Accion, 'N/A')  AS accion, ag.correlativo_hallazgo AS correlativo, IF(ag.norma='','--',ag.norma) norma, " +
                "p.proceso, uu.unidad AS unidad, ud.unidad AS dependencia, ag.descripcion, " +
                "ee.nombre AS enlace, ea.nombre AS analista, IFNULL(ag.fecha_solicitud,'N/A') AS fechaSolicitud, ta.accion AS tipoAccion, " +

                "IFNULL( "+
                "IF(ta.accion = 'AC' OR ta.accion = 'AM', " +
                    "(SELECT COUNT(car.id_accion_realizar) " +
                        "FROM sgc_accion_realizar car " +
                        "WHERE car.id_plan = pa.id_plan AND ag.id_status >= 11),NULL) " +
                ",0) "+
                "AS totalAcciones, " +

                "IFNULL( " +
                "IF(ta.accion = 'AC' OR ta.accion = 'AM', " +
                    "(SELECT COUNT(car.id_accion_realizar) " +
                        "FROM sgc_accion_realizar car " +
                        "WHERE car.id_plan = pa.id_plan AND car.id_status > 0),NULL) " +
                ",0) " +
                "AS accionesFinalizadas, " +

                "IFNULL((SELECT MIN(mar.fecha_fin) " +
                    "FROM sgc_accion_realizar mar " +
                    "WHERE mar.id_plan = pa.id_plan AND mar.id_status >= 0),'Sin fecha') " +
                "AS siguienteMonitoreo, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IF((SELECT totalAcciones), " +
                        "IF((SELECT MIN(mar.fecha_fin) " +
                            "FROM sgc_accion_realizar mar " +
                            "WHERE mar.id_plan = pa.id_plan) < now(), " +
                            "'Fuera de Tiempo','En Tiempo'),'Sin Acciones'),'N/A') " +
                "AS estatusEjecucion, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(pa.final_actividades, 'Sin acciones'),'N/A') " +
                "AS finalPlanificado, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(DATE_FORMAT(pa.final_actividades, '%M'), 'Sin acciones'),'N/A') " +
                "AS mesCierre, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(DATE_FORMAT(pa.final_actividades, '%Y'), 'Sin acciones'),'N/A') " +
                "AS anioCierre, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(CONCAT((SELECT round((accionesFinalizadas * 100) / totalAcciones)), '%'), " +
                    "'Sin Acciones'),'N/A')  " +
                "AS porcentaje, " +

                "IFNULL( " +
                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(CONCAT(pa.fecha_modificada, ' / ', pa.final_actividades), " +
                    "pa.final_actividades),'N/A')  " +
                    ",'--') " +
                "AS fechaModificada, " +

                "IFNULL( " +
                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "pa.fecha_finalizado,'N/A')  " +
                    ",'--') " +
                "AS fechaFinalizado, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(DATE_FORMAT(pa.fecha_finalizado, '%M'), 'En actividad'),'N/A') " +
                "AS mesRealCierre, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(DATE_FORMAT(pa.fecha_finalizado, '%Y'), 'En actividad'),'N/A') " +
                "AS anioRealCierre, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                "IF(pa.fecha_finalizado IS NULL, 'Abierto', 'Cerrado'),'N/A')  " +
                "AS estatusPlan, " +

                "IF((SELECT ta.accion)= 'AC' OR(SELECT ta.accion) = 'AM', " +
                "IF(pa.fecha_finalizado > pa.final_actividades, 'Atrasada', 'En tiempo'),'N/A') " +
                "AS estatusTiempo, " +

                "IFNULL(pa.no_ampliacion, 0) ampliaciones, " +
                    "IF((SELECT ta.accion) = 'AC' OR(SELECT ta.accion) = 'AM', " +
                    "IFNULL(CONCAT((SELECT round((accionesFinalizadas * 100) / totalAcciones)), '%'), " +
                    "'Sin Acciones'),'N/A')  " +
                "AS porcentaje, " +

                "IFNULL( " +
                "DATEDIFF(pa.fecha_finalizado, pa.inicio_actividades)  " +
                ",0) " +
                "AS diasFinalizado " +

            "FROM sgc_fuente f " +
                "INNER JOIN sgc_accion_generada ag ON ag.id_fuente = f.id_fuente " +
                "LEFT JOIN sgc_ccl_accion_generada tag ON tag.id_acciones = ag.id_ccl_accion_generada " +
                "LEFT JOIN sgc_proceso p ON p.id_proceso = ag.id_proceso " +
                "INNER JOIN sgc_unidad uu ON uu.id_unidad = ag.id_unidad " +
                "INNER JOIN sgc_unidad ud ON ud.id_unidad = ag.id_dependencia " +
                "INNER JOIN sgc_empleados ee ON ee.id_empleado = ag.id_enlace " +
                "INNER JOIN sgc_empleados ea ON ea.id_empleado = ag.id_analista " +
                "INNER JOIN sgc_tipo_accion ta ON ta.id_tipo_accion = ag.id_tipo_accion " +
                "LEFT JOIN sgc_plan_accion pa ON ag.id_accion_generada = pa.id_accion_generada; ";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public int rangoFechas(string fecha_inicio, string fecha_final, bool fin_semana)
        {
            string consulta = "";
            if (fin_semana)
                consulta = 
                "SET @FromDate	= '{0}'; " +
                "SET @ToDate    = '{1}'; " +
                "SET @Domingo   = 1; " +
                "SET @Sabado    = 7; " +
                "SET @Minimo    = DAYOFWEEK(@FromDate); " +
                "SET @Maximo    = IF(DAYOFWEEK(@ToDate) = 7, 1, 0); " +

                "SELECT datediff(@ToDate, @FromDate) +1 - " +
                    "(datediff(@ToDate, @FromDate) DIV 7 " +
                    "+ (CASE WHEN @Minimo = @Domingo THEN 1 ELSE 0 END)) - " +
                    "(datediff(@ToDate, @FromDate) DIV 7 " +
                    "+ (CASE WHEN @Minimo = @Sabado THEN 1 ELSE 0 END)) - @Maximo " +
                "AS 'dias';";
            else
                consulta = "SELECT datediff('{1}', '{0}') AS 'dias';";
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format(consulta, fecha_inicio, fecha_final);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    resultado = int.Parse(reader.GetString("dias"));

                conectar.CerrarConexion();
                return resultado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
