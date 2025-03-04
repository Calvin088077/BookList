using Microsoft.EntityFrameworkCore;
namespace myList.Models;

public partial class listContext : DbContext
{
    public listContext(DbContextOptions<listContext> options)
        : base(options)
    {
    }

    public virtual DbSet<book> book { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<book>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.book_name).HasMaxLength(255);
            entity.Property(e => e.date_added)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.notes).HasColumnType("text");
        });
    }
}
