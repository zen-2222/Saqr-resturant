using Microsoft.EntityFrameworkCore;
using SaqrResturant.Models;

namespace SaqrResturant.Data
{
    public class ResturantContext : DbContext
    {
        public DbSet<deliveryModel> Deliveries { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<orderModel> Orders { get; set; } = null!;
        public DbSet<OrderDetailsModel> OrderDetails { get; set; } = null!;
        public DbSet<menuModel> Menu { get; set; } = null!;
        public DbSet<ContactUsModel> Inbox { get; set; } = null!; 



        public ResturantContext(DbContextOptions<ResturantContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this line is to make sure that on deletion of a user nothing will happen,
            // if omitted, the nuGet package manager will complain that there's a cylce foreign key reference
            // meaning that delivery table has FK from orders & users and orders has FK from users
            // which could cause a cylce if we delete a user, nuGet is just paranoid.
            modelBuilder.Entity<deliveryModel>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.DeliveryUserId)
                .OnDelete(DeleteBehavior.NoAction);


            // seeding initial data

            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { Id=1, userName="zen2222", fullName="mohammed jalamnih", password="1111", phoneNumber="0598516128", deliveryLocation="jenin, abu-sufyan street", role=Role.Admin },
                new UserModel { Id = 2, userName ="muhaisen", fullName="Ahmed muhaisen",password="2222",phoneNumber="059851512",deliveryLocation="jenin, nablus street",role=Role.User },
                new UserModel { Id = 3, userName ="doe", fullName="john doe",password="3333",phoneNumber="056123423",deliveryLocation="Ramallah,jenin street",role=Role.Delivery },
                new UserModel { Id = 4, userName ="JD", fullName="jane doe",password="4444",phoneNumber="05678923412",deliveryLocation="nablus, jenin street",role=Role.Delivery },
                new UserModel { Id = 5, userName ="MJ", fullName="jickel mackson",password="5555",phoneNumber="05987234123",deliveryLocation="jerusalem, salah-al-deen street",role=Role.Admin }
                );



        }





    }
}
