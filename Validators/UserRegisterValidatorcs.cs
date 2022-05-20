
using FluentValidation;
using Gamification.Models.DTO;

namespace Gamification.Validators
{
    public class UserRegisterValidatorcs : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidatorcs()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$")
                .MaximumLength(30);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(50);
        }   
    }
}
