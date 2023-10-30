using System.Data;

namespace LabWork15
{
    internal class Program
    {
        static async Task Main()
        {
            Task1();
            Task2();

            Console.WriteLine("Проверка подключения...");
            if (!DAL.IsConnected())
            {
                Console.WriteLine("Подключение к БД не удалось.");
                return;
            }
            Console.WriteLine("Успешное подключение");

            //await Task3Async();
            //
            //await Task4Async();
            //
            //await Task5Async();
            
            //await Task6Async();
            
            await Task7Async();
            
            await Task8Async();
        }

        private static void Task1()
        {
            Console.WriteLine("Строка подключения:");
            Console.WriteLine(DAL.ConnectionString);
            Console.WriteLine();
        }

        private static void Task2()
        {
            DAL.ChangeConnectionSetting("prserver\\SQLEXPRESS", "ispp1103", "ispp1103", "1103", false);
            Console.WriteLine("Новая строка подключения:");
            Console.WriteLine(DAL.ConnectionString);
            Console.WriteLine();
        }

        private static async Task Task3Async()
        {
            Console.WriteLine("\n<<Task3>>");

            string sqlQuery = "UPDATE Product SET Price += 1 WHERE Type = 'планшет'";
            try
            {
                int rowsCount = await DAL.ExecuteSqlCommandAsync(sqlQuery);
                Console.WriteLine($"Количество измененных строк: {rowsCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task Task4Async()
        {
            Console.WriteLine("\n<<Task4>>");
            string sqlQuery = "SELECT MAX(Price) FROM Product";
            try
            {
                object? result = await DAL.GetValueAsync(sqlQuery);
                Console.WriteLine($"Результат: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task Task5Async()
        {
            Console.WriteLine("\n<<Task5>>");

            string productType = "планшет";
            decimal newPrice = 2222;

            try
            {
                int rowsAffected = await DAL.UpdateProductPrice(productType, newPrice);
                Console.WriteLine($"Rows affected: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task Task6Async()
        {
            Console.WriteLine("\n<<Task6>>");

            string login = "janedoe";
            string filePath = @$"{Environment.CurrentDirectory}\Imagetgs\img.png";

            try
            {
                await DAL.SetCustomerPhoto(login, filePath);
                Console.WriteLine("Photo uploaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task Task7Async()
        {
            Console.WriteLine("\n<<Task7>>");

            string login = "janedoe";
            string filePath = @$"{Environment.CurrentDirectory}\LoadedImages\img.png";

            try
            {
                await DAL.SaveCustomerPhoto(login, filePath);
                Console.WriteLine("File saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task Task8Async()
        {
            Console.WriteLine("\n<<Task8>>");

            try
            {
                DataTable dataTable = await DAL.GetCustomersWithPhone();
                PrintDataTable(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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