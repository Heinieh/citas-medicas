using System;
using System.Collections.Generic;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicio
{
    public class RegistrarCitasServicio
    {
        private AccesoSQLServer accesoSQLServer;
        private CitaSQL citaSQL;
        private PacienteSQL pacienteSQL;
        private MedicoSQL medicoSQL;

        public RegistrarCitasServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            citaSQL = new CitaSQL(accesoSQLServer);
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            medicoSQL = new MedicoSQL(accesoSQLServer);
        }

        public void GuardarCita(CitaDTO citaDTO)
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();

                // Validar que el paciente exista y cargar su historial
                Paciente paciente = pacienteSQL.buscarPorId(citaDTO.Id_paciente);
                if (paciente == null)
                {
                    throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.NO_EXISTE_REGISTRO);
                }
                paciente.Citas = citaSQL.consultarCitasPorPaciente(citaDTO.Id_paciente);

                // Validar que el médico exista y cargar su agenda
                Medico medico = medicoSQL.BuscarPorId(citaDTO.Id_medico);
                if (medico == null)
                {
                    throw new ExcepcionCitaInvalida("El médico especialista seleccionado no existe en el sistema.");
                }
                medico.Citas = citaSQL.consultarCitasPorMedico(citaDTO.Id_medico);

                // Mapeo de DTO a Entidad de Dominio
                Cita nuevaCita = new Cita
                {
                    Id_paciente = citaDTO.Id_paciente,
                    Id_medico = citaDTO.Id_medico,
                    Fecha_cita = citaDTO.Fecha_cita,
                    Hora_cita = citaDTO.Hora_cita,
                    Motivo = citaDTO.Motivo,
                    Estado = "Programada",
                    Observaciones = citaDTO.Observaciones,
                    Paciente = paciente,
                    Medico = medico
                };

                // Delegación de validaciones a la Capa de Dominio
                if (!paciente.PuedeAgendarNuevaCita())
                {
                    throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.PACIENTE_CON_PENDIENTES);
                }

                if (!medico.TieneDisponibilidadHoy())
                {
                    throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.MEDICO_NO_DISPONIBLE);
                }

                if (!nuevaCita.ValidarReglasDeNegocioCita())
                {
                    throw new ExcepcionCitaInvalida("La transacción ha sido denegada. Revise los campos obligatorios y el horario establecido.");
                }

                // Evitar superposición de horarios para el mismo médico
                Cita citaExistente = citaSQL.buscarCitaPorMedicoFechaYHora(citaDTO.Id_medico, citaDTO.Fecha_cita, citaDTO.Hora_cita);
                if (citaExistente != null && citaExistente.EsActiva())
                {
                    throw new ExcepcionCitaInvalida(ExcepcionCitaInvalida.HORARIO_SUPERPUESTO);
                }

                // Asentamiento en Base de Datos
                citaSQL.registrarCita(nuevaCita);

                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }
    }
}