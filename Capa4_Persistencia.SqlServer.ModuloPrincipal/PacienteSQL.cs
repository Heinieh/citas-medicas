using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class PacienteSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public PacienteSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }
 
        public List<Paciente> buscarPorFiltro(string criterio)
        {
            List<Paciente> listaPacientes = new List<Paciente>();
            Paciente paciente;
            string procedimientoSQL = "sel_consultar_pacientes_filtro";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pCriterio", string.IsNullOrEmpty(criterio) ? "" : criterio));

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    while (resultadoSQL.Read())
                    {
                        paciente = ObtenerPaciente(resultadoSQL);
                        listaPacientes.Add(paciente);
                    }
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaPacientes;
        }

        public Paciente buscarPorId(int idPaciente)
        {
            Paciente paciente = null;
            string procedimientoSQL = "sel_consultar_id_paciente";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdPaciente", idPaciente));

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    if (resultadoSQL.Read())
                    {
                        paciente = ObtenerPaciente(resultadoSQL);
                    }
                    else
                    {
                        throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.NO_EXISTE_REGISTRO);
                    }
                }
            }
            catch (ExcepcionPacienteInvalido ex)
            {
                throw ex;
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return paciente;
        }

        public Paciente buscarPorNumeroDocumento(string documento)
        {
            Paciente paciente = null;
            string procedimientoSQL = "sel_consultar_documento_paciente";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pDocumento", documento));

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    if (resultadoSQL.Read())
                    {
                        paciente = ObtenerPaciente(resultadoSQL);
                    }
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return paciente; 
        }

        private Paciente ObtenerPaciente(SqlDataReader resultadoSQL)
        {
            Paciente paciente = new Paciente
            {
                Id_paciente = resultadoSQL.GetInt32(0),
                Tipo_documento = resultadoSQL.GetString(1),
                Numero_documento = resultadoSQL.GetString(2),
                Nombres = resultadoSQL.GetString(3),
                Apellidos = resultadoSQL.GetString(4),
                Sexo = resultadoSQL.GetString(5)[0],
                Fecha_nacimiento = resultadoSQL.GetDateTime(6),
                // Campos nulos: validacin de seguridad
                Direccion = resultadoSQL.IsDBNull(7) ? null : resultadoSQL.GetString(7),
                Telefono = resultadoSQL.IsDBNull(8) ? null : resultadoSQL.GetString(8),
                Correo = resultadoSQL.IsDBNull(9) ? null : resultadoSQL.GetString(9),
                Observaciones = resultadoSQL.IsDBNull(10) ? null : resultadoSQL.GetString(10),
                Tipo_sangre = resultadoSQL.GetString(11)
            };

            return paciente;
        }

        public void insertar(Paciente paciente)
        {
            string procedimientoSQL = "ins_crear_paciente";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pTipoDocumento", paciente.Tipo_documento));
                comandoSQL.Parameters.Add(new SqlParameter("@pNumeroDocumento", paciente.Numero_documento));
                comandoSQL.Parameters.Add(new SqlParameter("@pNombres", paciente.Nombres));
                comandoSQL.Parameters.Add(new SqlParameter("@pApellidos", paciente.Apellidos));
                comandoSQL.Parameters.Add(new SqlParameter("@pSexo", paciente.Sexo.ToString()));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaNacimiento", paciente.Fecha_nacimiento));

                comandoSQL.Parameters.Add(new SqlParameter("@pDireccion", string.IsNullOrEmpty(paciente.Direccion) ? DBNull.Value : (object)paciente.Direccion));
                comandoSQL.Parameters.Add(new SqlParameter("@pTelefono", string.IsNullOrEmpty(paciente.Telefono) ? DBNull.Value : (object)paciente.Telefono));
                comandoSQL.Parameters.Add(new SqlParameter("@pCorreo", string.IsNullOrEmpty(paciente.Correo) ? DBNull.Value : (object)paciente.Correo));
                comandoSQL.Parameters.Add(new SqlParameter("@pObservaciones", string.IsNullOrEmpty(paciente.Observaciones) ? DBNull.Value : (object)paciente.Observaciones));
                comandoSQL.Parameters.Add(new SqlParameter("@pTipoSangre", paciente.Tipo_sangre));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_CREACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void modificar(Paciente paciente)
        {
            string procedimientoSQL = "upd_modificar_paciente";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdPaciente", paciente.Id_paciente));
                comandoSQL.Parameters.Add(new SqlParameter("@pTipoDocumento", paciente.Tipo_documento));
                comandoSQL.Parameters.Add(new SqlParameter("@pNumeroDocumento", paciente.Numero_documento));
                comandoSQL.Parameters.Add(new SqlParameter("@pNombres", paciente.Nombres));
                comandoSQL.Parameters.Add(new SqlParameter("@pApellidos", paciente.Apellidos));
                comandoSQL.Parameters.Add(new SqlParameter("@pSexo", paciente.Sexo.ToString()));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaNacimiento", paciente.Fecha_nacimiento));

                comandoSQL.Parameters.Add(new SqlParameter("@pDireccion", string.IsNullOrEmpty(paciente.Direccion) ? DBNull.Value : (object)paciente.Direccion));
                comandoSQL.Parameters.Add(new SqlParameter("@pTelefono", string.IsNullOrEmpty(paciente.Telefono) ? DBNull.Value : (object)paciente.Telefono));
                comandoSQL.Parameters.Add(new SqlParameter("@pCorreo", string.IsNullOrEmpty(paciente.Correo) ? DBNull.Value : (object)paciente.Correo));
                comandoSQL.Parameters.Add(new SqlParameter("@pObservaciones", string.IsNullOrEmpty(paciente.Observaciones) ? DBNull.Value : (object)paciente.Observaciones));
                comandoSQL.Parameters.Add(new SqlParameter("@pTipoSangre", paciente.Tipo_sangre));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_ACTUALIZACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void eliminar(int idPaciente)
        {
            string procedimientoSQL = "del_eliminar_paciente";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdPaciente", idPaciente));

                comandoSQL.ExecuteNonQuery();
            }
            catch (ExcepcionPacienteInvalido ex)
            {
                throw ex;
            }
            catch (SqlException)
            {
                throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.ERROR_DE_ELIMINACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}