using System;
using System.Collections.Generic;

namespace Farmabot.Models;

public partial class MovimientosStock
{
    public int IdMovimiento { get; set; }

    public int IdLote { get; set; }

    public int IdUsuario { get; set; }

    public string Tipo { get; set; } = null!;

    public int Cantidad { get; set; }

    public DateTime Fecha { get; set; }

    public string? Motivo { get; set; }

    public int? IdVenta { get; set; }

    public virtual LotesMedicamento IdLoteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual Venta? IdVentaNavigation { get; set; }
}
