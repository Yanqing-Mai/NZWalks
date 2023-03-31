using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Validators
{
    public class LoginRequestValidator :AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.PassWord).NotEmpty();
        }
    }
}
