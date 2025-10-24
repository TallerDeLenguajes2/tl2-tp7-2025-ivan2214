using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025_ivan2214.Models;
using tl2_tp7_2025_ivan2214.Repositorios;


namespace tl2_tp7_2025_ivan2214.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PresupuestosController : ControllerBase
  {
    private readonly PresupuestosRepository _repo;

    public PresupuestosController()
    {
      string cadenaConexion = "Data Source=Tienda.db;";
      _repo = new PresupuestosRepository(cadenaConexion);
    }


    [HttpPost]
    public ActionResult<string> CrearPresupuesto([FromBody] Presupuesto presupuesto)
    {
      _repo.CrearPresupuesto(presupuesto);
      return Ok("Presupuesto creado con exito.");
    }


    [HttpPost("{id}/ProductoDetalle")]
    public ActionResult<string> AgregarProductoDetalle(int id, [FromBody] PresupuestoDetalle detalle)
    {
      Presupuesto presupuesto = _repo.ObtenerPorId(id);
      if (presupuesto == null) return NotFound("Presupuesto no encontrado.");

      if (detalle.Producto == null || detalle.Producto.IdProducto == 0)
        return BadRequest("Producto invalido.");

      _repo.AgregarProducto(id, detalle.Producto.IdProducto, detalle.Cantidad);
      return NoContent();
    }


    [HttpGet("{id}")]
    public ActionResult ObtenerPorId(int id)
    {
      var presupuesto = _repo.ObtenerPorId(id);
      if (presupuesto == null) return NotFound("Presupuesto no encontrado.");

      return Ok(presupuesto);
    }


    [HttpGet]
    public ActionResult<List<Presupuesto>> ListarPresupuestos()
    {
      List<Presupuesto> listPresupuestos = _repo.ListarPresupuestos();
      return Ok(listPresupuestos);
    }


    [HttpDelete("{id}")]
    public ActionResult EliminarPresupuesto(int id)
    {
      var presupuesto = _repo.ObtenerPorId(id);
      if (presupuesto == null) return NotFound("Presupuesto no encontrado.");

      _repo.EliminarPresupuesto(id);
      return NoContent();
    }
  }
}
