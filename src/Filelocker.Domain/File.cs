namespace Filelocker.Domain
{
    public class File : BaseEntity
    {
        public string Name { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
    }
}
