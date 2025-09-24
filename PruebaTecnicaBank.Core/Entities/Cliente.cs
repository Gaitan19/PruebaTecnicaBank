using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public Sexo Sexo { get; set; }
        public decimal Ingresos { get; set; }

        public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    }
}
