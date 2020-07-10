using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Park_API.Models;
using Park_API.Models.DTOs;
using Park_API.Repository.IRepository;

namespace Park_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ParkAPISpecParks")]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _repository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var domainObjects = _repository.GetNationalParks();
            var dtoItems = _mapper.Map<IEnumerable<NationalParkDto>>(domainObjects);
            return Ok(dtoItems);
        }



        /// <summary>
        /// Get individual National Park
        /// </summary>
        /// <param name="nationalParkId">Id of the National Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}" , Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var domainObject = _repository.GetNationalPark(nationalParkId);

            if(domainObject == null)
            {
                return NotFound();
            }

            var dtoItem = _mapper.Map<NationalParkDto>(domainObject);


            return Ok(dtoItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Park Already Exists!");

                return StatusCode(404, ModelState);
            }

            

            var domianObject = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_repository.CreateNationalPark(domianObject))
            {
                ModelState.AddModelError(string.Empty, $"Saving record failed {domianObject.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalparkId = domianObject.Id }, domianObject);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(int nationalparkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalparkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var domianObject = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_repository.UpdateNationalPark(domianObject))
            {
                ModelState.AddModelError(string.Empty, $"Updating record failed {domianObject.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public IActionResult DeleteNationalPark(int nationalparkId)
        {
            if (!_repository.NationalParkExists(nationalparkId))
            {
                return NotFound();
            }

            var domianObject = _repository.GetNationalPark(nationalparkId);

            if (!_repository.DeleteNationalPark(domianObject))
            {
                ModelState.AddModelError(string.Empty, $"Deleting record failed {domianObject.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}