using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryManagement.Core.Enums;

namespace LibraryManagement.Core.DTOs
{
    public class BorrowingRequestDTO
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Waiting;
        public string Note { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> BookTitles { get; set; } = [];
    }

    public class BorrowingRequestDetailDTO
    {
        public Guid Id { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
    }

    public class CreateBorrowingRequestDTO
    {
        public Guid UserId { get; set; }
        public List<Guid> BookIds { get; set; } = [];
        public string Note { get; set; } = string.Empty;
    }

    public class UpdateBorrowingRequestDTO
    {
        public Guid RequestId { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}