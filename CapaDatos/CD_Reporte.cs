using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Reporte
    {

        public List<Reporte> ReporteVentas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lstRepVtas = new List<Reporte>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_Sales_Report", con);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) {
                        while (dr.Read())
                        {
                            lstRepVtas.Add(
                                new Reporte()
                                {
                                    FechaVenta = dr["FechaVenta"].ToString(),
                                    Cliente = dr["Cliente"].ToString(),
                                    Producto = dr["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"]),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    Total = Convert.ToDecimal(dr["Total"]),
                                    IdTransaccion = dr["IdTransaccion"].ToString()
                                });
                        }
                    };
                }
            }
            catch (Exception)
            {
                return new List<Reporte>();
            }
            return lstRepVtas;
        }

        public DashBoard VerDashBoard()
        {
            DashBoard objeto = new DashBoard();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_Report_Dashboard", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new DashBoard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                return objeto = new DashBoard();
            }
            return objeto;
        }
    }
}
