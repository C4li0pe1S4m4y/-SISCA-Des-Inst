
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controladores
{
    public class cAcciones
    {
        DBConexion conectar = new DBConexion();
        

        public DataSet ListadoAcciones(int id, string statusIEI, string statusAG)
        {
            switch (statusAG)
            {
                case "todos":
                    statusAG = "";
                    break;

                case "accionesEnlace":
                    statusAG = "AND (ag.id_status = 0 OR ag.id_status = 1 OR ag.id_status = -1 OR ag.id_status = -2)";
                    break;

                default:
                    statusAG = "AND ag.id_status = " + statusAG;
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
                        "where ag.id_enlace = '{0}' AND iei.id_status = '{1}' {2} ; ", id, statusIEI, statusAG);

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public void actualizarStatus_Accion(int id, int status)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_generada SET id_status = '{1}' WHERE id_accion_generada = '{0}'; ",
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

        public void actualizarTipoAccion(int id, int idTpoAccion)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_generada SET id_tipo_accion = '{1}' WHERE id_accion_generada = '{0}'; ",
                id, idTpoAccion);
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

        public void validarCausaRaiz_Accion(int id, int status)
        {
            actualizarStatus_Accion(id, status);
        }

        public void validarActividades_PlanAccion(int id, int status)
        {
            actualizarStatus_Accion(id, status);
        }

    }
}
