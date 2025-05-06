using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities
{
    public class BookBorrowingRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestorId { get; set; }

        [ForeignKey("RequestorId")]
        public User Requestor { get; set; }

        public int? ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public User Approver { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public DateTime? ProcessedDate { get; set; }

        [Required]
        public BorrowingRequestStatus Status { get; set; }

        [StringLength(500)]
        public string RejectionReason { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        // Navigation properties
        public ICollection<BookBorrowingRequestDetail> Details { get; set; }
    }

    public enum BorrowingRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}