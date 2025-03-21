using SendTo3PLSFTPApp.Models;

namespace SendTo3PLSFTPApp.Services
{
    public interface IDBRequestHelper
    {
        Task<List<DispatchRequest>> GetSalesOrderDetailsAsync(string salesOrder);
    }
}