using BookList.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookList.API.Context;

public partial class BookListContext : DbContext
{
    public BookListContext()
    {
    }

    public BookListContext(DbContextOptions<BookListContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=BookList;Trusted_Connection=true;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");

            entity.HasIndex(e => e.Name, "UC_Author_Name").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Publisher)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Subtitle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Genre");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookAuthor_Author"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookAuthor_Book"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId");
                        j.ToTable("BookAuthor");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.HasIndex(e => e.Name, "UC_Genre_Name").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
