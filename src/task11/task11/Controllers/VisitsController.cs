using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisits()
        {
            var visits = await _visitService.GetVisitsAsync();
            return Ok(visits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetVisit(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            return Ok(visit);
        }

        [HttpPost]
        public async Task<ActionResult<Visit>> CreateVisit(Visit visit)
        {
            await _visitService.CreateVisitAsync(visit);
            return CreatedAtAction(nameof(GetVisit), new { id = visit.Id }, visit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisit(int id, Visit visit)
        {
            if (id != visit.Id)
            {
                return BadRequest();
            }

            await _visitService.UpdateVisitAsync(visit);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            await _visitService.DeleteVisitAsync(id);
            return NoContent();
        }
    }
}
