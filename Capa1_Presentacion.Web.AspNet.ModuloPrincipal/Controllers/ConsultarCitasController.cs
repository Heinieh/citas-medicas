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

            int idMedicoAutenticado = 1;

            try
            {
                
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