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
    public class cInformeResultados
    {
        DBConexion conectar;
        public DataTable dropAcciones()
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select * from dbcdagsgc.sgc_ccl_acciones_generadas;";
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(tabla);
            conectar.CerrarConexion();
            return tabla;
        }

        public void dropProceso(DropDownList ddl)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT * FROM dbcdagsgc.sgc_proceso;";
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

        public void dropOpcionIngreso(DropDownList ddl)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "SELECT * FROM dbcdagsgc.sgc_tipo_solicitud;";
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

        public void dropUnidad(DropDownList ddl)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select id_unidad,Unidad from dbcdagsgc.sgc_unidad where id_unidad = id_padre;";
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

        public void dllDependencia(DropDownList ddl,int unidad)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = string.Format("select id_unidad,Unidad from dbcdagsgc.sgc_unidad where id_padre = {0};",unidad);
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

        public void dropTipoAccion(DropDownList ddl)
        {
            conectar = new DBConexion();
            DataTable tabla = new DataTable();
            conectar.AbrirConexion();
            string query = "select * from dbcdagsgc.sgc_tipo_accion;";
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


        public bool IngresarInforme(mAccionesGeneradas accion)
        {
            try
            {
                conectar = new DBConexion();

                conectar.AbrirConexion();
                string query = string.Format("INSERT INTO sgc_acciones_generadas ( `id_informe`, `id_accion`, `no_correlativo_hallazgo`, `norma`, `id_unidad`, `id_dependencia`, `descripcion`, `id_enlace`, `id_analista`, `fecha_accion`, `id_tipo_accion`, `fecha_recepecion`,`id_proceso`) " +
                    "VALUES ({0}, {1}, {2}, '{3}', {4}, {5},'{6}', {7},'{8}',now() ,{9}, '{10}',{11}); ",
                    accion.id_informe,accion.id_accion,accion.no_correlativo_accion,accion.norma,accion.id_unidad,accion.id_dependecia,accion.descripcion,accion.id_enlace,accion.id_analista,
                    accion.id_tipo_accion,accion.fecha_recepcion,accion.id_proceso);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();
                conectar.CerrarConexion();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

        }

        public int AlmacenarEncabezado(mInformeCorrecion mInforme)
        {
            int result = 0;
            try
            {
                conectar = new DBConexion();
                conectar.AbrirConexion();
                string query = string.Format("Insert Into sgc_informe_resultados(anio_inicio,no_informe,fecha_informe) Values({0},'{1}','{2}')",
                    mInforme.anio_inicio,mInforme.no_informe,mInforme.fecha_informe);
                MySqlCommand cmd = new MySqlCommand(query, conectar.conectar);
                cmd.ExecuteNonQuery();
                query = "select @@IDENTITY;";
                cmd = new MySqlCommand(query, conectar.conectar);
                MySqlDataReader reader =  cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = int.Parse(reader[0].ToString());
                }
                conectar.CerrarConexion();
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public mInformeCorrecion BuscarEncabezado(string noInforme,int anio)
        {
            mInformeCorrecion informe = new mInformeCorrecion();
            try
            {
                conectar = new DBConexion();
                DataSet tabla = new DataSet();
                conectar.AbrirConexion();
                string query = string.Format("Select id_correlativo,cantidad_acciones,Date_format(fecha_informe,'%Y-%m-%d') fecha from sgc_informe_resultados where no_informe ='{0}' and anio_inicio={1}", noInforme,anio);
                MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
                consulta.Fill(tabla);
                conectar.CerrarConexion();
                if (tabla.Tables[0].Rows.Count > 0)
                {
                    informe.id_correlativo = int.Parse(tabla.Tables[0].Rows[0]["id_correlativo"].ToString());
                    informe.fecha_informe = (tabla.Tables[0].Rows[0]["fecha"].ToString());
                    informe.cantidad_acciones = int.Parse(tabla.Tables[0].Rows[0]["cantidad_acciones"].ToString());
                    informe.anio_inicio = anio;
                    informe.no_informe = noInforme;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return informe;
        }

        public DataSet ListadoAcciones (int id)
        {
            DataSet result = new DataSet();
            conectar = new DBConexion();
            conectar.AbrirConexion();
            string query = string.Format("select ca.Accion,ag.no_correlativo_hallazgo correlativo,ag.norma as 'Punto de Norma',p.Proceso,u.Unidad,d.Unidad Dependencia,ag.descripcion, "+
                    "concat(ee.Nombre, ' ', ee.Apellido) Enlace,concat(ea.Nombre, ' ', ea.Apellido) Analista,ag.fecha_recepecion,ta.accion as 'Tipo Accion',ag.fecha_accion "+
                        "from sgc_acciones_generadas ag inner join sgc_ccl_acciones_generadas ca on ca.id_acciones = ag.id_accion "+
                    "inner join sgc_proceso p on p.id_proceso = ag.id_proceso inner join sgc_unidad u on u.id_unidad = ag.id_unidad "+
                    "inner join sgc_unidad d on d.id_unidad = ag.id_dependencia  inner join sgc_empleados ea on ea.id_empleado = ag.id_analista "+
                    "inner join sgc_empleados ee on ee.id_empleado = ag.id_enlace inner join sgc_tipo_accion ta on ta.id_tipo_accion = ag.id_tipo_accion where ag.id_informe = {0} ",
                    id);
            MySqlDataAdapter consulta = new MySqlDataAdapter(query, conectar.conectar);
            consulta.Fill(result);
            conectar.CerrarConexion();
            return result;
        }
    }
}
