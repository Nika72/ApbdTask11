using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using task11.Dtos;
using task11.Models;
using task11.Services;

namespace task11.Controllers
{
    [Route("api/visits")]
    [Authorize]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisits(CancellationToken cancellationToken = default)
        {
            try
            {
                var visits = await _visitService.GetVisitsAsync(cancellationToken);
                var visitDtos = visits.Select(v => new VisitDto
                {
                    Id = v.Id,
                    Animal = v.Animal.Name,
                    Employee = v.Employee.Name,
                    Date = v.Date.ToString("yyyy-MM-ddTHH:mm:ss")
                }).ToList();

                return Ok(visitDtos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VisitDto>> GetVisit(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var visit = await _visitService.GetVisitByIdAsync(id, cancellationToken);

                if (visit == null)
                {
                    return NotFound($"Visit with ID {id} not found!");
                }

                var visitDto = new VisitDto
                {
                    Id = visit.Id,
                    Animal = visit.Animal.Name,
                    Employee = visit.Employee.Name,
                    Date = visit.Date.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                return Ok(visitDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Visit>> CreateVisit([FromBody] CreateVisitDto createVisitDto, CancellationToken cancellationToken = default)
        {
            if (createVisitDto == null)
            {
                return BadRequest("Invalid visit data.");
            }

            var visit = new Visit
            {
                AnimalId = createVisitDto.AnimalId,
                EmployeeId = createVisitDto.EmployeeId,
                Date = DateTime.Parse(createVisitDto.Date)
            };

            await _visitService.CreateVisitAsync(visit, cancellationToken);

            return CreatedAtAction(nameof(GetVisit), new { id = visit.Id }, visit);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVisit(int id, [FromBody] UpdateVisitDto updateVisitDto, CancellationToken cancellationToken = default)
        {
            if (updateVisitDto == null || id != updateVisitDto.Id)
            {
                return BadRequest("Invalid visit data or mismatched ID.");
            }

            var visit = await _visitService.GetVisitByIdAsync(id, cancellationToken);

            if (visit == null)
            {
                return NotFound($"Visit with ID {id} not found!");
            }

            visit.AnimalId = updateVisitDto.AnimalId;
            visit.EmployeeId = updateVisitDto.EmployeeId;
            visit.Date = DateTime.Parse(updateVisitDto.Date);

            await _visitService.UpdateVisitAsync(visit, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVisit(int id, CancellationToken cancellationToken = default)
        {
            var visit = await _visitService.GetVisitByIdAsync(id, cancellationToken);
            if (visit == null)
            {
                return NotFound($"Visit with ID {id} not found!");
            }

            await _visitService.DeleteVisitAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
