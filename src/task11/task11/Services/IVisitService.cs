using System.Collections.Generic;
using System.Threading.Tasks;
using task11.Models;

namespace task11.Services
{
    public interface IVisitService
    {
        Task<IEnumerable<Visit>> GetVisitsAsync();
        Task<Visit> GetVisitByIdAsync(int id);
        Task CreateVisitAsync(Visit visit);
        Task UpdateVisitAsync(Visit visit);
        Task DeleteVisitAsync(int id);
    }
}
