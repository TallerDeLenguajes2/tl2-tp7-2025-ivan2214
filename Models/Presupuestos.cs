

namespace tl2_tp7_2025_ivan2214.Models
{
  public class Presupuesto
  {
    public int IdPresupuesto { get; set; } // clave primaria
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();


    public double MontoPresupuesto()
    {
      return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
    }


    public double MontoPresupuestoConIva()
    {
      double subtotal = MontoPresupuesto();
      return subtotal * 1.21;
    }


    public int CantidadProductos()
    {
      return Detalle.Sum(d => d.Cantidad);
    }
  }
}
