using System;
using System.Collections.Generic;

namespace Farmabot.Models;

public partial class LotesMedicamento
{
    public int IdLote { get; set; }

    public int IdMedicamento { get; set; }

    public string NumeroLote { get; set; } = null!;

    public DateOnly FechaVencimiento { get; set; }

    public DateTime FechaIngreso { get; set; }

    public int Stock { get; set; }

    public decimal PrecioCompra { get; set; }

    public virtual Medicamento IdMedicamentoNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; set; } = new List<MovimientosStock>();

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
