using System.Device.Location;
using System.Globalization;

namespace Services.Objects
{
    /// <summary>
    /// Construction Site location
    /// </summary>
    public class ConstructionSite
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public decimal Id { get; set; }

        /// <summary>
        /// Entity Name
        /// </summary>
        public string Entidade { get; set; }

        /// <summary>
        /// Latitude coordinates.
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude coordinates.
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// description of the place.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Local of the place.
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// Get the Geolocation from this construction site.
        /// </summary>
        /// <returns>GeoCoordenation from the construction site</returns>
        public GeoCoordinate Coordinates()
        {
            var latitude = double.Parse(Latitude, CultureInfo.InvariantCulture);
            var longitude = double.Parse(Longitude, CultureInfo.InvariantCulture);
            return new GeoCoordinate(latitude, longitude);
        }
    }
}