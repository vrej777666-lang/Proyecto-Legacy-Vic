using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        // Encryptar con Sha256
        public static string Encryptar(string texto)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create() )
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();

        }

        public static string GenerarContrasena()
        {
            String clave = Guid.NewGuid().ToString("N").Substring(0, 10);
            return clave;
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(correo);
                mail.From = new MailAddress("tienditadlhorror@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("tienditadlhorror@gmail.com", "vqqfvcpovfcrijfn"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;

            }
            catch (Exception)
            {
                resultado = false;
            }

            return resultado;
        }

        public static string ConvertirBase64(string ruta, out  bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);


            }
            catch (Exception)
            {
                conversion = false;
            }

            return textoBase64;
        }

    }

}
