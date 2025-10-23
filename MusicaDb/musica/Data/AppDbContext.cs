using Microsoft.EntityFrameworkCore;
using musica.Models;

namespace musica.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Musica> Musicas => Set<Musica>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=musica.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Musica>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Titulo).IsRequired();
            e.Property(m => m.Artista).IsRequired();
            e.Property(m => m.DataCadastro).IsRequired();
        });
    }
    
    
}