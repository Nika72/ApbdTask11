using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using task11.Dtos;
using task11.Models;
using task11.Services;
using task11.Data;

namespace task11.Controllers
{
    [Route("api/animals")]
    [Authorize]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimals(string queryBy = "name", CancellationToken cancellationToken = default)
        {
            if (!IsValidQueryBy(queryBy.ToLower()))
            {
                return BadRequest("The provided queryBy parameter is invalid. Please use one of the following values: name, description.");
            }

            try
            {
                IQueryable<Animal> sortedAnimals = queryBy.ToLower() switch
                {
                    "description" => _context.Animals.Include(a => a.AnimalType).OrderBy(a => a.Description),
                    _ => _context.Animals.Include(a => a.AnimalType).OrderBy(a => a.Name)
                };

                var result = await sortedAnimals.ToListAsync(cancellationToken);
                var dtoList = result.Select(a => new AnimalDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    AnimalType = a.AnimalType.Name
                }).ToList();

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnimalDto>> GetAnimal(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var animal = await _context.Animals.Include(a => a.AnimalType).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

                if (animal == null)
                {
                    return NotFound($"Animal with ID {id} not found!");
                }

                var dto = new AnimalDto
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    Description = animal.Description,
                    AnimalType = animal.AnimalType.Name
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal([FromBody] CreateAnimalDto createAnimalDto, CancellationToken cancellationToken = default)
        {
            if (createAnimalDto == null)
            {
                return BadRequest("Invalid animal data.");
            }

            var animalType = await _context.AnimalTypes.FirstOrDefaultAsync(at => at.Id == createAnimalDto.AnimalTypeId, cancellationToken);

            if (animalType == null)
            {
                return BadRequest("The provided AnimalType is not represented in the database.");
            }

            var animal = new Animal
            {
                Name = createAnimalDto.Name,
                Description = createAnimalDto.Description,
                AnimalTypesId = animalType.Id
            };

            await _animalService.CreateAnimalAsync(animal);

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] UpdateAnimalDto updateAnimalDto, CancellationToken cancellationToken = default)
        {
            if (updateAnimalDto == null || id != updateAnimalDto.Id)
            {
                return BadRequest("Invalid animal data or mismatched ID.");
            }

            var animal = await _context.Animals.Include(a => a.AnimalType).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (animal == null)
            {
                return NotFound($"Animal with ID {id} not found!");
            }

            var currentRowVersion = animal.RowVersion;

            animal.Name = updateAnimalDto.Name;
            animal.Description = updateAnimalDto.Description;
            animal.AnimalTypesId = updateAnimalDto.AnimalTypeId;

            _context.Entry(animal).State = EntityState.Modified;
            _context.Entry(animal).OriginalValues["RowVersion"] = currentRowVersion;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency conflict occurred. Please try again.");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAnimal(int id, CancellationToken cancellationToken = default)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound($"Animal with ID {id} not found!");
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        private static bool IsValidQueryBy(string queryBy)
        {
            var validParameters = new[] { "name", "description" };
            return validParameters.Contains(queryBy);
        }
    }
}
