using Microsoft.EntityFrameworkCore;
using Repository.Models.Entities;

namespace Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ExchangeNote> ExchangeNotes { get; set; }
        public DbSet<NoteItem> NoteItems { get; set; }
        public DbSet<StockCheckNote> StockCheckNotes { get; set; }
        public DbSet<StockCheckProduct> StockCheckProducts { get; set; }
        public DbSet<InvalidatedToken> InvalidatedTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=swd392-mysql-server.mysql.database.azure.com;Database=InventoryManagement;User=duongtb;Password=17122004Admin;SslMode=Required;";
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 41)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            
            // Category unique constraint
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryCode)
                .IsUnique();

            // Warehouse unique constraint
            modelBuilder.Entity<Warehouse>()
                .HasIndex(w => w.WarehouseCode)
                .IsUnique();

            // ProductType relationships
            modelBuilder.Entity<ProductType>()
                .HasOne(pt => pt.Category)
                .WithMany()
                .HasForeignKey("CategoryCode")
                .HasPrincipalKey(c => c.CategoryCode);

            modelBuilder.Entity<ProductType>()
                .HasIndex(pt => pt.ProductTypeCode)
                .IsUnique();

            // Product relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.ProductTypeCode)
                .HasPrincipalKey(pt => pt.ProductTypeCode);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductCode)
                .IsUnique();

            // Explicit column mapping for Product - commented out due to EF version conflict
            // modelBuilder.Entity<Product>()
            //     .Property(p => p.ProductTypeCode)
            //     .HasColumnName("productType_code");

            // User relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey("RoleId")
                .HasPrincipalKey(r => r.RoleId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Warehouse)
                .WithMany()
                .HasForeignKey("WarehouseCode")
                .HasPrincipalKey(w => w.WarehouseCode)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserCode)
                .IsUnique();

            // ExchangeNote relationships
            modelBuilder.Entity<ExchangeNote>()
                .HasOne(en => en.SourceWarehouse)
                .WithMany()
                .HasForeignKey("SourceWarehouseCode")
                .HasPrincipalKey(w => w.WarehouseCode)
                .IsRequired(false);

            modelBuilder.Entity<ExchangeNote>()
                .HasOne(en => en.DestinationWarehouse)
                .WithMany()
                .HasForeignKey("DestinationWarehouseCode")
                .HasPrincipalKey(w => w.WarehouseCode)
                .IsRequired(false);

            modelBuilder.Entity<ExchangeNote>()
                .HasOne(en => en.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedByUserCode")
                .HasPrincipalKey(u => u.UserCode);

            modelBuilder.Entity<ExchangeNote>()
                .HasOne(en => en.ApprovedBy)
                .WithMany()
                .HasForeignKey("ApprovedByUserCode")
                .HasPrincipalKey(u => u.UserCode)
                .IsRequired(false);

            // NoteItem relationships
            modelBuilder.Entity<NoteItem>()
                .HasOne(ni => ni.Product)
                .WithMany()
                .HasForeignKey("ProductCode")
                .HasPrincipalKey(p => p.ProductCode);

            modelBuilder.Entity<NoteItem>()
                .HasOne(ni => ni.ExchangeNote)
                .WithMany(en => en.NoteItems)
                .HasForeignKey("ExchangeNoteId")
                .HasPrincipalKey(en => en.ExchangeNoteId);

            modelBuilder.Entity<NoteItem>()
                .HasIndex(ni => ni.NoteItemCode)
                .IsUnique();

            // StockCheckNote relationships
            modelBuilder.Entity<StockCheckNote>()
                .HasOne(scn => scn.Warehouse)
                .WithMany()
                .HasForeignKey("WarehouseCode")
                .HasPrincipalKey(w => w.WarehouseCode);

            modelBuilder.Entity<StockCheckNote>()
                .HasOne(scn => scn.Checker)
                .WithMany()
                .HasForeignKey("CheckerUserCode")
                .HasPrincipalKey(u => u.UserCode);

            // StockCheckProduct relationships
            modelBuilder.Entity<StockCheckProduct>()
                .HasOne(scp => scp.StockCheckNote)
                .WithMany(scn => scn.StockCheckProducts)
                .HasForeignKey("StockCheckNoteId")
                .HasPrincipalKey(scn => scn.StockCheckNoteId);

            modelBuilder.Entity<StockCheckProduct>()
                .HasOne(scp => scp.Product)
                .WithMany()
                .HasForeignKey("ProductCode")
                .HasPrincipalKey(p => p.ProductCode);

            // Configure enum conversions
            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ExchangeNote>()
                .Property(en => en.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ExchangeNote>()
                .Property(en => en.TransactionType)
                .HasConversion<string>();

            modelBuilder.Entity<NoteItem>()
                .Property(ni => ni.Status)
                .HasConversion<string>();

            modelBuilder.Entity<StockCheckNote>()
                .Property(scn => scn.StockCheckStatus)
                .HasConversion<string>();

            modelBuilder.Entity<StockCheckProduct>()
                .Property(scp => scp.StockCheckProductStatus)
                .HasConversion<string>();
        }
    }
}
