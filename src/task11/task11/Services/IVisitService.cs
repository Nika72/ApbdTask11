using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using task11.Models;

namespace task11.Services
{
    public interface IVisitService
    {
        Task<IEnumerable<Visit>> GetVisitsAsync(CancellationToken cancellationToken);
        Task<Visit> GetVisitByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateVisitAsync(Visit visit, CancellationToken cancellationToken);
        Task UpdateVisitAsync(Visit visit, CancellationToken cancellationToken);
        Task DeleteVisitAsync(int id, CancellationToken cancellationToken);
    }
}