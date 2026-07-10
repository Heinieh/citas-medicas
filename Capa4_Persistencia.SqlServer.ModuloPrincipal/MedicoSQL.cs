using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class MedicoSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public MedicoSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

        public List<Medico> BuscarTodos()
        {
            List<Medico> listaMedicos = new List<Medico>();
            Medico medico;
            string procedimientoSQL = "sel_consultar_medicos";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    while (resultadoSQL.Read())
                    {
                        medico = ObtenerMedico(resultadoSQL);
                        listaMedicos.Add(medico);
                    }
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaMedicos;
        }

        public Medico BuscarPorId(int idMedico)
        {
            Medico medico;
            string procedimientoSQL = "sel_consultar_id_medico";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", idMedico));

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    if (resultadoSQL.Read())
                    {
                        medico = ObtenerMedico(resultadoSQL);
                    }
                    else
                    {
                        throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.NO_EXISTE_REGISTRO);
                    }
                }
            }
            catch (ExcepcionMedicoInvalido ex)
            {
                throw ex;
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return medico;
        }


        private Medico ObtenerMedico(SqlDataReader resultadoSQL)
        {
            Medico medico = new Medico
            {
                Id_medico = resultadoSQL.GetInt32(0),
                Nombres = resultadoSQL.GetString(1),
                Apellidos = resultadoSQL.GetString(2),
                Colegiatura = resultadoSQL.IsDBNull(3) ? null : resultadoSQL.GetString(3),
                Telefono = resultadoSQL.IsDBNull(4) ? null : resultadoSQL.GetString(4),

                Id_especialidad = resultadoSQL.GetInt32(5)
            };

            return medico;
        }

      
        public void Guardar(Medico medico)
        {
            string procedimientoSQL = "ins_crear_medico";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pNombres", medico.Nombres));
                comandoSQL.Parameters.Add(new SqlParameter("@pApellidos", medico.Apellidos));
                comandoSQL.Parameters.Add(new SqlParameter("@pColegiatura", string.IsNullOrEmpty(medico.Colegiatura) ? DBNull.Value : (object)medico.Colegiatura));
                comandoSQL.Parameters.Add(new SqlParameter("@pTelefono", string.IsNullOrEmpty(medico.Telefono) ? DBNull.Value : (object)medico.Telefono));
                comandoSQL.Parameters.Add(new SqlParameter("@pIdEspecialidad", medico.Id_especialidad));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_CREACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Medico medico)
        {
            string procedimientoSQL = "upd_modificar_medico";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", medico.Id_medico));
                comandoSQL.Parameters.Add(new SqlParameter("@pNombres", medico.Nombres));
                comandoSQL.Parameters.Add(new SqlParameter("@pApellidos", medico.Apellidos));
                comandoSQL.Parameters.Add(new SqlParameter("@pColegiatura", string.IsNullOrEmpty(medico.Colegiatura) ? DBNull.Value : (object)medico.Colegiatura));
                comandoSQL.Parameters.Add(new SqlParameter("@pTelefono", string.IsNullOrEmpty(medico.Telefono) ? DBNull.Value : (object)medico.Telefono));
                comandoSQL.Parameters.Add(new SqlParameter("@pIdEspecialidad", medico.Id_especialidad));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_ACTUALIZACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Eliminar(Medico medico)
        {
            string procedimientoSQL = "del_eliminar_medico";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", medico.Id_medico));

                comandoSQL.ExecuteNonQuery();
            }
            catch (ExcepcionMedicoInvalido ex)
            {
                throw ex;
            }
            catch (SqlException)
            {
                throw new ExcepcionMedicoInvalido(ExcepcionMedicoInvalido.ERROR_DE_ELIMINACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}