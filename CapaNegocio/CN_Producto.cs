using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Producto
    {

        private CD_Producto oCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return oCapaDato.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nomobre del Producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion del Producto no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0) 
            {
                Mensaje = "Debe seleccionar una Marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el Precio del Producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el Stock del Producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oCapaDato.Registrar(obj, out Mensaje);
            }
            else
                return 0;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nomobre del Producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion del Producto no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0) 
            {
                Mensaje = "Debe seleccionar una Marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el Precio del Producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el Stock del Producto";
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

        public bool GuardarDatosImagen(Producto obj, out string Mensaje) => oCapaDato.GuardarDatosImagen(obj, out Mensaje);
    }
}
