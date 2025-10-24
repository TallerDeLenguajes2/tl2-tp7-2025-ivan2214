using Microsoft.Data.Sqlite;
using tl2_tp7_2025_ivan2214.Models;

namespace tl2_tp7_2025_ivan2214.Repositorios
{
  public interface IPresupuestsosRepository
  {
    void CrearPresupuesto(Presupuesto p);
    List<Presupuesto> ListarPresupuestos();
    Presupuesto ObtenerPorId(int id);
    void AgregarProducto(int idPresupuesto, int idProducto, int cantidad);
    void EliminarPresupuesto(int id);
  }

  public class PresupuestosRepository : IPresupuestsosRepository
  {
    private readonly string _connectionString;

    public PresupuestosRepository(string connectionString)
    {
      _connectionString = connectionString;
    }



    public void CrearPresupuesto(Presupuesto p)
    {
      using var conexion = new SqliteConnection(_connectionString);
      conexion.Open();

      string sql = "INSERT INTO Presupuesto (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion); SELECT last_insert_rowid();";

      using var comando = new SqliteCommand(sql, conexion);
      comando.Parameters.AddWithValue("@NombreDestinatario", p.NombreDestinatario);
      comando.Parameters.AddWithValue("@FechaCreacion", p.FechaCreacion.ToString("yyyy-MM-dd"));

      long idPresupuesto = (long)comando.ExecuteScalar();
      p.IdPresupuesto = (int)idPresupuesto;


      foreach (var d in p.Detalle)
      {
        string sqlDetalle = "INSERT INTO PresupuestoDetalle (PresupuestoId, ProductoId, Cantidad) VALUES (@PresupuestoId, @ProductoId, @Cantidad)";
        using var cmdDetalle = new SqliteCommand(sqlDetalle, conexion);
        cmdDetalle.Parameters.AddWithValue("@PresupuestoId", idPresupuesto);
        cmdDetalle.Parameters.AddWithValue("@ProductoId", d.Producto.IdProducto);
        cmdDetalle.Parameters.AddWithValue("@Cantidad", d.Cantidad);
        cmdDetalle.ExecuteNonQuery();
      }
    }


    public List<Presupuesto> ListarPresupuestos()
    {
      var presupuestos = new List<Presupuesto>();
      using var conexion = new SqliteConnection(_connectionString);
      conexion.Open();

      string sql = "SELECT IdPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuesto";
      using var comando = new SqliteCommand(sql, conexion);
      using var lector = comando.ExecuteReader();

      while (lector.Read())
      {
        presupuestos.Add(new Presupuesto
        {
          IdPresupuesto = Convert.ToInt32(lector["IdPresupuesto"]),
          NombreDestinatario = lector["NombreDestinatario"].ToString(),
          FechaCreacion = DateTime.Parse(lector["FechaCreacion"].ToString())
        });
      }

      return presupuestos;
    }


    public Presupuesto ObtenerPorId(int id)
    {
      using var conexion = new SqliteConnection(_connectionString);
      conexion.Open();

      string sqlPresupuesto = "SELECT Id, NombreDestinatario, FechaCreacion FROM Presupuesto WHERE Id = @Id";
      using var cmdPresupuesto = new SqliteCommand(sqlPresupuesto, conexion);
      cmdPresupuesto.Parameters.Add(new SqliteParameter("@Id", id));

      using var lector = cmdPresupuesto.ExecuteReader();
      if (!lector.Read()) return null;

      var presupuesto = new Presupuesto
      {
        IdPresupuesto = Convert.ToInt32(lector["Id"]),
        NombreDestinatario = lector["NombreDestinatario"].ToString(),
        FechaCreacion = DateTime.Parse(lector["FechaCreacion"].ToString())
      };

      lector.Close();


      string sqlDetalle = @"
            SELECT pd.Cantidad, p.Id AS ProductoId, p.Nombre, p.Precio
            FROM PresupuestoDetalle pd
            INNER JOIN Producto p ON pd.ProductoId = p.Id
            WHERE pd.PresupuestoId = @Id";

      using var cmdDetalle = new SqliteCommand(sqlDetalle, conexion);
      cmdDetalle.Parameters.Add(new SqliteParameter("@Id", id));
      using var lectorDetalle = cmdDetalle.ExecuteReader();

      while (lectorDetalle.Read())
      {
        presupuesto.Detalle.Add(new PresupuestoDetalle
        {
          Cantidad = Convert.ToInt32(lectorDetalle["Cantidad"]),
          Producto = new Producto
          {
            IdProducto = Convert.ToInt32(lectorDetalle["ProductoId"]),
            Descripcion = lectorDetalle["Descripcion"].ToString(),
            Precio = Convert.ToInt32(lectorDetalle["Precio"])
          }
        });
      }

      return presupuesto;
    }


    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
      using var conexion = new SqliteConnection(_connectionString);
      conexion.Open();

      string sql = "INSERT INTO PresupuestoDetalle (PresupuestoId, ProductoId, Cantidad) VALUES (@PresupuestoId, @ProductoId, @Cantidad)";
      using var comando = new SqliteCommand(sql, conexion);
      comando.Parameters.AddWithValue("@PresupuestoId", idPresupuesto);
      comando.Parameters.AddWithValue("@ProductoId", idProducto);
      comando.Parameters.AddWithValue("@Cantidad", cantidad);

      comando.ExecuteNonQuery();
    }


    public void EliminarPresupuesto(int id)
    {
      using var conexion = new SqliteConnection(_connectionString);
      conexion.Open();

      string sql = "DELETE FROM Presupuesto WHERE Id = @Id";
      using var comando = new SqliteCommand(sql, conexion);
      comando.Parameters.AddWithValue("@Id", id);
      comando.ExecuteNonQuery();
    }
  }

}