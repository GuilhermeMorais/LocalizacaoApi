using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Services.Utilities
{
    /// <summary>
    /// Extension Class of <see cref="ValidationFailure"/>.
    /// </summary>
    public static class ValidationFailureExtensions
    {
        /// <summary>
        /// Return in just one simple string every error catch during validations.
        /// </summary>
        /// <param name="fails">List of problems</param>
        /// <returns>One string with all errors.</returns>
        public static string ToSummary(this IList<ValidationFailure> fails)
        {
            return string.Join(Environment.NewLine, fails.Select(x => $"{x.PropertyName}:{x.ErrorMessage}").ToList());
        }
    }
}