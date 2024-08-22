using Application.DTOs;
using FluentValidation;
public class Validation
{
    public void validateRegister(RegisterUserDTO registerUserDTO )
    {
        var validator = new RegisterUserDTOValidator();
        var results = validator.Validate(registerUserDTO);

        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine($"Error: {failure.ErrorMessage}");
            }
            throw new ValidationException("Validation failed for the register object.");
        }

        // Further processing
    }

    public void validateLogin(LoginDTO loginDTO )
    {
        var validator = new loginDTOValidator();
        var results = validator.Validate(loginDTO);

        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine($"Error: {failure.ErrorMessage}");
            }
            throw new ValidationException("Validation failed for the login object.");
        }
        // Further processing
    }
}