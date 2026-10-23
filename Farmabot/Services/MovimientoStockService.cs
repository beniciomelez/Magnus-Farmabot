
using System.Data;
using Farmabot.Data;
using Farmabot.Models;
using Microsoft.EntityFrameworkCore;

namespace Farmabot.Services
{
    public class MovimientoStockService
    {
        private readonly IDbContextFactory<ConexionBD> _factory;

        public MovimientoStockService(
            IDbContextFactory<ConexionBD> factory)
        {
            _factory = factory;
        }

        public async Task<List<MovimientosStock>> ObtenerTodosAsync()
        {
            await using var context =
                await _factory.CreateDbContextAsync();

            return await context.MovimientosStocks
                .Include(m => m.IdLoteNavigation)
                    .ThenInclude(l => l.IdMedicamentoNavigation)
                .Include(m => m.IdUsuarioNavigation)
                .AsNoTracking()
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }

        public async Task RegistrarAsync(
            int idMedicamento,
            string tipo,
            int cantidad,
            int idUsuario,
            string? motivo)
        {
            if (idMedicamento <= 0)
                throw new InvalidOperationException(
                    "Seleccioná un medicamento válido.");

            if (cantidad <= 0)
                throw new InvalidOperationException(
                    "La cantidad debe ser mayor que cero.");

            if (tipo != "Entrada" && tipo != "Salida")
                throw new InvalidOperationException(
                    "El tipo debe ser Entrada o Salida.");

            await using var context =
                await _factory.CreateDbContextAsync();

            await using var transaccion =
                await context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var usuarioExiste = await context.Usuarios
                    .AnyAsync(u =>
                        u.IdUsuario == idUsuario && u.Activo);

                if (!usuarioExiste)
                    throw new InvalidOperationException(
                        "El usuario no existe o está inactivo.");

                var medicamentoExiste = await context.Medicamentos
                    .AnyAsync(m =>
                        m.IdMedicamento == idMedicamento && m.Activo);

                if (!medicamentoExiste)
                    throw new InvalidOperationException(
                        "El medicamento no existe o está inactivo.");

                var hoy = DateOnly.FromDateTime(DateTime.Today);

                var lotes = await context.LotesMedicamentos
                    .Where(l => l.IdMedicamento == idMedicamento)
                    .OrderBy(l => l.FechaVencimiento)
                    .ThenBy(l => l.IdLote)
                    .ToListAsync();

                var lotesDisponibles = lotes
                    .Where(l => l.FechaVencimiento >= hoy)
                    .ToList();

                if (tipo == "Entrada")
                {
                    var loteEntrada = lotesDisponibles.FirstOrDefault();

                    if (loteEntrada == null)
                        throw new InvalidOperationException(
                            "El medicamento no tiene lotes vigentes. " +
                            "Se necesita registrar un lote antes de ingresar stock.");

                    loteEntrada.Stock = checked(
                        loteEntrada.Stock + cantidad);

                    context.MovimientosStocks.Add(new MovimientosStock
                    {
                        IdLote = loteEntrada.IdLote,
                        IdUsuario = idUsuario,
                        Tipo = tipo,
                        Cantidad = cantidad,
                        Fecha = DateTime.Now,
                        Motivo = motivo
                    });
                }
                else
                {
                    var lotesConStock = lotesDisponibles
                        .Where(l => l.Stock > 0)
                        .ToList();

                    var stockDisponible = lotesConStock.Sum(l => (long)l.Stock);

                    if (stockDisponible < cantidad)
                        throw new InvalidOperationException(
                            "No hay suficiente stock vigente disponible.");

                    var restante = cantidad;

                    foreach (var lote in lotesConStock)
                    {
                        if (restante == 0)
                            break;

                        var descontar = Math.Min(lote.Stock, restante);

                        lote.Stock -= descontar;
                        restante -= descontar;

                        context.MovimientosStocks.Add(
                            new MovimientosStock
                            {
                                IdLote = lote.IdLote,
                                IdUsuario = idUsuario,
                                Tipo = tipo,
                                Cantidad = descontar,
                                Fecha = DateTime.Now,
                                Motivo = motivo
                            });
                    }

                    if (restante != 0)
                        throw new InvalidOperationException(
                            "No se pudo completar el descuento de stock.");
                }

                await context.SaveChangesAsync();
                await transaccion.CommitAsync();
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }
    }
}
