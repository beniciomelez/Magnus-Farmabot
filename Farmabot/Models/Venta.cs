using System;
using System.Collections.Generic;

namespace Farmabot.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public DateTime Fecha { get; set; }

    public int? IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public decimal Total { get; set; }

    public string? MetodoPago { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; set; } = new List<MovimientosStock>();

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
