using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filelocker.Domain.Constants
{
    public enum FilelockerEventTypes
    {
        FileUpload = 1000,
        FileDownload = 1001,
        FileSharePrivately = 1002,
        FileSharePublicly = 1003
    }
}
