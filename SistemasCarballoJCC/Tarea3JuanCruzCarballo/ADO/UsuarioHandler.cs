using Tarea3JuanCruzCarballo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Tarea3JuanCruzCarballo.ADO
{
    public static class UsuarioHandler
    {
        public static string connectionString = "Data Source=DESKTOP-IHFL947;Initial Catalog=carballo_Sistema_Gestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
       
        
        
        public static List<Usuario> GetUsuarios(DataTable table)
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (DataRow row in table.Rows)
            {
                Usuario getUsuario = new Usuario();
                getUsuario.Id = Convert.ToInt32(row["Id"]);
                getUsuario.Nombre = row["Nombre"].ToString();
                getUsuario.Apellido = row["Apellido"].ToString();
                getUsuario.NombreUsuario = row["NombreUsuario"].ToString();
                getUsuario.Contrasena = row["Contrasena"].ToString();
                getUsuario.Mail = row["Email"].ToString();

                usuarios.Add(getUsuario);
            }
            return usuarios;
        }

        public static Usuario GetUsuarioByPassword(string Nombre, string Contrasena)
        {
            
            Usuario usuario = new Usuario();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.Connection.Open();

                    command.CommandText = @" SELECT * 
                                FROM usuario 
                                WHERE NombreUsuario = @Nombre
                                AND   Contrasena = @Contrasena;";

                    command.Parameters.AddWithValue("@Nombre", Nombre);
                    command.Parameters.AddWithValue("@Contrasena", Contrasena);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)
                    {
                        return new Usuario();
                    }


                    List<Usuario> usuarios = GetUsuarios(table);
                    usuario = usuarios[0];

                    command.Connection.Close();
                }
            }
            return usuario;
        }



        public static bool UpdateUsuario(Usuario usuario)
        {
            bool modificado = false;

            if (usuario.NombreUsuario == null ||
                usuario.NombreUsuario.Trim() == "" ||
                usuario.Contrasena == null ||
                usuario.Contrasena.Trim() == "" ||
                usuario.Nombre == null ||
                usuario.Nombre.Trim() == "" ||
                usuario.Apellido == null ||
                usuario.Apellido.Trim() == "")
            {
                return modificado;
                throw new Exception("Faltan datos obligatorios");
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @" UPDATE usuario
                                                SET 
                                                   Nombre = @Nombre,
                                                   Apellido = @Apellido,
                                                   NombreUsuario = @NombreUsuario,
										           Contrasena = @Contrasena,
										           Email = @Email
                                                WHERE Id = @Id";

                        sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        sqlCommand.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        sqlCommand.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        sqlCommand.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                        sqlCommand.Parameters.AddWithValue("@Email", usuario.Mail); ;
                        sqlCommand.Parameters.AddWithValue("@Id", usuario.Id);


                        int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente UPDATE

                        sqlCommand.Connection.Close();
                        if (recordsAffected == 0){
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

        public static bool InsertUsuario(Usuario usuario)
        {
            bool alta = false;
            Usuario usuarioRepetido = GetUsuarioByUserName(usuario.NombreUsuario);

            if (usuario.NombreUsuario == null ||
                usuario.NombreUsuario.Trim() == "" ||
                usuario.Contrasena == null ||
                usuario.Contrasena.Trim() == "" ||
                usuario.Nombre == null ||
                usuario.Nombre.Trim() == "" ||
                usuario.Apellido == null ||
                usuario.Apellido.Trim() == "")
            {
                return alta;
                throw new Exception("Faltan datos obligatorios");
            }
            else if (usuarioRepetido.Id != 0)
            {
                return alta;
                throw new Exception("El nombre de usuario ya existe");
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Connection.Open();
                sqlCommand.CommandText = @"INSERT INTO usuario
                                    ([Nombre]
                                    ,[Apellido]
                                    ,[NombreUsuario]
									,[Contrasena]
									,[Mail] )
                                    VALUES
                                    (@Nombre,
                                        @Apellido,
                                        @NombreUsuario,
										@Contrasena,
										@Mail)";

                sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                sqlCommand.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                sqlCommand.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@Contraseña", usuario.Contrasena);
                sqlCommand.Parameters.AddWithValue("@Mail", usuario.Mail);

                sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
                usuario.Id = GetId.Get(sqlCommand);

                alta = usuario.Id != 0 ? true : false;
                sqlCommand.Connection.Close();
                return alta;

            }
        }


        public static Usuario GetUsuarioByUserName(string nombre)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.Connection.Open();

                    command.CommandText = @"SELECT * 
                                FROM Usuario 
                                WHERE NombreUsuario = @Nombre;";

                    command.Parameters.AddWithValue("@Nombre", nombre);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)
                    {
                        return new Usuario();
                    }


                    List<Usuario> usuarios = GetUsuarios(table);
                    usuario = usuarios[0];

                    command.Connection.Close();
                }
            }
            return usuario;
        }

        public static bool EliminarUsuario(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();

                    sqlCommand.CommandText = @" DELETE 
                                                    usuario
                                                WHERE 
                                                    Id = @Id
                                            ";

                    sqlCommand.Parameters.AddWithValue("@Id", id);


                    int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el DELETE

                    sqlCommand.CommandText = @" DELETE 
                                                    usuario
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

