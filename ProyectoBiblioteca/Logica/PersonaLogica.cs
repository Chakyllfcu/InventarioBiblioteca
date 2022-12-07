using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace ProyectoBiblioteca.Logica
{
    public class PersonaLogica
    {

        private static PersonaLogica instancia = null;

        public PersonaLogica()
        {

        }

        public static PersonaLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new PersonaLogica();
                }

                return instancia;
            }
        }


        public bool Registrar(Persona objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPersona", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", objeto.Apellido);
                    cmd.Parameters.AddWithValue("Correo", objeto.Correo);
                    cmd.Parameters.AddWithValue("Clave", objeto.Clave);
                    cmd.Parameters.AddWithValue("IdTipoPersona", objeto.oTipoPersona.IdTipoPersona);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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

        public bool Modificar(Persona objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarPersona", oConexion);
                    cmd.Parameters.AddWithValue("IdPersona", objeto.IdPersona);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", objeto.Apellido);
                    cmd.Parameters.AddWithValue("Correo", objeto.Correo);
                    cmd.Parameters.AddWithValue("Clave", objeto.Clave);
                    cmd.Parameters.AddWithValue("IdTipoPersona", objeto.oTipoPersona.IdTipoPersona);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

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


        public List<Persona> Listar()
        {
            List<Persona> Lista = new List<Persona>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    //AFP Y ARS CALCULADO DESDE EL SCRIPT DE SQL
                    sb.AppendLine("select p.IdPersona,p.Nombre,p.Apellido,p.Correo,p.Clave,p.Codigo,tp.IdTipoPersona,tp.Descripcion, p.Estado, ISNULL(p.Salario,0) AS Salario, ISNULL(p.Salario, 0)*0.0298 AS AFP, ISNULL(p.Salario, 0)*0.0302 AS ARS from persona p");
                    sb.AppendLine("LEFT join TIPO_PERSONA tp on tp.IdTipoPersona = p.IdTipoPersona");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Persona()
                            {
                                IdPersona = Convert.ToInt32(dr["IdPersona"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Codigo = dr["Codigo"].ToString(),
                                Salario = Convert.ToInt32(dr["Salario"]),
                                AFP = Convert.ToInt32(dr["AFP"]),
                                ARS = Convert.ToInt32(dr["ARS"]),
                                SalarioNeto = (Convert.ToInt32(dr["Salario"]) - Convert.ToInt32(dr["AFP"])) - Convert.ToInt32(dr["ARS"]),
                                oTipoPersona = new TipoPersona() { IdTipoPersona = Convert.ToInt32(dr["IdTipoPersona"]), Descripcion = dr["Descripcion"].ToString() },
                                Estado = Convert.ToBoolean(dr["Estado"]),

                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Persona>();
                }
            }
            return Lista;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from persona where IdPersona = @id", oConexion);
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