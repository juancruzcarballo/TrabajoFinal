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
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{idUsuario}")]
        public IEnumerable<Producto> GetAllProductos(int idUsuario)
        {
            return ProductoHandler.GetProductos(idUsuario);
        }

        [HttpPut]
        public void PutProductos(Producto producto)
        {

            ProductoHandler.ModificarProductos(producto);
        }

        [HttpPost]


        [HttpDelete("{idProducto}")]
        public void DeleteProductos(int idProducto)
        {
            ProductoHandler.EliminarProducto(idProducto);
        }

    }
}
