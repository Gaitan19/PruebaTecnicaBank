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
    public class CuentaServicio : ICuentaServicio
    {
        private readonly ICuentaRepositorio _cuentaRepo;
        private readonly ITransaccionRepositorio _transaccionRepo;
        private readonly IClienteRepositorio _clienteRepo;
        private readonly IMapper _mapper;

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

        public async Task<decimal> ObtenerSaldoAsync(string numeroCuenta)
        {
            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
            if (cuenta == null) throw new Exception("Cuenta no encontrada");
            return cuenta.Saldo;
        }

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
    }
}