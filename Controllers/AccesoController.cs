using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Restablecer()
        {
            return View();
        }

        public ActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.Encryptar(clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();
            }
            else
            {
                if (oCliente.Restablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarPassword", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = String.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = String.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = String.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            // -------------------------------------------------------------------
            if (string.IsNullOrEmpty(objeto.Clave) || string.IsNullOrWhiteSpace(objeto.Clave))
            {
                //TempData["IdUsuario"] = idusuario;
                //ViewData["vClave"] = claveactual;
                ViewBag.Error = "La contraseña no puede estar en blanco";
                return View();
            }
            else if (string.IsNullOrEmpty(objeto.ConfirmarClave) || string.IsNullOrWhiteSpace(objeto.ConfirmarClave))
            {
                //TempData["IdUsuario"] = idusuario;
                //ViewData["vClave"] = claveactual;
                ViewBag.Error = "La confirmacion de la contraseña no puede estar en blanco";
                return View();
            }
            // -------------------------------------------------------------------

            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if (resultado > 0) 
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            //Usuario ousuario = new Usuario();

            Cliente ocliente = new CN_Cliente().Listar().Where(u => u.Correo == correo).FirstOrDefault();

            if (ocliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().RestablecerPassword(ocliente.IdCliente, correo, out mensaje);

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

        [HttpPost]
        public ActionResult CambiarPassword(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            Cliente oCliente = new Cliente();
            oCliente = new CN_Cliente().Listar().Where(c => c.IdCliente == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.Encryptar(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            // -------------------------------------------------------------------
            else if (string.IsNullOrEmpty(nuevaclave) || string.IsNullOrWhiteSpace(nuevaclave))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vClave"] = claveactual;
                ViewBag.Error = "La nueva contraseña no puede estar en blanco";
                return View();
            }
            else if (string.IsNullOrEmpty(confirmarclave) || string.IsNullOrWhiteSpace(confirmarclave))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vClave"] = claveactual;
                ViewBag.Error = "La confirmacion de la contraseña no puede estar en blanco";
                return View();
            }
            // -------------------------------------------------------------------
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vClave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";
            nuevaclave = CN_Recursos.Encryptar(nuevaclave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarPassword(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index", "Acceso"); // Estamos en el controlador Acceso por lo cual no es necesario pasar el paramtro controlador ... pero esta vez si se lo voy  a pasar
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}