using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendTo3PLSFTPApp.Services;

namespace SendTo3PLSFTPApp
{
    public class Sendto3PL
    {
        private readonly ILogger<Sendto3PL> _logger;
        private readonly ISendto3PLService _sendTo3PL;

        public Sendto3PL(ILogger<Sendto3PL> logger, ISendto3PLService sendTo3PL)
        {
            _logger = logger;
            _sendTo3PL = sendTo3PL;
        }

        [Function("Sendto3PL")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Starting Upload via SFTP: " + req);
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            _sendTo3PL.SendDispatchAsync(requestBody);
            _logger.LogInformation("Finished Upload via SFTP: " + req);
        }
    }
}
