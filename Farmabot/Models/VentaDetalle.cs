using System;
using System.Collections.Generic;

namespace Farmabot.Models;

public partial class VentaDetalle
{
    public int IdDetalle { get; set; }

    public int IdVenta { get; set; }

    public int IdLote { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual LotesMedicamento IdLoteNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
