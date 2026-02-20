using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte oCapaDato = new  CD_Reporte();

        public List<Reporte> ReporteVentas(string fechainicio, string fechafin, string idtransaccion)
        {
            return oCapaDato.ReporteVentas(fechainicio, fechafin, idtransaccion);
        }

        public DashBoard VerDashBoard()
        {
            return oCapaDato.VerDashBoard();
        }
    }
}
