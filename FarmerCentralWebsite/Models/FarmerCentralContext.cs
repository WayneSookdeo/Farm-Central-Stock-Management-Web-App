using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FarmerCentralWebsite.Models;

public partial class FarmerCentralContext : DbContext
{
    public FarmerCentralContext()
    {
    }

    public FarmerCentralContext(DbContextOptions<FarmerCentralContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Farmer> Farmers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:practicepoeserver.database.windows.net,1433;Initial Catalog=FarmerCentral;Persist Security Info=False;User ID=admn;Password=Wentnews.46;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeEmail).HasName("PK__Employee__05BE9A852B1A6F5A");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("employeeEmail");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Farmer>(entity =>
        {
            entity.HasKey(e => e.FarmerEmail).HasName("PK__Farmers__B0DE2CCE732BCA3A");

            entity.Property(e => e.FarmerEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("farmerEmail");
            entity.Property(e => e.FarmerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("farmerName");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__2D10D14A0F5AF966");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.DateSupplied)
                .HasColumnType("date")
                .HasColumnName("dateSupplied");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("productName");
            entity.Property(e => e.ProductType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("productType");
            entity.Property(e => e.StockPrice).HasColumnName("stockPrice");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stock__CBAD8743932F347F");

            entity.ToTable("Stock");

            entity.Property(e => e.StockId).HasColumnName("stockID");
            entity.Property(e => e.FarmerEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("farmerEmail");
            entity.Property(e => e.ProductId).HasColumnName("productID");

            entity.HasOne(d => d.FarmerEmailNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.FarmerEmail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__farmerEma__619B8048");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__productID__60A75C0F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
