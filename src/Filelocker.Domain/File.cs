namespace Filelocker.Domain
{
    public class FilelockerFile : BaseEntity
    {
        public string Name { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
    }
}
