using System;

namespace Filelocker.Domain
{
    public class PrivateFileShare : BaseEntity
    {
        public int FilelockerFileId { get; set; }

        public int ShareTargetId { get; set; }

        public DateTime Expiration { get; set; }
    }
}
