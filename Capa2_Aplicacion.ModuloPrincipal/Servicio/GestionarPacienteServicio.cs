using System;
using System.Collections.Generic;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicio
{
    public class GestionarPacienteServicio
    {
        private AccesoSQLServer accesoSQLServer;
        private PacienteSQL pacienteSQL;
        private CitaSQL citaSQL;

        public GestionarPacienteServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            pacienteSQL = new PacienteSQL(accesoSQLServer);
            citaSQL = new CitaSQL(accesoSQLServer);
        }

        public List<PacienteDTO> ConsultarPacientes(String criterio)
        {
            List<Paciente> listaPacientes;
            List<PacienteDTO> listaPacientesDTO = new List<PacienteDTO>();
            try
            {
                accesoSQLServer.AbrirConexion();
                listaPacientes = pacienteSQL.buscarPorFiltro(criterio);
                accesoSQLServer.CerrarConexion();

                // Mapeo de Entidad a DTO para enviar a la Vista
                foreach (Paciente paciente in listaPacientes)
                {
                    PacienteDTO pacienteDTO = new PacienteDTO
                    {
                        Id_paciente = paciente.Id_paciente,
                        Tipo_documento = paciente.Tipo_documento,
                        Numero_documento = paciente.Numero_documento,
                        Nombres = paciente.Nombres,
                        Apellidos = paciente.Apellidos,
                        Sexo = paciente.Sexo,
                        Fecha_nacimiento = paciente.Fecha_nacimiento,
                        Direccion = paciente.Direccion,
                        Telefono = paciente.Telefono,
                        Correo = paciente.Correo,
                        Observaciones = paciente.Observaciones,
                        Tipo_sangre = paciente.Tipo_sangre
                    };
                    listaPacientesDTO.Add(pacienteDTO);
                }
            }
            catch (Exception ex)
            {
                accesoSQLServer.CerrarConexion();
                throw ex;
            }
            return listaPacientesDTO;
        }

        public List<PacienteDTO> ConsultarPacientesConFiltro(string criterio)
        {
            List<PacienteDTO> listaPacientesDTO = new List<PacienteDTO>();
            try
            {
                accesoSQLServer.AbrirConexion();
                List<Paciente> listaPacientes = pacienteSQL.buscarPorFiltro(criterio);
                accesoSQLServer.CerrarConexion();

                foreach (Paciente paciente in listaPacientes)
                {
                    PacienteDTO pacienteDTO = new PacienteDTO
                    {
                        Id_paciente = paciente.Id_paciente,
                        Tipo_documento = paciente.Tipo_documento,
                        Numero_documento = paciente.Numero_documento,
                        Nombres = paciente.Nombres,
                        Apellidos = paciente.Apellidos,
                        Sexo = paciente.Sexo,
                        Fecha_nacimiento = paciente.Fecha_nacimiento,
                        Direccion = paciente.Direccion,
                        Telefono = paciente.Telefono,
                        Correo = paciente.Correo,
                        Observaciones = paciente.Observaciones,
                        Tipo_sangre = paciente.Tipo_sangre
                    };
                    listaPacientesDTO.Add(pacienteDTO);
                }
            }
            catch (Exception ex)
            {
                accesoSQLServer.CerrarConexion();
                throw ex;
            }
            return listaPacientesDTO;
        }

        public void GuardarPaciente(PacienteDTO pacienteDTO)
        {
            try
            {
                // Mapeo de DTO a Entidad para aplicar Reglas de Negocio
                Paciente paciente = new Paciente
                {
                    Id_paciente = pacienteDTO.Id_paciente,
                    Tipo_documento = pacienteDTO.Tipo_documento,
                    Numero_documento = pacienteDTO.Numero_documento,
                    Nombres = pacienteDTO.Nombres,
                    Apellidos = pacienteDTO.Apellidos,
                    Sexo = pacienteDTO.Sexo,
                    Fecha_nacimiento = pacienteDTO.Fecha_nacimiento,
                    Direccion = pacienteDTO.Direccion,
                    Telefono = pacienteDTO.Telefono,
                    Correo = pacienteDTO.Correo,
                    Observaciones = pacienteDTO.Observaciones,
                    Tipo_sangre = pacienteDTO.Tipo_sangre
                };

                accesoSQLServer.IniciarTransaccion();

                // Validación de duplicidad por Documento de Identidad
                Paciente pacientePorDocumento = pacienteSQL.buscarPorNumeroDocumento(paciente.Numero_documento);
                if (pacientePorDocumento != null)
                {
                    throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.DOCUMENTO_DUPLICADO);
                }

                pacienteSQL.insertar(paciente);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }

        public void ModificarPaciente(PacienteDTO pacienteDTO)
        {
            try
            {
                // Mapeo de DTO a Entidad
                Paciente paciente = new Paciente
                {
                    Id_paciente = pacienteDTO.Id_paciente,
                    Tipo_documento = pacienteDTO.Tipo_documento,
                    Numero_documento = pacienteDTO.Numero_documento,
                    Nombres = pacienteDTO.Nombres,
                    Apellidos = pacienteDTO.Apellidos,
                    Sexo = pacienteDTO.Sexo,
                    Fecha_nacimiento = pacienteDTO.Fecha_nacimiento,
                    Direccion = pacienteDTO.Direccion,
                    Telefono = pacienteDTO.Telefono,
                    Correo = pacienteDTO.Correo,
                    Observaciones = pacienteDTO.Observaciones,
                    Tipo_sangre = pacienteDTO.Tipo_sangre
                };

                accesoSQLServer.IniciarTransaccion();

                // Verificar colisiones de identidad con otros pacientes
                Paciente copiaPorDocumento = pacienteSQL.buscarPorNumeroDocumento(paciente.Numero_documento);
                if (copiaPorDocumento != null && copiaPorDocumento.Id_paciente != paciente.Id_paciente)
                {
                    throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.DOCUMENTO_DUPLICADO);
                }

                pacienteSQL.modificar(paciente);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }

        public void EliminarPaciente(PacienteDTO pacienteDTO)
        {
            try
            {
                accesoSQLServer.IniciarTransaccion();

                Paciente paciente = pacienteSQL.buscarPorId(pacienteDTO.Id_paciente);
                if (paciente == null)
                {
                    throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.NO_EXISTE_REGISTRO);
                }

                // RN-05: Evaluar si tiene citas médicas programadas
                paciente.Citas = citaSQL.consultarCitasPorPaciente(paciente.Id_paciente);

                if (paciente.TieneCitasPendientes())
                {
                    throw new ExcepcionPacienteInvalido(ExcepcionPacienteInvalido.PACIENTE_CON_CITAS);
                }

                pacienteSQL.eliminar(paciente.Id_paciente);
                accesoSQLServer.TerminarTransaccion();
            }
            catch (Exception ex)
            {
                accesoSQLServer.CancelarTransaccion();
                throw ex;
            }
        }
    }
}