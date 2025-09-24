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
    public class ClienteServicio : IClienteServicio
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IMapper _mapper;

        public ClienteServicio(IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        public async Task<ClienteRespuestaDto> CrearClienteAsync(ClienteCrearDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.Id = Guid.NewGuid();

            var clienteCreado = await _clienteRepositorio.AgregarAsync(cliente);
            return _mapper.Map<ClienteRespuestaDto>(clienteCreado);
        }

        public async Task<ClienteRespuestaDto?> ObtenerClienteAsync(Guid id)
        {
            var cliente = await _clienteRepositorio.ObtenerPorIdAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteRespuestaDto>(cliente);
        }
        
        public async Task<IEnumerable<ClienteRespuestaDto>> ObtenerTodosClientesAsync()
        {
            var clientes = await _clienteRepositorio.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ClienteRespuestaDto>>(clientes);
        }
    }
}
