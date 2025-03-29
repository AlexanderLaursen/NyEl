namespace Common.Models.TemplateGenerator
{
    public interface ITemplateGenerator
    {
        string GenerateTemplate(Invoice invoice, Consumer consumer);
    }
}
