using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente oCapaDato = new CD_Cliente();

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del Cliente no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.Encryptar(obj.Clave);
                return oCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }   
        }

        public List<Cliente> Listar()
        {
            return oCapaDato.Listar();
        }


        public bool CambiarPassword(int idCliente, string nuevopassword, out string Mensaje)
        {
            return oCapaDato.CambiarPassword(idCliente, nuevopassword, out Mensaje);
        }


        public bool RestablecerPassword(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarContrasena();
            bool resultado = oCapaDato.RestablecerPassword(idCliente, CN_Recursos.Encryptar(nuevaclave), out Mensaje);

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
            }
            else
            {
                Mensaje = "No se pudo restablecer la contrasena";
                return false;
            }
        }

    }
}
