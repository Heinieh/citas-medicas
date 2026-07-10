using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class EspecialidadSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public EspecialidadSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

        public List<Especialidad> BuscarTodos()
        {
            List<Especialidad> listaEspecialidades = new List<Especialidad>();
            Especialidad especialidad;
            string procedimientoSQL = "sel_consultar_especialidades";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                while (resultadoSQL.Read())
                {
                    especialidad = ObtenerEspecialidad(resultadoSQL);
                    listaEspecialidades.Add(especialidad);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionEspecialidadInvalida(ExcepcionEspecialidadInvalida.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaEspecialidades;
        }

        public Especialidad BuscarPorId(int especialidadId)
        {
            Especialidad especialidad;
            string procedimientoSQL = "sel_consultar_id_especialidad";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdEspecialidad", especialidadId));

                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();

                if (resultadoSQL.Read())
                {
                    especialidad = ObtenerEspecialidad(resultadoSQL);
                }
                else
                {
                    throw new ExcepcionEspecialidadInvalida(ExcepcionEspecialidadInvalida.NO_EXISTE_REGISTRO);
                }
            }
            catch (ExcepcionEspecialidadInvalida ex)
            {
                throw ex;
            }
            catch (SqlException)
            {
                throw new ExcepcionEspecialidadInvalida(ExcepcionEspecialidadInvalida.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return especialidad;
        }

        private Especialidad ObtenerEspecialidad(SqlDataReader resultadoSQL)
        {
            Especialidad especialidad = new Especialidad
            {
                Id_especialidad = resultadoSQL.GetInt32(0),
                Nombre = resultadoSQL.GetString(1),
                Descripcion = resultadoSQL.IsDBNull(2) ? null : resultadoSQL.GetString(2)
            };

            return especialidad;
        }


        public void Guardar(Especialidad especialidad)
        {
            string procedimientoSQL = "ins_crear_especialidad";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pNombre", especialidad.Nombre));
                comandoSQL.Parameters.Add(new SqlParameter("@pDescripcion", string.IsNullOrEmpty(especialidad.Descripcion) ? DBNull.Value : (object)especialidad.Descripcion));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionEspecialidadInvalida(ExcepcionEspecialidadInvalida.ERROR_DE_CREACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Especialidad especialidad)
        {
            string procedimientoSQL = "upd_modificar_especialidad";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdEspecialidad", especialidad.Id_especialidad));
                comandoSQL.Parameters.Add(new SqlParameter("@pNombre", especialidad.Nombre));
                comandoSQL.Parameters.Add(new SqlParameter("@pDescripcion", string.IsNullOrEmpty(especialidad.Descripcion) ? DBNull.Value : (object)especialidad.Descripcion));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionEspecialidadInvalida(ExcepcionEspecialidadInvalida.ERROR_DE_ACTUALIZACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Eliminar(Especialidad especialidad)
        {
            string procedimientoSQL = "del_eliminar_especialidad";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdEspecialidad", especialidad.Id_especialidad));

                comandoSQL.ExecuteNonQuery();
            }
            catch (ExcepcionEspecialidadInvalida ex)
            {
                throw ex;
            }
            catch (SqlException)
            {
                throw new ExcepcionEspecialidadInvalida(ExcepcionEspecialidadInvalida.ERROR_DE_ELIMINACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}