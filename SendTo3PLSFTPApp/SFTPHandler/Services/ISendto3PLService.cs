namespace SendTo3PLSFTPApp.Services
{
    public interface ISendto3PLService
    {
        Task SendDispatchAsync(string requestBody);
    }
}