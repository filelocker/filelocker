using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Filelocker.Domain.Interfaces;
using FilelockerFile = Filelocker.Domain.FilelockerFile;

namespace Filelocker.Services
{
    public class FileService
    {
        private IUnitOfWork _unitOfWork;
        private IFileStorageProvider _fileStorageProvider;

        public FileService(IUnitOfWork unitOfWork, IFileStorageProvider fileStorageProvider)
        {
            _unitOfWork = unitOfWork;
            _fileStorageProvider = fileStorageProvider;
        }

        public Stream GetStream(string fileName)
        {
            return _fileStorageProvider.GetStream(fileName);
        } 

        public async Task CreateFileAsync(Stream fileStream, FilelockerFile file)
        {
            try
            {
                _unitOfWork.FileRepository.Add(file);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
