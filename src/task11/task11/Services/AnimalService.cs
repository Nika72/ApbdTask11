using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using task11.Data;
using task11.Models;

namespace task11.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly AnimalClinicContext _context;

        public AnimalService(AnimalClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Animal>> GetAnimalsAsync(string queryBy)
        {
            return queryBy switch
            {
                "Description" => await _context.Animals.Include(a => a.AnimalType).OrderBy(a => a.Description).ToListAsync(),
                _ => await _context.Animals.Include(a => a.AnimalType).OrderBy(a => a.Name).ToListAsync(),
            };
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            return await _context.Animals.Include(a => a.AnimalType).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAnimalAsync(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            try
            {
                _context.Entry(animal).OriginalValues["RowVersion"] = animal.RowVersion;
                _context.Entry(animal).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency conflict occurred", ex);
            }
        }

        public async Task DeleteAnimalAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();
            }
        }
    }
}