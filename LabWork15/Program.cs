using System.Data;

namespace LabWork15
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Task1();
            //Task2();

            Console.WriteLine("Проверка подключения...");
            if (!DAL.IsConnected())
            {
                Console.WriteLine("Подключение к БД не удалось.");
                return;
            }
            Console.WriteLine("Успешное подключение");

            //await Task3(); //change type
            //
            //await Task4();

            await Task5(); //change type

            //await Task6(); // Add Photo to Customer, 
            //
            //await Task7();
            //
            //await Task8();
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

        private static async Task Task3()
        {
            Console.WriteLine("\n<<Task3>>");

            string sqlQuery = "UPDATE Product SET Price += 1 WHERE Type = 'Phone'";
            try
            {
                int rowsAffected = await DAL.ExecuteSqlCommandAsync(sqlQuery);
                Console.WriteLine($"Количество измененных строк: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static async Task Task4()
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

        private static async Task Task5()
        {
            Console.WriteLine("\n<<Task5>>");

            string productType = "Tablet";
            decimal newPrice = 1099;

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

        static async Task Task6()
        {
            Console.WriteLine("\n<<Task6>>");

            string login = "janedoe";
            string filePath = @$"{Environment.CurrentDirectory}\Images\img.png";

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

        private static async Task Task7()
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

        private static async Task Task8()
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