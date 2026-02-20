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
    public class CD_DashProductos
    {
        public List<DashProductos> ObtenerProductos()
        {
            List<DashProductos> oLista = new List<DashProductos>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Dsh_ProductsLast3Months", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new DashProductos()
                            {
                                Producto = dr["Producto"].ToString(),
                                Cantidad = int.Parse(dr["Cantidad"].ToString())
                            });

                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<DashProductos>();
            }
            return oLista;
        }

    }
}
