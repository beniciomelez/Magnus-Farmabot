using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Services
{
    public class MedicamentoService
    {
        private readonly IDbContextFactory<ConexionBD> _factory;

        public MedicamentoService(IDbContextFactory<ConexionBD> factory)
        {
            _factory = factory;
        }

        public async Task<List<Medicamento>> ObtenerTodosAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Medicamentos
                .Include(m => m.IdCategoriaNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Medicamento?> ObtenerPorIdAsync(int id)
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Medicamentos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMedicamento == id);
        }

        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task AgregarAsync(Medicamento medicamento)
        {
            await using var context = await _factory.CreateDbContextAsync();

            context.Medicamentos.Add(medicamento);
            await context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Medicamento medicamento)
        {
            await using var context = await _factory.CreateDbContextAsync();

            var existente = await context.Medicamentos
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

            await context.SaveChangesAsync();
        }
    }
}