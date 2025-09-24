using AutoMapper;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Services
{
    /// <summary>
    /// Servicio para gestionar operaciones relacionadas con clientes.
    /// </summary>
    public class ClienteServicio : IClienteServicio
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ClienteServicio"/>.
        /// </summary>
        /// <param name="clienteRepositorio">Repositorio para acceder a los datos de clientes.</param>
        /// <param name="mapper">Instancia de AutoMapper para la conversión de objetos.</param>
        public ClienteServicio(IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea un nuevo cliente de forma asíncrona.
        /// </summary>
        /// <param name="clienteDto">Datos del cliente a crear.</param>
        /// <returns>Un objeto <see cref="ClienteRespuestaDto"/> que representa el cliente creado.</returns>
        public async Task<ClienteRespuestaDto> CrearClienteAsync(ClienteCrearDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.Id = Guid.NewGuid();

            var clienteCreado = await _clienteRepositorio.AgregarAsync(cliente);
            return _mapper.Map<ClienteRespuestaDto>(clienteCreado);
        }

        /// <summary>
        /// Obtiene un cliente por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <returns>Un objeto <see cref="ClienteRespuestaDto"/> si se encuentra el cliente; de lo contrario, null.</returns>
        public async Task<ClienteRespuestaDto?> ObtenerClienteAsync(Guid id)
        {
            var cliente = await _clienteRepositorio.ObtenerPorIdAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteRespuestaDto>(cliente);
        }

        /// <summary>
        /// Obtiene todos los clientes de forma asíncrona.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="ClienteRespuestaDto"/> que representan todos los clientes.</returns>
        public async Task<IEnumerable<ClienteRespuestaDto>> ObtenerTodosClientesAsync()
        {
            var clientes = await _clienteRepositorio.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ClienteRespuestaDto>>(clientes);
        }
    }
}
