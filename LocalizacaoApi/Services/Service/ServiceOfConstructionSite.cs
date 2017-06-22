
using Services.Interfaces.Repository;
using Services.Interfaces.Service;
using Services.Objects;
using Services.Repository;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace Services.Service
{
    /// <summary>
    /// Service of Construction Sites
    /// </summary>
    public class ServiceOfConstructionSite : IServiceOfConstructionSite
    {
        private readonly IConstructionSiteRepository repository;

        /// <summary>
        /// Create Instance of the service.
        /// </summary>
        public ServiceOfConstructionSite()
        {
            repository = new ConstructionSiteRepository();
        }

        /// <summary>
        /// Create Instance of the service.
        /// </summary>
        /// <param name="repository"> The repository. </param>
        public ServiceOfConstructionSite(IConstructionSiteRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get All Constructions Sites close to Coordinate.
        /// </summary>
        /// <param name="latitude"> The Latitude </param>
        /// <param name="longitude"> The longitude </param>
        /// <param name="distance"> Distance from this point in meters </param>
        /// <returns> List of closest constructions. </returns>
        public List<ConstructionSite> GetAllCloseTo(double latitude, double longitude, double distance)
        {
            if (distance <= 1)
            {
                return new List<ConstructionSite>();
            }

            var place = new GeoCoordinate(latitude, longitude);
            
            var all = repository.GetAll();

            var closets = all.Where(x => x.Coordinates().GetDistanceTo(place) <= distance).ToList();
            return closets;
        }
    }
}