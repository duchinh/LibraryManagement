using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Services
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBorrowRepository _borrowRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ReportGenerator(IUnitOfWork unitOfWork, IConfiguration configuration, IBorrowRepository borrowRepository, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _borrowRepository = borrowRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<byte[]> GenerateBookReportAsync(string format)
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            return await GenerateReportAsync(books, "Báo cáo sách", format);
        }

        public async Task<byte[]> GenerateBorrowReportAsync(string format, string? startDate = null, string? endDate = null)
        {
            DateTime? start = startDate != null ? DateTime.Parse(startDate) : null;
            DateTime? end = endDate != null ? DateTime.Parse(endDate) : null;

            var borrows = await _borrowRepository.GetBorrowsByDateRangeAsync(start, end);

            // TODO: Generate report based on format
            return Array.Empty<byte>();
        }

        public async Task<byte[]> GenerateOverdueReportAsync(string format)
        {
            var borrows = await _unitOfWork.Borrows.GetOverdueBorrowsAsync();
            return await GenerateReportAsync(borrows, "Báo cáo sách quá hạn", format);
        }

        public async Task<byte[]> GenerateUserActivityReportAsync(string format, string? startDate = null, string? endDate = null)
        {
            DateTime? start = startDate != null ? DateTime.Parse(startDate) : null;
            DateTime? end = endDate != null ? DateTime.Parse(endDate) : null;

            var borrows = await _borrowRepository.GetBorrowsByDateRangeAsync(start, end);

            // TODO: Generate report based on format
            return Array.Empty<byte>();
        }

        public async Task<byte[]> GenerateCategoryStatisticsReportAsync(string format)
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var categoryStats = new List<CategoryStatistics>();

            foreach (var category in categories)
            {
                var books = await _categoryRepository.GetBooksByCategoryIdAsync(category.Id);
                var borrowCount = (await _borrowRepository.GetAllAsync())
                    .Count(b => b.Book.CategoryId == category.Id);

                categoryStats.Add(new CategoryStatistics
                {
                    Category = category,
                    BookCount = books.Count(),
                    BorrowCount = borrowCount
                });
            }

            return await GenerateReportAsync(categoryStats, "Báo cáo thống kê thể loại", format);
        }

        public async Task<byte[]> GeneratePopularBooksReportAsync(string format, int topCount)
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            var popularBooks = books
                .OrderByDescending(b => b.Borrows.Count)
                .Take(topCount);

            return await GenerateReportAsync(popularBooks, "Báo cáo sách phổ biến", format);
        }

        public async Task<byte[]> GenerateCategoryReportAsync(string format)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var report = new List<CategoryReport>();

            foreach (var category in categories)
            {
                var books = await _categoryRepository.GetBooksByCategoryIdAsync(category.Id);
                report.Add(new CategoryReport
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    BookCount = books.Count()
                });
            }

            // TODO: Generate report based on format
            return Array.Empty<byte>();
        }

        public async Task<byte[]> GenerateBookStatusReportAsync(string format)
        {
            var borrows = await _borrowRepository.GetAllAsync();
            var report = new List<BookStatusReport>();

            foreach (var borrow in borrows)
            {
                report.Add(new BookStatusReport
                {
                    BookId = borrow.BookId,
                    BookTitle = borrow.Book.Title,
                    Status = borrow.Status.ToString(),
                    BorrowDate = borrow.BorrowDate,
                    DueDate = borrow.DueDate,
                    ReturnDate = borrow.ReturnDate
                });
            }

            // TODO: Generate report based on format
            return Array.Empty<byte>();
        }

        private async Task<byte[]> GenerateReportAsync<T>(IEnumerable<T> data, string title, string format)
        {
            // TODO: Implement report generation based on format (PDF, Excel, etc.)
            // This is a placeholder implementation
            using (var memoryStream = new MemoryStream())
            {
                // Generate report content based on format
                switch (format.ToLower())
                {
                    case "pdf":
                        // Generate PDF report
                        break;
                    case "excel":
                        // Generate Excel report
                        break;
                    case "csv":
                        // Generate CSV report
                        break;
                    default:
                        throw new Exception("Định dạng báo cáo không được hỗ trợ");
                }

                return memoryStream.ToArray();
            }
        }
    }

    public class CategoryStatistics
    {
        public required Category Category { get; set; }
        public int BookCount { get; set; }
        public int BorrowCount { get; set; }
    }

    public class CategoryReport
    {
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public int BookCount { get; set; }
    }

    public class BookStatusReport
    {
        public int BookId { get; set; }
        public required string BookTitle { get; set; }
        public required string Status { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}