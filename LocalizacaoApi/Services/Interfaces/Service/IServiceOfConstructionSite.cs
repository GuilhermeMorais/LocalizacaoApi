using Services.Objects;
using System.Collections.Generic;

namespace Services.Interfaces.Service
{
    /// <summary>
    /// Service of Construction Site.
    /// </summary>
    public interface IServiceOfConstructionSite
    {
        /// <summary>
        /// Get All Constructions Sites close to Coordinate.
        /// </summary>
        /// <param name="latitude"> The Latitude </param>
        /// <param name="longitude"> The longitude </param>
        /// <param name="distance"> Distance from this point in meters </param>
        /// <returns> List of closest constructions. </returns>
        List<ConstructionSite> GetAllCloseTo(double latitude, double longitude, double distance);
    }
}