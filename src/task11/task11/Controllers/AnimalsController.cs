using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using task11.Models;
using task11.Services;
using Microsoft.EntityFrameworkCore;
using task11.Data;

namespace task11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly AnimalClinicContext _context;

        public AnimalsController(IAnimalService animalService, AnimalClinicContext context)
        {
            _animalService = animalService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals([FromQuery] string queryBy = "Name")
        {
            var animals = await _animalService.GetAnimalsAsync(queryBy);
            return Ok(animals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal([FromBody] Animal animal)
        {
            if (string.IsNullOrWhiteSpace(animal.Name) || string.IsNullOrWhiteSpace(animal.Description))
            {
                return BadRequest("Name and Description are required.");
            }

            var animalTypeExists = await _context.AnimalTypes.AnyAsync(at => at.Id == animal.AnimalTypesId);
            if (!animalTypeExists)
            {
                return BadRequest("Invalid AnimalType ID.");
            }

            try
            {
                await _animalService.CreateAnimalAsync(animal);
                return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            try
            {
                await _animalService.UpdateAnimalAsync(animal);
            }
            catch (ConcurrencyException ex)
            {
                return StatusCode(409, ex.Message); // Conflic
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            await _animalService.DeleteAnimalAsync(id);
            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
