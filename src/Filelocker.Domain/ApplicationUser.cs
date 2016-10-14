using System.Collections;
using System.Collections.Generic;

namespace Filelocker.Domain
{
    public class ApplicationUser : BaseEntity
    {
        public string DisplayName { get; set; }

        public IEnumerable<FilelockerFile> Files { get; set; }
    }
}
