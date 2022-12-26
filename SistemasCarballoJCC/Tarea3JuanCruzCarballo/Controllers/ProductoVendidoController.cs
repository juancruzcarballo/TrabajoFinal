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
    public class ProductoVendidoController : ControllerBase
    {

        private readonly ILogger<ProductoVendidoController> _logger;

        public ProductoVendidoController(ILogger<ProductoVendidoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{IdUsuario}")]
        public IEnumerable<ProductoVendido> GetAllProductosVendidos(int IdUsuario)
        {
            return ProductoVendidoHandler.GetProductosVendidos(IdUsuario);
        }
    }
}
