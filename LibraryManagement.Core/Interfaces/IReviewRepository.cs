using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByBookAsync(int bookId);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId);
        Task<double> GetAverageRatingForBookAsync(int bookId);
        Task<bool> HasUserReviewedBookAsync(int userId, int bookId);
    }
} 