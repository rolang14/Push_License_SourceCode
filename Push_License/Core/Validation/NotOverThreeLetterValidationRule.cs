using System.Globalization;
using System.Windows.Controls;

namespace Push_License.Core.Validation
{
    class NotOverThreeLetterValidationRule : ValidationRule
    {
        public string PropertyName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null || (value.ToString().Length > 3) ?
                new ValidationResult(false, $"{PropertyName} cannot be longer than 3 characters.") : ValidationResult.ValidResult;
        }
    }
}