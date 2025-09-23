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
    /// Repositorio para manejar operaciones relacionadas con cuentas bancarias.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly BankDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AccountRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        public AccountRepository(BankDbContext context) => _context = context;

        /// <summary>
        /// Agrega una nueva cuenta de forma asíncrona.
        /// </summary>
        /// <param name="account">La cuenta a agregar.</param>
        /// <returns>La cuenta agregada.</returns>
        public async Task<Account> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        /// <summary>
        /// Obtiene una cuenta por su número de cuenta de forma asíncrona.
        /// </summary>
        /// <param name="accountNumber">El número de cuenta a buscar.</param>
        /// <returns>La cuenta encontrada o null si no existe.</returns>
        public async Task<Account?> GetByNumberAsync(string accountNumber) =>
            await _context.Accounts.Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

        /// <summary>
        /// Actualiza una cuenta existente de forma asíncrona.
        /// </summary>
        /// <param name="account">La cuenta a actualizar.</param>
        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}
