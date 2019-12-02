﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamA.CustomerAccounts.Data;

namespace TeamA.CustomerAccounts.Data.Migrations
{
    [DbContext(typeof(AccountsDb))]
    [Migration("20191128160757_integration")]
    partial class integration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TeamA.CustomerAccounts.Models.CustomerAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<bool>("CanPurchase");

                    b.Property<DateTime>("DOB");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleteRequested");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("LoggedOnAt");

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("Postcode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CustomerAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}