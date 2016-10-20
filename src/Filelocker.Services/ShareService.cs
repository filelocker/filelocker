using System;
using System.IO;
using System.Threading.Tasks;

using Filelocker.Domain.Interfaces;
using Filelocker.Domain;
using System.Linq;
using System.Collections.Generic;
using Filelocker.DataAccess.Extensions;

namespace Filelocker.Services
{
    public class ShareService : IShareService
    {
        private IUnitOfWork _unitOfWork;

        public ShareService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivateFileShare[]> GetPrivateSharesAsync(int fileId)
        {
            var file = await _unitOfWork.FileRepository.GetByIdWithSharesAsync(fileId);
            if (file != null)
            {
                return file.PrivateFileShares.ToArray();
            }
            else
            {
                throw new Exception("File record not found");
            }
        }

        public async Task ShareFilePrivatelyAsync(int fileId, IEnumerable<int> userIds)
        {
            var file = await _unitOfWork.FileRepository.GetByIdWithSharesAsync(fileId);
            if (file != null)
            {
                foreach(var userId in userIds)
                {
                    if (file.PrivateFileShares.Any(s => s.ShareTargetId == userId)) continue; //Already shared
                    var privateShare = new PrivateFileShare()
                    {
                        FilelockerFileId = fileId,
                        ShareTargetId = userId
                    };
                    _unitOfWork.PrivateFileShareRepository.Add(privateShare);
                }
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("File record not found");
            }
        }
        
    }
}
