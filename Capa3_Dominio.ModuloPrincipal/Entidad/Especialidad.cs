using System;
using System.Collections.Generic;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Especialidad
    {
        private int id_especialidad;
        private string nombre;
        private string descripcion;
        private List<Medico> medicos;

        public int Id_especialidad { get => id_especialidad; set => id_especialidad = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public List<Medico> Medicos { get => medicos; set => medicos = value; }


        // ====================================================================
        // Métodos de Regla de Negocio (Experto en Información)
        // ====================================================================

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Verifica si la especialidad cuenta actualmente con médicos asignados en la clínica.
        /// </summary>
        public Boolean TieneMedicosAsignados()
        {
            // Verificamos que la lista no sea nula antes de contar
            if (this.medicos == null) return false;

            return this.medicos.Count > 0;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Obtiene la cantidad total de médicos que pertenecen a esta especialidad.
        /// </summary>
        public int ObtenerCantidadMedicos()
        {
            // Si la lista es nula, significa que hay 0 médicos
            if (this.medicos == null) return 0;

            return this.medicos.Count;
        }

        /// <summary>
        /// REGLAS DE NEGOCIO
        /// Valida que la especialidad tenga los datos mínimos requeridos para ser registrada o mostrada.
        /// </summary>
        public Boolean ValidarDatosEspecialidad()
        {
            // El nombre de la especialidad es obligatorio
            if (string.IsNullOrWhiteSpace(this.nombre))
            {
                return false;
            }

            return true;
        }
    }
}
