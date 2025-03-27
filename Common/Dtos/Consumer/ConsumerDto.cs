using Common.Enums;

namespace Common.Dtos.Consumer
{
    public struct ConsumerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CPR { get; set; }
        public string UserId { get; set; }
        public BillingModelType BillingModel { get; set; }
    }
}
