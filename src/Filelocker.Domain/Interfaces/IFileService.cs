using System.IO;
using System.Threading.Tasks;

namespace Filelocker.Domain.Interfaces
{
    public interface IFileService
    {
        Task DeleteFileAsync(int id);

        Stream GetReadStream(FilelockerFile file);

        Stream GetWriteStream(string fileName);

        Task CreateOrUpdateFileAsync(FilelockerFile file);

        Task<FilelockerFile[]> GetFilesByUserIdAsync(int userId);

        Task<FilelockerFile> GetFileByIdAsync(int id);

    }
}