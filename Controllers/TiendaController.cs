using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().ToList().Where(p => p.IdProducto == idproducto).FirstOrDefault();
                
            if(oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }
            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categorias().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMarcaXCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaXCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
               p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
               p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&
               p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }


        [HttpPost]
        public JsonResult AgregarCarrito (int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;

            string mensje = string.Empty;

            string tipo = "success";

            string swltxt = "¿ Desea ir al carrito ?";

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensje);

            if (String.IsNullOrEmpty(mensje))
            {
                if (existe)
                {
                    tipo = "warning";
                    swltxt = "¿ Desea ir al carrito a verificar la compra ?";
                    mensje = "El producto ya existe en el carrito, pero fue adicionado !";
                }
                else
                {
                    mensje = "Agregado al carrito";
                    //"Agregado al carrito",        
                }
            }
       
            return Json(new { respuesta = respuesta, mensaje = mensje, tipo = tipo, swltxt = swltxt }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

        // PPPPPPPPPPPPPPPRRRRRRRRRRRRRRRRRRUUUUUUUBBBBBBBBBBAAAAAAAAAAAAA
        //[HttpPost]
        //public JsonResult ExisteEnCarrito(int idproducto)
        //{
        //    int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
        //    bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
        //    return Json(new { existe = existe }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ListarProductosEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new CN_Carrito().ListarProductosEnCarrito(idcliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    NombreImagen =  oc.oProducto.NombreImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad 
            }).ToList();

            // Opcion 1
            //var json = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            //json.MaxJsonLength = 500000000;
            // Opcion 2
            var jsonresult = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult; // json //Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensje);
           
            return Json(new { respuesta = respuesta, mensaje = mensje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ElimiarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensje = string.Empty;

            respuesta = new CN_Carrito().ELiminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult ObtenerDepartamentos()
        {
            List<Departamento> olista = new List<Departamento>();

            olista = new CN_Ubicacion().ObtenerDepartamentos();

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerMunicipios(string iddepartamento)
        {
            List<Municipio> olista = new List<Municipio>();

            olista = new CN_Ubicacion().ObtenerMunicipios(iddepartamento);

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Carrito()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-GT");
            detalle_venta.Columns.Add("IdProducto", typeof(int));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            foreach(Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = oCarrito.Cantidad * oCarrito.oProducto.Precio;

                total += subtotal;

                detalle_venta.Rows.Add( new object[]
                {
                    oCarrito.oProducto.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new { Status = true, Link = "/Tienda/PagoEfectuado?idTransaccion=code0001&status=true" }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> PagoEfectuado()
        {
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean(Request.QueryString["status"]);

            ViewData["Status"] = status;
            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];

                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = idtransaccion;

                string Mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out Mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;

            }

            return View();

        }

    }

}