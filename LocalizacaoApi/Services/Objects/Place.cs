namespace Services.Objects
{
    /// <summary>
    /// Constructions Sites.
    /// </summary>
    public class Place
    {
        /// <summary>
        /// Id of the place.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Construction site Name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Latitude coordinates.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude coordinates.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Description of the local.
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// Text with description of the construction.
        /// </summary>
        public string Description { get; set; }
    }
}