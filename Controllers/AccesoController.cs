using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso  
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarPassword()
        {
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.Encryptar(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (oUsuario.Restablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario; // permite compartir info entre multiples Vistas q estan dentro de un mismo Controlador (viewbag solo comparte info en la vista en ka q estas)    
                    return RedirectToAction("CambiarPassword"); // Cuando la vista se encuentra en el mismo controlador em q estas llamandola no hay necesidad de enviar el parametro "Controlador" en el redirectToAction
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult CambiarPassword(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.Encryptar(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            // -------------------------------------------------------------------
            else if (string.IsNullOrEmpty(nuevaclave) || string.IsNullOrWhiteSpace(nuevaclave))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vClave"] = claveactual;
                ViewBag.Error = "La nueva contraseña no puede estar en blanco";
                return View();
            }
            else if (string.IsNullOrEmpty(confirmarclave) || string.IsNullOrWhiteSpace(confirmarclave))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vClave"] = claveactual;
                ViewBag.Error = "La confirmacion de la contraseña no puede estar en blanco";
                return View();
            }
            // -------------------------------------------------------------------
            else if (nuevaclave != confirmarclave)
            {

                TempData["IdUsuario"] = idusuario;
                ViewData["vClave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";
            nuevaclave = CN_Recursos.Encryptar(nuevaclave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarPassword(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index","Acceso"); // Estamos en el controlador Acceso por lo cual no es necesario pasar el paramtro controlador ... pero esta vez si se lo voy  a pasar
            }
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }


        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            //Usuario ousuario = new Usuario();

            Usuario ousuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo).FirstOrDefault();
            
            if (ousuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().RestablecerPassword(ousuario.IdUsuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
            
        }


        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
    // vignabadro@gufum.com ricota
}