using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private  CD_Usuarios oCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return oCapaDato.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarContrasena();
                string asunto = "Registro de Usuario";
                string mensaje_correo = "<h3>Su cuenta fue creada exitosamente</h3></br><p>Su contrasena para acceder es :!clave!</p> ";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                int Registrado = 0;
                obj.Clave = CN_Recursos.Encryptar(clave);
                Registrado = oCapaDato.Registrar(obj, out Mensaje);

                if (Registrado > 0) 
                {
                    bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);
                    if (respuesta)
                    {
                        return Registrado;
                    }
                    else
                    {
                        Mensaje = "No se puede enviar el correo";
                        return Registrado;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oCapaDato.Editar(obj, out Mensaje);
            }
            else
                return false;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return oCapaDato.Eliminar(id, out Mensaje);
        }


        public bool CambiarPassword(int idusuario, string nuevopassword, out string Mensaje)
        {
            return oCapaDato.CambiarPassword( idusuario, nuevopassword, out Mensaje);
        }


        public bool RestablecerPassword(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarContrasena();
            bool resultado = oCapaDato.RestablecerPassword(idusuario, CN_Recursos.Encryptar(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Restablecida";
                string mensaje_correo = "<h3>Su cuenta fue restablecida exitosamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p> ";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            } else
            {
                Mensaje = "No se pudo restablecer la contrasena";
                return false;
            }
        }

    }
}
