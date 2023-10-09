using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Push_License.Core.Validation
{
    class BeNumberValidationRule : ValidationRule
    {
        public string PropertyName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex("^[0-9]+$");
            return value == null || !regex.IsMatch(value.ToString()) ?
                new ValidationResult(false, $"{PropertyName} should be Number.") : ValidationResult.ValidResult;
        }
    }
}
