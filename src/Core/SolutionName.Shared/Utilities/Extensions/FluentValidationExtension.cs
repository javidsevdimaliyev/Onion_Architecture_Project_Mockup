using Application.Shared.Resources;
using FluentValidation;

namespace SolutionName.Application.Shared.Utilities.Extensions
{
    public static class FluentValidationExtension
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustNotBeEmpty<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder)
        {
            return ruleBuilder.Must(list => list is not null && list!.Any()).WithMessage(SharedResources.NotEmptyValidator);
        }

        public static IRuleBuilderOptions<T, string> MustBeAlphabetic<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value =>
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                // The name must consist of only letters and spaces.
                if (!value.All(c => char.IsLetter(c) || c == ' '))
                {
                    return false;
                }

                // The name must contain at least one letter.
                if (!value.Any(char.IsLetter))
                {
                    return false;
                }

                // The name can contain multiple words separated by spaces.
                // However, a word cannot consist of only one or more spaces.
                string[] words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0 || !words.All(w => w.Length > 1 && w.All(char.IsLetter)))
                {
                    return false;
                }

                return true;
            })
                .WithMessage(SharedResources.LettersOnly);
        }

        public static IRuleBuilderOptions<T, string> IsNullOrEmpty<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => !string.IsNullOrEmpty(value)).WithMessage(SharedResources.RequiredAttribute);
        }

    }
}
