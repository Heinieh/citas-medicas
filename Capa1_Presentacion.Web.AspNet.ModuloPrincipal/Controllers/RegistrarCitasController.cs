using System;
using System.Web.Mvc;
using Capa2_Aplicacion.ModuloPrincipal.DTO;
using Capa2_Aplicacion.ModuloPrincipal.Servicio;
using Capa3_Dominio.ModuloPrincipal.Entidad;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal.Controllers
{
    public class RegistrarCitasController : Controller
    {
        public ActionResult PaginaRegistrarCita()
        {
            return View();
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