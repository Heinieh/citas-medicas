using System;
using System.Data.SqlClient;
using Capa3_Dominio.ModuloPrincipal.Entidad;
using Capa4_Persistencia.SqlServer.ModuloBase;

namespace Capa4_Persistencia.SqlServer.ModuloPrincipal
{
    public class UsuarioSQL
    {
        private AccesoSQLServer accesoSQLServer;

        public UsuarioSQL(AccesoSQLServer accesoSQLServer)
        {
            this.accesoSQLServer = accesoSQLServer;
        }

        public Usuario BuscarPorCredenciales(string username, string password)
        {
            Usuario usuario = null;
            string procedimientoSQL = "sel_validar_usuario";

            try
            {
                SqlCommand comandoSQL = accesoSQLServer.ObtenerComandoDeProcedimiento(procedimientoSQL);
                comandoSQL.Parameters.Add(new SqlParameter("@pUsername", username));
                comandoSQL.Parameters.Add(new SqlParameter("@pPassword", password));

                using (SqlDataReader resultadoSQL = comandoSQL.ExecuteReader())
                {
                    if (resultadoSQL.Read())
                    {
                        usuario = new Usuario
                        {
                            Id_usuario = resultadoSQL.GetInt32(0),
                            Username = resultadoSQL.GetString(1),
                            Password = resultadoSQL.GetString(2),
                            Rol = resultadoSQL.GetString(3),
                            Id_medico = resultadoSQL.IsDBNull(4) ? (int?)null : resultadoSQL.GetInt32(4)
                        };
                    }
                }
            }
            catch (SqlException)
            {
                throw new Exception("Error al conectar con la base de datos para validar usuario.");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuario;
        }
    }
}
