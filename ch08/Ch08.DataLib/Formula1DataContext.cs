using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using System.Text.Json;

namespace Ch08.DataLib;

public class Formula1DataContext(DbContextOptions<Formula1DataContext> options) : DbContext(options)
{
    public DbSet<Racer> Racers => Set<Racer>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<RacerTeamMap> RacersTeamsMap => Set<RacerTeamMap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var isPostgres = Database.ProviderName?.Contains("Npgsql") ?? false;

        modelBuilder.Entity<Racer>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(r => r.LastName).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Country).IsRequired().HasMaxLength(50);
            entity.Property(r => r.BirthDay).IsRequired();
            entity.Property(r => r.Wins).IsRequired();

            // Configure relationship with Teams through RacerTeamMap
            entity.HasMany(r => r.Teams)
                  .WithOne(rt => rt.Racer)
                  .HasForeignKey(rt => rt.RacerId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure Championships as JSON column
            var championshipsProperty = entity.Property(r => r.Championships);
            if (isPostgres)
            {
                championshipsProperty
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonContext.Default.ListInt32),
                        v => JsonSerializer.Deserialize(v, JsonContext.Default.ListInt32) ?? new List<int>()
                    );
            }
            else
            {
                // SQL Server
                championshipsProperty
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonContext.Default.ListInt32),
                        v => JsonSerializer.Deserialize(v, JsonContext.Default.ListInt32) ?? new List<int>()
                    );
            }
        });

        modelBuilder.Entity<RacerTeamMap>(entity =>
        {
            // combined key
            entity.HasKey(rt => new { rt.RacerId, rt.TeamId, rt.Year });

            // Configure relationships
            entity.HasOne(rt => rt.Team)
                  .WithMany(t => t.Racers)
                  .HasForeignKey(rt => rt.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(rt => rt.Racer)
                  .WithMany(r => r.Teams)
                  .HasForeignKey(rt => rt.RacerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Country).IsRequired().HasMaxLength(50);
            entity.Property(t => t.FoundedYear).IsRequired();

            // Configure relationship with Racers through RacerTeamMap
            entity.HasMany(t => t.Racers)
                  .WithOne(rt => rt.Team)
                  .HasForeignKey(rt => rt.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure Championships as JSON column
            var championshipsProperty = entity.Property(t => t.Championships);
            if (isPostgres)
            {
                championshipsProperty
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonContext.Default.ListInt32),
                        v => JsonSerializer.Deserialize(v, JsonContext.Default.ListInt32) ?? new List<int>()
                    );
            }
            else
            {
                // SQL Server
                championshipsProperty
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonContext.Default.ListInt32),
                        v => JsonSerializer.Deserialize(v, JsonContext.Default.ListInt32) ?? new List<int>()
                    );
            }
        });

        base.OnModelCreating(modelBuilder);
    }
}