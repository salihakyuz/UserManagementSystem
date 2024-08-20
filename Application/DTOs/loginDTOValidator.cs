using Application.DTOs;
using FluentValidation;

public class loginDTOValidator: AbstractValidator<LoginDTO>{
    public loginDTOValidator()
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}