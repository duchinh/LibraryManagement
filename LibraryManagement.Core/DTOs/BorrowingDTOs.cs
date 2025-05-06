using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Core.DTOs
{
    public class BorrowingRequestDTO
    {
        public int Id { get; set; }
        public int RequestorId { get; set; }
        public string RequestorName { get; set; }
        public int? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
        public List<BorrowingRequestDetailDTO> Details { get; set; }
    }

    public class BorrowingRequestDetailDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    public class CreateBorrowingRequestDTO
    {
        [Required]
        public List<int> BookIds { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public string Notes { get; set; }
    }

    public class UpdateBorrowingRequestDTO
    {
        [Required]
        public string Status { get; set; }

        public string RejectionReason { get; set; }
    }
}