using System.Data.SqlClient;
using SendTo3PLSFTPApp.Models;

namespace SendTo3PLSFTPApp.Services
{
    public class DBRequestHelper : IDBRequestHelper
    {
            // Method to query the database for SalesOrder details
        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        public async Task<List<DispatchRequest>> GetSalesOrderDetailsAsync(string salesOrder)
        {
            var dispatchDetails = new List<DispatchRequest>();

            string connectionString = "YourDatabaseConnectionString"; // Replace with your DB connection string
            string query = "SELECT ProductName, Quantity, Price FROM SalesOrderDetails WHERE SalesOrder = @SalesOrder";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SalesOrder", salesOrder);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var orderDetail = new DispatchRequest
                            {
                                //Add Request Details from DB
                            };
                            dispatchDetails.Add(orderDetail);
                        }
                    }
                }
            }

            return dispatchDetails;
        }
    }
}