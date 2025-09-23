using PruebaTecnicaBank.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Interfaces
{
    /// <summary>
    /// Interfaz que define los servicios relacionados con los clientes.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Crea un nuevo cliente de forma asíncrona.
        /// </summary>
        /// <param name="clientDto">Objeto que contiene la información del cliente a crear.</param>
        /// <returns>Un objeto ClientResponseDto que representa el cliente creado.</returns>
        Task<ClientResponseDto> CreateClientAsync(ClientCreateDto clientDto);

        /// <summary>
        /// Obtiene un cliente por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único del cliente.</param>
        /// <returns>Un objeto ClientResponseDto que representa el cliente, o null si no se encuentra.</returns>
        Task<ClientResponseDto?> GetClientAsync(Guid id);
    }
}
