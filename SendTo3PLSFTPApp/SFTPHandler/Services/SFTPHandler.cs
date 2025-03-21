using Renci.SshNet;

namespace SendTo3PLSFTPApp.Services
{
    public class SftpHandler : ISFTPHandler
    {
        private readonly ILogger<Sendto3PL> _logger;
        private readonly string _sftpHost;
        private readonly string _sftpUsername;
        private readonly string _sftpPassword;
        private readonly int _sftpPort;

        public SftpHandler(string host, string username, string password, int port = 22, ILogger<Sendto3PL> logger)
        {
            _sftpHost = host;
            _sftpUsername = username;
            _sftpPassword = password;
            _sftpPort = port;
            _logger = logger;
        }

        public void UploadFile(Stream fileStream, string remoteFilePath)
        {
            // Should handle the logic to take the file (most likely in memory) and uplaod to the SFTP server
            using (var sftp = new SftpClient(_sftpHost, _sftpPort, _sftpUsername, _sftpPassword))
            {
                try
                {
                    sftp.Connect();
                    using (var reader = new StreamReader(fileStream))
                    {
                        var csvContent = reader.ReadToEnd();
                        using (var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent)))
                        {
                            sftp.UploadFile(memoryStream, remoteFilePath);
                        }
                    }
                }
            catch (SshConnectionException ex)
            {
                _logger.LogError($"SFTP Connection Error: {ex.Message}");
                throw new Exception("Failed to connect to SFTP server.", ex);
            }
            catch (SftpPathNotFoundException ex)
            {
                _logger.LogError($"Invalid SFTP Path: {ex.Message}");
                throw new Exception("Invalid remote file path on SFTP server.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Error: {ex.Message}");
                throw new Exception("An error occurred while uploading the file.", ex);
            }
            finally
            {
                if (sftp.IsConnected)
                {
                    sftp.Disconnect();
                }
            }
        }
    }
}