using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public enum WeatherType
{
    Cloudy,
    Sunny
}

public class WeatherEntry
{
    public int Id { get; set; }

    [Column(TypeName = "citext")]
    public required string Name { get; set; }

    public WeatherType Kind { get; set; }
}

public class ApplicationContext(DbContextOptions<ApplicationContext> context) : DbContext(context)
{
    public DbSet<WeatherEntry> WeatherEntries { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
        builder.UseNpgsql(
            "Host=localhost;Username=name;Password=password;Database=db;Port=5432",
            opt => opt.MapEnum<WeatherType>()
        ).UseSeeding((context, _) =>
        {
            context.Add<WeatherEntry>(new()
            {
                Name = "Name",
                Kind = WeatherType.Cloudy,
            });

            context.SaveChanges();
        }).UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            context.Add<WeatherEntry>(new()
            {
                Name = "Name",
                Kind = WeatherType.Cloudy,
            });

            await context.SaveChangesAsync(cancellationToken);
        });
}
