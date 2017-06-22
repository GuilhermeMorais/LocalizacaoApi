
namespace LocalizacaoApi.Models
{
    /// <summary>
    /// Construction Site location
    /// </summary>
    public class DtoConstructionSite
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
    }
}