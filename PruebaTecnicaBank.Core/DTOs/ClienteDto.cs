using PruebaTecnicaBank.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.DTOs
{
    public class ClienteCrearDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }
        
        [Required(ErrorMessage = "El sexo es obligatorio")]
        [SexoValidation]
        public string Sexo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Los ingresos son obligatorios")]
        [Range(1, double.MaxValue, ErrorMessage = "El ingreso debe de ser mayor a 0")]
        public decimal Ingresos { get; set; }
    }

    public class ClienteRespuestaDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public decimal Ingresos { get; set; }
        public List<CuentaRespuestaDto> Cuentas { get; set; } = new List<CuentaRespuestaDto>();
    }
}
