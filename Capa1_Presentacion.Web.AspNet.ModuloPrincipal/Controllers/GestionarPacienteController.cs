using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal.Entidad;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class GestionarPacienteController : Controller
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

        public ActionResult PaginaGestionarPaciente()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarPacientes()
        {
            Boolean esCorrecto;
            String mensaje;
            List<PacienteDTO> listaPacientesDTO;
            GestionarPacienteServicio gestionarPacienteServicio = new GestionarPacienteServicio();

            try
            {
                listaPacientesDTO = gestionarPacienteServicio.ConsultarPacientes("");
                esCorrecto = true;
                mensaje = "";
            }
            catch (Exception e)
            {
                listaPacientesDTO = null;
                esCorrecto = false;
                mensaje = e.Message;
            }

            return Json(new { data = listaPacientesDTO, estadoCorrecto = esCorrecto, mensajeError = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarPaciente(PacienteDTO pacienteDTO)
        {
            Boolean esCorrecto;
            String mensaje;
            GestionarPacienteServicio gestionarPacienteServicio = new GestionarPacienteServicio();

            try
            {
                gestionarPacienteServicio.GuardarPaciente(pacienteDTO);
                esCorrecto = true;
                mensaje = "Paciente registrado correctamente.";
            }
            catch (Exception e)
            {
                esCorrecto = false;
                mensaje = e.Message;
            }

            return Json(new { estadoCorrecto = esCorrecto, mensajeError = mensaje });
        }

        [HttpPost]
        public JsonResult ModificarPaciente(PacienteDTO pacienteDTO)
        {
            Boolean esCorrecto;
            String mensaje;
            GestionarPacienteServicio gestionarPacienteServicio = new GestionarPacienteServicio();

            try
            {
                gestionarPacienteServicio.ModificarPaciente(pacienteDTO);
                esCorrecto = true;
                mensaje = "Paciente actualizado correctamente.";
            }
            catch (Exception e)
            {
                esCorrecto = false;
                mensaje = e.Message;
            }

            return Json(new { estadoCorrecto = esCorrecto, mensajeError = mensaje });
        }


        [HttpPost]
        public JsonResult EliminarPaciente(PacienteDTO pacienteDTO)
        {
            Boolean esCorrecto;
            String mensaje;
            GestionarPacienteServicio gestionarPacienteServicio = new GestionarPacienteServicio();

            try
            {
                gestionarPacienteServicio.EliminarPaciente(pacienteDTO);
                esCorrecto = true;
                mensaje = "Paciente eliminado correctamente.";
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