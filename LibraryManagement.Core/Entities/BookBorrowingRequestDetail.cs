using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities
{
    public class BookBorrowingRequestDetail : BaseEntity
    {
        [Required]
        public Guid BookBorrowingRequestId { get; set; }

        [ForeignKey("BookBorrowingRequestId")]
        public BookBorrowingRequest BookBorrowingRequest { get; set; } = null!;

        [Required]
        public Guid BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }

}