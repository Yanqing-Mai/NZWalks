using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficulityRequestValidator : AbstractValidator<AddWalkDifficultyRequest>
    {
        public AddWalkDifficulityRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
