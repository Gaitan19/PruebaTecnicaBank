using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Entities
{
    public class Transaccion
    {
        public Guid Id { get; set; }
        public Guid CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoDespues { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
