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
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
                        
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ShoppingCartExists", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ShoppingCartTrans", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public int CantidadEnCarrito(int idcliente)
        {
            int Resultado = 0;
           
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ShoppingCartQTY", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    Resultado = Convert.ToInt32( cmd.ExecuteScalar() ); //IMPORTANTE : el "cmd" decuelve un RECORDESET(Objetco)
                }                                                       //con el resultado del COUNT del Select, por eso hay q convertirlo a un INt Escalar 
            }
            catch (Exception)
            {
                Resultado = 0;
            }
            return Resultado;
        }


        public List<Carrito> ListarProductosEnCarrito(int idcliente)
        {
            List<Carrito> lstProductos = new List<Carrito>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {

                    string str = "Select * from GetCustomerShoppingCart(@idcliente)";

                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lstProductos.Add( new Carrito()
                            {
                                oProducto = new Producto()
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    oMarca = new Marca() { Descripcion = dr["DesMarca"].ToString() },
                                    Precio = Convert.ToDecimal(dr["Precio"]),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"])
                            });
                        }

                    }
                }
            }
            catch (Exception )
            {
                return new List<Carrito>();
            }
            return lstProductos;
        }

        public bool ELiminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("DeleteShopingCart", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }

    }
}
