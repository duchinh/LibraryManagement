using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBorrowRequestDetailService
    {
        Task<List<BorrowingRequestDetailDTO>> GetDetailsByRequestIdAsync(Guid requestId);
    }
}