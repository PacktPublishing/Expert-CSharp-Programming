using Microsoft.EntityFrameworkCore;

namespace Ch08.DataLib;

public class Formula1DataContext(DbContextOptions<Formula1DataContext> options) : DbContext(options)
{
    public DbSet<Racer> Racers => Set<Racer>();
    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Racer>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(r => r.LastName).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Country).IsRequired().HasMaxLength(50);
            entity.Property(r => r.BirthDay).IsRequired();
            entity.Property(r => r.NumberWins).IsRequired();
            
            entity.HasOne(r => r.Team)
                  .WithMany(t => t.Racers)
                  .HasForeignKey(r => r.TeamId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Country).IsRequired().HasMaxLength(50);
            entity.Property(t => t.FoundedYear).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}