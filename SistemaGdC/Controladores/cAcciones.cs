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
    public class cAcciones
    {
        DBConexion conectar = new DBConexion();

        public void dropProceso(DropDownList ddl) //OK
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT * FROM sgc_proceso;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Proceso >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "Proceso";
            ddl.DataValueField = "id_proceso";
            ddl.DataBind();

        }

        public void dropOpcionIngreso(DropDownList ddl) //OK
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT * FROM sgc_tipo_solicitud;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Proceso >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "descripcion";
            ddl.DataValueField = "id_tipo";
            ddl.DataBind();
        }

        public void dropUnidad(DropDownList ddl) //OK
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select id_unidad,Unidad from sgc_unidad where id_unidad = id_padre;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Unidad >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "Unidad";
            ddl.DataValueField = "id_unidad";
            ddl.DataBind();
        }

        public void dllDependencia(DropDownList ddl, int unidad) //OK
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("select id_unidad,Unidad from sgc_unidad where id_padre = {0};", unidad);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Dependencia >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "Unidad";
            ddl.DataValueField = "id_unidad";
            ddl.DataBind();
        }

        public void dropTipoAccion(DropDownList ddl) //OK
        {
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select * from sgc_tipo_accion;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            ddl.ClearSelection();
            ddl.Items.Clear();
            ddl.AppendDataBoundItems = true;
            ddl.Items.Add("<< Elija Tipo Accion >>");
            ddl.Items[0].Value = "0";
            ddl.DataSource = tabla;
            ddl.DataTextField = "accion";
            ddl.DataValueField = "id_tipo_accion";
            ddl.DataBind();
        }

        public bool ingresarAcción(mAccionesGeneradas accion)
        {
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_accion_generada (" +
                "correlativo_hallazgo, norma, descripcion, id_status, " +
                "id_fuente, id_analista, id_lider, id_enlace, id_unidad, " +
                "id_dependencia, id_ccl_accion_generada, id_proceso, id_tipo_accion, fecha) " +

                "VALUES({0},'{1}','{2}',0,{3},(SELECT id_analista FROM sgc_unidad WHERE id_unidad = '{7}'),{4},{5},{6},{7},{8},{9},{10},now());",

                accion.correlativo_hallazgo, accion.norma, accion.descripcion,
                accion.id_fuente, accion.id_lider, accion.id_enlace, accion.id_unidad,
                accion.id_dependencia, accion.id_ccl_accion_generada, accion.id_proceso, accion.id_tipo_accion);

                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);

                cmd.ExecuteNonQuery();
                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public mAccionesGeneradas Obtner_AccionGenerada(int id) //ok
        {
            mAccionesGeneradas objAccionGenerada = new mAccionesGeneradas();
            conectar = new DBConexion();
            string query = string.Format("SELECT * FROM sgc_accion_generada WHERE id_accion_generada = {0}; "
            , id);
            conectar.AbrirConexion();
            MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
            MySqlDataReader dr = cmd.ExecuteReader();
            //conectar.CerrarConexion();
            while (dr.Read())
            {
                objAccionGenerada.id_accion_generada = int.Parse(dr.GetString("id_accion_generada"));
                objAccionGenerada.correlativo_hallazgo = int.Parse(dr.GetString("correlativo_hallazgo"));
                objAccionGenerada.norma = dr.GetString("norma");
                objAccionGenerada.descripcion = dr.GetString("descripcion");

                if (!dr.IsDBNull(dr.GetOrdinal("fecha")))
                {
                    DateTime fecha = DateTime.Parse(dr.GetString("fecha"));
                    objAccionGenerada.fecha = fecha.ToString("yyyy-MM-dd");
                }

                if (!dr.IsDBNull(dr.GetOrdinal("fecha_inicio")))
                {
                    DateTime fecha_inicio = DateTime.Parse(dr.GetString("fecha_inicio"));
                    objAccionGenerada.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
                }

                if (!dr.IsDBNull(dr.GetOrdinal("fecha_fin")))
                {
                    DateTime fecha_fin = DateTime.Parse(dr.GetString("fecha_fin"));
                    objAccionGenerada.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
                }

                //DateTime fecha_inicio = DateTime.Parse(dr.GetString("fecha_inicio"));
                //objAccionGenerada.fecha_inicio = fecha_inicio.ToString("yyyy-MM-dd");
                //DateTime fecha_fin = DateTime.Parse(dr.GetString("fecha_fin"));
                //objAccionGenerada.fecha_fin = fecha_fin.ToString("yyyy-MM-dd");
                objAccionGenerada.id_status = int.Parse(dr.GetString("id_status"));
                objAccionGenerada.id_fuente = int.Parse(dr.GetString("id_fuente"));
                objAccionGenerada.id_analista = int.Parse(dr.GetString("id_analista"));
                objAccionGenerada.id_lider = int.Parse(dr.GetString("id_lider"));
                objAccionGenerada.id_enlace = int.Parse(dr.GetString("id_enlace"));
                objAccionGenerada.id_unidad = int.Parse(dr.GetString("id_unidad"));
                objAccionGenerada.id_dependencia = int.Parse(dr.GetString("id_dependencia"));
                objAccionGenerada.id_ccl_accion_generada = int.Parse(dr.GetString("id_ccl_accion_generada"));
                objAccionGenerada.id_proceso = int.Parse(dr.GetString("id_proceso"));
                objAccionGenerada.id_tipo_accion = int.Parse(dr.GetString("id_tipo_accion"));
                objAccionGenerada.aprobado = int.Parse(dr.GetString("aprobado"));
            }
            return objAccionGenerada;
        }


        public void EliminarAccion(int id) //ok
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("DELETE FROM sgc_accion_generada WHERE id_accion_generada = {0};",
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


        public DataSet ListadoAcciones(int id, string aprobAG, string statusAG)
        {
            switch (statusAG)
            {
                case "todos":
                    statusAG = "";
                    break;

                case "accionesEnlace":
                    statusAG = "AND (ag.id_status = 0 OR ag.id_status = 1 OR ag.id_status = -1 OR ag.id_status = -2 OR ag.id_status = -3)";
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
                        "inner join sgc_fuente f on f.id_fuente = ag.id_fuente " +
                        "left join sgc_status_accion_generada sag on sag.id_status = ag.id_status " +
                        "where ag.id_enlace = '{0}' AND ag.aprobado = '{1}' {2} ; ", id, aprobAG, statusAG);

            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        public bool actualizar_Accion(mAccionesGeneradas ag) //ok
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_generada SET norma = '{1}', "+
                    "descripcion = '{2}', id_analista = (SELECT id_analista FROM sgc_unidad WHERE id_unidad = '{6}'), id_lider = '{3}', id_enlace = '{4}', " +
                    "id_unidad = '{5}', id_dependencia = '{6}', id_ccl_accion_generada = '{7}', "+
                    "id_proceso = '{8}', id_tipo_accion = '{9}', correlativo_hallazgo = '{10}' " +
                    "WHERE id_accion_generada = '{0}'; ",
                ag.id_accion_generada,ag.norma,ag.descripcion, ag.id_lider, ag.id_enlace,ag.id_unidad,
                ag.id_dependencia,ag.id_ccl_accion_generada,ag.id_proceso,ag.id_tipo_accion,ag.correlativo_hallazgo);
                command.ExecuteNonQuery();
                transaccion.Commit();
                conectar.CerrarConexion();
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    transaccion.Rollback();
                }
                catch
                {};
                conectar.CerrarConexion();
                return false;
            };
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

        public void aprobar_Accion(int id, int aprob)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_generada SET aprobado = '{1}' WHERE id_accion_generada = '{0}'; ",
                id, aprob);
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

        public void aprobarTodo_Accion(int idFuente, string aprob)
        {
            switch(aprob)
            {
                case "aprobado":
                    aprob = "2";
                    break;

                case "rechazado":
                    aprob = "-2";
                    break;
            }
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_accion_generada SET aprobado = '{1}' WHERE id_fuente = '{0}' AND(aprobado IS NULL OR aprobado = 0);",
                idFuente, aprob);
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
