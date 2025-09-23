using PruebaTecnicaBank.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de cuentas.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Agrega una nueva cuenta de forma asíncrona.
        /// </summary>
        /// <param name="account">La cuenta a agregar.</param>
        /// <returns>La cuenta agregada.</returns>
        Task<Account> AddAsync(Account account);

        /// <summary>
        /// Obtiene una cuenta por su número de cuenta de forma asíncrona.
        /// </summary>
        /// <param name="accountNumber">El número de cuenta a buscar.</param>
        /// <returns>La cuenta encontrada o null si no existe.</returns>
        Task<Account?> GetByNumberAsync(string accountNumber);

        /// <summary>
        /// Actualiza una cuenta de forma asíncrona.
        /// </summary>
        /// <param name="account">La cuenta a actualizar.</param>
        Task UpdateAsync(Account account);
    }
}
