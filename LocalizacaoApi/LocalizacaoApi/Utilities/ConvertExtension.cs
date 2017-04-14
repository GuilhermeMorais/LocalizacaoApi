using System.Collections.Generic;
using LocalizacaoApi.Models;
using Services.Objects;
using WebGrease.Css.Extensions;

namespace LocalizacaoApi.Utilities
{
    /// <summary>
    /// Extensions to convert to DTO objects.
    /// </summary>
    public static class ConvertExtension
    {
        /// <summary>
        /// Convert list of <see cref="Inspection"/> to DTOs.
        /// </summary>
        /// <param name="list">List to convert.</param>
        /// <returns>List.</returns>
        public static IList<DtoInspection> ToDtoList(this IList<Inspection> list)
        {
            var newList = new List<DtoInspection>();
            list.ForEach(x => newList.Add(x.ToDto()));
            return newList;
        }

        /// <summary>
        /// Convert <see cref="Inspection"/> into <see cref="DtoInspection"/>.
        /// </summary>
        /// <param name="inspection">Inspection</param>
        /// <returns>DtoInspection.</returns>
        public static DtoInspection ToDto(this Inspection inspection)
        {
            if (inspection == null)
            {
                return null;
            }

            return new DtoInspection
            {
                Id             = inspection.Id,
                Create         = inspection.Create,
                Latitude       = inspection.Latitude,
                Local          = inspection.Local,
                Longitude      = inspection.Longitude,
                Observacao     = inspection.Observacao,
                Precisao       = inspection.Precisao,
                TipoLancamento = inspection.TipoLancamento
            };
        }


        /// <summary>
        /// Convert <see cref="DtoInspection"/> into <see cref="Inspection"/>.
        /// </summary>
        /// <param name="inspection">Inspection</param>
        /// <returns>Inspection.</returns>
        public static Inspection ToObj(this DtoInspection inspection)
        {
            if (inspection == null)
            {
                return null;
            }

            return new Inspection
            {
                Id             = inspection.Id,
                Create         = inspection.Create,
                Latitude       = inspection.Latitude,
                Local          = inspection.Local,
                Longitude      = inspection.Longitude,
                Observacao     = inspection.Observacao,
                Precisao       = inspection.Precisao,
                TipoLancamento = inspection.TipoLancamento
            };
        }
    }
}