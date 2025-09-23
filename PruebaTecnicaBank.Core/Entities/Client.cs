using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; } = string.Empty;
        public decimal Income { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
