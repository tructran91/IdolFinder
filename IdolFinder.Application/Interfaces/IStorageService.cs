namespace IdolFinder.CrawData.Services
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(string folderName, string fileName, byte[] data);
        Task<bool> DeleteFileAsync(string folderName, string fileName);
        Task<Stream?> GetFileAsync(string folderName, string fileName);
    }
}
