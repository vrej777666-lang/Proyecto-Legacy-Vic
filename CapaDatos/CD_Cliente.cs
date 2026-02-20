using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("AddNewCustomer", cn);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }

        public List<Cliente> Listar()
        {
            List<Cliente> lstClientes = new List<Cliente>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("GetCustomer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Cliente Cliente = new Cliente();
                        Cliente.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        Cliente.Nombres = dr["Nombres"].ToString();
                        Cliente.Apellidos = dr["Apellidos"].ToString();
                        Cliente.Correo = dr["Correo"].ToString();
                        Cliente.Clave = dr["Clave"].ToString();
                        Cliente.Restablecer = Convert.ToBoolean(dr["Restablecer"].ToString());

                        lstClientes.Add(Cliente);
                    }
                    con.Close();
                }
            }
            catch (Exception)
            {
                return new List<Cliente>();
            }
            return lstClientes;
        }

        public bool CambiarPassword(int idcliente, string nuevopassword, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UpdateCustomerPass", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("NewPassword", nuevopassword);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool RestablecerPassword(int idcliente, string pass, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ResetCustomerPass", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("Pass", pass);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

    }
}
