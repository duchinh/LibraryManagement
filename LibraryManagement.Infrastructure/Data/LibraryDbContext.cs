using LibraryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookBorrowingRequest> BookBorrowingRequests { get; set; }
        public DbSet<BookBorrowingRequestDetail> BookBorrowingRequestDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasMany(e => e.BookBorrowingRequests)
                    .WithOne(br => br.User)
                    .HasForeignKey(br => br.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasMany(e => e.Books)
                    .WithOne(b => b.Category)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Author).IsRequired();
                entity.HasOne(b => b.Category)
                    .WithMany(c => c.Books)
                    .HasForeignKey(b => b.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(b => b.BookBorrowingRequestDetails)
                    .WithOne(bd => bd.Book)
                    .HasForeignKey(bd => bd.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BookBorrowingRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(br => br.User)
                    .WithMany(u => u.BookBorrowingRequests)
                    .HasForeignKey(br => br.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(br => br.BookBorrowingRequestDetails)
                    .WithOne(bd => bd.BookBorrowingRequest)
                    .HasForeignKey(bd => bd.BookBorrowingRequestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BookBorrowingRequestDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(bd => bd.Book)
                    .WithMany(b => b.BookBorrowingRequestDetails)
                    .HasForeignKey(bd => bd.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(bd => bd.BookBorrowingRequest)
                    .WithMany(br => br.BookBorrowingRequestDetails)
                    .HasForeignKey(bd => bd.BookBorrowingRequestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
