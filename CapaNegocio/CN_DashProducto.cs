using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_DashProducto
    {
        private CD_DashProductos oCapaDatos = new CD_DashProductos();

        public List<DashProductos> ObtenerProductos()
        {
            return oCapaDatos.ObtenerProductos();
        }
    }
}
