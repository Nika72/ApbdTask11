using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using task11.Data;
using task11.Models;

public class AnimalService : IAnimalService
{
    private readonly AnimalClinicContext _context;

    public AnimalService(AnimalClinicContext context)
    {
        _context = context;
    }

    public async Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        await _context.Animals.AddAsync(animal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Animal>> GetAnimalsAsync(string queryBy, CancellationToken cancellationToken = default)
    {
        return queryBy switch
        {
            "description" => await _context.Animals.Include(a => a.AnimalType).OrderBy(a => a.Description).ToListAsync(cancellationToken),
            _ => await _context.Animals.Include(a => a.AnimalType).OrderBy(a => a.Name).ToListAsync(cancellationToken),
        };
    }

    public async Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Animals.Include(a => a.AnimalType).FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task UpdateAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        _context.Animals.Update(animal);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAnimalAsync(int id, CancellationToken cancellationToken = default)
    {
        var animal = await _context.Animals.FindAsync(new object[] { id }, cancellationToken);
        if (animal != null)
        {
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}