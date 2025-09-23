using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Infrastructure
{
    /// <summary>
    /// Contexto de la base de datos para la aplicación bancaria.
    /// </summary>
    public class BankDbContext : DbContext
    {
       
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        /// <summary>
        /// Conjunto de entidades de clientes.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Conjunto de entidades de cuentas.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Conjunto de entidades de transacciones.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Configura el modelo de la base de datos.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        }
    }
}
