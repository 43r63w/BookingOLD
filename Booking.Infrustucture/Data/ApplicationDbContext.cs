using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrustucture.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Price = 500,
                    Sqrt = 300,
                    Occupancy = 8,
                    ImageUrl =  "12345",
                },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Price = 400,
                    Sqrt = 250,
                    Occupancy = 6,
                    ImageUrl = "12345",
                }
                );


        }
    }
}
