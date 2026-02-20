using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_DashVenta
    {
        private CD_DashVentas oCapaDatos = new CD_DashVentas();

        public List<DashVentas> ObtenerVentas()
        {
            return oCapaDatos.ObtenerVentas();
        }
    }
}
