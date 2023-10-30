using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWork15
{
    public static class DAL
    {
        private static string _serverName = "localhost\\SQLEXPRESS";
        private static string _dbName = "LabWork5";
        private static string _userName = "";
        private static string _password = "";
        private static bool _integratedSecurity = true;

        public static string ConnectionString => new SqlConnectionStringBuilder()
        {
            TrustServerCertificate = true,
            DataSource = _serverName,
            InitialCatalog = _dbName,
            UserID = _userName,
            Password = _password,
            IntegratedSecurity = _integratedSecurity
        }.ConnectionString;

        public static void ChangeConnectionSetting(string serverName, string dbName, string userName, string password, bool integratedSecurity)
        {
            _serverName = serverName;
            _dbName = dbName;
            _userName = userName;
            _password = password;
            _integratedSecurity = integratedSecurity;
        }

        public static bool IsConnected()
        {
            try
            {
                using SqlConnection connection = new(ConnectionString);
                connection.Open();
                return true;
            }
            catch
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
            using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            using SqlCommand command = new(sqlQuery, connection);
            return await command.ExecuteScalarAsync();
        }

        //Task5
        public static async Task<int> UpdateProductPrice(string productType, decimal newPrice)
        {
            string sqlQuery = "UPDATE Product SET Price = @NewPrice WHERE Type = @ProductType";

            using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            using SqlCommand command = new(sqlQuery, connection);
            command.Parameters.AddWithValue("@NewPrice", newPrice);
            command.Parameters.AddWithValue("@ProductType", productType);

            return await command.ExecuteNonQueryAsync();
        }

        //Task6
        public static async Task SetCustomerPhoto(string login, string filePath)
        {
            if (!File.Exists(filePath))
                throw new IOException("Photo not found");
            byte[] photo = File.ReadAllBytes(filePath);

            if (photo.Length > 10240)
                throw new ArgumentOutOfRangeException("Photo is too big");

            string query = "UPDATE Customer SET Photo = @Photo WHERE Login = @Login;";
            using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Login", login);
            command.Parameters.AddWithValue("@Photo", photo);

            await command.ExecuteNonQueryAsync();
        }

        //Task7 
        public static async Task SaveCustomerPhoto(string customerLogin, string filePath)
        {
            using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();
            string query = "SELECT Photo FROM Customer WHERE Login = @Login";
            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Login", customerLogin);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                await reader.ReadAsync();
                byte[] fileData = (byte[])reader["Photo"];
                await File.WriteAllBytesAsync(filePath, fileData);
            }
            else
            {
                throw new Exception("File not found");
            }
        }

        //Task 8
        public static async Task<DataTable> GetCustomersWithPhone()
        {
            string sqlQuery = "SELECT * FROM Customer WHERE PhoneNumber IS NOT NULL";

            using SqlConnection connection = new(ConnectionString);
            await connection.OpenAsync();

            SqlCommand command = new(sqlQuery, connection);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}
