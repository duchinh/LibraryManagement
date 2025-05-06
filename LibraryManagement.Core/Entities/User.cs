using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Entities
{
    public class User
    {
        public User()
        {
            Borrows = new List<Borrow>();
            Reviews = new List<Review>();
            BorrowingRequests = new List<BookBorrowingRequest>();
            ApprovedRequests = new List<BookBorrowingRequest>();
            CreatedAt = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsEmailVerified { get; set; }

        public string? EmailVerificationToken { get; set; }

        public string? ResetPasswordToken { get; set; }

        public DateTime? ResetPasswordTokenExpiry { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiresAt { get; set; }

        // Navigation properties
        public ICollection<Borrow> Borrows { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<BookBorrowingRequest> BorrowingRequests { get; set; }
        public ICollection<BookBorrowingRequest> ApprovedRequests { get; set; }
    }
}