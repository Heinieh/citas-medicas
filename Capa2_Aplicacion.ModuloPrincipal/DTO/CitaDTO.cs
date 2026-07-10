using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa2_Aplicacion.ModuloPrincipal.DTO
{
    public class CitaDTO
    {
        private int id_cita;
        private DateTime fecha_cita;
        private TimeSpan hora_cita;
        private string motivo;
        private string estado;
        private string observaciones;

        // Datos del Paciente
        private int id_paciente;
        private string pacienteDocumento;
        private string pacienteNombres;
        private string pacienteApellidos;

        // Datos Médico y su especialidad
        private int id_medico;
        private string medicoNombres;
        private string medicoApellidos;
        private string especialidadNombre;

        public int Id_cita { get => id_cita; set => id_cita = value; }
        public DateTime Fecha_cita { get => fecha_cita; set => fecha_cita = value; }
        public TimeSpan Hora_cita { get => hora_cita; set => hora_cita = value; }
        public string Motivo { get => motivo; set => motivo = value; }
        public string Estado { get => estado; set => estado = value; }
        public string Observaciones { get => observaciones; set => observaciones = value; }

        public int Id_paciente { get => id_paciente; set => id_paciente = value; }
        public string PacienteDocumento { get => pacienteDocumento; set => pacienteDocumento = value; }
        public string PacienteNombres { get => pacienteNombres; set => pacienteNombres = value; }
        public string PacienteApellidos { get => pacienteApellidos; set => pacienteApellidos = value; }

        public int Id_medico { get => id_medico; set => id_medico = value; }
        public string MedicoNombres { get => medicoNombres; set => medicoNombres = value; }
        public string MedicoApellidos { get => medicoApellidos; set => medicoApellidos = value; }
        public string EspecialidadNombre { get => especialidadNombre; set => especialidadNombre = value; }
    }
}