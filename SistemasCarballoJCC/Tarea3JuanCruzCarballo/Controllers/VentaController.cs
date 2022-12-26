using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tarea3JuanCruzCarballo.ADO;
using Tarea3JuanCruzCarballo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tarea3JuanCruzCarballo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly ILogger<ProductoVendido> _logger;

        public VentaController(ILogger<ProductoVendido> logger)
        {
            _logger = logger;
        }

        [HttpGet("{idUsuario}")]
        public IEnumerable<Venta> GetVentas(int idUsuario)
        {
            return VentaHandler.GetVentas(idUsuario);
        }


        [HttpPost("{idUsuario}")]
        public void PostVenta(List<Producto> productos, int idUsuario)
        {
            VentaHandler.InsertVenta(productos, idUsuario);
        }
        [HttpDelete("{idUsuario}")]
        public void DeleteProductos(int Id)
        {
            VentaHandler.EliminarVenta(Id);
        }

    }
}