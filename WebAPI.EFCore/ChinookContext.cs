﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore;
using WebAPI.DataAccess.Models;

namespace WebAPI.DataAccess;

public partial class ChinookContext : DbContext
{
    public ChinookContext(DbContextOptions<ChinookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<MediaType> MediaTypes { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");

            entity.HasIndex(e => e.ArtistId, "IFK_AlbumArtistId");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR(160)");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");

            entity.Property(e => e.ArtistId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.SupportRepId, "IFK_CustomerSupportRepId");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasColumnType("NVARCHAR(70)");
            entity.Property(e => e.City).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.Company).HasColumnType("NVARCHAR(80)");
            entity.Property(e => e.Country).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("NVARCHAR(60)");
            entity.Property(e => e.Fax).HasColumnType("NVARCHAR(24)");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnType("NVARCHAR(20)");
            entity.Property(e => e.Phone).HasColumnType("NVARCHAR(24)");
            entity.Property(e => e.PostalCode).HasColumnType("NVARCHAR(10)");
            entity.Property(e => e.State).HasColumnType("NVARCHAR(40)");

            entity.HasOne(d => d.SupportRep).WithMany(p => p.Customers).HasForeignKey(d => d.SupportRepId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.ReportsTo, "IFK_EmployeeReportsTo");

            entity.Property(e => e.EmployeeId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasColumnType("NVARCHAR(70)");
            entity.Property(e => e.BirthDate).HasColumnType("DATETIME");
            entity.Property(e => e.City).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.Country).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.Email).HasColumnType("NVARCHAR(60)");
            entity.Property(e => e.Fax).HasColumnType("NVARCHAR(24)");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnType("NVARCHAR(20)");
            entity.Property(e => e.HireDate).HasColumnType("DATETIME");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnType("NVARCHAR(20)");
            entity.Property(e => e.Phone).HasColumnType("NVARCHAR(24)");
            entity.Property(e => e.PostalCode).HasColumnType("NVARCHAR(10)");
            entity.Property(e => e.State).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.Title).HasColumnType("NVARCHAR(30)");

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation).HasForeignKey(d => d.ReportsTo);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.HasIndex(e => e.CustomerId, "IFK_InvoiceCustomerId");

            entity.Property(e => e.InvoiceId).ValueGeneratedNever();
            entity.Property(e => e.BillingAddress).HasColumnType("NVARCHAR(70)");
            entity.Property(e => e.BillingCity).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.BillingCountry).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.BillingPostalCode).HasColumnType("NVARCHAR(10)");
            entity.Property(e => e.BillingState).HasColumnType("NVARCHAR(40)");
            entity.Property(e => e.InvoiceDate).HasColumnType("DATETIME");
            entity.Property(e => e.Total).HasColumnType("NUMERIC(10,2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.ToTable("InvoiceLine");

            entity.HasIndex(e => e.InvoiceId, "IFK_InvoiceLineInvoiceId");

            entity.HasIndex(e => e.TrackId, "IFK_InvoiceLineTrackId");

            entity.Property(e => e.InvoiceLineId).ValueGeneratedNever();
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC(10,2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Track).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MediaType>(entity =>
        {
            entity.ToTable("MediaType");

            entity.Property(e => e.MediaTypeId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.ToTable("Playlist");

            entity.Property(e => e.PlaylistId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("NVARCHAR(120)");

            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TrackId");
                        j.ToTable("PlaylistTrack");
                        j.HasIndex(new[] { "PlaylistId" }, "IFK_PlaylistTrackPlaylistId");
                        j.HasIndex(new[] { "TrackId" }, "IFK_PlaylistTrackTrackId");
                    });
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Track");

            entity.HasIndex(e => e.AlbumId, "IFK_TrackAlbumId");

            entity.HasIndex(e => e.GenreId, "IFK_TrackGenreId");

            entity.HasIndex(e => e.MediaTypeId, "IFK_TrackMediaTypeId");

            //entity.Property(e => e.TrackId).ValueGeneratedNever();
            entity.Property(e => e.Composer).HasColumnType("NVARCHAR(220)");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("NVARCHAR(200)");
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC(10,2)");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks).HasForeignKey(d => d.AlbumId);

            entity.HasOne(d => d.Genre).WithMany(p => p.Tracks).HasForeignKey(d => d.GenreId);

            entity.HasOne(d => d.MediaType).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.MediaTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}