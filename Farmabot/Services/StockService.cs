using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;


namespace Farmabot.Services
{
    public class StockService
    {
        private readonly ConexionBD _context;

        public StockService(ConexionBD context)
        {
            _context = context;
        }

        public async Task<List<LotesMedicamento>> ObtenerLotesAsync()
        {
            return await _context.LotesMedicamentos
                .Include(l => l.IdMedicamentoNavigation)
                    .ThenInclude(m => m.IdCategoriaNavigation)
                .AsNoTracking()
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();
        }
    }
}
