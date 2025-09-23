using PruebaTecnicaBank.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Interfaces
{
    /// <summary>
    /// Interfaz que define los servicios relacionados con las cuentas bancarias.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Crea una nueva cuenta bancaria.
        /// </summary>
        /// <param name="accountDto">Objeto que contiene los datos necesarios para crear la cuenta.</param>
        /// <returns>Un objeto que representa la respuesta de la creación de la cuenta.</returns>
        Task<AccountResponseDto> CreateAccountAsync(AccountCreateDto accountDto);

        /// <summary>
        /// Obtiene el saldo de una cuenta bancaria.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <returns>El saldo actual de la cuenta.</returns>
        Task<decimal> GetBalanceAsync(string accountNumber);

        /// <summary>
        /// Realiza un depósito en una cuenta bancaria.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <param name="transactionDto">Objeto que contiene los datos de la transacción de depósito.</param>
        /// <returns>Un objeto que representa la respuesta de la transacción de depósito.</returns>
        Task<TransactionResponseDto> DepositAsync(string accountNumber, TransactionCreateDto transactionDto);

        /// <summary>
        /// Realiza un retiro de una cuenta bancaria.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <param name="transactionDto">Objeto que contiene los datos de la transacción de retiro.</param>
        /// <returns>Un objeto que representa la respuesta de la transacción de retiro.</returns>
        Task<TransactionResponseDto> WithdrawAsync(string accountNumber, TransactionCreateDto transactionDto);

        /// <summary>
        /// Obtiene las transacciones realizadas en una cuenta bancaria.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <returns>Una colección de objetos que representan las transacciones de la cuenta.</returns>
        Task<IEnumerable<TransactionResponseDto>> GetTransactionsAsync(string accountNumber);
    }
}
