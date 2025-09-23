using PruebaTecnicaBank.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de clientes.
    /// Proporciona métodos para agregar, obtener por ID y obtener todos los clientes.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Agrega un nuevo cliente de manera asíncrona.
        /// </summary>
        /// <param name="client">El cliente a agregar.</param>
        /// <returns>El cliente agregado.</returns>
        Task<Client> AddAsync(Client client);

        /// <summary>
        /// Obtiene un cliente por su ID de manera asíncrona.
        /// </summary>
        /// <param name="id">El ID del cliente a buscar.</param>
        /// <returns>El cliente encontrado o null si no existe.</returns>
        Task<Client?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene todos los clientes de manera asíncrona.
        /// </summary>
        /// <returns>Una colección de todos los clientes.</returns>
        Task<IEnumerable<Client>> GetAllAsync();
    }
}
