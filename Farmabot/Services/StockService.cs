
using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Services
{
    public class StockService
    {
        private readonly IDbContextFactory<ConexionBD> _factory;

        public StockService(IDbContextFactory<ConexionBD> factory)
        {
            _factory = factory;
        }

        public async Task<List<Medicamento>> ObtenerMedicamentosConStockAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Medicamentos
                .Include(m => m.IdCategoriaNavigation)
                .Include(m => m.LotesMedicamentos)
                .AsNoTracking()
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        public async Task<List<LotesMedicamento>> ObtenerLotesAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.LotesMedicamentos
                .Include(l => l.IdMedicamentoNavigation)
                    .ThenInclude(m => m.IdCategoriaNavigation)
                .AsNoTracking()
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();
        }
    }
}
