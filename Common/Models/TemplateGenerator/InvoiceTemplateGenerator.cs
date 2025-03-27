namespace Common.Models.TemplateGenerator
{
    public class InvoiceTemplateGenerator : ITemplateGenerator
    {
        public string GenerateTemplate()
        {
            return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Regning #0000</title>
        </head>
        <body>
            <h1>Rening #</h1>
            <p>Periode: 25/4-2025 - 25/4-2025 </p>
            <p>Beløb: 500</p>
            <p>Consumer ID: 145465</p>
            <p>Billing Model ID: 1</p>
        </body>
        </html>";
        }
    }
}
