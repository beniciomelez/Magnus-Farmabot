
using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Services
{
    public class MovimientoStockService
    {
        private readonly ConexionBD _context;

        public MovimientoStockService(ConexionBD context)
        {
            _context = context;
        }

        public async Task<List<Farmabot.Models.MovimientosStock>> ObtenerTodosAsync()
        {
            return await _context.MovimientosStocks
       .Include(m => m.IdLoteNavigation)
           .ThenInclude(l => l.IdMedicamentoNavigation)
       .Include(m => m.IdUsuarioNavigation)
       .AsNoTracking()
       .OrderByDescending(m => m.Fecha)
       .ToListAsync()
        }
    }
}
