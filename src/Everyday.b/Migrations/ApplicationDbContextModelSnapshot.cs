﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Everyday.b.Data;

namespace Everyday.b.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Everyday.b.Models.Check", b =>
                {
                    b.Property<string>("Id");

                    b.Property<bool>("Checked");

                    b.Property<DateTime>("CheckedDate");

                    b.Property<string>("Comment");

                    b.Property<string>("TodoItemId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("TodoItemId");

                    b.ToTable("Checks");
                });

            modelBuilder.Entity("Everyday.b.Models.TodoItem", b =>
                {
                    b.Property<string>("Id");

                    b.Property<DateTime>("BeginDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BeginDate")
                        .HasName("BeginDateIndex");

                    b.HasIndex("EndDate")
                        .HasName("EndDateIndex");

                    b.HasIndex("UserId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("Everyday.b.Models.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Token");

                    b.Property<DateTime>("TokenExpires");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("EmailIndex");

                    b.HasIndex("UserName")
                        .HasName("UserNameIndex");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Everyday.b.Models.Check", b =>
                {
                    b.HasOne("Everyday.b.Models.TodoItem", "TodoItem")
                        .WithMany("Checks")
                        .HasForeignKey("TodoItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Everyday.b.Models.TodoItem", b =>
                {
                    b.HasOne("Everyday.b.Models.User", "User")
                        .WithMany("TodoItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
