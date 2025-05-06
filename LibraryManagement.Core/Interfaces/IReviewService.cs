using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByBookIdAsync(int bookId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review> CreateReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Task<bool> IsReviewExistsAsync(int id);
        Task<double> GetAverageRatingByBookIdAsync(int bookId);
        Task<int> GetReviewCountByBookIdAsync(int bookId);
        Task<bool> HasUserReviewedBookAsync(int userId, int bookId);
    }
} 