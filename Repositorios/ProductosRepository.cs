using Microsoft.Data.Sqlite;
using tl2_tp7_2025_ivan2214.Models;

namespace tl2_tp7_2025_ivan2214.Repositorios
{
  public interface IProductoRepository
  {
    Producto BuscarPorId(int id);
    List<Producto> BuscarTodosLosProductos();
    void CrearProducto(Producto producto);
    void ActualizarProducto(int id, Producto producto);
    void EliminarProducto(int id);
  }
  public class ProductoRepository : IProductoRepository
  {
    private readonly string _connectionString;

    public ProductoRepository(string connectionString)
    {
      _connectionString = connectionString;
    }


    public List<Producto> BuscarTodosLosProductos()
    {
      List<Producto> productos = new List<Producto>();

      using var connection = new SqliteConnection(_connectionString);
      connection.Open();

      var query = "SELECT idProducto, descripcion, precio FROM Productos;";
      using var command = new SqliteCommand(query, connection);
      using var reader = command.ExecuteReader();

      while (reader.Read())
      {
        productos.Add(new Producto
        {
          IdProducto = reader.GetInt32(0),
          Descripcion = reader.GetString(1),
          Precio = reader.GetFloat(2)
        });
      }

      return productos;
    }


    public Producto BuscarPorId(int id)
    {
      using var connection = new SqliteConnection(_connectionString);
      connection.Open();

      var query = "SELECT idProducto, descripcion, precio FROM Productos WHERE idProducto = @id;";
      using var command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@id", id);

      using var reader = command.ExecuteReader();

      if (reader.Read())
      {
        return new Producto
        {
          IdProducto = reader.GetInt32(0),
          Descripcion = reader.GetString(1),
          Precio = reader.GetInt32(2)
        };
      }

      return null;
    }


    public void CrearProducto(Producto producto)
    {
      using var connection = new SqliteConnection(_connectionString);
      connection.Open();

      var query = "INSERT INTO Productos (descripcion, precio) VALUES (@descripcion, @precio);";
      using var command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
      command.Parameters.AddWithValue("@precio", producto.Precio);
      command.ExecuteNonQuery();
    }


    public void ActualizarProducto(int id, Producto producto)
    {
      using var connection = new SqliteConnection(_connectionString);
      connection.Open();

      var query = "UPDATE Productos SET descripcion = @descripcion, precio = @precio WHERE idProducto = @id;";
      using var command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
      command.Parameters.AddWithValue("@precio", producto.Precio);
      command.Parameters.AddWithValue("@id", id);

      command.ExecuteNonQuery();
    }


    public void EliminarProducto(int id)
    {
      using var connection = new SqliteConnection(_connectionString);
      connection.Open();

      var query = "EliminarProducto FROM Productos WHERE idProducto = @id;";
      using var command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@id", id);

      command.ExecuteNonQuery();
    }
  }
}