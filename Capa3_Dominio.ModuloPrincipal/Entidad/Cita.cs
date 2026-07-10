using System;
using System.Collections.Generic;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Cita
    {
        private int id_cita;
        private int id_paciente;
        private int id_medico;
        private DateTime fecha_cita;
        private TimeSpan hora_cita;
        private string motivo;
        private string estado;
        private string observaciones;
        private Paciente paciente;
        private Medico medico;

        // La clínica atiende desde las 8:00 AM (8, 0, 0)
        private static readonly TimeSpan HORA_INICIO_ATENCION = new TimeSpan(8, 0, 0);
        // La clínica atiende hasta las 6:00 PM (18, 0, 0)
        private static readonly TimeSpan HORA_FIN_ATENCION = new TimeSpan(18, 0, 0);

        public int Id_cita { get => id_cita; set => id_cita = value; }
        public int Id_paciente { get => id_paciente; set => id_paciente = value; }
        public int Id_medico { get => id_medico; set => id_medico = value; }
        public DateTime Fecha_cita { get => fecha_cita; set => fecha_cita = value; }
        public TimeSpan Hora_cita { get => hora_cita; set => hora_cita = value; }
        public string Motivo { get => motivo; set => motivo = value; }
        public string Estado { get => estado; set => estado = value; }
        public string Observaciones { get => observaciones; set => observaciones = value; }
        public Paciente Paciente { get => paciente; set => paciente = value; }
        public Medico Medico { get => medico; set => medico = value; }


        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Determina si la cita actual se encuentra activa o pendiente de atención basándose en su estado.
        /// </summary>
        public Boolean EsActiva()
        {
            if (string.IsNullOrEmpty(this.estado)) return false;

            return this.estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) ||
                   this.estado.Equals("Confirmada", StringComparison.OrdinalIgnoreCase) ||
                   this.estado.Equals("Programada", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Verifica si la hora solicitada para la cita se encuentra dentro del rango de horario de atención de la clínica.
        /// </summary>
        public Boolean EstaDentroDelHorarioAtencion()
        {
            return this.hora_cita >= HORA_INICIO_ATENCION && this.hora_cita <= HORA_FIN_ATENCION;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Valida si el horario de la cita solicitada no se está programando en el pasado.
        /// </summary>
        public Boolean VerificarHorarioFuturoValido()
        {
            // Si la cita es para hoy, la hora de la cita debe ser mayor a la hora actual
            if (this.fecha_cita.Date == DateTime.Today)
            {
                return this.hora_cita > DateTime.Now.TimeOfDay;
            }

            // Si no es hoy, al menos la fecha debe ser en el futuro
            return this.fecha_cita.Date > DateTime.Today;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Realiza la evaluación integral de las directrices y reglas operativas requeridas antes de asentar una nueva cita en el dominio.
        /// </summary>
        public Boolean ValidarReglasDeNegocioCita()
        {
            // 1. El paciente y el médico deben estar asignados
            if (this.paciente == null || this.medico == null)
            {
                return false;
            }

            // 2. La cita debe estar dentro del horario de trabajo de la clínica
            if (!EstaDentroDelHorarioAtencion())
            {
                return false;
            }

            // 3. El horario debe ser válido (no puede ser en el pasado)
            if (!VerificarHorarioFuturoValido())
            {
                return false;
            }

            // 4. Se debe especificar un motivo para la atención
            if (string.IsNullOrWhiteSpace(this.motivo))
            {
                return false;
            }

            return true;
        }
    }
}