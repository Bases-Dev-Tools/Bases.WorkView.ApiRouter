
//public class SomeObjectValidator : AbstractValidator<SomeObject>
//{
//    private const int MaxSubjectLength = 250;

//    private const int MaxBodyLength = 3000;

//    public SomeObjectValidator()
//    {
//        RuleFor(_ => _.Subject)
//            .NotEmpty()
//            .MaximumLength(MaxSubjectLength)
//            .WithMessage($"The subject must be no longer than {MaxSubjectLength} characters");

//        RuleFor(_ => _.Body)
//            .NotEmpty()
//            .MaximumLength(MaxBodyLength)
//            .WithMessage($"Body cannot exceed {MaxBodyLength} characters");
//    }
//}