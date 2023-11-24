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
        public DbSet<VillaNumber> VillaNumbers { get; set; }

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
                    ImageUrl = "12345",
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
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1,
                },
                 new VillaNumber
                 {
                     Villa_Number = 102,
                     VillaId = 1,
                 },
                  new VillaNumber
                  {
                      Villa_Number = 103,
                      VillaId = 1,
                  },
                   new VillaNumber
                   {
                       Villa_Number = 104,
                       VillaId = 1,
                   },
                    new VillaNumber
                    {
                        Villa_Number = 201,
                        VillaId = 2,
                    },
                       new VillaNumber
                       {
                           Villa_Number = 202,
                           VillaId = 2,
                       },
                          new VillaNumber
                          {
                              Villa_Number = 203,
                              VillaId = 2,
                          },
                            new VillaNumber
                            {
                                Villa_Number = 204,
                                VillaId = 2,
                            }
                );


        }
    }
}
