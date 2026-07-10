using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class CitaSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public CitaSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

        
        public List<Cita> consultarCitasPorPaciente(int idPaciente)
        {
            List<Cita> listaCitas = new List<Cita>();
            Cita cita;
            string procedimientoSQL = "sel_consultar_citas_por_paciente";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdPaciente", idPaciente));

                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    cita = ObtenerCita(resultadoSQL);
                    listaCitas.Add(cita);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaCitas;
        }

        public List<Cita> consultarCitasPorMedico(int idMedico)
        {
            List<Cita> listaCitas = new List<Cita>();
            Cita cita;
            string procedimientoSQL = "sel_consultar_citas_por_medico";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", idMedico));

                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    cita = ObtenerCita(resultadoSQL);
                    listaCitas.Add(cita);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaCitas;
        }

        public Cita buscarCitaPorMedicoFechaYHora(int idMedico, DateTime fechaCita, TimeSpan horaCita)
        {
            Cita cita = null;
            string procedimientoSQL = "sel_consultar_cita_medico_fecha_hora";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", idMedico));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaCita", fechaCita));
                comandoSQL.Parameters.Add(new SqlParameter("@pHoraCita", horaCita));

                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                if (resultadoSQL.Read())
                {
                    cita = ObtenerCita(resultadoSQL);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cita;
        }

        public List<Cita> consultarCitasPorFiltros(int idMedico, DateTime? fechaEspecifica, DateTime? fechaInicio, DateTime? fechaFin, string estadoCita)
        {
            List<Cita> listaCitas = new List<Cita>();
            Cita cita;
            string procedimientoSQL = "sel_consultar_citas_filtros";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", idMedico));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaEspecifica", (object)fechaEspecifica ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaInicio", (object)fechaInicio ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaFin", (object)fechaFin ?? DBNull.Value));
                comandoSQL.Parameters.Add(new SqlParameter("@pEstadoCita", string.IsNullOrEmpty(estadoCita) ? DBNull.Value : (object)estadoCita));

                SqlDataReader resultadoSQL = comandoSQL.ExecuteReader();
                while (resultadoSQL.Read())
                {
                    cita = ObtenerCita(resultadoSQL);
                    listaCitas.Add(cita);
                }
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CONSULTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaCitas;
        }

        private Cita ObtenerCita(SqlDataReader resultadoSQL)
        {
            Cita cita = new Cita
            {
                Id_cita = resultadoSQL.GetInt32(0),
                Id_paciente = resultadoSQL.GetInt32(1),
                Id_medico = resultadoSQL.GetInt32(2),
                Fecha_cita = resultadoSQL.GetDateTime(3),
                Hora_cita = resultadoSQL.GetTimeSpan(4),
                Motivo = resultadoSQL.GetString(5),
                Estado = resultadoSQL.GetString(6),
                Observaciones = resultadoSQL.IsDBNull(7) ? null : resultadoSQL.GetString(7)
            };

            return cita;
        }

        public void registrarCita(Cita cita)
        {
            string procedimientoSQL = "ins_crear_cita";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdPaciente", cita.Id_paciente));
                comandoSQL.Parameters.Add(new SqlParameter("@pIdMedico", cita.Id_medico));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaCita", cita.Fecha_cita));
                comandoSQL.Parameters.Add(new SqlParameter("@pHoraCita", cita.Hora_cita));
                comandoSQL.Parameters.Add(new SqlParameter("@pMotivo", cita.Motivo));
                comandoSQL.Parameters.Add(new SqlParameter("@pEstado", cita.Estado));
                comandoSQL.Parameters.Add(new SqlParameter("@pObservaciones", string.IsNullOrEmpty(cita.Observaciones) ? DBNull.Value : (object)cita.Observaciones));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_CREACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Cita cita)
        {
            string procedimientoSQL = "upd_modificar_cita";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdCita", cita.Id_cita));
                comandoSQL.Parameters.Add(new SqlParameter("@pFechaCita", cita.Fecha_cita));
                comandoSQL.Parameters.Add(new SqlParameter("@pHoraCita", cita.Hora_cita));
                comandoSQL.Parameters.Add(new SqlParameter("@pEstado", cita.Estado));
                comandoSQL.Parameters.Add(new SqlParameter("@pObservaciones", string.IsNullOrEmpty(cita.Observaciones) ? DBNull.Value : (object)cita.Observaciones));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_ACTUALIZACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Eliminar(Cita cita)
        {
            string procedimientoSQL = "del_eliminar_cita";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pIdCita", cita.Id_cita));

                comandoSQL.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.ERROR_DE_ELIMINACION);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}