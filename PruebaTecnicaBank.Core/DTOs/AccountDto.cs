using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.DTOs
{
    public class AccountCreateDto
    {
        public Guid ClientId { get; set; }
        public decimal InitialBalance { get; set; }
    }

    public class AccountResponseDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
