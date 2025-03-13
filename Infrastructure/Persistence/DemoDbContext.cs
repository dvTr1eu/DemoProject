using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DemoDbContext : IdentityDbContext<User>
    {

        public DemoDbContext()
        {
        }

        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ShowtimeDetail> ShowTimeDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieType> MovieTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<BookingSeat>()
            .HasKey(bs => new { bs.BookId, bs.SeatId, bs.ShowTimeId });

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany()
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.ShowtimeDetail)
                .WithMany() 
                .HasForeignKey(bs => bs.ShowTimeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Seats)
                .WithOne(s => s.Room)
                .HasForeignKey(s => s.RoomId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);

            modelBuilder.Entity<MovieType>()
                .HasKey(mt => new { mt.MovieId, mt.GenreId });

            modelBuilder.Entity<MovieType>()
                .HasOne(mt => mt.Movie)
                .WithMany(m => m.MovieTypes)
                .HasForeignKey(mt => mt.MovieId);

            modelBuilder.Entity<MovieType>()
                .HasOne(mt => mt.Genre)
                .WithMany(t => t.MovieTypes)
                .HasForeignKey(mt => mt.GenreId);
            modelBuilder.Entity<ShowtimeDetail>()
                .HasOne(s => s.Show)
                .WithMany(m => m.ShowTimeDetails)
                .HasForeignKey(s => s.ShowId);
        }
    }
}
