using System.Globalization;
using System.Windows.Controls;

namespace Push_License.Core.Validation
{
    class NotEmptyValidationRule : ValidationRule
    {
        public string PropertyName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null || string.IsNullOrEmpty((value ?? "").ToString()) ?
                new ValidationResult(false, $"{PropertyName} is Required.") : ValidationResult.ValidResult;
        }
    }
}
