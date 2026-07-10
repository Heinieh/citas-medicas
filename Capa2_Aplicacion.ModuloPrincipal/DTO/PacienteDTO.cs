using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa2_Aplicacion.ModuloPrincipal.DTO
{
    public class PacienteDTO
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
    }
}