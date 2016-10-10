namespace Filelocker.Domain
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
    }
}
