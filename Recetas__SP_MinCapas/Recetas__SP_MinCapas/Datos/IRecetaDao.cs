using Recetas__SP_MinCapas.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetas__SP_MinCapas.Datos
{
    internal interface IRecetaDao
    {
        DataTable ObtenerIngredientes();
        int proximaReceta();
        bool ejecutarSP(Receta receta);

    }
}
