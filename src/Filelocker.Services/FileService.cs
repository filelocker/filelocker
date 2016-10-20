using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Filelocker.Domain.Interfaces;
using Filelocker.Domain;
using System.Linq;

namespace Filelocker.Services
{
    public class FileService : IFileService
    {
        private IUnitOfWork _unitOfWork;
        private IFileStorageProvider _fileStorageProvider;

        public FileService(IUnitOfWork unitOfWork, IFileStorageProvider fileStorageProvider)
        {
            _unitOfWork = unitOfWork;
            _fileStorageProvider = fileStorageProvider;
        }

        public async Task CreateOrUpdateFileAsync(FilelockerFile file)
        {
            _unitOfWork.FileRepository.Add(file);
            await _unitOfWork.CommitAsync(); //Commit here to get thet File ID
        }

        public Stream GetReadStream(FilelockerFile file)
        {
            var fs = _fileStorageProvider.GetReadStream(file.Id.ToString());
            var decryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(file.EncryptionKey, file.EncryptionSalt.ToByteArray());
            decryptor.Key = pdb.GetBytes(32);
            decryptor.IV = pdb.GetBytes(16);
            var cs = new CryptoStream(fs, decryptor.CreateDecryptor(), CryptoStreamMode.Read);
            return cs;
        }

        public Stream GetWriteStream(string fileName)
        {
            return _fileStorageProvider.GetWriteStream(fileName);
        }

        public async Task DeleteFileAsync(int id)
        {
            var filelockerFile = await _unitOfWork.FileRepository.GetByIdAsync(id);
            if (filelockerFile != null)
            {
                _unitOfWork.FileRepository.Delete(filelockerFile);
            }

            var dbTask = _unitOfWork.CommitAsync();
            var fsTask = _fileStorageProvider.DeleteFileAsync(filelockerFile.Id.ToString());
            await Task.WhenAll(dbTask, fsTask);
        }

        public async Task<FilelockerFile[]> GetFilesByUserIdAsync(int userId)
        {
            var files = await _unitOfWork.FileRepository.FindAsync(f => f.UserId == userId);
            return files.ToArray();
        }

        public async Task<FilelockerFile> GetFileByIdAsync(int id)
        {
            var file = await _unitOfWork.FileRepository.GetByIdAsync(id);
            return file;
        }
    }
}
