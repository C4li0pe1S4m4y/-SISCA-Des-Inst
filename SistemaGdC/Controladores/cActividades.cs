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
    public class cActividades
    {
        DBConexion conectar = new DBConexion();

        public DataSet ListadoActividades(int id_plan, string status)
        {
            switch(status)
            {
                case "seguimientoEnlace":
                    status = "AND (id_status = 0 OR id_status = -2)";
                    break;

                case "seguimientoAnalista":
                    status = "AND (id_status = 1 OR id_status = -3)";
                    break;

                case "seguimientoLider":
                    status = "AND (id_status = 2 OR id_status = -4)";
                    break;

                case "seguimientoDirector":
                    status = "AND id_status = 3";
                    break;

                ////////////////////////////////////////////////////////////////////////////////////////////

                case "actPendientes":
                    status = "AND id_status = 0";
                    break;

                case "actTerminadas":
                    status = "AND id_status = 1";
                    break;

                case "actValidadas":
                    status = "AND id_status = 2";
                    break;

                case "actRechazadas":
                    status = "AND id_status = -2";
                    break;
            }

            DataSet result = new DataSet();            
            conectar.AbrirConexion();
            string query = string.Format("select id_accion_realizar as 'No.',accion,responsable,date_format(fecha_inicio,'%d/%m/%Y')  as 'Fecha Inicio',date_format(fecha_fin,'%d/%m/%Y') " +
                " as 'Fecha Fin',observaciones from sgc_accion_realizar where id_plan = {0} {1}", id_plan, status);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public mActividad Obtner_Actividad(int idActividad)
        {
            mActividad objActividad = new mActividad();
            string query = string.Format("SELECT * FROM sgc_accion_realizar where id_accion_realizar = {0}; "
            , idActividad);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objActividad.id_accion_realizar = int.Parse(dr.GetString("id_accion_realizar"));
                objActividad.id_plan = int.Parse(dr.GetString("id_plan"));
                objActividad.accion = dr.GetString("accion");
                objActividad.responsable = dr.GetString("responsable");
                DateTime fecha_inicio = DateTime.Parse(dr.GetString("fecha_inicio"));
                objActividad.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
                DateTime fecha_fin = DateTime.Parse(dr.GetString("fecha_fin"));
                objActividad.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
                objActividad.observaciones = dr.GetString("observaciones");
                objActividad.id_status = int.Parse(dr.GetString("id_status"));
            }
            conectar.CerrarConexion();
            return objActividad;
        }

        public void actualizarActividad(mActividad act)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_realizar SET accion = '{1}', " +
                    "responsable = '{2}', fecha_inicio = '{3}', fecha_fin = '{4}', observaciones = '{5}' " +
                    "WHERE id_accion_realizar = '{0}'; ",
                act.id_accion_realizar, act.accion, act.responsable, act.fecha_inicio, act.fecha_fin, act.observaciones);
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

        public void actualizarStatusActividad(int id, int status)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_realizar SET id_status = '{1}' WHERE id_accion_realizar = '{0}'; ",
                id, status);
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

        public void actualizarObsActividad(int id, string observaciones)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_realizar SET observaciones = '{1}' WHERE id_accion_realizar = '{0}'; ",
                id, observaciones);
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
                //return false;
            };
        }

        public void EliminarAccion(int id)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("DELETE FROM sgc_accion_realizar WHERE id_accion_realizar = {0};",
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
    }
}
