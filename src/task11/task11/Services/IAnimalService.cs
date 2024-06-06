using System.Collections.Generic;
using System.Threading.Tasks;
using task11.Models;

namespace task11.Services
{
    public interface IAnimalService
    {
        Task<IEnumerable<Animal>> GetAnimalsAsync(string queryBy);
        Task<Animal> GetAnimalByIdAsync(int id);
        Task CreateAnimalAsync(Animal animal);
        Task UpdateAnimalAsync(Animal animal);
        Task DeleteAnimalAsync(int id);
    }
}
