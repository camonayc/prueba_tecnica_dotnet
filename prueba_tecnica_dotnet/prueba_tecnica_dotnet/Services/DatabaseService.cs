namespace prueba_tecnica_dotnet.Services;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using prueba_tecnica_dotnet.Models.Request;
using prueba_tecnica_dotnet.Models.Response;
using System;

public interface IDatabaseService
{
    ValidateResponse ValidateCredentials(UserRequest userRequest);
    ValidateResponse RegisterUser(RegisterUserRequest registerRequest);
}

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public ValidateResponse ValidateCredentials(UserRequest userRequest)
    {
        using MySqlConnection connection = new MySqlConnection(_connectionString);
        connection.Open();

        var User = userRequest.UserName;
        var Password = userRequest.Password;

        try
        {
            string consulta = "SELECT COUNT(*) FROM usuario WHERE Usuario = @User AND Password = @Password;";
            using MySqlCommand command = new MySqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@User", User);
            command.Parameters.AddWithValue("@Password", Password);

            int count = Convert.ToInt32(command.ExecuteScalar());

            if (count > 0)
            {
                return new ValidateResponse
                {
                    Response = true,
                    Messages = "Inicio de sesión exitoso.",
                    StatusCode = 200 // Código de estado OK
                };
            }
            else
            {
                return new ValidateResponse
                {
                    Response= false,
                    Messages = "Usuario o contraseña incorrectos.",
                    StatusCode = 401 // Código de estado no autorizado
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al validar credenciales: {ex.Message}");
            return new ValidateResponse
            {
                Response = false,
                Messages = "Error al validar las credenciales.",
                StatusCode = 500 // Código de estado de error interno del servidor
            };
        }
    }

    public ValidateResponse RegisterUser(RegisterUserRequest registerRequest)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // Verificar si el usuario ya existe
                string verificarExistenciaQuery = "SELECT COUNT(*) FROM usuario WHERE Usuario = @User;";
                using (MySqlCommand verificarExistenciaCommand = new MySqlCommand(verificarExistenciaQuery, connection))
                {
                    verificarExistenciaCommand.Parameters.AddWithValue("@User", registerRequest.user.UserName);

                    int existenciaCount = Convert.ToInt32(verificarExistenciaCommand.ExecuteScalar());

                    if (existenciaCount > 0)
                    {
                        return new ValidateResponse
                        {
                            Response = false,
                            Messages = "El usuario ya existe.",
                            StatusCode = 409 // Código de estado de conflicto
                        };
                    }
                }

                // Si el usuario no existe, proceder con el registro en ambas tablas
                string registroUsuarioQuery = "INSERT INTO usuario (Usuario, Password) VALUES (@User, @Password);";
                using (MySqlCommand registroUsuarioCommand = new MySqlCommand(registroUsuarioQuery, connection))
                {
                    registroUsuarioCommand.Parameters.AddWithValue("@User", registerRequest.user.UserName);
                    registroUsuarioCommand.Parameters.AddWithValue("@Password", registerRequest.user.Password);

                    int registrosAfectadosUsuario = registroUsuarioCommand.ExecuteNonQuery();

                    // Verificar que se insertó el usuario
                    if (registrosAfectadosUsuario > 0)
                    {
                        string registroPersonaQuery = "INSERT INTO personas (nombres, apellidos, num_identificacion, email, tipo_identificacion) VALUES (@Name, @LastName, @NumIdentification, @Email, @TypeIdentification);";
                        using (MySqlCommand registroPersonaCommand = new MySqlCommand(registroPersonaQuery, connection))
                        {
                            registroPersonaCommand.Parameters.AddWithValue("@Name", registerRequest.person.Name);
                            registroPersonaCommand.Parameters.AddWithValue("@LastName", registerRequest.person.LastName);
                            registroPersonaCommand.Parameters.AddWithValue("@NumIdentification", registerRequest.person.LastName);
                            registroPersonaCommand.Parameters.AddWithValue("@Email", registerRequest.person.Email);
                            registroPersonaCommand.Parameters.AddWithValue("@TypeIdentification", registerRequest.person.LastName);

                            int registrosAfectadosPersona = registroPersonaCommand.ExecuteNonQuery();

                            if (registrosAfectadosPersona > 0)
                            {
                                transaction.Commit();
                                return new ValidateResponse
                                {
                                    Response = true,
                                    Messages = "Usuario registrado exitosamente.",
                                    StatusCode = 201 // Código de estado de creado
                                };
                            }
                        }
                    }
                }

                // En caso de error, deshacer la transacción
                transaction.Rollback();
                return new ValidateResponse
                {
                    Response = false,
                    Messages = "Error al registrar el usuario.",
                    StatusCode = 500 // Código de estado de error interno del servidor
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar usuario y persona: {ex.Message}");

                // En caso de excepción, deshacer la transacción
                transaction.Rollback();

                return new ValidateResponse
                {
                    Response = false,
                    Messages = "Error al registrar el usuario.",
                    StatusCode = 500 // Código de estado de error interno del servidor
                };
            }
        }
    }



}
