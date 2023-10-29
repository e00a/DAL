using System.Data;
using System.Data.SqlClient;

namespace LabWork15
{
    public static class DAL
    {
        private static string _serverName = "server";
        private static string _dbName = "db";
        private static string _userName = "login";
        private static string _password = "passs";

        public static string ConnectionString
        {
            get
            {
                return new SqlConnectionStringBuilder()
                {
                    DataSource = _serverName,
                    InitialCatalog = _dbName,
                    UserID = _userName,
                    Password = _password,
                    TrustServerCertificate = true
                }.ConnectionString;
            }
        }

        public static void ChangeConnectionSetting(string serverName, string dbName, string userName, string password)
        {
            _serverName = serverName;
            _dbName = dbName;
            _userName = userName;
            _password = password;
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
            catch (Exception)
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
        public static async Task<object?> GetValueAsync(string sqlQuery)
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
        public async Task UploadCustomerPhotoAsync(string customerLogin, string filePath)
        {
            byte[] fileBytes;

            try
            {
                // Чтение файла и преобразование его в массив байтов
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Подготовка SQL-запроса с параметрами
                    string query = "UPDATE Customers SET Photo = @Photo WHERE Login = @Login";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Photo", fileBytes);
                        command.Parameters.AddWithValue("@Login", customerLogin);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                Console.WriteLine("Фото успешно загружено в базу данных.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке фото: " + ex.Message);
            }
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
