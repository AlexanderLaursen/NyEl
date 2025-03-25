using Common.Enums;

namespace Common.Dtos.BillingModel
{
    public record struct BillingModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BillingModelMethod BillingModelMethod { get; set; }
    }
}
