using AutoMapper;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Services
{
    /// <summary>
    /// Servicio para manejar operaciones relacionadas con cuentas bancarias.
    /// </summary>
    public class CuentaServicio : ICuentaServicio
    {
        private readonly ICuentaRepositorio _cuentaRepo;
        private readonly ITransaccionRepositorio _transaccionRepo;
        private readonly IClienteRepositorio _clienteRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor del servicio de cuentas.
        /// </summary>
        /// <param name="cuentaRepo">Repositorio de cuentas.</param>
        /// <param name="transaccionRepo">Repositorio de transacciones.</param>
        /// <param name="clienteRepo">Repositorio de clientes.</param>
        /// <param name="mapper">Mapper para la conversión de objetos.</param>
        public CuentaServicio(
            ICuentaRepositorio cuentaRepo,
            ITransaccionRepositorio transaccionRepo,
            IClienteRepositorio clienteRepo,
            IMapper mapper)
        {
            _cuentaRepo = cuentaRepo;
            _transaccionRepo = transaccionRepo;
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva cuenta bancaria.
        /// </summary>
        /// <param name="cuentaDto">Datos de la cuenta a crear.</param>
        /// <returns>Detalles de la cuenta creada.</returns>
        /// <exception cref="Exception">Lanza una excepción si el cliente no existe o el número de cuenta ya está en uso.</exception>
        public async Task<CuentaRespuestaDto> CrearCuentaAsync(CuentaCrearDto cuentaDto)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(cuentaDto.ClienteId);
            if (cliente == null) throw new Exception("Cliente no encontrado");

            // Verificar que el número de cuenta sea único
            if (await _cuentaRepo.ExisteNumeroCuentaAsync(cuentaDto.NumeroCuenta))
            {
                throw new Exception("Ya existe una cuenta con ese número");
            }

            var cuenta = _mapper.Map<Cuenta>(cuentaDto);
            cuenta.Id = Guid.NewGuid();
            cuenta.NumeroCuenta = cuentaDto.NumeroCuenta;
            cuenta.Saldo = cuentaDto.SaldoInicial;

            var cuentaCreada = await _cuentaRepo.AgregarAsync(cuenta);
            return _mapper.Map<CuentaRespuestaDto>(cuentaCreada);
        }

        /// <summary>
        /// Obtiene el saldo de una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <returns>Saldo de la cuenta.</returns>
        /// <exception cref="Exception">Lanza una excepción si la cuenta no existe.</exception>
        public async Task<decimal> ObtenerSaldoAsync(string numeroCuenta)
        {
            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
            if (cuenta == null) throw new Exception("Cuenta no encontrada");
            return cuenta.Saldo;
        }

        /// <summary>
        /// Realiza un depósito en una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <param name="transaccionDto">Datos de la transacción de depósito.</param>
        /// <returns>Detalles de la transacción realizada.</returns>
        /// <exception cref="Exception">Lanza una excepción si la cuenta no existe.</exception>
        public async Task<TransaccionRespuestaDto> DepositarAsync(string numeroCuenta, TransaccionCrearDto transaccionDto)
        {
            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
            if (cuenta == null) throw new Exception("Cuenta no encontrada");

            cuenta.Saldo += transaccionDto.Monto;

            var transaccion = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Tipo = TipoTransaccion.Deposito,
                Monto = transaccionDto.Monto,
                SaldoDespues = cuenta.Saldo,
                FechaHora = DateTime.UtcNow
            };

            await _transaccionRepo.AgregarAsync(transaccion);
            await _cuentaRepo.ActualizarAsync(cuenta);

            return _mapper.Map<TransaccionRespuestaDto>(transaccion);
        }

        /// <summary>
        /// Realiza un retiro de una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <param name="transaccionDto">Datos de la transacción de retiro.</param>
        /// <returns>Detalles de la transacción realizada.</returns>
        /// <exception cref="Exception">Lanza una excepción si la cuenta no existe o si el saldo es insuficiente.</exception>
        public async Task<TransaccionRespuestaDto> RetirarAsync(string numeroCuenta, TransaccionCrearDto transaccionDto)
        {
            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
            if (cuenta == null) throw new Exception("Cuenta no encontrada");

            if (cuenta.Saldo < transaccionDto.Monto)
                throw new Exception("Saldo insuficiente para realizar el retiro");

            cuenta.Saldo -= transaccionDto.Monto;

            var transaccion = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Tipo = TipoTransaccion.Retiro,
                Monto = transaccionDto.Monto,
                SaldoDespues = cuenta.Saldo,
                FechaHora = DateTime.UtcNow
            };

            await _transaccionRepo.AgregarAsync(transaccion);
            await _cuentaRepo.ActualizarAsync(cuenta);

            return _mapper.Map<TransaccionRespuestaDto>(transaccion);
        }

        /// <summary>
        /// Obtiene el historial de transacciones de una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <returns>Historial de transacciones y saldo final.</returns>
        /// <exception cref="Exception">Lanza una excepción si la cuenta no existe.</exception>
        public async Task<HistorialTransaccionesDto> ObtenerTransaccionesAsync(string numeroCuenta)
        {
            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
            if (cuenta == null) throw new Exception("Cuenta no encontrada");

            var transacciones = await _transaccionRepo.ObtenerPorCuentaAsync(cuenta.Id);
            var transaccionesDto = _mapper.Map<IEnumerable<TransaccionRespuestaDto>>(transacciones);

            return new HistorialTransaccionesDto
            {
                Transacciones = transaccionesDto,
                SaldoFinal = cuenta.Saldo
            };
        }

        /// <summary>
        /// Aplica intereses al saldo de una cuenta bancaria.
        /// </summary>
        /// <param name="numeroCuenta">Número de la cuenta.</param>
        /// <param name="tasaInteres">Tasa de interés a aplicar.</param>
        /// <returns>Detalles de la transacción de interés aplicado.</returns>
        /// <exception cref="Exception">Lanza una excepción si la cuenta no existe o la tasa de interés es inválida.</exception>
        public async Task<TransaccionRespuestaDto> AplicarInteresAsync(string numeroCuenta, decimal tasaInteres)
        {
            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
            if (cuenta == null) throw new Exception("Cuenta no encontrada");

            if (tasaInteres <= 0) throw new Exception("La tasa de interes tiene que ser mayor a 0");

            var montoInteres = cuenta.Saldo * tasaInteres;
            cuenta.Saldo += montoInteres;

            var transaccion = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Tipo = TipoTransaccion.Interes,
                Monto = montoInteres,
                SaldoDespues = cuenta.Saldo,
                FechaHora = DateTime.UtcNow
            };

            await _transaccionRepo.AgregarAsync(transaccion);
            await _cuentaRepo.ActualizarAsync(cuenta);

            return _mapper.Map<TransaccionRespuestaDto>(transaccion);
        }
    }
}