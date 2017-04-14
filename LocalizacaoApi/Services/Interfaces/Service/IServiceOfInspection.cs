using System;
using System.Collections.Generic;
using Services.Objects;

namespace Services.Interfaces.Service
{
    /// <summary>
    /// Service responsible for register and validate Inspections.
    /// </summary>
    public interface IServiceOfInspection
    {
        /// <summary>
        /// Find by id
        /// </summary>
        /// <param name="idUser">Id of User</param>
        /// <param name="id">Id of inspection</param>
        /// <returns>return founded inspection otherwise null.</returns>
        Inspection Find(int idUser, int id);

        /// <summary>
        ///  Get the list of <see cref="Inspection"/> by his <param name="idUser">User</param>, you can also ask by the <param name="page"> page number</param>.
        /// </summary>
        /// <returns>List of Inspections.</returns>
        IList<Inspection> GetLast(int idUser, int page = 1);

        /// <summary>
        /// Add or Update the <param name="inspection">Inspection</param> received.
        /// </summary>
        /// <returns>Id of the <see cref="Inspection"/>.</returns>
        /// <exception cref="InvalidOperationException">If any error occur during saving process.</exception>
        int Save(Inspection inspection);

        /// <summary>
        /// Remove the <param name="inspection">Inspection</param> received.
        /// </summary>
        /// <exception cref="InvalidOperationException">If any error occur during removal.</exception>
        void Remove(Inspection inspection);

        /// <summary>
        /// Get a list of <see cref="Inspection"/> by his <param name="idUser">User</param>, after the <param name="inicial">Date</param> indicated.
        /// </summary>
        /// <returns>List of Inspections.</returns>
        IList<Inspection> GetAfterDate(int idUser, DateTime inicial);

        /// <summary>
        /// Get all the <see cref="Inspection"/>s made the <param name="idUser">user</param>.
        /// </summary>
        /// <returns>List of Inspections.</returns>
        IList<Inspection> GetAll(int idUser);
    }
}