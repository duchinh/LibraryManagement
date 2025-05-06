using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        [StringLength(100)]
        public required string Author { get; set; }

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public required Category Category { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public BookStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [StringLength(200)]
        public string Publisher { get; set; }

        [Range(1, int.MaxValue)]
        public int PublicationYear { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Navigation properties
        public ICollection<BookBorrowingRequestDetail> BorrowingDetails { get; set; }
        public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();

        public bool IsAvailable()
        {
            return Quantity > 0;
        }
    }

    public enum BookStatus
    {
        Available,
        Unavailable,
        Borrowed,
        Reserved,
        Lost
    }
}