using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Services
{
    public class MedicamentoService
    {
        private readonly ConexionBD _context;
        public MedicamentoService(ConexionBD context)
        {
            _context = context;
        }

        public async Task<List<Medicamento>> ObtenerTodosAsync()
        {
            return await _context.Medicamentos
                .Include(m => m.IdCategoriaNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Medicamento?> ObtenerPorIdAsync(int id)
        {
            return await _context.Medicamentos
                .Include(m => m.IdCategoriaNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMedicamento == id);
        }


        public async Task AgregarAsync(Medicamento medicamento)
        {
            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();
        }

    }

}
