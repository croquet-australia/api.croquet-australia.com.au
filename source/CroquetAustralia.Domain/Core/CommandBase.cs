using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace CroquetAustralia.Domain.Core
{
    public abstract class CommandBase<TValidator> : CommandBase where TValidator : IValidator
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return FluentValidator.Validate<TValidator>(this);
        }
    }

    public abstract class CommandBase : ICommand
    {
        [Required]
        public Guid EntityId { get; set; }

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}