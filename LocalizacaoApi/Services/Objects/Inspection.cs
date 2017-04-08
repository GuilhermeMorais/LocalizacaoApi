using System;
using Services.Enums;

namespace Services.Objects
{
    /// <summary>
    /// Inspection Object.
    /// </summary>
    public class Inspection
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Latitude coordinates.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude coordinates.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Precision of the mobile GPS.
        /// </summary>
        public double Precisao { get; set; }

        /// <summary>
        /// Date which was inspected.
        /// </summary>
        public DateTime Create { get; set; }

        /// <summary>
        /// description of the place.
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// Any annotation about the place.
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Type of registry.
        /// </summary>
        public EnumTipoLancamento TipoLancamento { get; set; }

        /// <summary>
        /// User id.
        /// </summary>
        public int UsuarioId { get; set; }
    }
}