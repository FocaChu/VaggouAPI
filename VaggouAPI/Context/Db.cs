using Microsoft.EntityFrameworkCore;
using VaggouAPI;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options)
    {
    }

    // --- DbSets ---
    public DbSet<Address> Adresses { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<MonthlyReport> MonthlyReports { get; set; }
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleModel> VehicleModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User -> Client (!:1)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Client)
            .WithOne(c => c.User)
            .HasForeignKey<Client>(c => c.Id);

        // User -> Role (N:N)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles")); 

        // Client 
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Favorites)
            .WithOne(f => f.Client)
            .HasForeignKey(f => f.ClientId);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Reservations)
            .WithOne(r => r.Client)
            .HasForeignKey(r => r.ClientId);
        
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Vehicles)
            .WithOne(v => v.Owner)
            .HasForeignKey(v => v.OwnerId);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Reviews)
            .WithOne(r => r.Client)
            .HasForeignKey(r => r.ClientId);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.OwnedParkingLots) 
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.OwnerId);

        // Address -> ParkingLot (1:N)
        modelBuilder.Entity<Address>()
            .HasMany(a => a.ParkingLots)
            .WithOne(p => p.Address)
            .HasForeignKey(p => p.AddressId);

        // Favorite -> ParkingLot (N:1)
        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.ParkingLot)
            .WithMany(p => p.Favorites)
            .HasForeignKey(f => f.ParkingLotId);

        // MonthlyReport -> ParkingLot (N:1)
        modelBuilder.Entity<MonthlyReport>()
            .HasOne(r => r.ParkingLot)
            .WithMany(p => p.MonthlyReports)
            .HasForeignKey(r => r.ParkingLotId);

        // ParkingLot
        modelBuilder.Entity<ParkingLot>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // ParkingSpot -> Reservation (1:N)
        modelBuilder.Entity<ParkingSpot>()
            .HasMany(s => s.Reservations)
            .WithOne(r => r.ParkingSpot)
            .HasForeignKey(r => r.ParkingSpotId);

        // Payment -> PaymentMethod (N:1)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.PaymentMethod)
            .WithMany(m => m.Payments)
            .HasForeignKey(p => p.PaymentMethodId);

        // Reservation -> Payment (1:1)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Payment)
            .WithOne(p => p.Reservation)
            .HasForeignKey<Payment>(p => p.Id);

        // Review -> ParkingLot (N:1)
        modelBuilder.Entity<Review>()
            .HasOne(r => r.ParkingLot)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ParkingLotId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vehicle -> VehicleModel (N:1)
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.VehicleModel)
            .WithMany(m => m.Vehicles)
            .HasForeignKey(v => v.VehicleModelId);
    }
}