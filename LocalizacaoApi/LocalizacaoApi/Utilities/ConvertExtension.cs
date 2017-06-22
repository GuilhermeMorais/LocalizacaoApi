using LocalizacaoApi.Models;
using Services.Objects;
using System.Collections.Generic;
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
        /// <returns>List of Inspections.</returns>
        public static IList<DtoInspection> ToDtoList(this IList<Inspection> list)
        {
            var newList = new List<DtoInspection>();
            list.ForEach(x => newList.Add(x.ToDto()));
            return newList;
        }

        /// <summary>
        /// Convert <see cref="Inspection"/> into <see cref="DtoInspection"/>.
        /// </summary>
        /// <param name="inspection">Inspection Object</param>
        /// <returns>Dto of Inspection.</returns>
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
        /// <param name="inspection">Inspection Object</param>
        /// <returns>Dto of Inspection.</returns>
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

        /// <summary>
        /// Convert list of <see cref="ConstructionSite"/> to DTOs.
        /// </summary>
        /// <param name="list">List to convert.</param>
        /// <returns>List of Construction sites.</returns>
        public static IList<DtoConstructionSite> ToDtoList(this IList<ConstructionSite> list)
        {
            var newList = new List<DtoConstructionSite>();
            list.ForEach(x => newList.Add(x.ToDto()));
            return newList;
        }

        /// <summary>
        /// Convert <see cref="DtoConstructionSite"/> into <see cref="ConstructionSite"/>.
        /// </summary>
        /// <param name="construction">Construction site</param>
        /// <returns>Construction Site.</returns>
        public static ConstructionSite ToObj(this DtoConstructionSite construction)
        {
            if (construction == null)
            {
                return null;
            }

            return new ConstructionSite
            {
                Id             = construction.Id,
                Latitude       = construction.Latitude,
                Local          = construction.Local,
                Longitude      = construction.Longitude,
                Descricao      = construction.Descricao,
                Entidade       = construction.Entidade
            };
        }

        /// <summary>
        /// Convert <see cref="ConstructionSite"/> into <see cref="DtoConstructionSite"/>.
        /// </summary>
        /// <param name="construction">Construction Site</param>
        /// <returns>Construction Site.</returns>
        public static DtoConstructionSite ToDto(this ConstructionSite construction)
        {
            if (construction == null)
            {
                return null;
            }

            return new DtoConstructionSite
            {
                Id        = construction.Id,
                Latitude  = construction.Latitude,
                Local     = construction.Local,
                Longitude = construction.Longitude,
                Descricao = construction.Descricao,
                Entidade  = construction.Entidade
            };
        }
    }
}