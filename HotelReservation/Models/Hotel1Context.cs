using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Models;

public partial class Hotel1Context : DbContext
{
    public Hotel1Context()
    {
    }

    public Hotel1Context(DbContextOptions<Hotel1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=Hotel_1;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Booking__490D1AE1AA59CEDF");

            entity.ToTable("Booking");

            entity.Property(e => e.BookId)
                .ValueGeneratedNever()
                .HasColumnName("book_id");
            entity.Property(e => e.AmountPayed).HasColumnName("amount_Payed");
            entity.Property(e => e.BillId)
                .HasComputedColumnSql("([book_id])", false)
                .HasColumnName("Bill_id");
            entity.Property(e => e.CheckinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("checkin_date");
            entity.Property(e => e.ChenckoutDate).HasColumnName("chenckout_date");
            entity.Property(e => e.GuestId).HasColumnName("Guest_id");
            entity.Property(e => e.PayDate).HasColumnName("payDate");
            entity.Property(e => e.RoomId).HasColumnName("Room_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Not_Payed")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.Guest).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK__Booking__Guest_i__3F466844");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Booking__Room_id__403A8C7D");

            entity.HasMany(d => d.Services).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "ServiceBooking",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Service_B__Servi__46E78A0C"),
                    l => l.HasOne<Booking>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Service_B__book___45F365D3"),
                    j =>
                    {
                        j.HasKey("BookId", "ServiceId").HasName("PK__Service___12DD499A52D2B2CE");
                        j.ToTable("Service_Booking");
                        j.IndexerProperty<long>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<int>("ServiceId").HasColumnName("Service_id");
                    });
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GId).HasName("PK__Guest__15398ADA7A63772A");

            entity.ToTable("Guest");

            entity.Property(e => e.GId)
                .ValueGeneratedNever()
                .HasColumnName("G_id");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Gname)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("PK__Room__DE142AC1BCB3EAB1");

            entity.ToTable("Room");

            entity.Property(e => e.RId)
                .ValueGeneratedNever()
                .HasColumnName("R_id");
            entity.Property(e => e.Availability)
                .HasDefaultValueSql("('0')")
                .HasColumnName("availability");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.RType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("R_type");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__Services__A3DCCCA5D4E69A7A");

            entity.Property(e => e.SId)
                .ValueGeneratedNever()
                .HasColumnName("S_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.SName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
