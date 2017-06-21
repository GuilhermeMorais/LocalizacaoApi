using FluentValidation;
using Services.Enums;
using Services.Interfaces.Repository;
using Services.Objects;
using Services.Repository;
using System;

namespace Services.Validations
{
    /// <summary>
    /// Local Validator for <see cref="Inspection"/>.
    /// </summary>
    public class InspectionValidation : AbstractLocalValidator<Inspection>
    {
        private readonly IInspectionRepository repo;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public InspectionValidation()
        {
            repo = new InspectionRepository();
        }

        /// <summary>
        /// Especial constructor for mock tests.
        /// </summary>
        /// <param name="repository"></param>
        public InspectionValidation(IInspectionRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Add rules for the Update action.
        /// </summary>
        public override void UpdateChecks()
        {
            Basics();
            UserShouldBeTheSame();
            FiveDaysChanged();
            base.UpdateChecks();
        }

        /// <summary>
        /// Add rules for the Remove action.
        /// </summary>
        public override void RemoveChecks()
        {
            ValidDate();
            UserShouldBeTheSame();
            FiveDaysChanged();
            UserShouldBeInformed();
            base.RemoveChecks();
        }

        /// <summary>
        /// Add rules for the Add action.
        /// </summary>
        public override void AddChecks()
        {
            Basics();
            CreatedToday();
            base.AddChecks();
        }

        private void ValidDate()
        {
            RuleFor(x => x.Create.Date)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("A data da criação deve ser definida.");
        }

        private void CreatedToday()
        {
            RuleFor(x => x.Create.Date)
                .Equal(DateTime.Today)
                .When(x => x?.Create > DateTime.MinValue)
                .WithMessage("A criação deve ser a data de hoje.");
        }

        private void FiveDaysChanged()
        {
            RuleFor(x => x.Create.Date)
                .GreaterThan(DateTime.Today.AddDays(-5))
                .When(x => x?.Create > DateTime.MinValue)
                .WithMessage("A alteração não pode ocorrer depois de 5 dias de publicado.");
        }

        private void UserShouldBeTheSame()
        {
            RuleFor(x => x.UsuarioId)
                .Must(ShouldBeTheOwner)
                .When(x => x != null)
                .WithMessage("O usuário deve ser responsável pela fiscalização.");
        }

        private bool ShouldBeTheOwner(Inspection obj, int idUsuario)
        {
                var objBanco = repo.Find(obj.Id);

                if (objBanco == null)
                {
                    return true;
                }

                return objBanco.UsuarioId == idUsuario;
         }

        private void Basics()
        {
            UserShouldBeInformed();
            ValidateDate();

            RuleFor(x => x.Latitude)
                .NotEqual(default(long))
                .WithMessage("A latitude deve ser informada.");

            RuleFor(x => x.Longitude)
                .NotEqual(default(long))
                .WithMessage("A longitude deve ser informada.");

            RuleFor(x => x.Precisao)
                .InclusiveBetween(0.00001f, 24.99f)
                .WithMessage("A precisão deve ser inferior a 5 metros.");

            RuleFor(x => x.Observacao)
                .NotEmpty()
                .WithMessage("A descrição deve ser informada.");

            RuleFor(x => x.Local)
                .NotEmpty()
                .WithMessage("O local deve ser informado.");

            RuleFor(x => x.TipoLancamento)
                .NotEqual(EnumTipoLancamento.NoDefined)
                .WithMessage("O tipo do lançamento não foi identificado.");
        }

        private void ValidateDate()
        {
            RuleFor(x => x.Create.Date)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("A data da criação deve ser definida.");
        }

        private void UserShouldBeInformed()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThanOrEqualTo(1)
                .WithMessage("O usuário deve ser informado.");
        }
    }
}