using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        [Authorize]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimals([FromQuery] string queryBy = "Name")
        {
            var animals = await _animalService.GetAnimalsAsync(queryBy);
            var animalDtos = animals.Select(a => new AnimalDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                AnimalType = a.AnimalType.Name
            });
            return Ok(animalDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AnimalDto>> GetAnimal(int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            var animalDto = new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                AnimalType = animal.AnimalType.Name
            };
            return Ok(animalDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Animal>> CreateAnimal([FromBody] CreateAnimalDto createAnimalDto)
        {
            if (string.IsNullOrWhiteSpace(createAnimalDto.Name))
            {
                return BadRequest("Name is required.");
            }

            var animalTypeExists = await _context.AnimalTypes.AnyAsync(at => at.Id == createAnimalDto.AnimalTypeId);
            if (!animalTypeExists)
            {
                return BadRequest("Invalid AnimalType ID.");
            }

            var animal = new Animal
            {
                Name = createAnimalDto.Name,
                Description = createAnimalDto.Description,
                AnimalTypesId = createAnimalDto.AnimalTypeId
            };

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
        [Authorize]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] UpdateAnimalDto updateAnimalDto)
        {
            if (id != updateAnimalDto.Id)
            {
                return BadRequest("ID in the URL does not match ID in the body.");
            }

            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.Name = updateAnimalDto.Name;
            animal.Description = updateAnimalDto.Description;
            animal.AnimalTypesId = updateAnimalDto.AnimalTypeId;

            try
            {
                await _animalService.UpdateAnimalAsync(animal);
            }
            catch (ConcurrencyException ex)
            {
                return StatusCode(409, ex.Message); // Conflict
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
    }

    public class AnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AnimalType { get; set; }
    }

    public class CreateAnimalDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int AnimalTypeId { get; set; }
    }

    public class UpdateAnimalDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int AnimalTypeId { get; set; }
    }
}
