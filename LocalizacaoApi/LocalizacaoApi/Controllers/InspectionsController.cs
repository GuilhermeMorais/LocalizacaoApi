using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using LocalizacaoApi.Filters;
using LocalizacaoApi.Models;
using LocalizacaoApi.Utilities;
using Newtonsoft.Json;
using Services.Interfaces.Service;
using Services.Objects;
using Services.Service;

namespace LocalizacaoApi.Controllers
{
    /// <summary>
    /// Controller of every inspection.
    /// This Api
    /// </summary>
    [BasicAuthorization]
    [RoutePrefix("api/Inspections")]
    public class InspectionsController : BaseApiController
    {
        private readonly IServiceOfInspection service;

        /// <summary>
        /// Create a normal instance.
        /// </summary>
        public InspectionsController()
        {
            service = new ServiceOfInspection();
        }

        /// <summary>
        /// Create a especial instance, for fake tests.
        /// </summary>
        /// <param name="serviceFake">To help with unit tests.</param>
        public InspectionsController(IServiceOfInspection serviceFake)
        {
            service = serviceFake;
        }

        /// <summary>
        /// Get last 15 Inspections.
        /// </summary>
        /// <returns>List of inspections founded.</returns>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [Route("get/last")]
        [HttpGet]
        [ResponseType(typeof(DtoInspection))]
        public IHttpActionResult GetLast()
        {
            try
            {
                var inspections = service.GetLast(UserId);
                return Ok(inspections.ToDtoList());
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        /// <summary>
        /// Get 15 inspections by the page informed
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>List of inspections founded.</returns>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [Route("get/page/{page:int}")]
        [HttpGet]
        [ResponseType(typeof(DtoInspection))]
        public IHttpActionResult GetByPage(int page = 1)
        {
            try
            {
                var inspections = service.GetLast(UserId, page);
                return Ok(inspections.ToDtoList());
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        /// <summary>
        /// Get by Id.
        /// </summary>
        /// <param name="id">Id of inspection.</param>
        /// <returns>Return Inspection if founded.</returns>
        /// <response code="404">Not found</response>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [HttpGet]
        [Route("get/{id:long}")]
        [ResponseType(typeof(DtoInspection))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var inspection = service.Find(UserId, id);
                if (inspection == null)
                {
                    return NotFound();
                }

                return Ok(inspection.ToDto());
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        /// <summary>
        /// Get all Inspections after informed date.
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <param name="day">Day</param>
        /// <returns>List of inspections founded.</returns>
        /// <response code="404">Nothing was found.</response>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [HttpGet]
        [Route("get/beginning/{year}/{month}/{day}")]
        [ResponseType(typeof(DtoInspection))]
        public IHttpActionResult GetFrom(int year, int month, int day)
        {
            try
            {
                var inicio = new DateTime(year, month, day);
                var inspections = service.GetAfterDate(UserId, inicio);
                if (inspections.Count > 0)
                {
                    return Ok(inspections.ToDtoList());
                }

                return NotFound();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest($"{ex.Message} ({year}/{month}/{day})");
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        /// <summary>
        /// Create new Inspection.
        /// </summary>
        /// <param name="model">New inspection details.
        /// <remarks>When creating using swagger uses: tipoLancamento with number 3.</remarks>
        /// </param>
        /// <response code="400">Validation errors, should see the message body.</response>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [HttpPost]
        [Route("new")]
        [ResponseType(typeof(DtoInspection))]
        public IHttpActionResult Post([FromBody] DtoInspection model)
        {
            if (model == null)
            {
                return BadRequest("I can't ready his model, try again.");
            }

            model.Id = 0;
            return SaveInternal(model);
        }

        /// <summary>
        /// Update one existing Inspection.
        /// </summary>
        /// <param name="model">Inspection to update</param>
        /// <response code="400">Validation errors, should see the message body.</response>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [HttpPut]
        [Route("update")]
        [ResponseType(typeof(DtoInspection))]
        public IHttpActionResult Put([FromBody] DtoInspection model)
        {
            if (model == null)
            {
                return BadRequest("I can't ready this model, try again.");
            }

            return SaveInternal(model, update: true);
        }

        private IHttpActionResult SaveInternal(DtoInspection dto, bool update = false)
        {
            try
            {
                var model = dto.ToObj();
                model.UsuarioId = UserId;
                model.Create = DateTime.Now;
                service.Save(model);
                dto.Id = model.Id;
                dto.Create = model.Create;
                var baseUrl = Request.RequestUri.AbsolutePath;
                if (update)
                {
                    return Ok(dto);
                }

                return Created(baseUrl + "get/" + model.Id, dto);
            }
            catch (InvalidOperationException erro)
            {
                return BadRequest(erro.Message);
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        /// <summary>
        /// Remove the inspection 
        /// <remarks>Only works if within 5 days after created.</remarks>
        /// </summary>
        /// <param name="id">Id of Inspection.</param>
        /// <response code="400">Validation errors, should see the message body.</response>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [HttpDelete]
        [Route("remove/{id:long}")]
        public IHttpActionResult Remove(int id)
        {
            try
            {
                var model = new Inspection
                {
                    Id = id,
                    UsuarioId = UserId,
                    Create = DateTime.Now
                };
                service.Remove(model);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (InvalidOperationException erro)
            {
                return BadRequest(erro.Message);
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        /// <summary>
        /// Summary of inspection for the current user.
        /// </summary>
        /// <remarks>List of inspections by date.</remarks>
        /// <returns>List of Tuples with date and quantity of inspections.</returns>
        /// <response code="404">Nothing was found.</response>
        /// <response code="500">Unexpected Error. (ops...)</response>
        /// <response code="401">Missing Authentication token?</response>
        [HttpGet]
        [Route("get/summary")]
        [ResponseType(typeof(Tuple<DateTime, int>))]
        public IHttpActionResult Summay()
        {
            try
            {
                var inspections = service.GetAll(UserId);
                var groupBy = inspections.GroupBy(x => x.Create.Date);
                var result = groupBy.Select(group => new Tuple<DateTime, int>(group.Key, group.Count())).ToList();

                if (result.Count > 0)
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return DealWithThis(ex);
            }
        }

        private IHttpActionResult DealWithThis(Exception exOriginal)
        {
            var error = JsonConvert.SerializeObject(exOriginal);
            var nameFile = UserName + "_" + DateTime.Now.ToString("O");
            if (HttpContext.Current != null)
            {
                var path = HttpContext.Current.Server.MapPath("~/App_Data/errors/" + nameFile + ".xml");
                File.WriteAllText(path, error);
                var ex = new Exception("See file " + nameFile + " for help.");
                return InternalServerError(ex);
            }

            return InternalServerError(exOriginal);
        }
    }
}