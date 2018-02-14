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
        DBConexion conectar;
        public int IngresraInforme(mPlanAccion obj)
        {
            try
            {
                int resultado = 0;
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format("Insert Into sgc_plan_accion(id_accion,tecnica_analisis,causa_raiz,id_lider,usur_ingreso,fecha_ingreso)  " +
                    "Values({0},'{1}','{2}','{3}','{4}',now())", obj.id_accion, obj.tecnica_analisis, obj.causa_raiz, obj.id_lider, obj.usuario_ing);
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
                string query = string.Format("Insert Into sgc_accion_realizar(id_plan,accion,responsable,fecha_inicio,fecha_fin,observaciones)  " +
                    "Values({0},'{1}','{2}','{3}','{4}','{5}')", obj.id_plan, obj.accion, obj.responsable, obj.fechaInicio, obj.fechaFinal,obj.observaciones);
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

        public DataSet ListadoAccionesRealizar(int id_plan)
        {
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
    }
}
