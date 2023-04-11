using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoBiblioteca.Logica
{

    public class MoviliariosLogica : Marca
    {
        private static MoviliariosLogica instancia = null;

        public MoviliariosLogica()
        {

        }

        public static MoviliariosLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new MoviliariosLogica();
                }

                return instancia;
            }
        }

        public List<Mobiliarios> Listar()
        {

            List<Mobiliarios> rptListarMoviliario = new List<Mobiliarios>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                //analizar
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select l.IdLibro,l.Titulo,l.RutaPortada,l.NombrePortada,");
                sb.AppendLine("a.IdAutor,a.Descripcion[DescripcionAutor],");
                sb.AppendLine("c.IdCategoria,c.Descripcion[DescripcionCategoria],");
                sb.AppendLine("e.IdEditorial,e.Descripcion[DescripcionEditorial],");
                sb.AppendLine("l.Ubicacion,l.Ejemplares,l.Estado");
                sb.AppendLine("from LIBRO l");
                sb.AppendLine("inner join AUTOR a on a.IdAutor = l.IdAutor");
                sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = l.IdCategoria");
                sb.AppendLine("inner join EDITORIAL e on e.IdEditorial = l.IdEditorial");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                cmd.CommandType = CommandType.Text;
                //Falta tabla sql
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListarMoviliario.Add(new Mobiliarios()
                        {
                            IdMobiliario = Convert.ToInt32(dr["IdLibro"].ToString()),
                            NombreObjeto = dr["Titulo"].ToString(),
                            Marca = dr["RutaPortada"].ToString(),
                            Color = dr["NombrePortada"].ToString(),

                            Ubicacion = dr["Ubicacion"].ToString(),
                            Cantidad = Convert.ToInt32(dr["Ejemplares"].ToString()),
                            base64 = Utilidades.convertirBase64(Path.Combine(dr["RutaPortada"].ToString(), dr["NombrePortada"].ToString())),
                            extension = Path.GetExtension(dr["NombrePortada"].ToString()).Replace(".", ""),
                            Estado = Convert.ToBoolean(dr["Estado"].ToString())
                        });
                    }
                    dr.Close();





                    return rptListarMoviliario;

                }
                catch (Exception ex)
                {
                    rptListarMoviliario = null;
                    return rptListarMoviliario;
                }
            }
        }


        public int Registrar(Mobiliarios objeto)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrarLibro", oConexion);
                    cmd.Parameters.AddWithValue("Titulo", objeto.IdMobiliario);
                    cmd.Parameters.AddWithValue("RutaPortada", objeto.NombreObjeto);
                    cmd.Parameters.AddWithValue("NombrePortada", objeto.Marca);
                    cmd.Parameters.AddWithValue("IdAutor", objeto.Color);
                    cmd.Parameters.AddWithValue("IdCategoria", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("IdEditorial", objeto.Cantidad);

                    cmd.Parameters.AddWithValue("Ejemplares", objeto.Estado);
                    cmd.Parameters.AddWithValue("Ejemplares", objeto.extension);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }


        public bool Modificar(Mobiliarios objeto)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_modificarLibro", oConexion);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.IdMobiliario);
                    cmd.Parameters.AddWithValue("Titulo", objeto.NombreObjeto);
                    cmd.Parameters.AddWithValue("IdAutor", objeto.Color);
                    cmd.Parameters.AddWithValue("IdCategoria", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("IdEditorial", objeto.Cantidad);
                    cmd.Parameters.AddWithValue("Ubicacion", objeto.extension);
                    cmd.Parameters.AddWithValue("Ejemplares", objeto);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool ActualizarRutaImagen(Mobiliarios objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.IdMobiliario);
                    cmd.Parameters.AddWithValue("NombrePortada", objeto.NombreObjeto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from LIBRO where IdLibro = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }
    }
}
