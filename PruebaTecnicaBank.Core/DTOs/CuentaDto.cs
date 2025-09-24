using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.DTOs
{
    public class CuentaCrearDto
    {
        [Required(ErrorMessage = "El ID del cliente es obligatorio")]
        public Guid ClienteId { get; set; }
        
        [Required(ErrorMessage = "El saldo inicial es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor a cero")]
        public decimal SaldoInicial { get; set; }
        
        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        [StringLength(20, ErrorMessage = "El número de cuenta no puede tener más de 20 caracteres")]
        public string NumeroCuenta { get; set; } = string.Empty;
    }

    public class CuentaRespuestaDto
    {
        public Guid Id { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}
