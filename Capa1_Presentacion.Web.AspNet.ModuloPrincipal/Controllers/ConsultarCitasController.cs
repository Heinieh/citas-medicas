using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal.Entidad;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class ConsultarCitasController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UsuarioLogueado"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new { estadoCorrecto = false, mensajeError = "Sesión expirada o no autenticada." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    filterContext.Result = RedirectToAction("Index", "Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult PaginaConsultarCitas()
        {
            return View();
        }

        [HttpGet]
        public JsonResult BuscarCitas(DateTime? fecha, String estado)
        {
            Boolean esCorrecto;
            String mensaje;
            List<CitaDTO> listaCitasDTO; 

            ConsultarCitasServicio consultarCitasServicio = new ConsultarCitasServicio();

            try
            {
                var usuario = Session["UsuarioLogueado"] as Capa2_Aplicacion.ModuloPrincipal.DTO.UsuarioDTO;
                if (usuario == null || !usuario.Id_medico.HasValue)
                {
                    throw new Exception("El usuario actual no está asociado a ningún médico especialista.");
                }
                int idMedicoAutenticado = usuario.Id_medico.Value;

                listaCitasDTO = consultarCitasServicio.buscarCitasPorFiltros(idMedicoAutenticado, fecha, null, null, estado);
                esCorrecto = true;
                mensaje = "";
            }
            catch (Exception e)
            {
                listaCitasDTO = null;
                esCorrecto = false;
                mensaje = e.Message;
            }

            return Json(new { data = listaCitasDTO, estadoCorrecto = esCorrecto, mensajeError = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}