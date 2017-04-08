
using System;
using System.Collections.Generic;
using Services.Objects;

namespace Services.Interfaces.Repository
{
    /// <summary>
    /// Repository for Inspections.
    /// </summary>
    public interface IInspectionRepository
    {
        /// <summary>
        /// Get all <see cref="Inspection"/> by given <see cref="User"/>.
        /// </summary>
        /// <param name="user">User Author.</param>
        /// <returns>List of Inspections founded.</returns>
        IList<Inspection> GetAllByUser(User user);

        /// <summary>
        /// Get all <see cref="Inspection"/> between <param name="initial">Initial Date</param> and <param name="final">Final date</param> by the <param name="user">User</param>.
        /// </summary>
        /// <returns>List of Inspections founded.</returns>
        IList<Inspection> GetAllByDates(User user,DateTime initial, DateTime final);

        /// <summary>
        /// Find by id
        /// </summary>
        /// <param name="id">Id of inspection</param>
        /// <returns>return founded inspection otherwise null.</returns>
        Inspection Find(int id);

        /// <summary>
        /// Add one inspection.
        /// </summary>
        /// <param name="inspec">Inspection which will be included.</param>
        /// <returns>The same inspection plus his <see cref="Inspection.Id"/>.</returns>
        Inspection Add(Inspection inspec);

        /// <summary>
        /// Update the inspection
        /// </summary>
        /// <param name="inspec">Inspection which will be updated.</param>
        /// <returns>Echo the inspection.</returns>
        Inspection Update(Inspection inspec);

        /// <summary>
        /// Remove the inspection by his <see cref="Inspection.Id"/>.
        /// </summary>
        /// <param name="id">Inspection Id.</param>
        /// <returns>true if succeed otherwise false.</returns>
        bool Remove(int id);
    }
}