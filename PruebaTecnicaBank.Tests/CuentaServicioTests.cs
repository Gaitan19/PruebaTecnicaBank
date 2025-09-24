using AutoMapper;
using Moq;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using PruebaTecnicaBank.Core.Services;
using PruebaTecnicaBank.Infrastructure.Mappings;

namespace PruebaTecnicaBank.Tests;

/// <summary>
/// Contiene pruebas unitarias para la clase <see cref="CuentaServicio"/>.
/// Se validan los métodos principales relacionados con la gestión de cuentas,
/// depósitos, retiros, aplicación de intereses, obtención de saldos
/// y consulta de transacciones.
/// </summary>
public class CuentaServicioTests
{
    private readonly Mock<ICuentaRepositorio> _mockCuentaRepo;
    private readonly Mock<ITransaccionRepositorio> _mockTransaccionRepo;
    private readonly Mock<IClienteRepositorio> _mockClienteRepo;
    private readonly IMapper _mapper;
    private readonly CuentaServicio _cuentaServicio;

    public CuentaServicioTests()
    {
        _mockCuentaRepo = new Mock<ICuentaRepositorio>();
        _mockTransaccionRepo = new Mock<ITransaccionRepositorio>();  
        _mockClienteRepo = new Mock<IClienteRepositorio>();

        // Configurar AutoMapper
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _cuentaServicio = new CuentaServicio(
            _mockCuentaRepo.Object,
            _mockTransaccionRepo.Object,
            _mockClienteRepo.Object,
            _mapper
        );
    }

    #region CrearCuentaAsync Tests

    /// <summary>
    /// Verifica que se pueda crear una cuenta cuando el cliente existe
    /// y el número de cuenta es único.
    /// </summary>
    [Fact]
    public async Task CrearCuentaAsync_ClienteExiste_CuentaUnica_RetornaCuentaCreada()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cuentaDto = new CuentaCrearDto
        {
            ClienteId = clienteId,
            NumeroCuenta = "1234567890",
            SaldoInicial = 1000m
        };

        var cliente = new Cliente { Id = clienteId, Nombre = "Cliente Test" };
        var cuentaCreada = new Cuenta
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            NumeroCuenta = "1234567890",
            Saldo = 1000m
        };

        _mockClienteRepo.Setup(x => x.ObtenerPorIdAsync(clienteId))
            .ReturnsAsync(cliente);
        _mockCuentaRepo.Setup(x => x.ExisteNumeroCuentaAsync("1234567890"))
            .ReturnsAsync(false);
        _mockCuentaRepo.Setup(x => x.AgregarAsync(It.IsAny<Cuenta>()))
            .ReturnsAsync(cuentaCreada);

        // Act
        var resultado = await _cuentaServicio.CrearCuentaAsync(cuentaDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("1234567890", resultado.NumeroCuenta);
        Assert.Equal(1000m, resultado.Saldo);
        _mockClienteRepo.Verify(x => x.ObtenerPorIdAsync(clienteId), Times.Once);
        _mockCuentaRepo.Verify(x => x.ExisteNumeroCuentaAsync("1234567890"), Times.Once);
        _mockCuentaRepo.Verify(x => x.AgregarAsync(It.IsAny<Cuenta>()), Times.Once);
    }

    /// <summary>
    /// Verifica que se lance una excepción si el cliente no existe
    /// al intentar crear una cuenta.
    /// </summary>
    [Fact]
    public async Task CrearCuentaAsync_ClienteNoExiste_LanzaExcepcion()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cuentaDto = new CuentaCrearDto
        {
            ClienteId = clienteId,
            NumeroCuenta = "1234567890",
            SaldoInicial = 1000m
        };

        _mockClienteRepo.Setup(x => x.ObtenerPorIdAsync(clienteId))
            .ReturnsAsync((Cliente?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.CrearCuentaAsync(cuentaDto));
        Assert.Equal("Cliente no encontrado", exception.Message);
        _mockClienteRepo.Verify(x => x.ObtenerPorIdAsync(clienteId), Times.Once);
    }

    /// <summary>
    /// Verifica que se lance una excepción si el número de cuenta ya existe.
    /// </summary>
    [Fact]
    public async Task CrearCuentaAsync_NumeroCuentaYaExiste_LanzaExcepcion()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cuentaDto = new CuentaCrearDto
        {
            ClienteId = clienteId,
            NumeroCuenta = "1234567890",
            SaldoInicial = 1000m
        };

        var cliente = new Cliente { Id = clienteId, Nombre = "Cliente Test" };

        _mockClienteRepo.Setup(x => x.ObtenerPorIdAsync(clienteId))
            .ReturnsAsync(cliente);
        _mockCuentaRepo.Setup(x => x.ExisteNumeroCuentaAsync("1234567890"))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.CrearCuentaAsync(cuentaDto));
        Assert.Equal("Ya existe una cuenta con ese número", exception.Message);
        _mockCuentaRepo.Verify(x => x.ExisteNumeroCuentaAsync("1234567890"), Times.Once);
    }

    #endregion

    #region DepositarAsync Tests

    /// <summary>
    /// Verifica que se realice un depósito correctamente cuando la cuenta existe.
    /// </summary>
    [Fact]
    public async Task DepositarAsync_CuentaExiste_RealizaDepositoCorrectamente()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var cuentaId = Guid.NewGuid();
        var transaccionDto = new TransaccionCrearDto { Monto = 500m };
        
        var cuenta = new Cuenta
        {
            Id = cuentaId,
            NumeroCuenta = numeroCuenta,
            Saldo = 1000m
        };

        var transaccionCreada = new Transaccion
        {
            Id = Guid.NewGuid(),
            CuentaId = cuentaId,
            Tipo = TipoTransaccion.Deposito,
            Monto = 500m,
            SaldoDespues = 1500m,
            FechaHora = DateTime.UtcNow
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);
        _mockTransaccionRepo.Setup(x => x.AgregarAsync(It.IsAny<Transaccion>()))
            .ReturnsAsync(transaccionCreada);
        _mockCuentaRepo.Setup(x => x.ActualizarAsync(It.IsAny<Cuenta>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _cuentaServicio.DepositarAsync(numeroCuenta, transaccionDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(500m, resultado.Monto);
        Assert.Equal(1500m, resultado.SaldoDespues);
        Assert.Equal("Deposito", resultado.Tipo);
        Assert.Equal(1500m, cuenta.Saldo);
        _mockTransaccionRepo.Verify(x => x.AgregarAsync(It.IsAny<Transaccion>()), Times.Once);
        _mockCuentaRepo.Verify(x => x.ActualizarAsync(cuenta), Times.Once);
    }

    /// <summary>
    /// Verifica que se lance una excepción si se intenta depositar en una cuenta inexistente.
    /// </summary>
    [Fact]
    public async Task DepositarAsync_CuentaNoExiste_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var transaccionDto = new TransaccionCrearDto { Monto = 500m };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync((Cuenta?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.DepositarAsync(numeroCuenta, transaccionDto));
        Assert.Equal("Cuenta no encontrada", exception.Message);
    }

    #endregion

    #region RetirarAsync Tests

    /// <summary>
    /// Verifica que un retiro se realice correctamente si el saldo es suficiente.
    /// </summary>
    [Fact]
    public async Task RetirarAsync_SaldoSuficiente_RealizaRetiroCorrectamente()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var cuentaId = Guid.NewGuid();
        var transaccionDto = new TransaccionCrearDto { Monto = 300m };
        
        var cuenta = new Cuenta
        {
            Id = cuentaId,
            NumeroCuenta = numeroCuenta,
            Saldo = 1000m
        };

        var transaccionCreada = new Transaccion
        {
            Id = Guid.NewGuid(),
            CuentaId = cuentaId,
            Tipo = TipoTransaccion.Retiro,
            Monto = 300m,
            SaldoDespues = 700m,
            FechaHora = DateTime.UtcNow
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);
        _mockTransaccionRepo.Setup(x => x.AgregarAsync(It.IsAny<Transaccion>()))
            .ReturnsAsync(transaccionCreada);
        _mockCuentaRepo.Setup(x => x.ActualizarAsync(It.IsAny<Cuenta>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _cuentaServicio.RetirarAsync(numeroCuenta, transaccionDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(300m, resultado.Monto);
        Assert.Equal(700m, resultado.SaldoDespues);
        Assert.Equal("Retiro", resultado.Tipo);
        Assert.Equal(700m, cuenta.Saldo);
        _mockTransaccionRepo.Verify(x => x.AgregarAsync(It.IsAny<Transaccion>()), Times.Once);
        _mockCuentaRepo.Verify(x => x.ActualizarAsync(cuenta), Times.Once);
    }

    /// <summary>
    /// Verifica que se lance una excepción al intentar retirar más dinero del disponible.
    /// </summary>
    [Fact]
    public async Task RetirarAsync_SaldoInsuficiente_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var transaccionDto = new TransaccionCrearDto { Monto = 1500m };
        
        var cuenta = new Cuenta
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = numeroCuenta,
            Saldo = 1000m
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.RetirarAsync(numeroCuenta, transaccionDto));
        Assert.Equal("Saldo insuficiente para realizar el retiro", exception.Message);
    }

    /// <summary>
    /// Verifica que se lance una excepción si la cuenta no existe al retirar.
    /// </summary>
    [Fact]
    public async Task RetirarAsync_CuentaNoExiste_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var transaccionDto = new TransaccionCrearDto { Monto = 300m };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync((Cuenta?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.RetirarAsync(numeroCuenta, transaccionDto));
        Assert.Equal("Cuenta no encontrada", exception.Message);
    }

    #endregion

    #region AplicarInteresAsync Tests

    /// <summary>
    /// Verifica que se aplique correctamente el interés positivo a una cuenta existente.
    /// </summary>
    [Fact]
    public async Task AplicarInteresAsync_CuentaExiste_AplicaInteresCorrectamente()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var cuentaId = Guid.NewGuid();
        var tasaInteres = 0.02m; // 2%
        
        var cuenta = new Cuenta
        {
            Id = cuentaId,
            NumeroCuenta = numeroCuenta,
            Saldo = 1000m
        };

        var transaccionCreada = new Transaccion
        {
            Id = Guid.NewGuid(),
            CuentaId = cuentaId,
            Tipo = TipoTransaccion.Interes,
            Monto = 20m,
            SaldoDespues = 1020m,
            FechaHora = DateTime.UtcNow
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);
        _mockTransaccionRepo.Setup(x => x.AgregarAsync(It.IsAny<Transaccion>()))
            .ReturnsAsync(transaccionCreada);
        _mockCuentaRepo.Setup(x => x.ActualizarAsync(It.IsAny<Cuenta>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _cuentaServicio.AplicarInteresAsync(numeroCuenta, tasaInteres);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(20m, resultado.Monto); // 1000 * 0.02
        Assert.Equal(1020m, resultado.SaldoDespues);
        Assert.Equal("Interes", resultado.Tipo);
        Assert.Equal(1020m, cuenta.Saldo);
        _mockTransaccionRepo.Verify(x => x.AgregarAsync(It.IsAny<Transaccion>()), Times.Once);
        _mockCuentaRepo.Verify(x => x.ActualizarAsync(cuenta), Times.Once);
    }

    /// <summary>
    /// Verifica que se lance una excepción si se intenta aplicar un interés negativo.
    /// </summary>
    [Fact]
    public async Task AplicarInteresAsync_TasaInteresNegativa_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var tasaInteres = -0.02m;
        
        var cuenta = new Cuenta
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = numeroCuenta,
            Saldo = 1000m
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.AplicarInteresAsync(numeroCuenta, tasaInteres));
        Assert.Equal("La tasa de interes tiene que ser mayor a 0", exception.Message);
    }

    /// <summary>
    /// Verifica que se lance una excepción si la cuenta no existe al aplicar interés.
    /// </summary>
    [Fact]
    public async Task AplicarInteresAsync_CuentaNoExiste_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var tasaInteres = 0.02m;

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync((Cuenta?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.AplicarInteresAsync(numeroCuenta, tasaInteres));
        Assert.Equal("Cuenta no encontrada", exception.Message);
    }

    #endregion

    #region ObtenerSaldoAsync Tests

    /// <summary>
    /// Verifica que se retorne el saldo correcto si la cuenta existe.
    /// </summary>
    [Fact]
    public async Task ObtenerSaldoAsync_CuentaExiste_RetornaSaldoCorrectamente()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var cuenta = new Cuenta
        {
            Id = Guid.NewGuid(),
            NumeroCuenta = numeroCuenta,
            Saldo = 1500m
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);

        // Act
        var saldo = await _cuentaServicio.ObtenerSaldoAsync(numeroCuenta);

        // Assert
        Assert.Equal(1500m, saldo);
        _mockCuentaRepo.Verify(x => x.ObtenerPorNumeroAsync(numeroCuenta), Times.Once);
    }

    /// <summary>
    /// Verifica que se lance una excepción al intentar obtener el saldo de una cuenta inexistente.
    /// </summary>
    [Fact]
    public async Task ObtenerSaldoAsync_CuentaNoExiste_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync((Cuenta?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.ObtenerSaldoAsync(numeroCuenta));
        Assert.Equal("Cuenta no encontrada", exception.Message);
    }

    #endregion

    #region ObtenerTransaccionesAsync Tests

    /// <summary>
    /// Verifica que se obtenga correctamente el historial de transacciones
    /// de una cuenta existente.
    /// </summary>
    [Fact]
    public async Task ObtenerTransaccionesAsync_CuentaExiste_RetornaHistorialCorrectamente()
    {
        // Arrange
        var numeroCuenta = "1234567890";
        var cuentaId = Guid.NewGuid();
        var cuenta = new Cuenta
        {
            Id = cuentaId,
            NumeroCuenta = numeroCuenta,
            Saldo = 1200m
        };

        var transacciones = new List<Transaccion>
        {
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuentaId,
                Tipo = TipoTransaccion.Deposito,
                Monto = 500m,
                SaldoDespues = 1500m,
                FechaHora = DateTime.UtcNow.AddDays(-2)
            },
            new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuentaId,
                Tipo = TipoTransaccion.Retiro,
                Monto = 300m,
                SaldoDespues = 1200m,
                FechaHora = DateTime.UtcNow.AddDays(-1)
            }
        };

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync(cuenta);
        _mockTransaccionRepo.Setup(x => x.ObtenerPorCuentaAsync(cuentaId))
            .ReturnsAsync(transacciones);

        // Act
        var resultado = await _cuentaServicio.ObtenerTransaccionesAsync(numeroCuenta);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1200m, resultado.SaldoFinal);
        Assert.Equal(2, resultado.Transacciones.Count());
        
        var transaccionesLista = resultado.Transacciones.ToList();
        Assert.Equal("Deposito", transaccionesLista[0].Tipo);
        Assert.Equal(500m, transaccionesLista[0].Monto);
        Assert.Equal("Retiro", transaccionesLista[1].Tipo);
        Assert.Equal(300m, transaccionesLista[1].Monto);
    }

    /// <summary>
    /// Verifica que se lance una excepción al intentar obtener transacciones de una cuenta inexistente.
    /// </summary>
    [Fact]
    public async Task ObtenerTransaccionesAsync_CuentaNoExiste_LanzaExcepcion()
    {
        // Arrange
        var numeroCuenta = "1234567890";

        _mockCuentaRepo.Setup(x => x.ObtenerPorNumeroAsync(numeroCuenta))
            .ReturnsAsync((Cuenta?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _cuentaServicio.ObtenerTransaccionesAsync(numeroCuenta));
        Assert.Equal("Cuenta no encontrada", exception.Message);
    }

    #endregion

}