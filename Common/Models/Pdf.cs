namespace Common.Models
{
    public class Pdf(byte[] file)
    {
        public byte[] File { get; set; } = file;
    }
}
