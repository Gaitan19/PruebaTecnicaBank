using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Interfaces;

namespace PruebaTecnicaBank.Api.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaServicio _cuentaServicio;

        public CuentasController(ICuentaServicio cuentaServicio)
        {
            _cuentaServicio = cuentaServicio;
        }

        /// <summary>
        /// Crea una nueva cuenta.
        /// </summary>
        /// <param name="cuentaDto">Datos de la cuenta a crear.</param>
        /// <returns>Detalles de la cuenta creada.</returns>
        [HttpPost]
        public async Task<ActionResult<CuentaRespuestaDto>> CrearCuenta([FromBody] CuentaCrearDto cuentaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cuentaCreada = await _cuentaServicio.CrearCuentaAsync(cuentaDto);
                return Ok(cuentaCreada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el saldo de una cuenta específica.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <returns>Saldo de la cuenta.</returns>
        [HttpGet("saldo/{numeroCuenta}")]
        public async Task<ActionResult<decimal>> ObtenerSaldo(string numeroCuenta)
        {
            try
            {
                var saldo = await _cuentaServicio.ObtenerSaldoAsync(numeroCuenta);
                return Ok(saldo);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Realiza un depósito en una cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <param name="dto">Datos de la transacción a realizar.</param>
        /// <returns>Detalles de la transacción realizada.</returns>
        [HttpPost("depositar/{numeroCuenta}")]
        public async Task<ActionResult<TransaccionRespuestaDto>> Depositar(string numeroCuenta, [FromBody] TransaccionCrearDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transaccion = await _cuentaServicio.DepositarAsync(numeroCuenta, dto);
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Realiza un retiro de una cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <param name="dto">Datos de la transacción a realizar.</param>
        /// <returns>Detalles de la transacción realizada.</returns>
        [HttpPost("retirar/{numeroCuenta}")]
        public async Task<ActionResult<TransaccionRespuestaDto>> Retirar(string numeroCuenta, [FromBody] TransaccionCrearDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transaccion = await _cuentaServicio.RetirarAsync(numeroCuenta, dto);
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el historial de transacciones de una cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <returns>Historial de transacciones de la cuenta.</returns>
        [HttpGet("transacciones/{numeroCuenta}")]
        public async Task<ActionResult<HistorialTransaccionesDto>> ObtenerTransacciones(string numeroCuenta)
        {
            try
            {
                var historial = await _cuentaServicio.ObtenerTransaccionesAsync(numeroCuenta);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
