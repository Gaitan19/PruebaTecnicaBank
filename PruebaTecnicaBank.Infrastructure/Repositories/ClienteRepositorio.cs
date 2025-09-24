using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using PruebaTecnicaBank.Infrastructure.Context;
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
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly BankDbContext _context;

        public ClienteRepositorio(BankDbContext context) => _context = context;

        /// <summary>
        /// Agrega un nuevo cliente a la base de datos.
        /// </summary>
        /// <param name="cliente">El cliente a agregar.</param>
        /// <returns>El cliente agregado.</returns>
        public async Task<Cliente> AgregarAsync(Cliente cliente)
        {
            _context.Clients.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        /// <summary>
        /// Obtiene un cliente por su identificador único.
        /// </summary>
        /// <param name="id">El identificador del cliente.</param>
        /// <returns>El cliente correspondiente o null si no se encuentra.</returns>
        public async Task<Cliente?> ObtenerPorIdAsync(Guid id) =>
            await _context.Clients.Include(c => c.Cuentas).FirstOrDefaultAsync(c => c.Id == id);

        /// <summary>
        /// Obtiene todos los clientes de la base de datos.
        /// </summary>
        /// <returns>Una lista de todos los clientes.</returns>
        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync() =>
            await _context.Clients.Include(c => c.Cuentas).AsNoTracking().ToListAsync();
    }
}
