using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Interfaces;
using PruebaTecnicaBank.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para manejar las transacciones en la base de datos.
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TransactionRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        public TransactionRepository(BankDbContext context) => _context = context;

        /// <summary>
        /// Agrega una nueva transacción a la base de datos.
        /// </summary>
        /// <param name="transaction">La transacción a agregar.</param>
        /// <returns>La transacción agregada.</returns>
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        /// <summary>
        /// Obtiene las transacciones asociadas a una cuenta específica.
        /// </summary>
        /// <param name="accountId">El identificador de la cuenta.</param>
        /// <returns>Una lista de transacciones.</returns>
        public async Task<IEnumerable<Transaction>> GetByAccountAsync(Guid accountId) =>
            await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderBy(t => t.Timestamp)
                .ToListAsync();
    }
}
