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
    public class CD_Ubicacion
    {
        public List<Departamento> ObtenerDepartamentos()
        {
            List<Departamento> lstDepartamentos = new List<Departamento>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    string query = "Select * From Departamento";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read())
                        {
                            Departamento depto = new Departamento();
                            depto.IdDepartamento = dr["IdDepartamento"].ToString();
                            depto.Descripcion = dr["Descripcion"].ToString();

                            lstDepartamentos.Add(depto);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<Departamento>();
            }
            return lstDepartamentos;
        }

        public List<Municipio> ObtenerMunicipios(string iddepartamento)
        {
            List<Municipio> lstMunicipios= new List<Municipio>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    string query = "Select * From Municipio Where IdDepartamento = @IdDepartamento";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@IdDepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Municipio muni = new Municipio();
                            muni.IdMunicipio = dr["IdMunicipio"].ToString();
                            muni.Descripcion = dr["Descripcion"].ToString();

                            lstMunicipios.Add(muni);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<Municipio>();
            }
            return lstMunicipios;
        }
    }
}
