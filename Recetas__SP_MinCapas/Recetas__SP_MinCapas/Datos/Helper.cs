using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetas__SP_MinCapas.Datos
{
    internal class Helper
    {
        protected SqlConnection conexion = new SqlConnection(Properties.Resources.cadenaConexion);
        protected SqlCommand comando = new SqlCommand();
        protected SqlParameter parametro = new SqlParameter();

        protected void conectar()
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
        }
        protected void desconectar()
        {
            conexion.Close();
        }
    }
}
