using System;

namespace Capa3_Dominio.ModuloPrincipal.Entidad
{
    public class Usuario
    {
        private int id_usuario;
        private string username;
        private string password;
        private string rol;
        private int? id_medico;

        public int Id_usuario { get => id_usuario; set => id_usuario = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Rol { get => rol; set => rol = value; }
        public int? Id_medico { get => id_medico; set => id_medico = value; }
    }
}
