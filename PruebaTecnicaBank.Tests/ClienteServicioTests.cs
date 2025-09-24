using AutoMapper;
using Moq;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using PruebaTecnicaBank.Core.Services;
using PruebaTecnicaBank.Infrastructure.Mappings;

namespace PruebaTecnicaBank.Tests;

/// <summary>
/// Conjunto de pruebas unitarias para <see cref="ClienteServicio"/>.
/// Se valida la creación, obtención individual y obtención de todos los clientes.
/// </summary>
public class ClienteServicioTests
{
    private readonly Mock<IClienteRepositorio> _mockClienteRepo;
    private readonly IMapper _mapper;
    private readonly ClienteServicio _clienteServicio;

    public ClienteServicioTests()
    {
        _mockClienteRepo = new Mock<IClienteRepositorio>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _clienteServicio = new ClienteServicio(_mockClienteRepo.Object, _mapper);
    }

    #region Crear cliente

    /// <summary>
    /// Verifica que al crear un cliente válido se retorne correctamente con sus propiedades.
    /// </summary>
    [Fact]
    public async Task CrearClienteAsync_DatosValidos_RetornaClienteCreado()
    {
        // Arrange
        var clienteDto = new ClienteCrearDto
        {
            Nombre = "Juan",
            FechaNacimiento = new DateTime(1990, 5, 15),
            Sexo = "Masculino",
            Ingresos = 50000m
        };

        var clienteCreado = new Cliente
        {
            Id = Guid.NewGuid(),
            Nombre = "Juan",
            FechaNacimiento = new DateTime(1990, 5, 15),
            Sexo = Sexo.Masculino,
            Ingresos = 50000m
        };

        _mockClienteRepo.Setup(x => x.AgregarAsync(It.IsAny<Cliente>()))
            .ReturnsAsync(clienteCreado);

        // Act
        var resultado = await _clienteServicio.CrearClienteAsync(clienteDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Juan", resultado.Nombre);
        Assert.Equal(new DateTime(1990, 5, 15), resultado.FechaNacimiento);
        Assert.Equal("Masculino", resultado.Sexo);
        Assert.Equal(50000m, resultado.Ingresos);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        _mockClienteRepo.Verify(x => x.AgregarAsync(It.IsAny<Cliente>()), Times.Once);
    }

    /// <summary>
    /// Verifica que se pueda crear correctamente un cliente femenino.
    /// </summary>
    [Fact]
    public async Task CrearClienteAsync_ClienteFemenino_RetornaClienteCreado()
    {
        // Arrange
        var clienteDto = new ClienteCrearDto
        {
            Nombre = "Maria",
            FechaNacimiento = new DateTime(1995, 8, 22),
            Sexo = "Femenino",
            Ingresos = 75000m
        };

        var clienteCreado = new Cliente
        {
            Id = Guid.NewGuid(),
            Nombre = "Maria",
            FechaNacimiento = new DateTime(1995, 8, 22),
            Sexo = Sexo.Femenino,
            Ingresos = 75000m
        };

        _mockClienteRepo.Setup(x => x.AgregarAsync(It.IsAny<Cliente>()))
            .ReturnsAsync(clienteCreado);

        // Act
        var resultado = await _clienteServicio.CrearClienteAsync(clienteDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Maria", resultado.Nombre);
        Assert.Equal(new DateTime(1995, 8, 22), resultado.FechaNacimiento);
        Assert.Equal("Femenino", resultado.Sexo);
        Assert.Equal(75000m, resultado.Ingresos);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        _mockClienteRepo.Verify(x => x.AgregarAsync(It.IsAny<Cliente>()), Times.Once);
    }

    /// <summary>
    /// Verifica la creación de un cliente con ingresos mínimos válidos.
    /// </summary>
    [Fact]
    public async Task CrearClienteAsync_IngresosMinimos_RetornaClienteCreado()
    {
        // Arrange
        var clienteDto = new ClienteCrearDto
        {
            Nombre = "Pedro",
            FechaNacimiento = new DateTime(1988, 3, 10),
            Sexo = "Masculino",
            Ingresos = 1m
        };

        var clienteCreado = new Cliente
        {
            Id = Guid.NewGuid(),
            Nombre = "Pedro",
            FechaNacimiento = new DateTime(1988, 3, 10),
            Sexo = Sexo.Masculino,
            Ingresos = 1m
        };

        _mockClienteRepo.Setup(x => x.AgregarAsync(It.IsAny<Cliente>()))
            .ReturnsAsync(clienteCreado);

        // Act
        var resultado = await _clienteServicio.CrearClienteAsync(clienteDto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Pedro", resultado.Nombre);
        Assert.Equal(1m, resultado.Ingresos);
        _mockClienteRepo.Verify(x => x.AgregarAsync(It.IsAny<Cliente>()), Times.Once);
    }

    #endregion

    #region Obtener cliente por Id

    /// <summary>
    /// Verifica que se retorne un cliente existente por su identificador.
    /// </summary>
    [Fact]
    public async Task ObtenerClienteAsync_ClienteExiste_RetornaCliente()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente
        {
            Id = clienteId,
            Nombre = "Ana",
            FechaNacimiento = new DateTime(1992, 7, 18),
            Sexo = Sexo.Femenino,
            Ingresos = 60000m
        };

        _mockClienteRepo.Setup(x => x.ObtenerPorIdAsync(clienteId))
            .ReturnsAsync(cliente);

        // Act
        var resultado = await _clienteServicio.ObtenerClienteAsync(clienteId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(clienteId, resultado.Id);
        Assert.Equal("Ana", resultado.Nombre);
        Assert.Equal("Femenino", resultado.Sexo);
        _mockClienteRepo.Verify(x => x.ObtenerPorIdAsync(clienteId), Times.Once);
    }

    /// <summary>
    /// Verifica que se retorne <c>null</c> al buscar un cliente inexistente.
    /// </summary>
    [Fact]
    public async Task ObtenerClienteAsync_ClienteNoExiste_RetornaNull()
    {
        // Arrange
        var clienteId = Guid.NewGuid();

        _mockClienteRepo.Setup(x => x.ObtenerPorIdAsync(clienteId))
            .ReturnsAsync((Cliente?)null);

        // Act
        var resultado = await _clienteServicio.ObtenerClienteAsync(clienteId);

        // Assert
        Assert.Null(resultado);
        _mockClienteRepo.Verify(x => x.ObtenerPorIdAsync(clienteId), Times.Once);
    }

    /// <summary>
    /// Verifica que un cliente con cuentas asociadas se retorne junto con ellas.
    /// </summary>
    [Fact]
    public async Task ObtenerClienteAsync_ClienteConCuentas_RetornaClienteConCuentas()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente
        {
            Id = clienteId,
            Nombre = "Carlos",
            FechaNacimiento = new DateTime(1985, 12, 5),
            Sexo = Sexo.Masculino,
            Ingresos = 80000m,
            Cuentas = new List<Cuenta>
            {
                new Cuenta { Id = Guid.NewGuid(), NumeroCuenta = "1234567890", Saldo = 5000m, ClienteId = clienteId },
                new Cuenta { Id = Guid.NewGuid(), NumeroCuenta = "0987654321", Saldo = 15000m, ClienteId = clienteId }
            }
        };

        _mockClienteRepo.Setup(x => x.ObtenerPorIdAsync(clienteId))
            .ReturnsAsync(cliente);

        // Act
        var resultado = await _clienteServicio.ObtenerClienteAsync(clienteId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(clienteId, resultado.Id);
        Assert.Equal(2, resultado.Cuentas.Count);
        Assert.Contains(resultado.Cuentas, c => c.NumeroCuenta == "1234567890");
        Assert.Contains(resultado.Cuentas, c => c.NumeroCuenta == "0987654321");
        _mockClienteRepo.Verify(x => x.ObtenerPorIdAsync(clienteId), Times.Once);
    }

    #endregion

    #region Obtener todos los clientes

    /// <summary>
    /// Verifica que se retornen todos los clientes existentes.
    /// </summary>
    [Fact]
    public async Task ObtenerTodosClientesAsync_ExistenClientes_RetornaTodosLosClientes()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new Cliente { Id = Guid.NewGuid(), Nombre = "Cliente 1", FechaNacimiento = new DateTime(1990, 1, 1), Sexo = Sexo.Masculino, Ingresos = 50000m },
            new Cliente { Id = Guid.NewGuid(), Nombre = "Cliente 2", FechaNacimiento = new DateTime(1995, 6, 15), Sexo = Sexo.Femenino, Ingresos = 70000m },
            new Cliente { Id = Guid.NewGuid(), Nombre = "Cliente 3", FechaNacimiento = new DateTime(1988, 9, 30), Sexo = Sexo.Masculino, Ingresos = 45000m }
        };

        _mockClienteRepo.Setup(x => x.ObtenerTodosAsync())
            .ReturnsAsync(clientes);

        // Act
        var resultado = await _clienteServicio.ObtenerTodosClientesAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(3, resultado.Count());
        _mockClienteRepo.Verify(x => x.ObtenerTodosAsync(), Times.Once);
    }

    /// <summary>
    /// Verifica que al no existir clientes se retorne una lista vacía.
    /// </summary>
    [Fact]
    public async Task ObtenerTodosClientesAsync_NoExistenClientes_RetornaListaVacia()
    {
        // Arrange
        var clientesVacios = new List<Cliente>();

        _mockClienteRepo.Setup(x => x.ObtenerTodosAsync())
            .ReturnsAsync(clientesVacios);

        // Act
        var resultado = await _clienteServicio.ObtenerTodosClientesAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        _mockClienteRepo.Verify(x => x.ObtenerTodosAsync(), Times.Once);
    }

    /// <summary>
    /// Verifica que se retornen clientes de ambos sexos correctamente.
    /// </summary>
    [Fact]
    public async Task ObtenerTodosClientesAsync_ClientesConDiferentesSexos_RetornaTodosCorrectamente()
    {
        // Arrange
        var clientes = new List<Cliente>
        {
            new Cliente { Id = Guid.NewGuid(), Nombre = "Hombre 1", FechaNacimiento = new DateTime(1990, 1, 1), Sexo = Sexo.Masculino, Ingresos = 50000m },
            new Cliente { Id = Guid.NewGuid(), Nombre = "Mujer 1", FechaNacimiento = new DateTime(1995, 6, 15), Sexo = Sexo.Femenino, Ingresos = 70000m }
        };

        _mockClienteRepo.Setup(x => x.ObtenerTodosAsync())
            .ReturnsAsync(clientes);

        // Act
        var resultado = await _clienteServicio.ObtenerTodosClientesAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        Assert.Equal("Masculino", resultado.First().Sexo);
        Assert.Equal("Femenino", resultado.Last().Sexo);
        _mockClienteRepo.Verify(x => x.ObtenerTodosAsync(), Times.Once);
    }

    #endregion
}
