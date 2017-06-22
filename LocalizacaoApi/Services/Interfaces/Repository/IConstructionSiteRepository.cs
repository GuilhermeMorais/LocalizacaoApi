using Services.Objects;
using System.Collections.Generic;

namespace Services.Interfaces.Repository
{
    /// <summary>
    /// Repository of Constructions (only read mode)
    /// </summary>
    public interface IConstructionSiteRepository
    {
        /// <summary>
        /// Get all <see cref="ConstructionSite"/>.
        /// </summary>
        /// <returns>List of Construction Sites.</returns>
        IList<ConstructionSite> GetAll();
    }
}