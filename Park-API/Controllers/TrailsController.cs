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
    [ApiExplorerSettings(GroupName = "ParkAPISpecTrails")]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _repository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var domainObjects = _repository.GetTrails();
            var dtoItems = _mapper.Map<IEnumerable<TrailDto>>(domainObjects);
            return Ok(dtoItems);
        }



        /// <summary>
        /// Get individual Trail
        /// </summary>
        /// <param name="trailId">Id of the Trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}" , Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var domainObject = _repository.GetTrail(trailId);

            if(domainObject == null)
            {
                return NotFound();
            }

            var dtoItem = _mapper.Map<TrailDto>(domainObject);


            return Ok(dtoItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Trail Already Exists!");

                return StatusCode(404, ModelState);
            }

            

            var domianObject = _mapper.Map<Trail>(trailDto);

            if (!_repository.CreateTrail(domianObject))
            {
                ModelState.AddModelError(string.Empty, $"Saving record failed {domianObject.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = domianObject.Id }, domianObject);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var domianObject = _mapper.Map<Trail>(trailDto);

            if (!_repository.UpdateTrail(domianObject))
            {
                ModelState.AddModelError(string.Empty, $"Updating record failed {domianObject.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public IActionResult DeleteTrail(int trailId)
        {
            if (!_repository.TrailExists(trailId))
            {
                return NotFound();
            }

            var domianObject = _repository.GetTrail(trailId);

            if (!_repository.DeleteTrail(domianObject))
            {
                ModelState.AddModelError(string.Empty, $"Deleting record failed {domianObject.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}