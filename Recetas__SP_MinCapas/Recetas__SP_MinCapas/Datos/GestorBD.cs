using Recetas__SP_MinCapas.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetas__SP_MinCapas.Datos
{
    internal class GestorBD
    {
        private IRecetaDao Dao;
        public DataTable ObtenerIngredientes()
        {
            Dao = new RecetaDao();
            return Dao.ObtenerIngredientes();
        }
        public int proximaReceta()
        {
            Dao = new RecetaDao();
            return Dao.proximaReceta();
        }
        public bool ejecutarSP(Receta receta)
        {
            Dao = new RecetaDao();
            return Dao.ejecutarSP(receta);
        }

    }
}
