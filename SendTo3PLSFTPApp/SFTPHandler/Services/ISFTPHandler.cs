namespace SendTo3PLSFTPApp.Services
{
    public interface ISFTPHandler
    {
        void UploadFile(Stream fileStream, string remoteFilePath);
    }
}