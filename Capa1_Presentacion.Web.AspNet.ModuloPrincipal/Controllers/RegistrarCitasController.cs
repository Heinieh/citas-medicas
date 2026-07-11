using System;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal.Entidad;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class RegistrarCitasController : Controller
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

        public ActionResult PaginaRegistrarCita()
        {
            return View();
        }

        [HttpGet]
        public JsonResult BuscarPacientes(string criterio)
        {
            Boolean esCorrecto;
            String mensaje = "";
            System.Collections.Generic.List<PacienteDTO> listaPacientesDTO = null;

            try
            {
                GestionarPacienteServicio gestionarPacienteServicio = new GestionarPacienteServicio();
                listaPacientesDTO = gestionarPacienteServicio.ConsultarPacientesConFiltro(criterio);
                esCorrecto = true;
            }
            catch (Exception ex)
            {
                esCorrecto = false;
                mensaje = ex.Message;
            }

            return Json(new { data = listaPacientesDTO, estadoCorrecto = esCorrecto, mensajeError = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarCitaMedica(CitaDTO citaDTO)
        {
            Boolean esCorrecto;
            String mensaje;

            RegistrarCitasServicio registrarCitasServicio = new RegistrarCitasServicio();

            try
            {
                registrarCitasServicio.GuardarCita(citaDTO);

                esCorrecto = true;
                mensaje = "Cita médica registrada correctamente.";
            }
            catch (Exception e)
            {
                esCorrecto = false;
                mensaje = e.Message;
            }

            return Json(new { estadoCorrecto = esCorrecto, mensajeError = mensaje });
        }
    }
}