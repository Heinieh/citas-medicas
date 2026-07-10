using System;
using System.Collections.Generic;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Paciente
    {
        private int id_paciente;
        private string tipo_documento;
        private string numero_documento;
        private string nombres;
        private string apellidos;
        private char sexo;
        private DateTime fecha_nacimiento;
        private string direccion;
        private string telefono;
        private string correo;
        private string observaciones;
        private string tipo_sangre;
        private List<Cita> citas;

        // ====================================================================
        // Constantes para las Reglas de Negocio
        // ====================================================================
        private const int EDAD_MAYORIA = 18;

        public int Id_paciente { get => id_paciente; set => id_paciente = value; }
        public string Tipo_documento { get => tipo_documento; set => tipo_documento = value; }
        public string Numero_documento { get => numero_documento; set => numero_documento = value; }
        public string Nombres { get => nombres; set => nombres = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public char Sexo { get => sexo; set => sexo = value; }
        public DateTime Fecha_nacimiento { get => fecha_nacimiento; set => fecha_nacimiento = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Observaciones { get => observaciones; set => observaciones = value; }
        public string Tipo_sangre { get => tipo_sangre; set => tipo_sangre = value; }
        public List<Cita> Citas { get => citas; set => citas = value; }


        // ====================================================================
        // Métodos de Regla de Negocio (Experto en Información)
        // ====================================================================

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Calcula la edad exacta del paciente basándose en su fecha de nacimiento y la fecha actual.
        /// </summary>
        public int CalcularEdad()
        {
            int edad = DateTime.Today.Year - this.fecha_nacimiento.Year;

            // Si todavía no ha cumplido años en este año en curso, le restamos 1
            if (this.fecha_nacimiento.Date > DateTime.Today.AddYears(-edad))
            {
                edad--;
            }
            return edad;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Determina si el paciente es mayor de edad.
        /// </summary>
        public Boolean EsMayorDeEdad()
        {
            return CalcularEdad() >= EDAD_MAYORIA;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Verifica si el paciente ya cuenta actualmente con alguna cita activa (sin atender).
        /// </summary>
        public Boolean TieneCitasPendientes()
        {
            if (this.citas == null) return false; // Seguridad si la lista está vacía

            foreach (Cita cita in this.citas)
            {
                if (cita.EsActiva())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Recupera la última cita a la que el paciente asistió y fue atendido.
        /// </summary>
        public Cita ObtenerUltimaCitaAtendida()
        {
            if (this.citas == null) return null; // Seguridad si la lista está vacía

            Cita ultimaCita = null;

            foreach (Cita cita in this.citas)
            {
                if (!string.IsNullOrEmpty(cita.Estado) && cita.Estado.Equals("Atendida", StringComparison.OrdinalIgnoreCase))
                {
                    if (ultimaCita == null || cita.Fecha_cita > ultimaCita.Fecha_cita)
                    {
                        ultimaCita = cita;
                    }
                }
            }
            return ultimaCita;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Evalúa si al paciente se le permite agendar una nueva cita en la clínica.
        /// </summary>
        public Boolean PuedeAgendarNuevaCita()
        {
            // Regla 1: No se permite agendar si ya tiene una cita pendiente sin asistir
            if (TieneCitasPendientes())
            {
                return false;
            }

            // Regla 2: Obliga a tener al menos un dato de contacto registrado
            if (string.IsNullOrWhiteSpace(this.telefono) && string.IsNullOrWhiteSpace(this.correo))
            {
                return false;
            }

            return true;
        }
    }
}