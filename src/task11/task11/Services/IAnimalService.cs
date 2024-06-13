using System.Threading;
using System.Threading.Tasks;
using task11.Models;

public interface IAnimalService
{
    Task CreateAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<IEnumerable<Animal>> GetAnimalsAsync(string queryBy, CancellationToken cancellationToken = default);
    Task<Animal> GetAnimalByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAnimalAsync(Animal animal, CancellationToken cancellationToken = default);
    Task DeleteAnimalAsync(int id, CancellationToken cancellationToken = default);
}