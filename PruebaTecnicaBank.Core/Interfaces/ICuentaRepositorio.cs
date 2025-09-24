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
    public interface ICuentaRepositorio
    {
        /// <summary>
        /// Agrega una nueva cuenta de forma asíncrona.
        /// </summary>
        /// <param name="cuenta">La cuenta a agregar.</param>
        /// <returns>La cuenta agregada.</returns>
        Task<Cuenta> AgregarAsync(Cuenta cuenta);

        /// <summary>
        /// Obtiene una cuenta por su número de cuenta de forma asíncrona.
        /// </summary>
        /// <param name="numeroCuenta">El número de cuenta a buscar.</param>
        /// <returns>La cuenta encontrada o null si no existe.</returns>
        Task<Cuenta?> ObtenerPorNumeroAsync(string numeroCuenta);

        /// <summary>
        /// Actualiza una cuenta de forma asíncrona.
        /// </summary>
        /// <param name="cuenta">La cuenta a actualizar.</param>
        Task ActualizarAsync(Cuenta cuenta);
        
        /// <summary>
        /// Verifica si existe una cuenta con el número especificado.
        /// </summary>
        /// <param name="numeroCuenta">El número de cuenta a verificar.</param>
        /// <returns>True si existe, false si no.</returns>
        Task<bool> ExisteNumeroCuentaAsync(string numeroCuenta);
    }
}
