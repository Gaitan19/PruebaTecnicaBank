using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Interfaces;

namespace PruebaTecnicaBank.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los clientes.
    /// </summary>
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteServicio _clienteServicio;

        public ClientesController(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="clienteDto">Datos del cliente a crear.</param>
        /// <returns>El cliente creado.</returns>
        [HttpPost]
        public async Task<ActionResult<ClienteRespuestaDto>> CrearCliente([FromBody] ClienteCrearDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteCreado = await _clienteServicio.CrearClienteAsync(clienteDto);
            return Ok(clienteCreado);
        }

        /// <summary>
        /// Obtiene un cliente por su identificador.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <returns>El cliente correspondiente al identificador.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClienteRespuestaDto>> ObtenerCliente(Guid id)
        {
            var cliente = await _clienteServicio.ObtenerClienteAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        /// <returns>Lista de todos los clientes.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteRespuestaDto>>> ObtenerTodosClientes()
        {
            var clientes = await _clienteServicio.ObtenerTodosClientesAsync();
            return Ok(clientes);
        }
    }
}
