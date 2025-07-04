using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace VaggouAPI
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        public DbSet<Address> Adresses { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<MonthlyReport> MonthlyReports { get; set; }

        public DbSet<MonthlyReportData> MonthlyReportDatas { get; set; }

        public DbSet<ParkingLot> ParkingLots { get; set; }

        public DbSet<ParkingSpot> ParkingSpots { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Adress → ParkingLot
            modelBuilder.Entity<Address>()
                .HasMany(a => a.ParkingLots)
                .WithOne(p => p.Address)
                .HasForeignKey(p => p.AdressId);

            // Client → User
            //modelBuilder.Entity<Client>()
            //    .HasOne(c => c.User)
            //    .WithOne(u => u.Client)
            //    .HasForeignKey<Client>(c => c.UserId);

            // Client → Favorite, ParkingLot, Reservation, Vehicle
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Favorites)
                .WithOne(f => f.Client)
                .HasForeignKey(f => f.ClientId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.ParkingLots)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Reservations)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Owner)
                .HasForeignKey(v => v.OwnerId);

            // Favorite → ParkingLot
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.ParkingLot)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ParkingLotId);

            modelBuilder.Entity<MonthlyReport>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<MonthlyReport>()
                .HasOne(r => r.MonthlyReportData)
                .WithOne(d => d.MonthlyReport)
                .HasForeignKey<MonthlyReportData>(d => d.MonthlyReportId)
                .OnDelete(DeleteBehavior.Cascade); 

            // MonthlyReportData
            modelBuilder.Entity<MonthlyReportData>()
                .HasKey(d => d.Id);

            // ParkingLot → ParkingSpot
            modelBuilder.Entity<ParkingLot>()
                .HasMany(p => p.ParkingSpots)
                .WithOne(s => s.ParkingLot)
                .HasForeignKey(s => s.ParkingLotId);

            modelBuilder.Entity<ParkingLot>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.ParkingLots)
                .WithOne(p => p.Address)
                .HasForeignKey(p => p.AdressId);

            // Review
            modelBuilder.Entity<Review>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Review>()
                .Property(r => r.Score)
                .IsRequired();

            modelBuilder.Entity<Review>()
                .Property(r => r.Comment)
                .HasMaxLength(500);

            // Relacionamento 1:N entre ParkingLot e Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.ParkingLot)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ParkingLotId)
                .OnDelete(DeleteBehavior.Cascade);

            // ParkingSpot → Reservation
            modelBuilder.Entity<ParkingSpot>()
                .HasMany(s => s.Reservations)
                .WithOne(r => r.ParkingSpot)
                .HasForeignKey(r => r.ParkingSpotId);

            // Payment → PaymentMethod, Reservation
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.PaymentMethodId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithOne(r => r.Payment)
                .HasForeignKey<Reservation>(r => r.PaymentId)
                .IsRequired();

            // Reservation → Client, Vehicle, ParkingSpot, Payment
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Vehicle)
                .WithMany(v => v.Reservations)
                .HasForeignKey(r => r.VehicleId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ParkingSpot)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.ParkingSpotId);

            // Vehicle → VehicleModel
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleModel)
                .WithMany(m => m.Vehicles)
                .HasForeignKey(v => v.VehicleModelId);

            // Vehicle → Client
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Owner)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.OwnerId);
        }

    }
}
