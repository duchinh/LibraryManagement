using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities
{
    public class BookBorrowingRequestDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookBorrowingRequestId { get; set; }

        [ForeignKey("BookBorrowingRequestId")]
        public BookBorrowingRequest BookBorrowingRequest { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public BorrowingDetailStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }

    public enum BorrowingDetailStatus
    {
        Borrowed,
        Returned,
        Overdue
    }
}