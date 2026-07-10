using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa2_Aplicacion.ModuloPrincipal.DTO
{
    public class MedicoDTO
    {
        private int id_medico;
        private string nombres;
        private string apellidos;
        private string colegiatura;
        private string telefono;

        // Datos Especialidad
        private int id_especialidad;
        private string especialidadNombre;

        public int Id_medico { get => id_medico; set => id_medico = value; }
        public string Nombres { get => nombres; set => nombres = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public string Colegiatura { get => colegiatura; set => colegiatura = value; }
        public string Telefono { get => telefono; set => telefono = value; }

        public int Id_especialidad { get => id_especialidad; set => id_especialidad = value; }
        public string EspecialidadNombre { get => especialidadNombre; set => especialidadNombre = value; }
    }
}