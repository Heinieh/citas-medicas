using System;
using System.Collections.Generic;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Medico
    {
        private int id_medico;
        private string nombres;
        private string apellidos;
        private string colegiatura;
        private string telefono;
        private int id_especialidad;
        private Especialidad especialidad;
        private List<Cita> citas;
        private const int LIMITE_CITAS_DIARIAS = 15;

        public int Id_medico { get => id_medico; set => id_medico = value; }
        public string Nombres { get => nombres; set => nombres = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public string Colegiatura { get => colegiatura; set => colegiatura = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public int Id_especialidad { get => id_especialidad; set => id_especialidad = value; }
        public Especialidad Especialidad { get => especialidad; set => especialidad = value; }
        public List<Cita> Citas { get => citas; set => citas = value; }


        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Verifica si el médico tiene citas programadas para el día de hoy que aún están pendientes de atención.
        /// </summary>
        public Boolean TieneCitasPendientesHoy()
        {
            if (citas == null) return false;

            foreach (Cita cita in citas)
            {
                if (cita.EsActiva() && cita.Fecha_cita.Date == DateTime.Today)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Evalúa si el médico tiene disponibilidad para atender a un nuevo paciente hoy (límite de 15 citas).
        /// </summary>
        public Boolean TieneDisponibilidadHoy()
        {
            if (citas == null) return true;

            int citasDeHoy = 0;
            foreach (Cita cita in citas)
            {
                if (cita.Fecha_cita.Date == DateTime.Today && cita.EsActiva())
                {
                    citasDeHoy++;
                }
            }

            return citasDeHoy < LIMITE_CITAS_DIARIAS;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Recupera la próxima cita inmediata que el médico debe atender en el día actual.
        /// </summary>
        public Cita ObtenerProximaCita()
        {
            if (citas == null) return null;

            Cita proxima = null;

            foreach (Cita cita in citas)
            {
                if (cita.EsActiva() && cita.Fecha_cita.Date == DateTime.Today)
                {
                    if (proxima == null || cita.Hora_cita < proxima.Hora_cita)
                    {
                        proxima = cita;
                    }
                }
            }
            return proxima;
        }
    }
}