using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class cPlanAcion
    {
        DBConexion conectar = new DBConexion();
        public int IngresraCausaRaiz(mPlanAccion obj)
        {
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format("Insert Into sgc_plan_accion(tecnica_analisis,causa_raiz,usur_ingreso,fecha_ingreso,id_accion_generada,id_status)  " +
                    "Values('{0}','{1}','{2}',now(),'{3}',1)", obj.tecnica_analisis, obj.causa_raiz, obj.usuario_ingreso, obj.id_accion_generada);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                resultado = cmd.ExecuteNonQuery();
                query = "select @@IDENTITY;";
                cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    resultado = int.Parse(reader[0].ToString());
                }
                conectar.CerrarConexion();
                return resultado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int IngresarAccionRealizar(mActividad obj)
        {
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format("Insert Into sgc_accion_realizar(id_plan,accion,responsable,fecha_inicio,fecha_fin,observaciones,id_status)  " +
                    "Values({0},'{1}','{2}','{3}','{4}','{5}',0)", obj.id_plan, obj.accion, obj.responsable, obj.fecha_inicio, obj.fecha_fin,obj.observaciones);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                resultado = cmd.ExecuteNonQuery();
                conectar.CerrarConexion();
                return resultado;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataSet ListadoPlanesAccion(string status)
        {
            //public DataSet ListadoAccionesRealizar(int id_plan, string status)
            //string status = "";
            if (status == "todos")
            {
                status = "";
            }
            else status = "AND id_status=" + status;

            DataSet result = new DataSet();
            conectar = new DBConexion();
            conectar.AbrirConexion();
            string query = string.Format("SELECT ag.id_accion_generada ID, "+

                    "CASE f.id_tipo_fuente "+
                        "WHEN 1 THEN CONCAT('EI-', f.anio, '-', f.no_informe_ei) "+
                        "WHEN 2 THEN CONCAT('EE-', f.anio, '-', f.no_informe_ee) "+
                        "WHEN 3 THEN CONCAT('Q-', f.anio, '-', f.no_queja) "+
                        "WHEN 4 THEN CONCAT('IP-', f.anio, '-', f.no_iniciativa_pro) " +
                        "WHEN 5 THEN CONCAT('MI-', f.anio, '-', f.no_medicion_ind) " +
                        "WHEN 6 THEN CONCAT('MSC-', f.anio, '-', f.no_medicion_satisfaccion) " +
                        "WHEN 7 THEN CONCAT('MRAD-', f.anio, '-', f.no_minuta_rev_ad) " +
                        "WHEN 8 THEN CONCAT('SNC-', f.anio, '-', f.no_salida_no_conforme) " +
                        "WHEN 9 THEN CONCAT('I-', f.anio, '-', f.no_ineficacia) " +
                    "END AS Informe, " +

                    "pa.causa_raiz, ag.descripcion, pa.id_plan, " +
                    "TRUNCATE(((pa.id_status * 100) / 5), 0) Progreso " +
                "FROM sgc_plan_accion pa " +
                    "INNER JOIN sgc_accion_generada ag ON pa.id_accion_generada = ag.id_accion_generada " +
                    "INNER JOIN sgc_fuente f ON f.id_fuente = ag.id_fuente; ");
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataSet ListadoAccionesRealizar(int id_plan)
        {
            //public DataSet ListadoAccionesRealizar(int id_plan, string status)
            string status = "";
            if (status == "todos")
            {
                status = "";
            }
            else status = "AND id_status=" + status;
        
            DataSet result = new DataSet();
            conectar = new DBConexion();
            conectar.AbrirConexion();
            string query = string.Format("select id_accion_realizar as 'No.',accion as 'Actividad a realizar',Responsable,date_format(fecha_inicio,'%d/%m/%Y')  as 'Fecha Inicio',date_format(fecha_fin,'%d/%m/%Y') "+
                " as 'Fecha Fin',Observaciones from sgc_accion_realizar where id_plan={0}", id_plan);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public DataSet ListadoSeguimientoActividades(int id_plan, string status)
        {
            switch(status)
            {
                case "pendientes":
                    status = " AND id_status = 0";
                    break;
            }

            if (status == "todos")
            {
                status = "";
            }
            else status = "AND id_status=" + status;

            DataSet result = new DataSet();
            conectar = new DBConexion();
            conectar.AbrirConexion();
            string query = string.Format("select id_accion_realizar as 'No.',accion,responsable,date_format(fecha_inicio,'%d/%m/%Y')  as 'Fecha Inicio',date_format(fecha_fin,'%d/%m/%Y') " +
                " as 'Fecha Fin',observaciones from sgc_accion_realizar where id_plan={0}", id_plan);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public mPlanAccion Obtner_PlanAccion(int idAccion) //ok
        {
            mPlanAccion objPlanAccion = new mPlanAccion();
            conectar = new DBConexion();
            string query = string.Format(" select * from sgc_plan_accion where id_accion_generada = {0}; "
            , idAccion);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objPlanAccion.id_plan = int.Parse(dr.GetString("id_plan"));
                objPlanAccion.causa_raiz = dr.GetString("causa_raiz");
                objPlanAccion.tecnica_analisis = dr.GetString("tecnica_analisis");
                //objPlanAccion.id_lider = int.Parse(dr.GetString("id_lider"));
                objPlanAccion.usuario_ingreso = dr.GetString("usur_ingreso");
                DateTime fecha_ingreso = DateTime.Parse(dr.GetString("fecha_ingreso"));
                    objPlanAccion.fecha_ingreso = fecha_ingreso.ToString("yyyy-MM-dd");
                if (!dr.IsDBNull(dr.GetOrdinal("fecha_recepcion")))
                {
                    DateTime fecha_recepcion = DateTime.Parse(dr.GetString("fecha_recepcion"));
                    objPlanAccion.fecha_recepcion = fecha_recepcion.ToString("yyyy-MM-dd");
                }                
                objPlanAccion.id_accion_generada = int.Parse(dr.GetString("id_accion_generada"));
                objPlanAccion.id_status = int.Parse(dr.GetString("id_status"));
                objPlanAccion.no_ampliacion = int.Parse(dr.GetString("no_ampliacion"));
                if (!dr.IsDBNull(dr.GetOrdinal("inicio_actividades")))
                {
                    DateTime inicio_actividades = DateTime.Parse(dr.GetString("inicio_actividades"));
                    objPlanAccion.inicio_actividades = inicio_actividades.ToString("yyyy-MM-dd");
                }
                if (!dr.IsDBNull(dr.GetOrdinal("final_actividades")))
                {
                    DateTime final_actividades = DateTime.Parse(dr.GetString("final_actividades"));
                    objPlanAccion.final_actividades = final_actividades.ToString("yyyy-MM-dd");
                }
            }
            conectar.CerrarConexion();
            return objPlanAccion;
        }

        public DataSet ListadoAcciones(int id, string aprobAG, string statusAG)
        {
            string condicion = "";
            string join = "";
            switch (statusAG)
            {
                case "todos":
                    condicion = "";
                    break;

//////////// VALIDACIÓN DEL PLAN DE ACCIÓN /////////////////////////////////////////////////////////////////
                case "validarDirector": //Plan de Acción
                    condicion = "AND ag.id_status = 13";
                    break;

                case "validarAnalista": //Plan de Acción
                    condicion = "AND ag.id_analista = " + id + " AND ag.id_status = 12";
                    break;

                case "validarLider": //Plan de Acción
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_lider = " + id + " AND ag.id_status = 11";
                    break;

                //////////// SEGUIMIENTO DE ACTIVIDADES ////////////////////////////////////////////////////////////////////
                case "seguimientoEnlace": //Enlace
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_enlace = " + id + " AND ag.id_status = 14 AND (pa.id_status = 1 OR pa.id_status = 6)";
                    break;

                case "seguimientoAnalista": //Analista
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_analista = " + id + " AND ag.id_status = 14 AND (pa.id_status = 1 OR pa.id_status = 2 OR pa.id_status = -3)";
                    break;

                //case "seguimientoLider": //Lider
                //    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                //    condicion = "AND ag.id_lider = " + id + " AND ag.id_status = 14 AND pa.id_status = 3";
                //    break;

                case "seguimientoDirector": //Director
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_status = 14 AND pa.id_status = 3";
                    break;

//////////// VALIDAR INFORME DE CORRECCIÓN /////////////////////////////////////////////////////////////////

                case "validarInformeCoDirector": //Informe de Corrección                    
                    condicion = "AND ag.id_status = 22";
                    break;

                case "validarInformeCoAnalista": //Informe de Corrección
                    condicion = "AND ag.id_status = 21";
                    break;

                case "validarInformeCoLider": //Informe de Corrección
                    join = "inner join sgc_informe_co ic on ic.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_lider = " + id + " AND ag.id_status = 2";
                    //condicion = "AND ag.id_status = 2";
                    break;

//////////// VALIDAR INFORME DE OPORTUNIDAD DE MEJORA /////////////////////////////////////////////////////////////////

                case "validarInformeOMDirector": //Informe de Corrección                    
                    condicion = "AND ag.id_status = 32";
                    break;

                case "validarInformeOMAnalista": //Informe de Corrección
                    condicion = "AND ag.id_status = 31";
                    break;

                case "validarInformeOMLider": //Informe de Corrección
                    join = "inner join sgc_informe_om ic on ic.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_lider = " + id + " AND ag.id_status = 3";
                    //condicion = "AND ag.id_status = 2";
                    break;

////////////////////////////////////////////////////////////////////////////////////////////////////////////
                default:
                    condicion = "AND ag.id_status = " + statusAG;
                    break;
            }

            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("SELECT ag.id_accion_generada as 'id',ca.Accion as 'Acción',ag.correlativo_hallazgo as 'Correlativo', " +
                "ag.correlativo_compromiso as 'Compromiso', ag.norma as 'Punto de Norma', sag.nombre as 'Status', " +
                "p.Proceso, u.Unidad, d.Unidad Dependencia, ag.descripcion as 'Descripción', ee.Nombre Enlace, " +
                "ea.Nombre Analista, Date_format(ag.fecha, '%d/%m/%Y') as 'Fecha', " +
                "ta.accion as 'Tipo Acción', " +
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
                    "END AS Fuente " +

                        "FROM sgc_accion_generada ag " +
                        "LEFT JOIN sgc_ccl_accion_generada ca on ca.id_acciones = ag.id_ccl_accion_generada " +
                        "INNER JOIN sgc_proceso p on p.id_proceso = ag.id_proceso " +
                        "INNER JOIN sgc_unidad u on u.id_unidad = ag.id_unidad " +
                        "INNER JOIN sgc_unidad d on d.id_unidad = ag.id_dependencia  " +
                        "INNER JOIN sgc_empleados ea on ea.id_empleado = ag.id_analista " +
                        "INNER JOIN sgc_empleados ee on ee.id_empleado = ag.id_enlace " +
                        "INNER JOIN sgc_tipo_accion ta on ta.id_tipo_accion = ag.id_tipo_accion " +
                        "INNER JOIN sgc_fuente f on f.id_fuente = ag.id_fuente " +
                        "INNER JOIN sgc_tipo_fuente tf ON f.id_tipo_fuente = tf.id_tipo_fuente " +
                        "LEFT JOIN sgc_status_accion_generada sag on sag.id_status = ag.id_status " +
                        "{1} " +                        
                        "where ag.aprobado = '{0}' {2}; ", aprobAG, join, condicion);

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public void actualizar_planAccion(mPlanAccion plan)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion SET tecnica_analisis = '{1}', causa_raiz = '{2}', "+
                    "usur_ingreso = '{3}', id_accion_generada = '{4}' "+
                    "WHERE id_plan = '{0}'; ",
                plan.id_plan,plan.tecnica_analisis,plan.causa_raiz,plan.usuario_ingreso,plan.id_accion_generada);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }


        public void actualizar_statusPlan(int id, int status)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion SET id_status = '{1}' WHERE id_plan = '{0}'; ",
                id, status);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }

        public void agregar_Ampliacion(int id, int ampliacion)
        {
            ampliacion += 1;
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion SET no_ampliacion = '{1}' WHERE id_plan = '{0}'; ",
                id, ampliacion);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }

        public void fechaAnterior_Ampliacion(int id)
        {            
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion SET fecha_modificada = final_actividades WHERE id_plan = '{0}'; ",
                id);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }

        public void finalizarPlan(int idPlan)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion SET fecha_finalizado = now() WHERE id_plan = '{0}'; ",
                idPlan);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }

        public void fechaRecepcion_plan(int id)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion SET fecha_recepcion = now() WHERE id_plan = '{0}'; ",
                id);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }

        public void asignarTiempoPlan(int idPlan)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_plan_accion " +
                    "SET inicio_actividades = (SELECT MIN(fecha_inicio) " +
                        "FROM sgc_accion_realizar WHERE id_plan = '{0}'), " +
                    "final_actividades = (SELECT MAX(fecha_fin) " +
                        "FROM sgc_accion_realizar WHERE id_plan = '{0}') " +
                    "WHERE id_plan = '{0}'; ", idPlan);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
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
            };
        }

        public int calcularTiempoPlan(string idPlan)
        {
            try
            {
                cGeneral cGeneral = new cGeneral();
                int resultado = 0;
                string inicio = "";
                string final = "";
                conectar = new DBConexion();
                conectar.AbrirConexion();

            string query1 = string.Format("SELECT inicio_actividades inicio, final_actividades final " +
                "FROM sgc_plan_accion WHERE id_plan = '{0}'; ", idPlan);
            MySqlCommand cmd = new MySqlCommand(query1, conectar.conectar);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                inicio = (DateTime.Parse(reader.GetString("inicio"))).ToString("yyyy-MM-dd");
                final = (DateTime.Parse(reader.GetString("final"))).ToString("yyyy-MM-dd");
            }
            conectar.CerrarConexion();
                resultado = cGeneral.rangoFechas(inicio, final,true);
                return resultado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
