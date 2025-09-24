using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Interfaces;
using PruebaTecnicaBank.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PruebaTecnicaBank.Infrastructure.Context;

namespace PruebaTecnicaBank.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para manejar las transacciones en la base de datos.
    /// </summary>
    public class TransaccionRepositorio : ITransaccionRepositorio
    {
        private readonly BankDbContext _context;

        public TransaccionRepositorio(BankDbContext context) => _context = context;

        /// <summary>
        /// Agrega una nueva transacción a la base de datos.
        /// </summary>
        /// <param name="transaccion">La transacción a agregar.</param>
        /// <returns>La transacción agregada.</returns>
        public async Task<Transaccion> AgregarAsync(Transaccion transaccion)
        {
            _context.Transactions.Add(transaccion);
            await _context.SaveChangesAsync();
            return transaccion;
        }

        /// <summary>
        /// Obtiene las transacciones asociadas a una cuenta específica.
        /// </summary>
        /// <param name="cuentaId">El identificador de la cuenta.</param>
        /// <returns>Una lista de transacciones.</returns>
        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaAsync(Guid cuentaId) =>
            await _context.Transactions
                .Where(t => t.CuentaId == cuentaId)
                .OrderBy(t => t.FechaHora)
                .ToListAsync();
    }
}
