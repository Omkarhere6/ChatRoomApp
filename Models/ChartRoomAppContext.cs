﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatRoomApp.Models
{
    public partial class ChartRoomAppContext : DbContext
    {
        public ChartRoomAppContext()
        {
        }

        public ChartRoomAppContext(DbContextOptions<ChartRoomAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<chatrooms> chatrooms { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<chatrooms>(entity =>
            {
                entity.HasKey(e => e.room_id)
                    .HasName("PK__chatroom__19675A8A6FE09D09");

                entity.Property(e => e.room_code).HasMaxLength(255);

                entity.Property(e => e.room_description).HasMaxLength(1000);

                entity.Property(e => e.room_name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.room_type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.user_name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<users>(entity =>
            {
                entity.HasKey(e => e.user_id)
                    .HasName("PK__users__B9BE370FD18A97E3");

                entity.Property(e => e.created_datetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.user_name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}