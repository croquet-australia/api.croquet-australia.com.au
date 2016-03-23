using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CroquetAustralia.Library;
using FluentValidation;

namespace CroquetAustralia.Domain.Core
{
    public static class FluentValidator
    {
        static FluentValidator()
        {
            var defaultDisplayNameResolver = ValidatorOptions.DisplayNameResolver;

            ValidatorOptions.DisplayNameResolver = (type, memberInfo, lambdaExpression) => DisplayNameResolver(defaultDisplayNameResolver, type, memberInfo, lambdaExpression);
            ValidatorOptions.ResourceProviderType = typeof(FluentValidatorResources);
        }

        private static string DisplayNameResolver(Func<Type, MemberInfo, LambdaExpression, string> defaultNameResolver, Type type, MemberInfo memberInfo, LambdaExpression lambdaExpression)
        {
            var name = defaultNameResolver(type, memberInfo, lambdaExpression);

            // FluentValidation.Internal.PropertyRule.GetDisplayName() is used
            // when building error messages. If display name is null then the
            // default value is propertyName.SplitPascalCase(). By supplying a
            // display name for Id properties we get what we want.
            if (name == null && memberInfo.Name.EndsWith("Id"))
            {
                return memberInfo.Name;
            }

            return name;
        }

        public static IEnumerable<ValidationResult> Validate<TValidator>(object obj) where TValidator : IValidator
        {
            var validator = ServiceFactory.Get<TValidator>();
            var result = validator.Validate(obj);
            var errors = result.Errors.Select(e => new ValidationResult(e.ErrorMessage, new[] {e.PropertyName}));

            return errors;
        }
    }
}