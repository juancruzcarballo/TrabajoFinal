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
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<ProductoVendido> _logger;

        public UsuarioController(ILogger<ProductoVendido> logger)
        {
            _logger = logger;
        }

        [HttpGet("{NombreUsuario}/{Contrasena}")]
        public Usuario GetUsuarioByContraseña(string NombreUsuario, string Contrasena)
        {
            var usuario = UsuarioHandler.GetUsuarioByPassword(NombreUsuario, Contrasena);

            return usuario == null ? new Usuario() : usuario;
        }

        [HttpGet("{NombreUsuario}")]
        public Usuario GetUsuarioByNombre(string NombreUsuario)
        {
            var usuario = UsuarioHandler.GetUsuarioByUserName(NombreUsuario);

            return usuario == null ? new Usuario() : usuario;
        }

        [HttpPost]


        [HttpPut]
        public bool PutUsuario(Usuario usuario)
        {
            return UsuarioHandler.UpdateUsuario(usuario);
        }
        [HttpDelete("{Id}")]
        public void DeleteProductos(int Id)
        {
            UsuarioHandler.EliminarUsuario(Id);
        }
    }

}
