using Recetas__SP_MinCapas.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetas__SP_MinCapas.Datos
{
    internal class RecetaDao : Helper, IRecetaDao
    {
        public bool ejecutarSP(Receta receta)
        {
            bool ok = true;
            SqlTransaction t = null;
            try
            {
                conectar();
                t = conexion.BeginTransaction();
                comando.Transaction = t;
                comando.CommandText = "SP_INSERTAR_RECETA";
                //comando.Parameters.AddWithValue("@id_receta", oReceta.RecetaNro);
                comando.Parameters.AddWithValue("@tipo_receta", receta.TipoReceta);
                comando.Parameters.AddWithValue("@nombre", receta.Nombre);
                if (receta.Chef != null)
                {
                    comando.Parameters.AddWithValue("@cheff", receta.Chef);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cheff", DBNull.Value);
                }
                comando.ExecuteNonQuery();
                comando.Parameters.Clear();
                int count = 1;
                foreach (DetalleReceta detalle in receta.DetalleRecetas)
                {
                    comando.CommandText = "SP_INSERTAR_DETALLES";
                    comando.Parameters.AddWithValue("@id_receta", receta.RecetaNro);
                    comando.Parameters.AddWithValue("@id_ingrediente", detalle.Ingrediente.IngredienteID);
                    comando.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    count++;
                    comando.ExecuteNonQuery();
                    comando.Parameters.Clear();

                }
                t.Commit();
            }
            catch (Exception)
            {
                t.Rollback();
                ok = false;
            }
            finally
            {
                desconectar();
            }
            return ok;
        }

        public DataTable ObtenerIngredientes()
        {
            conectar();
            comando.CommandText = "SP_CONSULTAR_INGREDIENTES";
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            desconectar();
            return tabla;
        }

        public int proximaReceta()
        {
            SqlParameter param = new SqlParameter("@next", SqlDbType.Int);
            try
            {
                conectar();
                comando.Parameters.Clear();
                comando.CommandText = "SP_PROXIMO_ID_RECETA";
                param.Direction = ParameterDirection.Output;
                comando.Parameters.Add(param);
                comando.ExecuteNonQuery();
                return (int)param.Value;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                desconectar();
            }

        }

    }

}
