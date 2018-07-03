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

        

        public int AlmacenarEncabezado(mFuente mInforme) //OK -- agregar switch por tipo de fuente
        {
            int result = 0;
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_fuente(anio, no_informe_ei, fecha, id_status, id_tipo_fuente) " +
                    "VALUES('{0}', '{1}', '{2}', 0, 1); ",
                    mInforme.anio, mInforme.no_informe_ei, mInforme.fecha);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();
                query = string.Format("SELECT * from sgc_fuente WHERE anio = '{0}' ORDER BY no_informe_ei DESC LIMIT 1; ", mInforme.anio);
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

        public int ultimoInforme(string anio) //OK -- agregar switch por tipo de fuente
        {
            int result = 0;
            try
            {
                conectar.AbrirConexion();
                string query = string.Format("SELECT * from sgc_fuente WHERE anio = '{0}' ORDER BY no_informe_ei DESC LIMIT 1; ", anio);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    result = int.Parse(reader["no_informe_ei"].ToString());

                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {
                return -10;
            }
            return result+1;
        }        

        public mFuente BuscarEncabezado(string noInforme, int anio) //OK -- agregar switch por tipo de fuente
        {
            mFuente informe = new mFuente();
            try
            {
                DataSet tabla = new DataSet();
                conectar.AbrirConexion();
                string query = string.Format("Select id_fuente,Date_format(fecha,'%Y-%m-%d') fecha, id_status from sgc_fuente where no_informe_ei = '{0}' AND anio = {1};", noInforme, anio);
                MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
                consulta.Fill(tabla);
                conectar.CerrarConexion();
                if (tabla.Tables[0].Rows.Count > 0)
                {
                    informe.id_fuente = int.Parse(tabla.Tables[0].Rows[0]["id_fuente"].ToString());
                    informe.anio = anio;
                    informe.no_informe_ei = int.Parse(noInforme);
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

        public DataSet ListadoFuentes(int status, int tipoFuente) ///////////////////////
        {
            DataSet result = new DataSet();
            conectar.AbrirConexion();
            string query2 = string.Format("SELECT * FROM sgc_fuente f LEFT JOIN sgc_status_informe_ei siei ON siei.id_status = f.id_status WHERE id_tipo_fuente = '{0}' AND f.id_status = '{1}';", tipoFuente, status);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query2, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }

        
        public void actualizarInforme(int anio, int noInforme, int status)
        {
            conectar.AbrirConexion();
            MySqlTransaction transaccion = conectar.conectar.BeginTransaction();
            MySqlCommand command = conectar.conectar.CreateCommand();
            command.Transaction = transaccion;
            try
            {
                command.CommandText = string.Format("UPDATE sgc_fuente SET id_status = '{2}' WHERE anio = '{0}' AND no_informe_ei = '{1}'; ",
                anio, noInforme, status);
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
