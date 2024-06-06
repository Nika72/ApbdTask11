using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Visit>> GetVisitsAsync()
        {
            return await _context.Visits.Include(v => v.Animal).Include(v => v.Employee).OrderBy(v => v.Date).ToListAsync();
        }

        public async Task<Visit> GetVisitByIdAsync(int id)
        {
            return await _context.Visits.Include(v => v.Animal).Include(v => v.Employee).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task CreateVisitAsync(Visit visit)
        {
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVisitAsync(Visit visit)
        {
            _context.Entry(visit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit != null)
            {
                _context.Visits.Remove(visit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
