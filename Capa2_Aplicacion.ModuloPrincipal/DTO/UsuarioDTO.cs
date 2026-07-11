using System;

namespace Capa2_Aplicacion.ModuloPrincipal.DTO
{
    public class UsuarioDTO
    {
        public int Id_usuario { get; set; }
        public string Username { get; set; }
        public string Rol { get; set; }
        public int? Id_medico { get; set; }
    }
}
