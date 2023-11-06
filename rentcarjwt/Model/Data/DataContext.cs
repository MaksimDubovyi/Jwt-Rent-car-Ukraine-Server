using Microsoft.EntityFrameworkCore;
using rentcarjwt.Model.Data.Entity;

namespace rentcarjwt.Model.Data
{
    public class DataContext : DbContext
    {



        public static readonly String SchemaName = "ServerRcar";


        public DbSet<User> Users { get; set; }
        public DbSet<Entity.Car> Cars { get; set; }
        public DbSet<MarkerCar> MarkerCars { get; set; }
        public DbSet<ReserveCar> ReserveCars { get; set; }
        public DbSet<Messages> Messages { get; set; }



        public DataContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //схема - як спосіб розділення проєктів в одній БД
            modelBuilder.HasDefaultSchema(SchemaName);
        }
    }
}
