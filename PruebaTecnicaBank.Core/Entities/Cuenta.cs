using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PruebaTecnicaBank.Core.Entities
{
    public class Cuenta
    {
        public Guid Id { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
