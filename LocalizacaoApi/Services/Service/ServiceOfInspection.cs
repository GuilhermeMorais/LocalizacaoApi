using System;
using System.Collections.Generic;
using System.Linq;
using Services.Interfaces.Repository;
using Services.Interfaces.Service;
using Services.Objects;
using Services.Repository;
using Services.Utilities;
using Services.Validations;

namespace Services.Service
{
    /// <summary>
    /// Service of Inspections.
    /// </summary>
    public class ServiceOfInspection : IServiceOfInspection
    {
        private const int SizeOfPage = 15;
        private const string SorryInternalServerError = "Sorry, internal server error : ";
        private readonly InspectionValidation validator;
        private readonly IInspectionRepository inspecRepo;

        /// <summary>Initializes a new instance of the <see cref="ServiceOfInspection" /> class.</summary>
        public ServiceOfInspection()
        {
            validator = new InspectionValidation();
            inspecRepo = new InspectionRepository();
        }

        /// <summary>Initializes a new instance of the <see cref="ServiceOfInspection" /> class.</summary>
        /// <param name="inspecRepository">Inspection Repository</param>
        public ServiceOfInspection(IInspectionRepository inspecRepository)
        {
            validator = new InspectionValidation(inspecRepository);
            inspecRepo = inspecRepository;
        }

        /// <summary>
        ///  Get the list of <see cref="Inspection"/> by his <param name="idUser">User</param>, you can also ask by the <param name="page"> page number</param>.
        /// </summary>
        /// <returns>List of Inspections.</returns>
        public IEnumerable<Inspection> GetLast(int idUser, int page = 1)
        {
            var all = inspecRepo.GetAllByUser(new User {Id = idUser});
            return all.Skip((page - 1) * SizeOfPage).Take(SizeOfPage).ToList();
        }

        /// <summary>
        /// Add or Update the <param name="inspection">Inspection</param> received.
        /// </summary>
        public int Save(Inspection inspection)
        {
            var update = false;
            if (inspection.Id > 0)
            {
                validator.UpdateChecks();
                update = true;
            }
            else
            {
                validator.AddChecks();
            }

            var validate = validator.Validate(inspection);

            if (!validate.IsValid)
            {
                throw new InvalidOperationException(validate.Errors.ToSummary());
            }

            try
            {
                if (update)
                {
                    inspecRepo.Update(inspection);
                }
                else
                {
                    inspecRepo.Add(inspection);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(SorryInternalServerError + e.GetBaseException().Message);
            }

            return inspection.Id;
        }

        /// <summary>
        /// Remove the <param name="inspection">Inspection</param> received.
        /// </summary>
        public void Remove(Inspection inspection)
        {
            validator.RemoveChecks();

            var validate = validator.Validate(inspection);

            if (!validate.IsValid)
            {
                throw new InvalidOperationException(validate.Errors.ToSummary());
            }
            try
            {
                var result = inspecRepo.Remove(inspection.Id);

                if (!result)
                {
                    throw new InvalidOperationException("Error during the removal.");
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(SorryInternalServerError + e.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Get a list of <see cref="Inspection"/> by his <param name="idUser">User</param>, after the <param name="inicial">Date</param> indicated.
        /// </summary>
        /// <returns>List of Inspections.</returns>
        public IEnumerable<Inspection> GetAfterDate(int idUser, DateTime inicial)
        {
            return inspecRepo.GetAllByDates(new User { Id = idUser }, inicial, DateTime.Today);
        }

        /// <summary>
        /// Get all the <see cref="Inspection"/>s made the <param name="idUser">user</param>.
        /// </summary>
        /// <returns>List of Inspections.</returns>
        public IEnumerable<Inspection> GetAll(int idUser)
        {
            return inspecRepo.GetAllByUser(new User {Id = idUser});
        }
    }
}