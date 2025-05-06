using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;

namespace LibraryManagement.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Borrow>> GetAllBorrowsAsync()
        {
            return await _unitOfWork.Borrows.GetAllAsync();
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            return await _unitOfWork.Borrows.GetByIdAsync(id);
        }

        public async Task<Borrow> CreateBorrowAsync(Borrow borrow)
        {
            if (!await _unitOfWork.Books.IsBookAvailableAsync(borrow.BookId))
            {
                throw new InvalidOperationException("Sách không có sẵn để mượn");
            }

            if (await _unitOfWork.Borrows.HasUserReachedBorrowLimitAsync(borrow.UserId))
            {
                throw new InvalidOperationException("Người dùng đã đạt giới hạn mượn sách");
            }

            if (await _unitOfWork.Borrows.HasUserOverdueBooksAsync(borrow.UserId))
            {
                throw new InvalidOperationException("Người dùng có sách quá hạn chưa trả");
            }

            await _unitOfWork.Borrows.AddAsync(borrow);
            await _unitOfWork.SaveChangesAsync();
            return borrow;
        }

        public async Task UpdateBorrowAsync(Borrow borrow)
        {
            await _unitOfWork.Borrows.UpdateAsync(borrow);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBorrowAsync(int id)
        {
            var borrow = await _unitOfWork.Borrows.GetByIdAsync(id);
            if (borrow != null)
            {
                await _unitOfWork.Borrows.DeleteAsync(borrow);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Borrow>> GetBorrowsByUserIdAsync(int userId)
        {
            return await _unitOfWork.Borrows.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Borrow>> GetBorrowsByBookIdAsync(int bookId)
        {
            return await _unitOfWork.Borrows.GetByBookIdAsync(bookId);
        }

        public async Task<IEnumerable<Borrow>> GetOverdueBorrowsAsync()
        {
            return await _unitOfWork.Borrows.GetOverdueBorrowsAsync();
        }

        public async Task ReturnBookAsync(int borrowId)
        {
            var borrow = await _unitOfWork.Borrows.GetByIdAsync(borrowId);
            if (borrow == null)
                throw new InvalidOperationException("Không tìm thấy bản ghi mượn sách");

            borrow.ReturnDate = DateTime.Now;
            borrow.Status = BorrowStatus.Returned;
            await _unitOfWork.Borrows.UpdateAsync(borrow);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            return await _unitOfWork.Books.IsBookAvailableAsync(bookId);
        }

        public async Task<bool> HasUserReachedBorrowLimitAsync(int userId)
        {
            return await _unitOfWork.Borrows.HasUserReachedBorrowLimitAsync(userId);
        }

        public async Task<bool> HasUserOverdueBooksAsync(int userId)
        {
            return await _unitOfWork.Borrows.HasUserOverdueBooksAsync(userId);
        }

        public async Task<bool> IsBorrowOverdueAsync(int borrowId)
        {
            return await _unitOfWork.Borrows.IsBorrowOverdueAsync(borrowId);
        }
    }
}