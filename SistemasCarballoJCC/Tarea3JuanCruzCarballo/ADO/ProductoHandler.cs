using Tarea3JuanCruzCarballo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tarea3JuanCruzCarballo.ADO
{
    public static class ProductoHandler
    {
        public static string ConnectionString = "Data Source=DESKTOP-IHFL947; Initial Catalog = carballo_Sistema_Gestion; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static List<Producto> GetProductos(int id)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = @"select * from producto
                                                where IdUsuario = @idUsuario;";

                    sqlCommand.Parameters.AddWithValue("@idUsuario", id);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = sqlCommand;
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table); //Se ejecuta el Select
                    sqlCommand.Connection.Close();
                    foreach (DataRow row in table.Rows)
                    {
                        Producto producto = new Producto();
                        producto.Id = Convert.ToInt32(row["Id"]);
                        producto.Descripcion = row["Descripcion"].ToString();
                        producto.Costo = Convert.ToDecimal(row["Costo"]);
                        producto.PrecioVenta = Convert.ToDecimal(row["PrecioVenta"]);
                        producto.Stock = Convert.ToInt32(row["Stock"]);
                        producto.IdUsuario = Convert.ToInt32(row["IdUsuario"]);

                        productos.Add(producto);
                    }
                    
                }
            }
            return productos;
        }

        public static bool ModificarProductos(Producto producto)
        {
            bool modificado = false;

            if (producto.Descripcion == null ||
                producto.Descripcion == "" ||
                producto.IdUsuario == 0)
            {
                return modificado;
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @" UPDATE producto
                                                SET 
                                                   Descripcion = @Descripcion,
                                                   Costo = @Costo,
                                                   PrecioVenta = @PrecioVenta,
										           Stock = @Stock
                                                WHERE id = @Id";

                        sqlCommand.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        sqlCommand.Parameters.AddWithValue("@Costo", producto.Costo);
                        sqlCommand.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                        sqlCommand.Parameters.AddWithValue("@Id", producto.Id);


                        int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente UPDATE
                        sqlCommand.Connection.Close();

                        if (recordsAffected == 0)
                        {
                            return modificado;
                            throw new Exception("El registro a modificar no existe.");
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }
        public static bool InsertProducto(Producto producto)
        {
            bool alta = false;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = @"INSERT INTO producto
                                ([Descripcion]
                                ,[Costo]
                                ,[PrecioVenta]
								,[Stock]
                                ,[IdUsuario])
                                VALUES
                                (@Descripcion,
                                    @Costo,
                                    @PrecioVenta,
									@Stock,
                                    @IdUsuario)";



            sqlCommand.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            sqlCommand.Parameters.AddWithValue("@Costo", producto.Costo);
            sqlCommand.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
            sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
            sqlCommand.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);

            sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
            producto.Id = GetId.Get(sqlCommand);

            alta = producto.Id != 0 ? true : false;
            sqlCommand.Connection.Close();
            return alta;


        }

        public static bool EliminarProducto(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();

                    sqlCommand.CommandText = @" DELETE 
                                                    productoVendido
                                                WHERE 
                                                    IdProducto = @Id
                                            ";

                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    

                    int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el DELETE

                    sqlCommand.CommandText = @" DELETE 
                                                    producto
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
