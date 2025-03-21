using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using SendTo3PLSFTPApp.Services;
using Renci.SshNet.Common;

namespace SendTo3PLSFTPApp.Tests
{
    [TestFixture]
    public class SftpHandlerTests
    {
        private Mock<ILogger<Sendto3PL>> _loggerMock;
        private SftpHandler _sftpHandler;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<Sendto3PL>>();
            _sftpHandler = new SftpHandler("sftp.example.com", "username", "password", 22, _loggerMock.Object);
        }

        private static Stream CreateTestCsvStream()
        {
            var csvContent = "column1,column2\nvalue1,value2";
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent));
        }

        [Test]
        public void UploadFile_ThrowsException_WhenConnectionFails()
        {
            // Arrange
            var fileStream = CreateTestCsvStream();

            var sftpMock = new Mock<SftpClient>("sftp.example.com", 22, "username", "password")
            {
                CallBase = true
            };
            sftpMock.Setup(s => s.Connect()).Throws(new SshConnectionException("Connection failed"));

            _loggerMock.Setup(x => x.LogError(It.IsAny<string>()));

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _sftpHandler.UploadFile(fileStream, "/remote/path/test.csv"));
            Assert.That(ex.Message, Is.EqualTo("Failed to connect to SFTP server."));
        }

        [Test]
        public void UploadFile_ThrowsException_WhenPathIsInvalid()
        {
            // Arrange
            var fileStream = CreateTestCsvStream();

            var sftpMock = new Mock<SftpClient>("sftp.example.com", 22, "username", "password")
            {
                CallBase = true
            };
            sftpMock.Setup(s => s.UploadFile(It.IsAny<Stream>(), It.IsAny<string>()))
                    .Throws(new SftpPathNotFoundException("Invalid path"));

            _loggerMock.Setup(x => x.LogError(It.IsAny<string>()));

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _sftpHandler.UploadFile(fileStream, "/invalid/path/test.csv"));
            Assert.That(ex.Message, Is.EqualTo("Invalid remote file path on SFTP server."));
        }

        [Test]
        public void UploadFile_LogsError_OnGeneralException()
        {
            // Arrange
            var fileStream = CreateTestCsvStream();

            var sftpMock = new Mock<SftpClient>("sftp.example.com", 22, "username", "password")
            {
                CallBase = true
            };
            sftpMock.Setup(s => s.UploadFile(It.IsAny<Stream>(), It.IsAny<string>()))
                    .Throws(new Exception("Unexpected error"));

            _loggerMock.Setup(x => x.LogError(It.IsAny<string>()));

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _sftpHandler.UploadFile(fileStream, "/remote/path/test.csv"));
            Assert.That(ex.Message, Is.EqualTo("An error occurred while uploading the file."));
        }
    }
}
