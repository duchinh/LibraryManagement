using FluentValidation;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
                .Length(3, 50).WithMessage("Tên đăng nhập phải từ 3 đến 50 ký tự")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");
        }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
                .Length(3, 50).WithMessage("Tên đăng nhập phải từ 3 đến 50 ký tự")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
                .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ hoa")
                .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ thường")
                .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất một số")
                .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự đặc biệt");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ")
                .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống")
                .MaximumLength(100).WithMessage("Họ và tên không được vượt quá 100 ký tự");

            RuleFor(x => x.Phone)
                .Matches("^[0-9-+()]*$").WithMessage("Số điện thoại không hợp lệ")
                .MaximumLength(20).WithMessage("Số điện thoại không được vượt quá 20 ký tự");
        }
    }

    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Mật khẩu hiện tại không được để trống");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu mới không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự")
                .Matches("[A-Z]").WithMessage("Mật khẩu mới phải chứa ít nhất một chữ hoa")
                .Matches("[a-z]").WithMessage("Mật khẩu mới phải chứa ít nhất một chữ thường")
                .Matches("[0-9]").WithMessage("Mật khẩu mới phải chứa ít nhất một số")
                .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu mới phải chứa ít nhất một ký tự đặc biệt");
        }
    }

    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ")
                .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");
        }
    }
}