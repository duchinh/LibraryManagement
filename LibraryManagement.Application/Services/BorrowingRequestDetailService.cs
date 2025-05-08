using AutoMapper;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Services
{
    public class BorrowingRequestDetailService : IBorrowRequestDetailService
    {
        private readonly IBookBorrowingRequestDetailRepository _repository;
        private readonly IMapper _mapper;

        public BorrowingRequestDetailService(IBookBorrowingRequestDetailRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BorrowingRequestDetailDTO>> GetDetailsByRequestIdAsync(Guid requestId)
        {
            var details = await _repository.GetByRequestIdAsync(requestId);

            return _mapper.Map<List<BorrowingRequestDetailDTO>>(details);
        }
    }
}