using System.Globalization;
using System.Text;
using CsvHelper;
using Newtonsoft.Json;
using SendTo3PLSFTPApp.Models;

namespace SendTo3PLSFTPApp.Services
{
    public class Sendto3PL : ISendto3PLService
    {
        private readonly IDBRequestHelper _dbRequestHelper;

        public Sendto3PL(IDBRequestHelper dbRequestHelper)
        {
            _dbRequestHelper = dbRequestHelper;
        }

        /// <summary>
        /// This function will take a request body and process it accordingly.
        /// </summary>
        /// <param name="requestBody"></param>
        public async Task SendDispatchAsync(string requestBody)
        {
            // This will handle the logic to get the dispatch request, form the file (separate class) and then send to the sftp (separate class)
            var payload = JsonConvert.DeserializeObject<SendTo3PLRequestPayloadObject>(requestBody);

            // Database Request to get the Rows Associated with this Sales Order
            var dispatchDetails = await _dbRequestHelper.GetSalesOrderDetailsAsync(payload.SalesOrder);

            // Once we have the dispatch details we can now create the CSV file
            var csvFile = GenerateCsv(dispatchDetails);

            // Once we have the file we can send the request to the SFTP
        }

        /// <summary>
        /// This function will take the dispatch details, create the CSV and return it in a string format
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
         private string GenerateCsv(List<DispatchRequest> orderDetails)
        {
            var csvBuilder = new StringBuilder();
            using (var csv = new CsvWriter(new StringWriter(csvBuilder), CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(orderDetails);
            }
            return csvBuilder.ToString();
        }
    }
}