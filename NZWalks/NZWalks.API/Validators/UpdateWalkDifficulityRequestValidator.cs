using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDifficulityRequestValidator : AbstractValidator<UpdateWalkDifficulityRequest>
    {
        public UpdateWalkDifficulityRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
