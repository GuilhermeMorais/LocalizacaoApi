using LocalizacaoApi.Filters;
using LocalizacaoApi.Models;
using LocalizacaoApi.Utilities;
using Services.Interfaces.Service;
using Services.Service;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace LocalizacaoApi.Controllers
{
    /// <summary>
    /// Controller of every construction site
    /// </summary>
    [BasicAuthorization]
    [RoutePrefix("api/Constructions")]
    public class ConstructionSitesController : BaseApiController
    {
        private readonly IServiceOfConstructionSite service;

        /// <summary>
        /// Create a normal instance.
        /// </summary>
        public ConstructionSitesController()
        {
            service = new ServiceOfConstructionSite();
        }

        /// <summary>
        /// Create a especial instance, for fake tests.
        /// </summary>
        /// <param name="serviceFake">To help with unit tests.</param>
        public ConstructionSitesController(IServiceOfConstructionSite serviceFake)
        {
            service = serviceFake;
        }

        /// <summary>
        /// Get all constructions sites close to you.
        /// </summary>
        /// <param name="latitude">Your latitude.</param>
        /// <param name="longitude">Your longitude.</param>
        /// <param name="distance">The maximum distance from you</param>
        /// <returns>List of all constructions sites near you</returns>
        /// <response code="500">Unexpected Error. (ops...)</response>
        [HttpGet]
        [Route("around/{latitude}/{longitude}/{distance}")]
        [SwaggerResponse(HttpStatusCode.OK, "Return the list of Construction sites founded.", typeof(List<DtoConstructionSite>))]
        public IHttpActionResult GetClosest(double latitude, double longitude, double distance)
        {
            try
            {
                var inspections = service.GetAllCloseTo(latitude, longitude, distance);
                return Ok(inspections.ToDtoList());
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }
    }
}
