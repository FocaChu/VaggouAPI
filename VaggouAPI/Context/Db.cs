using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        public DbSet<Adress> Adresses { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<MonthlyReport> MonthlyReports { get; set; }

        public DbSet<ParkingLot> ParkingLots { get; set; }

        public DbSet<ParkingSpot> ParkingSpots { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
