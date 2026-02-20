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
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lstUsuarios = new List<Usuario>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("GetUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        usuario.Nombres = dr["Nombres"].ToString();
                        usuario.Apellidos = dr["Apellidos"].ToString();
                        usuario.Correo = dr["Correo"].ToString();
                        usuario.Clave = dr["Clave"].ToString();
                        usuario.Activo = Convert.ToBoolean(dr["Activo"]);
                        usuario.Restablecer = Convert.ToBoolean(dr["Restablecer"].ToString());

                        lstUsuarios.Add(usuario);
                    }
                    con.Close();
                }
            }
            catch (Exception )
            {
                return new List<Usuario>();
            }
            return lstUsuarios;
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;

            Mensaje = string.Empty;

            try
            {
                using ( SqlConnection cn = new SqlConnection(Conexion.cn) )
                {
                    SqlCommand cmd = new SqlCommand("AddNewUser", cn);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
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

        public bool Editar(Usuario obj, out string Mensaje)
        {

            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using ( SqlConnection cn = new SqlConnection(Conexion.cn) )
                {
                    SqlCommand cmd = new SqlCommand("UpdateUser", cn);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("DeleteUser", cn);
                    cmd.Parameters.AddWithValue("IdUsuario", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false ;
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }
            return Resultado;
        }

        public bool CambiarPassword(int idusuario, string nuevopassword, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UpdateUserPass", cn);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
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

        public bool RestablecerPassword(int idusuario, string pass, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ResetUserPass", cn);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
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
