using System;
using System.Collections.Generic;

namespace Farmabot.Models;

public partial class Medicamento
{
    public int IdMedicamento { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? PrincipioActivo { get; set; }

    public string? Concentracion { get; set; }

    public string? Presentacion { get; set; }

    public string? Laboratorio { get; set; }

    public int? IdCategoria { get; set; }

    public decimal PrecioVenta { get; set; }

    public bool RequiereReceta { get; set; }

    public bool Activo { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual ICollection<LotesMedicamento> LotesMedicamentos { get; set; } = new List<LotesMedicamento>();
}
