// Data/AppDbContext.cs - UPDATED VERSION
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Author entity
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.AuthorId);
                entity.Property(a => a.AuthorName).IsRequired();
            });

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.BookId);
                entity.Property(b => b.Title).IsRequired();
                entity.Property(b => b.Publisher).HasMaxLength(200);
                entity.Property(b => b.Barcode).HasMaxLength(50);
                entity.Property(b => b.ISBN).HasMaxLength(20);
                entity.Property(b => b.SubjectGenre).HasMaxLength(100);
                
                //  Configure availability fields
                entity.Property(b => b.TotalCopies).HasDefaultValue(1);
                entity.Property(b => b.AvailableCopies).HasDefaultValue(1);
                
                //relation
                entity.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Faculty).HasMaxLength(100);
                entity.Property(s => s.Semester).HasMaxLength(20);
            });

            // Configure Issue entity - ENHANCED
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(i => i.IssueId);
                
                // Configure date fields
                entity.Property(i => i.IssueDate).IsRequired();
                entity.Property(i => i.DueDate).IsRequired();
                entity.Property(i => i.ReturnDate).IsRequired(false);
                entity.Property(i => i.IsReturned).HasDefaultValue(false);
                
                //relation
                entity.HasOne(i => i.Book)
                    .WithMany(b => b.Issues)
                    .HasForeignKey(i => i.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(i => i.Student)
                    .WithMany(s => s.Issues)
                    .HasForeignKey(i => i.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}