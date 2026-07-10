using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionCitaInvalida : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe la cita.";
        public const string NO_EXISTEN_REGISTROS = "No existen citas.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar las citas, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear la cita, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ACTUALIZACION = "No se pudo modificar la cita, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ELIMINACION = "No se pudo eliminar la cita, intente nuevamente o consulte con el administrador.";

        public const string PACIENTE_CON_PENDIENTES = "El paciente ya cuenta con una cita pendiente y no puede agendar otra.";
        public const string MEDICO_NO_DISPONIBLE = "El médico ha alcanzado su límite máximo de citas para el día de hoy.";
        public const string HORARIO_SUPERPUESTO = "El médico ya tiene una cita activa programada para ese mismo horario.";

        public ExcepcionCitaInvalida(string mensaje)
            : base(mensaje)
        {
        }
    }
}
