using Common.Enums;

namespace Common.Models.TemplateGenerator
{
    public class TemplateFactory
    {
        public ITemplateGenerator CreateTemplateGenerator(TemplateType templateType)
        {
            switch (templateType)
            {
                case TemplateType.Invoice:
                    return new InvoiceTemplateGenerator();
                default:
                    throw new ArgumentException("Invalid template type");
            }
        }
    }
}
