using Microsoft.EntityFrameworkCore;
using musica.Models;

namespace musica.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Album> Albuns => Set<Album>();
    public DbSet<Musica> Musicas => Set<Musica>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Usa SQLite como recomendado em sala
            optionsBuilder.UseSqlite("Data Source=musica.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ================================
        // Album
        // ================================
        modelBuilder.Entity<Album>(e =>
        {
            e.HasKey(a => a.Id);

            e.Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(a => a.Ano)
                .IsRequired();

            // Exemplo: um álbum pode ter o mesmo nome? 
            // Se quiser evitar duplicados:
            e.HasIndex(a => a.Nome).IsUnique();
        });

        // ================================
        // Música
        // ================================
        modelBuilder.Entity<Musica>(e =>
        {
            e.HasKey(m => m.Id);

            e.Property(m => m.Titulo)
                .IsRequired()
                .HasMaxLength(120);

            e.Property(m => m.Artista)
                .IsRequired()
                .HasMaxLength(120);

            e.Property(m => m.Genero)
                .HasMaxLength(60);

            e.HasOne(m => m.Album)
                .WithMany(a => a.Musicas)
                .HasForeignKey(m => m.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

