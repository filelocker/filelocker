using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filelocker.Domain.Interfaces
{
    public interface IShareService
    {
        Task ShareFilePrivatelyAsync(int fileId, IEnumerable<int> userIds);

        Task<PrivateFileShare[]> GetPrivateSharesAsync(int fileId);
    }
}