using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.Entities
{
    public class User : BaseEntity
    {

        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;


        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        [Required]
        public bool IsActive { get; set; }

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
        public ICollection<BookBorrowingRequest> BookBorrowingRequests { get; set; } = [];

    }
}