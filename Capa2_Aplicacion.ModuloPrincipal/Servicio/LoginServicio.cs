using System;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Capa4_Persistencia.SqlServer.ModuloPrincipal;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicio
{
    public class LoginServicio
    {
        private AccesoSQLServer accesoSQLServer;
        private UsuarioSQL usuarioSQL;

        public LoginServicio()
        {
            accesoSQLServer = new AccesoSQLServer();
            usuarioSQL = new UsuarioSQL(accesoSQLServer);
        }

        public UsuarioDTO IniciarSesion(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("El usuario y la contraseña son campos obligatorios.");
            }

            UsuarioDTO usuarioDTO = null;

            try
            {
                accesoSQLServer.AbrirConexion();
                Usuario usuario = usuarioSQL.BuscarPorCredenciales(username, password);
                accesoSQLServer.CerrarConexion();

                if (usuario != null)
                {
                    usuarioDTO = new UsuarioDTO
                    {
                        Id_usuario = usuario.Id_usuario,
                        Username = usuario.Username,
                        Rol = usuario.Rol,
                        Id_medico = usuario.Id_medico
                    };
                }
            }
            catch (Exception ex)
            {
                accesoSQLServer.CerrarConexion();
                throw ex;
            }

            return usuarioDTO;
        }
    }
}
