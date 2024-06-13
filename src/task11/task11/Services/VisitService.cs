using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using task11.Data;
using task11.Models;

namespace task11.Services
{
    public class VisitService : IVisitService
    {
        private readonly AnimalClinicContext _context;

        public VisitService(AnimalClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Visit>> GetVisitsAsync(CancellationToken cancellationToken)
        {
            return await _context.Visits.Include(v => v.Animal).Include(v => v.Employee).ToListAsync(cancellationToken);
        }

        public async Task<Visit> GetVisitByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Visits.Include(v => v.Animal).Include(v => v.Employee).FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task CreateVisitAsync(Visit visit, CancellationToken cancellationToken)
        {
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateVisitAsync(Visit visit, CancellationToken cancellationToken)
        {
            _context.Entry(visit).OriginalValues["RowVersion"] = visit.RowVersion;
            _context.Entry(visit).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteVisitAsync(int id, CancellationToken cancellationToken)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit != null)
            {
                _context.Visits.Remove(visit);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}