using AppInventario.AccesoDatos.Repositorio.IRepositorio;
using AppInventario.Modelos;
using AppInventario.Modelos.Especificaciones;
using AppInventario.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;
        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index(int pageNumber = 1, string busqueda ="", string busquedaActual = "")
        {
            if (!string.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;   
            }
            ViewData["BusquedaActual"] = busqueda;

            if (pageNumber < 1) pageNumber = 1;

            Parametros parametros = new Parametros
            {
                PageNumber = pageNumber,
                PageSize = 4
            };

            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            if (!string.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, x => x.Descripcion.Contains(busqueda));
            }

            ViewData["TotalPaginas"] = resultado.metaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.metaData.TotalCount;
            ViewData["PageSize"] = resultado.metaData.PageSize;
            ViewData["PageNumber"] = pageNumber;

            //Clase css para desactivar boton
            ViewData["Previo"] = "disabled";

            ViewData["Siguiente"] = "";
            if (pageNumber > 1) ViewData["Previo"] = "";
            if (resultado.metaData.TotalPages <= pageNumber) ViewData["Siguiente"] = "disabled";

            return View(resultado);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}