using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public required Category Category { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(200)]
        public string Publisher { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int PublicationYear { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<BookBorrowingRequestDetail> BookBorrowingRequestDetails { get; set; } = [];
    }
}
