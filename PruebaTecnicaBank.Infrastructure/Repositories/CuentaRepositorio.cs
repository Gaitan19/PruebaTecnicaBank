using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para manejar operaciones relacionadas con cuentas bancarias.
    /// </summary>
    public class CuentaRepositorio : ICuentaRepositorio
    {
        private readonly BankDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CuentaRepositorio"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        public CuentaRepositorio(BankDbContext context) => _context = context;

        /// <summary>
        /// Agrega una nueva cuenta de forma asíncrona.
        /// </summary>
        /// <param name="cuenta">La cuenta a agregar.</param>
        /// <returns>La cuenta agregada.</returns>
        public async Task<Cuenta> AgregarAsync(Cuenta cuenta)
        {
            _context.Accounts.Add(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }

        /// <summary>
        /// Obtiene una cuenta por su número de cuenta de forma asíncrona.
        /// </summary>
        /// <param name="numeroCuenta">El número de cuenta a buscar.</param>
        /// <returns>La cuenta encontrada o null si no existe.</returns>
        public async Task<Cuenta?> ObtenerPorNumeroAsync(string numeroCuenta) =>
            await _context.Accounts.Include(a => a.Transacciones)
                .FirstOrDefaultAsync(a => a.NumeroCuenta == numeroCuenta);

        /// <summary>
        /// Actualiza una cuenta existente de forma asíncrona.
        /// </summary>
        /// <param name="cuenta">La cuenta a actualizar.</param>
        public async Task ActualizarAsync(Cuenta cuenta)
        {
            _context.Accounts.Update(cuenta);
            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Verifica si existe una cuenta con el número especificado.
        /// </summary>
        /// <param name="numeroCuenta">El número de cuenta a verificar.</param>
        /// <returns>True si existe, false si no.</returns>
        public async Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta) =>
            await _context.Accounts.AnyAsync(a => a.NumeroCuenta == numeroCuenta);
    }
}
