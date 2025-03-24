namespace Common.Dtos.Consumer
{
    public class CreateConsumerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CPR { get; set; }
        public int BillingModelId { get; set; }
        public string UserId { get; set; }
    }
}
