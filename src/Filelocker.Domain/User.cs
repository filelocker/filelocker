using System.Collections;
using System.Collections.Generic;

namespace Filelocker.Domain
{
    public class User : BaseEntity
    {
        public string DisplayName { get; set; }

        public IEnumerable<File> Files { get; set; }
    }
}
