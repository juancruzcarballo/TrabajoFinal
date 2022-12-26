using Tarea3JuanCruzCarballo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tarea3JuanCruzCarballo.ADO
{
    public static class VentaHandler
    {
        public static string ConnectionString = "Data Source=DESKTOP-IHFL947; Initial Catalog = carballo_Sistema_Gestion; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<Venta> GetVentas(int id)
        {
            List<Venta> ventas = new List<Venta>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"select * from venta
                                                where IdUsuario = @IdUsuario;";

                    sqlCommand.Parameters.AddWithValue("@IdUsuario", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); //Se ejecuta el Select

                    foreach (DataRow row in table.Rows)
                    {
                        Venta venta = new Venta();
                        venta.Id = Convert.ToInt32(row["Id"]);
                        venta.Comentarios = row["Comentarios"].ToString();
                        venta.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                        ventas.Add(venta);
                    }
                    sqlCommand.Connection.Close();
                }
            }
            return ventas;
        }
        public static void InsertVenta(List<Producto> productos, int IdUsuario)
        {
            Venta venta = new Venta();

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = @"INSERT INTO venta
                                ([Comentarios]
                                ,[IdUsuario])
                                VALUES
                                (@Comentarios,
                                    @IdUsuario)";

            sqlCommand.Parameters.AddWithValue("@Comentarios", "");
            sqlCommand.Parameters.AddWithValue("@IdUsuario", IdUsuario);

            sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
            venta.Id = GetId.Get(sqlCommand);
            venta.IdUsuario = IdUsuario;

            foreach (Producto producto in productos)
            {
                sqlCommand.CommandText = @"INSERT INTO productoVendido
                                ([Stock]
                                ,[IdProducto]
                                ,[IdVenta])
                                VALUES
                                (@Stock,
                                @IdProducto,
                                @IdVenta)";

                sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                sqlCommand.Parameters.AddWithValue("@IdProducto", producto.Id);
                sqlCommand.Parameters.AddWithValue("@IdVenta", venta.Id);

                sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
                sqlCommand.Parameters.Clear();

                sqlCommand.CommandText = @" UPDATE producto
                                                SET 
                                                Stock = Stock - @Stock
                                                WHERE id = @IdProducto";

                sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                sqlCommand.Parameters.AddWithValue("@IdProducto", producto.Id);

                sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
                sqlCommand.Parameters.Clear();
            }
            sqlCommand.Connection.Close();
        }
        public static bool EliminarVenta(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();

                    sqlCommand.CommandText = @" DELETE 
                                                    venta
                                                WHERE 
                                                    Id = @Id
                                            ";

                    sqlCommand.Parameters.AddWithValue("@Id", id);


                    int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el DELETE

                    sqlCommand.CommandText = @" DELETE 
                                                    venta
                                                WHERE 
                                                    Id = @Id
                                            ";

                    recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el DELETE
                    sqlCommand.Connection.Close();

                    if (recordsAffected != 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }
}


      

