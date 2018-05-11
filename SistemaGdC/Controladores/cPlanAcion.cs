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
                string query = string.Format("Insert Into sgc_plan_accion(tecnica_analisis,causa_raiz,id_lider,usur_ingreso,fecha_ingreso,id_accion_generada,id_status)  " +
                    "Values('{0}','{1}','{2}','{3}',now(),'{4}',1)", obj.tecnica_analisis, obj.causa_raiz, obj.id_lider, obj.usuario_ingreso, obj.id_accion_generada);
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

        public int IngresarAccionRealizar(mAccionesRealizar obj)
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
            string query = string.Format("SELECT ag.id_accion_generada ID, CONCAT(ag.anio_informe_ei,'-',ag.no_informe_ei) Informe, pa.causa_raiz, ag.descripcion, pa.id_plan,TRUNCATE(((pa.id_status*100)/5),0) Progreso " +
                " FROM sgc_plan_accion pa INNER JOIN sgc_accion_generada ag ON pa.id_accion_generada = ag.id_accion_generada;");
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
            string query = string.Format("select id_accion_realizar as 'No.',accion,responsable,date_format(fecha_inicio,'%d/%m/%Y')  as 'Fecha Inicio',date_format(fecha_fin,'%d/%m/%Y') "+
                " as 'Fecha Fin',observaciones from sgc_accion_realizar where id_plan={0}", id_plan);
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

        public mPlanAccion Obtner_PlanAccion(int id)
        {
            mPlanAccion objPlanAccion = new mPlanAccion();
            conectar = new DBConexion();
            string query = string.Format(" select * from sgc_plan_accion where id_accion_generada = {0}; "
            , id);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objPlanAccion.id_plan = int.Parse(dr.GetString("id_plan"));
                objPlanAccion.causa_raiz = dr.GetString("causa_raiz");
                objPlanAccion.tecnica_analisis = dr.GetString("tecnica_analisis");
                objPlanAccion.id_lider = int.Parse(dr.GetString("id_lider"));
                objPlanAccion.usuario_ingreso = dr.GetString("usur_ingreso");
                DateTime fecha = DateTime.Parse(dr.GetString("fecha_ingreso"));
                    objPlanAccion.fecha_ingreso = fecha.ToString("yyyy-MM-dd");
                objPlanAccion.id_accion_generada = int.Parse(dr.GetString("id_accion_generada"));
                objPlanAccion.id_status = int.Parse(dr.GetString("id_status"));
            }
            conectar.CerrarConexion();
            return objPlanAccion;
        }

        public DataSet ListadoAcciones(int id, string statusIEI, string statusAG)
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
                    condicion = "AND pa.id_lider = " + id +" AND ag.id_status = 11";
                    break;

//////////// SEGUIMIENTO DE ACTIVIDADES ////////////////////////////////////////////////////////////////////
                case "seguimientoEnlace": //Enlace
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_enlace = " + id + " AND ag.id_status = 14 AND pa.id_status = 1";
                    break;

                case "seguimientoAnalista": //Analista
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_analista = " + id + " AND ag.id_status = 14 AND (pa.id_status = 1 OR pa.id_status = 2 OR pa.id_status = -3)";
                    break;

                case "seguimientoLider": //Lider
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND pa.id_lider = " + id + " AND ag.id_status = 14 AND pa.id_status = 3";
                    break;

                case "seguimientoDirector": //Director
                    join = "inner join sgc_plan_accion pa on pa.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ag.id_status = 14 AND pa.id_status = 4";
                    break;

//////////// VALIDAR INFORME DE CORRECCIÓN /////////////////////////////////////////////////////////////////

                case "validarInformeCoDirector": //Informe de Corrección                    
                    condicion = "AND ag.id_status = 22";
                    break;

                case "validarInformeCoAnalista": //Informe de Corrección
                    condicion = "AND ag.id_status = 21";
                    break;

                case "validarInformeCoLider": //Informe de Corrección
                    join = "inner join sgc_informe_correcion ic on ic.id_accion_generada = ag.id_accion_generada ";
                    condicion = "AND ic.id_lider = " + id + " AND ag.id_status = 2";
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
                    condicion = "AND ic.id_lider = " + id + " AND ag.id_status = 3";
                    //condicion = "AND ag.id_status = 2";
                    break;

////////////////////////////////////////////////////////////////////////////////////////////////////////////
                default:
                    condicion = "AND ag.id_status = " + statusAG;
                    break;
            }

            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("select ag.id_accion_generada as 'id',ca.Accion as 'Acción',ag.correlativo_hallazgo as 'Correlativo',ag.norma as 'Punto de Norma', sag.nombre as 'Status', " +
                "p.Proceso,u.Unidad,d.Unidad Dependencia,ag.descripcion as 'Descripción', concat(ee.Nombre, ' ', ee.Apellido) Enlace, " +
                "concat(ea.Nombre, ' ', ea.Apellido) Analista, Date_format(ag.fecha,'%d/%m/%Y') as 'Fecha', ta.accion as 'Tipo Acción' " +

                        "from sgc_accion_generada ag inner join sgc_ccl_accion_generada ca on ca.id_acciones = ag.id_ccl_accion_generada " +
                        "inner join sgc_proceso p on p.id_proceso = ag.id_proceso " +
                        "inner join sgc_unidad u on u.id_unidad = ag.id_unidad " +
                        "inner join sgc_unidad d on d.id_unidad = ag.id_dependencia  " +
                        "inner join sgc_empleados ea on ea.id_empleado = ag.id_analista " +
                        "inner join sgc_empleados ee on ee.id_empleado = ag.id_enlace " +
                        "inner join sgc_tipo_accion ta on ta.id_tipo_accion = ag.id_tipo_accion " +
                        "inner join sgc_informe_ei iei on iei.anio = ag.anio_informe_ei and iei.no_informe = ag.no_informe_ei " +
                        "left join sgc_status_accion_generada sag on sag.id_status = ag.id_status " +
                        "{1} " +                        
                        "where iei.id_status = '{0}' {2}; ", statusIEI, join, condicion);

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
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
    }
}
