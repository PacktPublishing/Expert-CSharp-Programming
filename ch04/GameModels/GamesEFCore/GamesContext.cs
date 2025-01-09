using GameModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System.Text.Json;

namespace GamesEFCore;

internal class GamesContext(DbContextOptions<GamesContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Move> Moves => Set<Move>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .UseTpcMappingStrategy();
        modelBuilder.Entity<Game>()
            .Property(g => g.PlayerName)
            .HasMaxLength(40);
 
        modelBuilder.Entity<ColorGame>()
            .HasBaseType<Game>();

        modelBuilder.Entity<ShapeAndColorGame>()
            .HasBaseType<Game>();

        modelBuilder.Entity<ColorGame>()
            .Property(g => g.Solution)
            .HasJsonPropertyName("Solution");

        modelBuilder.Entity<ShapeAndColorGame>()
            .Property(g => g.Solution)
            .HasColumnType("nvarchar")
            .HasMaxLength(400)
            .HasConversion<ShapeAndColorListValueConverter>(new ShapeAndColorListValueComparer());

        modelBuilder.Entity<Move>()
            .UseTpcMappingStrategy();
        modelBuilder.Entity<Move>()
            .Property<int>("GameId"); // shadow property
        modelBuilder.Entity<ColorMove>()
            .HasBaseType<Move>();
        modelBuilder.Entity<ShapeAndColorMove>()
            .HasBaseType<Move>();
        modelBuilder.Entity<ShapeAndColorMove>()
            .Property(m => m.ShapesAndColors)
            .HasConversion<ShapeAndColorListValueConverter>(new ShapeAndColorListValueComparer());
    }
}

internal class ShapeAndColorListValueConverter() : ValueConverter<ShapeAndColor[], string>(
    convertToProviderExpression: shapesAndColors => JsonSerializer.Serialize(shapesAndColors, (JsonSerializerOptions?)null),
    convertFromProviderExpression: s => JsonSerializer.Deserialize<ShapeAndColor[]?>(s, (JsonSerializerOptions?)null) ?? Array.Empty<ShapeAndColor>())
{
}

internal class ShapeAndColorListValueComparer() : ValueComparer<ShapeAndColor[]>(
    equalsExpression: (c1, c2) => c1!.SequenceEqual(c2!),
    hashCodeExpression: c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
    snapshotExpression: c => c.ToArray())
{
}