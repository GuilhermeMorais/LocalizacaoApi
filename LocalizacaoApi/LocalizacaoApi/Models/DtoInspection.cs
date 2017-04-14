using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Services.Enums;

namespace LocalizacaoApi.Models
{
    /// <summary>
    /// Dto of Inspection.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Id: {Id}, Local: {Local} ({TipoLancamento}), {Create}")]
    public class DtoInspection
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Latitude coordinates.
        /// </summary>
        [Required]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude coordinates.
        /// </summary>
        [Required]
        public double Longitude { get; set; }

        /// <summary>
        /// Precision of the mobile GPS.
        /// </summary>
        [Required]
        public double Precisao { get; set; }

        /// <summary>
        /// Date which was inspected.
        /// </summary>
        public DateTime Create { get; set; }

        /// <summary>
        /// description of the place.
        /// </summary>
        [Required]
        public string Local { get; set; }

        /// <summary>
        /// Any annotation about the place.
        /// </summary>
        [Required]
        public string Observacao { get; set; }

        /// <summary>
        /// Type of registry. (0 - None, 1 - Web, 2 - Mobile, 3 - Manual)
        /// <returns>Uses value 3 during testes.</returns>
        /// </summary>
        [Required]
        public EnumTipoLancamento TipoLancamento { get; set; }
    }
}