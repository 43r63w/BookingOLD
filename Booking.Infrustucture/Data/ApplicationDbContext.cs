using Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrustucture.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<Amenity> Amenities { get; set; } 
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


            modelBuilder.Entity<Amenity>().HasData(
                 new Amenity
                 {
                     Id = 1,
                     VillaId = 1,
                     Name = "Private Pool"
                 }, new Amenity
                 {
                     Id = 2,
                     VillaId = 1,
                     Name = "Microwave"
                 }, new Amenity
                 {
                     Id = 3,
                     VillaId = 1,
                     Name = "Private Balcony"
                 }, new Amenity
                 {
                     Id = 4,
                     VillaId = 1,
                     Name = "1 king bed and 1 sofa bed"
                 },

              new Amenity
              {
                  Id = 5,
                  VillaId = 2,
                  Name = "Private Plunge Pool"
              }, new Amenity
              {
                  Id = 6,
                  VillaId = 2,
                  Name = "Microwave and Mini Refrigerator"
              }, new Amenity
              {
                  Id = 7,
                  VillaId = 2,
                  Name = "Private Balcony"
              }, new Amenity
              {
                  Id = 8,
                  VillaId = 2,
                  Name = "king bed or 2 double beds"
              }         
              );

        }
    }
}
