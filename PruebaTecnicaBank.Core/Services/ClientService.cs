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
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientResponseDto> CreateClientAsync(ClientCreateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            client.Id = Guid.NewGuid();

            var createdClient = await _clientRepository.AddAsync(client);
            return _mapper.Map<ClientResponseDto>(createdClient);
        }

        public async Task<ClientResponseDto?> GetClientAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return client == null ? null : _mapper.Map<ClientResponseDto>(client);
        }
    }
}
