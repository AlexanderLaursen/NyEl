using Common.Enums;

namespace Common.Dtos.BillingModel
{
    public class BillingModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BillingModelMethod BillingModelMethod { get; set; }
    }
}
