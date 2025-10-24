using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025_ivan2214.Models;
using tl2_tp7_2025_ivan2214.Repositorios;

namespace tl2_tp7_2025_ivan2214.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductoController : ControllerBase
  {
    private readonly ProductoRepository _repo;

    public ProductoController()
    {

      string cadenaConexion = "Data Source=Tienda.db;";
      _repo = new ProductoRepository(cadenaConexion);
    }


    [HttpPost]
    public ActionResult CrearProducto([FromBody] Producto producto)
    {
      _repo.CrearProducto(producto);
      return Ok("Producto creado exitosamente");
    }


    [HttpPut("{id}")]
    public ActionResult ModificarProducto(int id, [FromBody] Producto producto)
    {
      var existente = _repo.BuscarPorId(id);
      if (existente == null) return NotFound();

      _repo.ActualizarProducto(id, producto);
      return Ok("Producto actualizado exitosamente");
    }


    [HttpGet]
    public ActionResult ListarProductos()
    {
      var productos = _repo.BuscarTodosLosProductos();
      return Ok(productos);
    }


    [HttpGet("{id}")]
    public ActionResult ObtenerPorId(int id)
    {
      var producto = _repo.BuscarPorId(id);
      if (producto == null) return NotFound();

      return Ok(producto);
    }


    [HttpDelete("{id}")]
    public ActionResult EliminarProducto(int id)
    {
      var producto = _repo.BuscarPorId(id);
      if (producto == null) return NotFound();

      _repo.EliminarProducto(id);
      return NoContent();
    }
  }
}