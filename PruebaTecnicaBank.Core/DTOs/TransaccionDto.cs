using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.DTOs
{
    public class TransaccionCrearDto
    {
        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal Monto { get; set; }
    }

    public class TransaccionRespuestaDto
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal SaldoDespues { get; set; }
        public DateTime FechaHora { get; set; }
    }

    public class HistorialTransaccionesDto
    {
        public IEnumerable<TransaccionRespuestaDto> Transacciones { get; set; } = new List<TransaccionRespuestaDto>();
        public decimal SaldoFinal { get; set; }
    }
}
