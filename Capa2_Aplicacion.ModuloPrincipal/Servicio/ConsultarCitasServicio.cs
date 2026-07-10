using System;
using System.Collections.Generic;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicio
{
    public class ConsultarCitasServicio
    {
        private AccesoSQLServer accesoSQLServer;
        private CitaSQL citaSQL;
        private PacienteSQL pacienteSQL;
        private MedicoSQL medicoSQL;
        private EspecialidadSQL especialidadSQL;

        public ConsultarCitasServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            citaSQL = new CitaSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
            especialidadSQL = new EspecialidadSQL(accesoSQLServer);
        }

        public List<CitaDTO> buscarCitasPorFiltros(int idMedicoAutenticado, DateTime? fechaEspecifica, DateTime? fechaInicio, DateTime? fechaFin, String estadoCita)
        {
            if (!fechaEspecifica.HasValue && !fechaInicio.HasValue && !fechaFin.HasValue && String.IsNullOrWhiteSpace(estadoCita))
            {
                throw new ArgumentException("Debe seleccionar al menos un criterio de búsqueda para realizar la consulta.");
            }

            List<Cita> listaCitas;
            List<CitaDTO> listaCitasDTO = new List<CitaDTO>();

            try
            {
                accesoSQLServer.AbrirConexion();

                listaCitas = citaSQL.consultarCitasPorFiltros(idMedicoAutenticado, fechaEspecifica, fechaInicio, fechaFin, estadoCita);

                foreach (Cita cita in listaCitas)
                {
                    cita.Paciente = pacienteSQL.buscarPorId(cita.Id_paciente);
                    cita.Medico = medicoSQL.BuscarPorId(cita.Id_medico);
                    cita.Medico.Especialidad = especialidadSQL.BuscarPorId(cita.Medico.Id_especialidad);

                    CitaDTO citaDTO = new CitaDTO
                    {
                        Id_cita = cita.Id_cita,
                        Fecha_cita = cita.Fecha_cita,
                        Hora_cita = cita.Hora_cita,
                        Motivo = cita.Motivo,
                        Estado = cita.Estado,
                        Observaciones = cita.Observaciones,

                        Id_paciente = cita.Paciente.Id_paciente,
                        PacienteDocumento = cita.Paciente.Numero_documento,
                        PacienteNombres = cita.Paciente.Nombres,
                        PacienteApellidos = cita.Paciente.Apellidos,

                        Id_medico = cita.Medico.Id_medico,
                        MedicoNombres = cita.Medico.Nombres,
                        MedicoApellidos = cita.Medico.Apellidos,

                        EspecialidadNombre = (cita.Medico.Especialidad != null) ? cita.Medico.Especialidad.Nombre : ""
                    };

                    listaCitasDTO.Add(citaDTO);
                }

                accesoSQLServer.CerrarConexion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CerrarConexion();
                throw ex; 
            }

            return listaCitasDTO;
        }
    }
}