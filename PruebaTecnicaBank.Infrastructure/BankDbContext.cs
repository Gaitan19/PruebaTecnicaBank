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
        public DbSet<Cliente> Clients { get; set; }

        /// <summary>
        /// Conjunto de entidades de cuentas.
        /// </summary>
        public DbSet<Cuenta> Accounts { get; set; }

        /// <summary>
        /// Conjunto de entidades de transacciones.
        /// </summary>
        public DbSet<Transaccion> Transactions { get; set; }

        /// <summary>
        /// Configura el modelo de la base de datos.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Nombre).HasColumnName("Name");
                entity.Property(e => e.FechaNacimiento).HasColumnName("BirthDate");
                entity.Property(e => e.Sexo).HasColumnName("Sex");
                entity.Property(e => e.Ingresos).HasColumnName("Income");
            });

            // Configurar Cuenta
            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.Property(e => e.NumeroCuenta).HasColumnName("AccountNumber");
                entity.Property(e => e.Saldo).HasColumnName("Balance");
                entity.Property(e => e.ClienteId).HasColumnName("ClientId");
                
                entity.HasIndex(a => a.NumeroCuenta).IsUnique();
                entity.HasOne(a => a.Cliente)
                    .WithMany(c => c.Cuentas)
                    .HasForeignKey(a => a.ClienteId);
            });

            // Configurar Transacción
            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.Property(e => e.CuentaId).HasColumnName("AccountId");
                entity.Property(e => e.Tipo).HasColumnName("Type");
                entity.Property(e => e.Monto).HasColumnName("Amount");
                entity.Property(e => e.SaldoDespues).HasColumnName("BalanceAfter");
                entity.Property(e => e.FechaHora).HasColumnName("Timestamp");
                
                entity.HasOne(t => t.Cuenta)
                    .WithMany(a => a.Transacciones)
                    .HasForeignKey(t => t.CuentaId);
            });
        }
    }
}
