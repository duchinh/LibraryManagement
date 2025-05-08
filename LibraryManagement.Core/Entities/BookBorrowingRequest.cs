using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Core.Enums;
namespace LibraryManagement.Core.Entities
{
    public class BookBorrowingRequest : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public DateTime RequestDate { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Waiting;

        [StringLength(500)]
        public string Note { get; set; } = string.Empty;
        [StringLength(500)]
        public string RejectionReason { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<BookBorrowingRequestDetail> BookBorrowingRequestDetails { get; set; } = [];
    }

}