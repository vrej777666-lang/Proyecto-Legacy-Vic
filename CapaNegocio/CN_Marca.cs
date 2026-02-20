using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {

        private CD_Marca oCapaDato = new CD_Marca();

        public List<Marca> Listar()
        {
            return oCapaDato.Listar();
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la Marca no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return oCapaDato.Registrar(obj, out Mensaje);
            }
            else
                return 0;
        }

        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la Marca no puede ser vacio";
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

        public List<Marca> ListarMarcaXCategoria(int idcategoria)
        {
            return oCapaDato.ListarMarcaXCategoria(idcategoria);
        }

    }
}
