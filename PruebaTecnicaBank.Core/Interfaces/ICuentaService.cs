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
    public interface ICuentaServicio
    {
        /// <summary>
        /// Crea una nueva cuenta bancaria.
        /// </summary>
        /// <param name="cuentaDto">Objeto que contiene los datos necesarios para crear la cuenta.</param>
        /// <returns>Un objeto que representa la respuesta de la creación de la cuenta.</returns>
        Task<CuentaRespuestaDto> CrearCuentaAsync(CuentaCrearDto cuentaDto);

        /// <summary>
        /// Obtiene el saldo de una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta bancaria.</param>
        /// <returns>El saldo actual de la cuenta.</returns>
        Task<decimal> ObtenerSaldoAsync(string numeroCuenta);

        /// <summary>
        /// Realiza un depósito en una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta bancaria.</param>
        /// <param name="transaccionDto">Objeto que contiene los datos de la transacción de depósito.</param>
        /// <returns>Un objeto que representa la respuesta de la transacción de depósito.</returns>
        Task<TransaccionRespuestaDto> DepositarAsync(string numeroCuenta, TransaccionCrearDto transaccionDto);

        /// <summary>
        /// Realiza un retiro de una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta bancaria.</param>
        /// <param name="transaccionDto">Objeto que contiene los datos de la transacción de retiro.</param>
        /// <returns>Un objeto que representa la respuesta de la transacción de retiro.</returns>
        Task<TransaccionRespuestaDto> RetirarAsync(string numeroCuenta, TransaccionCrearDto transaccionDto);

        /// <summary>
        /// Obtiene las transacciones realizadas en una cuenta bancaria junto con el saldo final.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta bancaria.</param>
        /// <returns>Un objeto que contiene las transacciones de la cuenta y el saldo final.</returns>
        Task<HistorialTransaccionesDto> ObtenerTransaccionesAsync(string numeroCuenta);
    }
}
