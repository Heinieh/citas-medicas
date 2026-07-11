using System;
using System.Web.Mvc;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UsuarioLogueado"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
