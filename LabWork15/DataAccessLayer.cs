using System.Data;
using System.Data.SqlClient;

namespace LabWork15
{
    public static class DAL
    {
        private static string _serverName = "название сервера";
        private static string _dbName = "название БД";
        private static string _username = "логин пользователя";
        private static string _password = "пароль пользователя";

        public static string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new()
                {
                    DataSource = _serverName,
                    InitialCatalog = _dbName,
                    UserID = _username,
                    Password = _password,
                    TrustServerCertificate = true
                };

                return builder.ConnectionString;
            }
        }

        public static void ChangeConnectionSettings(string newServerName, string newDbName, string newUsername, string newPassword)
        {
            _serverName = newServerName;
            _dbName = newDbName;
            _username = newUsername;
            _password = newPassword;
        }

        public static bool IsConnected()
        {
            try
            {
                using (SqlConnection connection = new(ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        //Task3
        public static async Task<int> ExecuteSqlCommandAsync(string sqlQuery)
        {
            using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            using SqlCommand command = new(sqlQuery, connection);
            return await command.ExecuteNonQueryAsync();
        }

        //Task4
        public static async Task<object?> ExecuteScalarAsync(string sqlQuery)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new(sqlQuery, connection))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        //Task5
        public static async Task<int> ExecuteNonQueryAsync(string sqlQuery, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new(sqlQuery, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        //Task6
        public static async Task UploadPhotoToDatabase(string customerLogin, string filePath)
        {
            byte[] photoBytes = File.ReadAllBytes(filePath);

            string sqlQuery = "UPDATE Customers SET Photo = @Photo WHERE Login = @Login";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Photo", photoBytes),
                new SqlParameter("@Login", customerLogin)
            };

            await ExecuteNonQueryAsync(sqlQuery, parameters);
        }

        //Task7 
        public static async Task<byte[]?> ExecuteReaderAsync(string sqlQuery, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new(ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new(sqlQuery, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            byte[] fileBytes = (byte[])reader["Photo"];
                            return fileBytes;
                        }
                    }
                }
            }

            return null;
        }

        //Task 8
        public static async Task<DataTable> GetCustomersWithPhone(string sqlQuery, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        return dataTable;
                    }
                }
            }
        }
    }
}
