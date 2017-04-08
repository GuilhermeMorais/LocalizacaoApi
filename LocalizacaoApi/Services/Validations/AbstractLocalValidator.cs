using FluentValidation;

namespace Services.Validations
{
    /// <summary>
    /// Local Abstract Validator
    /// </summary>
    /// <typeparam name="T">Types</typeparam>
    public class AbstractLocalValidator<T> : AbstractValidator<T>
    {
        /// <summary>
        /// Add rules for the Update action.
        /// </summary>
        public virtual void UpdateChecks()
        {
        }

        /// <summary>
        /// Add rules for the Remove action.
        /// </summary>
        public virtual void RemoveChecks()
        {
        }

        /// <summary>
        /// Add rules for the Add action.
        /// </summary>
        public virtual void AddChecks()
        {
        }
    }
}