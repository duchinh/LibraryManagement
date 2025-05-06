using System;
using System.Threading.Tasks;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Repositories;

namespace LibraryManagement.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportGenerator _reportGenerator;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ReportService(
            IReportGenerator reportGenerator,
            IBookRepository bookRepository,
            IBorrowRepository borrowRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            _reportGenerator = reportGenerator;
            _bookRepository = bookRepository;
            _borrowRepository = borrowRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<byte[]> GenerateBookReportAsync(string format)
        {
            return await _reportGenerator.GenerateBookReportAsync(format);
        }

        public async Task<byte[]> GenerateBorrowReportAsync(string format, DateTime startDate, DateTime endDate)
        {
            return await _reportGenerator.GenerateBorrowReportAsync(format, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
        }

        public async Task<byte[]> GenerateOverdueReportAsync(string format)
        {
            return await _reportGenerator.GenerateOverdueReportAsync(format);
        }

        public async Task<byte[]> GenerateUserActivityReportAsync(string format, DateTime startDate, DateTime endDate)
        {
            return await _reportGenerator.GenerateUserActivityReportAsync(format, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
        }

        public async Task<byte[]> GenerateCategoryStatisticsReportAsync(string format)
        {
            return await _reportGenerator.GenerateCategoryStatisticsReportAsync(format);
        }

        public async Task<byte[]> GeneratePopularBooksReportAsync(string format, int topCount)
        {
            return await _reportGenerator.GeneratePopularBooksReportAsync(format, topCount);
        }
    }
} 