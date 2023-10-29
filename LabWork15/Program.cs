﻿using System.Data;
using System.Data.SqlClient;

namespace LabWork15
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = DAL.ConnectionString;
            Console.WriteLine("Строка подключения по умолчанию:");
            Console.WriteLine(connectionString);
            Console.WriteLine();

            Console.WriteLine("Изменяем настройки подключения...");
            DAL.ChangeConnectionSettings("новый сервер", "новая БД", "новый логин", "новый пароль");
            connectionString = DAL.ConnectionString;
            Console.WriteLine("Строка подключения после изменения настроек:");
            Console.WriteLine(connectionString);
            Console.WriteLine();

            Console.WriteLine("Проверка подключения к БД...");
            if (DAL.IsConnected())
            {
                Console.WriteLine("Успешное подключение к БД!");
            }
            else
            {
                Console.WriteLine("Подключение к БД не удалось.");
                return;
            }

            await Task3();

            await Task4();

            await Task5();

            await Task6();

            await Task7();

            await Task8();
        }

        private static async Task Task3()
        {
            string sqlQuery = "";
            int rowsAffected = await DAL.ExecuteSqlCommandAsync(sqlQuery);
            Console.WriteLine($"Количество измененных строк: {rowsAffected}");
        }


        private static async Task Task4()
        {
            string sqlQuery = "";
            object? result = await DAL.ExecuteScalarAsync(sqlQuery);
            Console.WriteLine($"Результат выполнения команды: {result}");
        }

        private static async Task Task5()
        {
            string sqlQuery = "";
            string productType = ""; //Введите тип товара для изменения цены
            decimal newPrice = 2; //новая цена товара

            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductType", productType),
                new SqlParameter("@NewPrice", newPrice)
            };

            int rowsAffected = await DAL.ExecuteNonQueryAsync(sqlQuery, parameters);
            Console.WriteLine($"Количество затронутых строк: {rowsAffected}");
        }

        private static async Task Task6()
        {
            string customerLogin = ""; //логин покупателя

            string filePath = ""; // путь к файлу изображения

            try
            {
                await DAL.UploadPhotoToDatabase(customerLogin, filePath);
                Console.WriteLine("Файл успешно загружен в БД.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке файла в БД: " + ex.Message);
            }
        }

        private static async Task Task7()
        {
            string customerLogin = "";
            string filePath = "";
            string sqlQuery = "SELECT Photo FROM Customers WHERE Login = @Login";

            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Login", customerLogin)
                };

                byte[]? fileBytes = await DAL.ExecuteReaderAsync(sqlQuery, parameters);

                if (fileBytes != null)
                {
                    File.WriteAllBytes(filePath, fileBytes);
                    Console.WriteLine("Файл успешно сохранен на ПК пользователя.");
                }
                else
                {
                    Console.WriteLine("Покупатель с указанным логином не найден или у него нет загруженного файла.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении файла: " + ex.Message);
                return;
            }
        }

        private static async Task Task8()
        {
            string sqlQuery = "";
            try
            {
                DataTable dataTable = await DAL.GetCustomersWithPhone(sqlQuery);
                Console.WriteLine("Результат выборки:");
                PrintDataTable(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выполнении выборки: " + ex.Message);
                return;
            }
        }

        private static void PrintDataTable(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    Console.Write(row[col] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}