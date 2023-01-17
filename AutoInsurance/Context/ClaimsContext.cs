using AutoInsurance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AutoInsurance.Context
{
    public class ClaimsContext : DbContext
    {
        public ClaimsContext(DbContextOptions<ClaimsContext> options)
            : base(options)
        {
        }

        public DbSet<ClaimRegistration> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClaimEntityConfiguraton());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class ClaimEntityConfiguraton : IEntityTypeConfiguration<ClaimRegistration>
    {
        public void Configure(EntityTypeBuilder<ClaimRegistration> builder)
        {
            builder.ToTable("Claims");
            builder.HasKey(o => o.Id);
            builder.Property(t => t.Id).IsRequired().HasColumnType("int")
                   .ValueGeneratedOnAdd();
            builder.Property(t => t.Name).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(t => t.FatherName).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(t => t.PolicyNumber).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(t => t.Status).IsRequired().HasColumnType("nvarchar(20)");
            builder.Property(t => t.Gender).IsRequired().HasColumnType("nvarchar(10)");
            builder.Property(t => t.Address).IsRequired().HasColumnType("nvarchar(1000)");
            builder.Property(t => t.Age).IsRequired().HasColumnType("int");
            builder.Property(t => t.CreatedDate).IsRequired().HasColumnType("datetime");
            builder.Property(t => t.UpdatedDate).HasColumnType("datetime");
            builder.Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar(50)");
            builder.Property(t => t.UpdatedBy).HasColumnType("nvarchar(50)");
            builder.Property(t => t.Documents).HasColumnType("nvarchar(MAX)");
            builder.Property(t => t.DocumentName).HasColumnType("nvarchar(100)");
        }
    }
}
