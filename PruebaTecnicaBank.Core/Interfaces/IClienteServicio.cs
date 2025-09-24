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
    public interface IClienteServicio
    {
        /// <summary>
        /// Crea un nuevo cliente de forma asíncrona.
        /// </summary>
        /// <param name="clienteDto">Objeto que contiene la información del cliente a crear.</param>
        /// <returns>Un objeto ClienteRespuestaDto que representa el cliente creado.</returns>
        Task<ClienteRespuestaDto> CrearClienteAsync(ClienteCrearDto clienteDto);

        /// <summary>
        /// Obtiene un cliente por su identificador de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único del cliente.</param>
        /// <returns>Un objeto ClienteRespuestaDto que representa el cliente, o null si no se encuentra.</returns>
        Task<ClienteRespuestaDto?> ObtenerClienteAsync(Guid id);
        
        /// <summary>
        /// Obtiene todos los clientes de forma asíncrona.
        /// </summary>
        /// <returns>Una colección de todos los clientes.</returns>
        Task<IEnumerable<ClienteRespuestaDto>> ObtenerTodosClientesAsync();
    }
}
