using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using task11.Models;
using task11.Services;

namespace task11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisits()
        {
            var visits = await _visitService.GetVisitsAsync();
            var visitDtos = visits.Select(v => new VisitDto
            {
                Id = v.Id,
                Animal = v.Animal.Name,
                Employee = v.Employee.Name,
                Date = v.Date.ToString("yyyy-MM-ddTHH:mm:ss")
            });
            return Ok(visitDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<VisitDto>> GetVisit(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Visit>> CreateVisit([FromBody] CreateVisitDto createVisitDto)
        {
            var visit = new Visit
            {
                AnimalId = createVisitDto.AnimalId,
                EmployeeId = createVisitDto.EmployeeId,
                Date = DateTime.Parse(createVisitDto.Date)
            };

            await _visitService.CreateVisitAsync(visit);
            return CreatedAtAction(nameof(GetVisit), new { id = visit.Id }, visit);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVisit(int id, [FromBody] UpdateVisitDto updateVisitDto)
        {
            if (id != updateVisitDto.Id)
            {
                return BadRequest("ID in the URL does not match ID in the body.");
            }

            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            visit.AnimalId = updateVisitDto.AnimalId;
            visit.EmployeeId = updateVisitDto.EmployeeId;
            visit.Date = DateTime.Parse(updateVisitDto.Date);

            await _visitService.UpdateVisitAsync(visit);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            await _visitService.DeleteVisitAsync(id);
            return NoContent();
        }
    }

    public class VisitDto
    {
        public int Id { get; set; }
        public string Animal { get; set; }
        public string Employee { get; set; }
        public string Date { get; set; }
    }

    public class CreateVisitDto
    {
        [Required]
        public int AnimalId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string Date { get; set; }
    }

    public class UpdateVisitDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string Date { get; set; }
    }
}
