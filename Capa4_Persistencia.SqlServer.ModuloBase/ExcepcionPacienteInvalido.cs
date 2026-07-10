using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class ExcepcionPacienteInvalido : Exception
    {
        public const string NO_EXISTE_REGISTRO = "No existe el paciente.";
        public const string NO_EXISTEN_REGISTROS = "No existen pacientes.";
        public const string ERROR_DE_CONSULTA = "No se pudo consultar los pacientes, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_CREACION = "No se pudo crear el paciente, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ACTUALIZACION = "No se pudo modificar el paciente, intente nuevamente o consulte con el administrador.";
        public const string ERROR_DE_ELIMINACION = "No se pudo eliminar el paciente, intente nuevamente o consulte con el administrador.";

        public const string DOCUMENTO_DUPLICADO = "Ya existe un paciente registrado con ese mismo número de documento.";
        public const string PACIENTE_CON_CITAS = "No se puede eliminar el paciente porque tiene citas médicas en su historial.";

        public ExcepcionPacienteInvalido(string mensaje)
            : base(mensaje)
        {
        }
    }
}
