
using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Services
{
    public class LoteMedicamentoService
    {
        private readonly IDbContextFactory<ConexionBD> _factory;

        public LoteMedicamentoService(
            IDbContextFactory<ConexionBD> factory)
        {
            _factory = factory;
        }

        public async Task<List<LotesMedicamento>> ObtenerTodosAsync()
        {
            await using var context =
                await _factory.CreateDbContextAsync();

            return await context.LotesMedicamentos
                .Include(l => l.IdMedicamentoNavigation)
                .AsNoTracking()
                .OrderBy(l => l.IdMedicamentoNavigation.Nombre)
                .ThenBy(l => l.FechaVencimiento)
                .ToListAsync();
        }

        public async Task RegistrarAsync(
            int idMedicamento,
            string numeroLote,
            DateOnly fechaVencimiento,
            int stock,
            decimal precioCompra)
        {
            if (idMedicamento <= 0)
                throw new InvalidOperationException(
                    "Seleccioná un medicamento válido.");

            if (string.IsNullOrWhiteSpace(numeroLote))
                throw new InvalidOperationException(
                    "Ingresá el número de lote.");

            if (fechaVencimiento < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException(
                    "La fecha de vencimiento no puede estar en el pasado.");

            if (stock <= 0)
                throw new InvalidOperationException(
                    "La cantidad inicial debe ser mayor que cero.");

            if (precioCompra < 0)
                throw new InvalidOperationException(
                    "El precio de compra no puede ser negativo.");

            await using var context =
                await _factory.CreateDbContextAsync();

            var medicamentoExiste = await context.Medicamentos
                .AnyAsync(m => m.IdMedicamento == idMedicamento && m.Activo);

            if (!medicamentoExiste)
                throw new InvalidOperationException(
                    "El medicamento no existe o está inactivo.");

            var loteExistente = await context.LotesMedicamentos
                .AnyAsync(l =>
                    l.IdMedicamento == idMedicamento &&
                    l.NumeroLote == numeroLote);

            if (loteExistente)
                throw new InvalidOperationException(
                    "Ese número de lote ya está registrado para este medicamento.");

            var lote = new LotesMedicamento
            {
                IdMedicamento = idMedicamento,
                NumeroLote = numeroLote.Trim(),
                FechaVencimiento = fechaVencimiento,
                FechaIngreso = DateTime.Now,
                Stock = stock,
                PrecioCompra = precioCompra
            };

            context.LotesMedicamentos.Add(lote);
            await context.SaveChangesAsync();
        }
    }
}
