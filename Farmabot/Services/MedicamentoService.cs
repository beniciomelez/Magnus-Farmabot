
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

                .FirstOrDefaultAsync(m => m.IdMedicamento == id);
        }

        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task AgregarAsync(Medicamento medicamento)
        {
            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Medicamento medicamento)
        {
            var existente = await _context.Medicamentos
                .FirstOrDefaultAsync(
                    m => m.IdMedicamento == medicamento.IdMedicamento);

            if (existente == null)
            {
                throw new InvalidOperationException(
                    "El medicamento no existe en la base de datos.");
            }

            existente.Nombre = medicamento.Nombre;
            existente.Descripcion = medicamento.Descripcion;
            existente.PrincipioActivo = medicamento.PrincipioActivo;
            existente.Concentracion = medicamento.Concentracion;
            existente.Presentacion = medicamento.Presentacion;
            existente.Laboratorio = medicamento.Laboratorio;
            existente.IdCategoria = medicamento.IdCategoria;
            existente.PrecioVenta = medicamento.PrecioVenta;
            existente.RequiereReceta = medicamento.RequiereReceta;
            existente.Activo = medicamento.Activo;

            await _context.SaveChangesAsync();
        }
    }
}
