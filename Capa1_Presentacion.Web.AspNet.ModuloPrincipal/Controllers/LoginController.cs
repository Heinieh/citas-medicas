using System;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UsuarioLogueado"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult IniciarSesion(string username, string password)
        {
            Boolean esCorrecto = false;
            String mensaje = "";

            try
            {
                LoginServicio loginServicio = new LoginServicio();
                UsuarioDTO usuarioDTO = loginServicio.IniciarSesion(username, password);

                if (usuarioDTO != null)
                {
                    Session["UsuarioLogueado"] = usuarioDTO;
                    esCorrecto = true;
                }
                else
                {
                    mensaje = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(new { estadoCorrecto = esCorrecto, mensajeError = mensaje });
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session["UsuarioLogueado"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}
