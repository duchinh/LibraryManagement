using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Core.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string Status { get; set; }
    }

    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int CategoryId { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public class UpdateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int CategoryId { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}