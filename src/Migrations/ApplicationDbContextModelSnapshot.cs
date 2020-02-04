﻿// <auto-generated />
using CheapFlights.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CheapFlights.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("CheapFlights.Models.AirportModel", b =>
                {
                    b.Property<string>("IataCode")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(3);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("IataCode");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("CheapFlights.Models.FlightModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.Property<string>("DestinationIataCode")
                        .IsRequired();

                    b.Property<string>("OriginIataCode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DestinationIataCode");

                    b.HasIndex("OriginIataCode");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("CheapFlights.Models.FlightModel", b =>
                {
                    b.HasOne("CheapFlights.Models.AirportModel", "Destination")
                        .WithMany()
                        .HasForeignKey("DestinationIataCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CheapFlights.Models.AirportModel", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginIataCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
