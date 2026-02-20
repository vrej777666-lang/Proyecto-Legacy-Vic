using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_DashVentas
    {
        public List<DashVentas> ObtenerVentas()
        {
            List<DashVentas> oLista = new List<DashVentas>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Dsh_SalesLast3Months", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new DashVentas()
                            {
                                Mes = dr["Mes"].ToString(),
                                Valor = decimal.Parse(dr["Valor"].ToString())
                            });

                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<DashVentas>();
            }
            return oLista;
        }
    }
}
