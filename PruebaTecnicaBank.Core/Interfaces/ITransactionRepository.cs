using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PruebaTecnicaBank.Core.Entities;


namespace PruebaTecnicaBank.Core.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de transacciones.
    /// Proporciona métodos para agregar y obtener transacciones.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Agrega una nueva transacción de forma asíncrona.
        /// </summary>
        /// <param name="transaction">La transacción a agregar.</param>
        /// <returns>La transacción agregada.</returns>
        Task<Transaction> AddAsync(Transaction transaction);

        /// <summary>
        /// Obtiene las transacciones asociadas a una cuenta específica de forma asíncrona.
        /// </summary>
        /// <param name="accountId">El identificador de la cuenta.</param>
        /// <returns>Una colección de transacciones asociadas a la cuenta.</returns>
        Task<IEnumerable<Transaction>> GetByAccountAsync(Guid accountId);
    }
}
