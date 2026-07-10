using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionEspecialidadInvalida : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe la especialidad.";
        public const string NO_EXISTEN_REGISTROS = "No existen especialidades.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar las especialidades, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear la especialidad, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ACTUALIZACION = "No se pudo modificar la especialidad, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ELIMINACION = "No se pudo eliminar la especialidad, intente nuevamente o consulte con el administrador.";

        public ExcepcionEspecialidadInvalida(string mensaje)
            : base(mensaje)
        {
        }
    }
}
