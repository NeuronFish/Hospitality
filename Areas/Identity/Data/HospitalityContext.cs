using Hospitality.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospitality.Data;

public class HospitalityContext : IdentityDbContext<UserIdent>
{
    public HospitalityContext(DbContextOptions<HospitalityContext> options)
        : base(options)
    {
    }

    public DbSet<UserIdent> Users { get; set; }
    public DbSet<Hostel> Hostels { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Assignment> Assignments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Hostel>()
            .HasKey(e => e.Id);
        builder.Entity<Hostel>()
            .HasMany(e => e.Staff)
            .WithOne(e => e.Place)
            .HasForeignKey(e => e.PlaceId);
        builder.Entity<Hostel>()
            .HasOne(e => e.Chief)
            .WithOne(e => e.Owned)
            .HasForeignKey<Hostel>(e => e.ChiefId);
        builder.Entity<Hostel>()
            .HasMany(e => e.Floors)
            .WithOne(e => e.MHostel)
            .HasForeignKey(e => e.HostelId);
        builder.Entity<Floor>()
            .HasMany(e => e.Rooms)
            .WithOne(e => e.MFloor)
            .HasForeignKey(e => e.FloorId);
        builder.Entity<UserIdent>()
            .HasMany(e => e.Assignments)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);
        builder.Entity<Room>()
            .HasMany(e => e.Guests)
            .WithOne(e => e.MRoom)
            .HasForeignKey(e => e.RoomId);
        base.OnModelCreating(builder);
    }
}
