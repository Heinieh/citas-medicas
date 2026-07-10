using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa2_Aplicacion.ModuloPrincipal.DTO
{
    public class EspecialidadDTO
    {
        private int id_especialidad;
        private string nombre;
        private string descripcion;

        public int Id_especialidad { get => id_especialidad; set => id_especialidad = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}