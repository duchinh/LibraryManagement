using FluentValidation;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Validators
{
    public class CreateBookDTOValidator : AbstractValidator<CreateBookDTO>
    {
        public CreateBookDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tên sách không được để trống")
                .MaximumLength(200).WithMessage("Tên sách không được vượt quá 200 ký tự");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Tác giả không được để trống")
                .MaximumLength(100).WithMessage("Tên tác giả không được vượt quá 100 ký tự");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN không được để trống")
                .Matches(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$")
                .WithMessage("ISBN không hợp lệ");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Danh mục không được để trống");

            RuleFor(x => x.Publisher)
                .NotEmpty().WithMessage("Nhà xuất bản không được để trống")
                .MaximumLength(100).WithMessage("Tên nhà xuất bản không được vượt quá 100 ký tự");

            RuleFor(x => x.PublicationYear)
                .NotEmpty().WithMessage("Năm xuất bản không được để trống")
                .GreaterThanOrEqualTo(1800).WithMessage("Năm xuất bản phải từ 1800 trở lên")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Năm xuất bản không được vượt quá năm hiện tại");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Số lượng không được để trống")
                .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Mô tả không được vượt quá 2000 ký tự");
        }
    }

    public class UpdateBookDTOValidator : AbstractValidator<UpdateBookDTO>
    {
        public UpdateBookDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tên sách không được để trống")
                .MaximumLength(200).WithMessage("Tên sách không được vượt quá 200 ký tự");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Tác giả không được để trống")
                .MaximumLength(100).WithMessage("Tên tác giả không được vượt quá 100 ký tự");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN không được để trống")
                .Matches(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$")
                .WithMessage("ISBN không hợp lệ");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Danh mục không được để trống");

            RuleFor(x => x.Publisher)
                .NotEmpty().WithMessage("Nhà xuất bản không được để trống")
                .MaximumLength(100).WithMessage("Tên nhà xuất bản không được vượt quá 100 ký tự");

            RuleFor(x => x.PublicationYear)
                .NotEmpty().WithMessage("Năm xuất bản không được để trống")
                .GreaterThanOrEqualTo(1800).WithMessage("Năm xuất bản phải từ 1800 trở lên")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Năm xuất bản không được vượt quá năm hiện tại");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Số lượng không được để trống")
                .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Mô tả không được vượt quá 2000 ký tự");
        }
    }
}