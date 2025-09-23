using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para gestionar operaciones relacionadas con los clientes.
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly BankDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ClientRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        public ClientRepository(BankDbContext context) => _context = context;

        /// <summary>
        /// Agrega un nuevo cliente a la base de datos.
        /// </summary>
        /// <param name="client">El cliente a agregar.</param>
        /// <returns>El cliente agregado.</returns>
        public async Task<Client> AddAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        /// <summary>
        /// Obtiene un cliente por su identificador único.
        /// </summary>
        /// <param name="id">El identificador del cliente.</param>
        /// <returns>El cliente correspondiente o null si no se encuentra.</returns>
        public async Task<Client?> GetByIdAsync(Guid id) =>
            await _context.Clients.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == id);

        /// <summary>
        /// Obtiene todos los clientes de la base de datos.
        /// </summary>
        /// <returns>Una lista de todos los clientes.</returns>
        public async Task<IEnumerable<Client>> GetAllAsync() =>
            await _context.Clients.AsNoTracking().ToListAsync();
    }
}
