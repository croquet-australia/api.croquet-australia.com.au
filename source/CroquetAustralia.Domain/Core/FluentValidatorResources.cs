// ReSharper disable InconsistentNaming because the property names are defined by FluentValidation library.

namespace CroquetAustralia.Domain.Core
{
    public class FluentValidatorResources
    {
        public static string notempty_error => "{PropertyName} is required.";
        public static string notnull_error => notempty_error;
    }
}