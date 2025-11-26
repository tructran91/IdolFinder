namespace IdolFinder.CrawData.Services
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(string fileName, byte[] data);
        Task<bool> DeleteFileAsync(string fileName);
        Task<Stream?> GetFileAsync(string fileName);
    }
}
