using System;
using API.Models;
using API.Services.Interfaces;
using Common.Enums;
using Common.Models;

namespace Api.Models
{
    public class PdfInvoiceJob : PdfJob
    {

        public Invoice Invoice { get; set; }
        public Consumer Consumer { get; set; }

        public override HtmlContent GenerateHtml()
        {
            string Paid = Invoice.Paid ? "Ja" : "Nej";

            string table = "";

            if (Invoice.InvoicePeriodData != null)
            {
                foreach (var period in Invoice.InvoicePeriodData)
                {
                    table += $@"
                <tr>
                    <td>{period.PeriodStart.ToString("dd/MM-yyyy")}</td>
                    <td>{period.PeriodEnd.ToString("dd/MM-yyyy")}</td>
                    <td>{period.Consumption.ToString("N2")}</td>
                    <td>kr {period.Cost.ToString("N2")}</td>
                </tr>";
                }
            }

            string billingMethodDisplay = Invoice.BillingModel?.BillingModelType == BillingModelType.FixedPrice ? "Fast pris" : "Variabel pris";

            string content = $@"
        <!DOCTYPE html>
<html lang=""da"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Faktura #{Invoice.Id}</title>
    <style>
        body {{
            font-family: sans-serif;
            line-height: 1.6;
            margin: 20px;
        }}
        .invoice-container {{
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            position: relative;
        }}
        .invoice-header {{
            text-align: center;
            margin-bottom: 20px;
        }}
        .logo {{
        position: absolute;
        top: 20px;
        right: 0px;
        max-width: 150px;
        height: auto;
        .info-container {{
        display: flex;
        justify-content: space-between;
        .consumer-info, .invoice-info {{
            margin-bottom: 15px;
        }}
        .info-line {{
            margin-bottom: 5px;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }}
        th, td {{
            border: 1px solid #ccc;
            padding: 8px;
            text-align: left;
        }}
        .total-amount {{
            font-weight: bold;
            text-align: right;
        }}
    </style>
</head>
<body>
<div class=""invoice-container"">
<img src=""Src/logo.png"" alt=""Logo"" class=""logo"">
    <div class=""invoice-container"">
        <div class=""invoice-header"">
            <h1>Faktura</h1>
            <p>Faktura #{Invoice.Id}</p>
        </div>

        <div class=""consumer-info"">
            <h2>Kundeinformation</h2>
            <div class=""info-line"">
                <label>Navn:</label> {Consumer.FirstName} {Consumer.LastName}
            </div>
            <div class=""info-line"">
                <label>Adresse:</label> {Consumer.Address}
            </div>
            <div class=""info-line"">
                <label>By:</label> {Consumer.City}, {Consumer.ZipCode}
            </div>
            <div class=""info-line"">
                <label>Telefonnummer:</label> {Consumer.PhoneNumber}
            </div>
            <div class=""info-line"">
                <label>E-mail:</label> {Consumer.Email}
            </div>
            <div class=""info-line"">
                <label>CPR-nummer:</label> {Consumer.CPR}
            </div>
            <div class=""info-line"">
                <label>Kundenummer:</label> {Consumer.Id}
            </div>
        </div>

        <div class=""invoice-info"">
            <h2>Fakturaoplysninger</h2>
            <div class=""info-line"">
                <label>Faktureringsperiode: {Invoice.BillingPeriodStart.ToString("dd/MM-yyyy")} - {Invoice.BillingPeriodEnd.ToString("dd/MM-yyyy")}</label> 
            </div>
            <div class=""info-line"">
                <label>Total forbrug:</label> {Invoice.TotalConsumption.ToString("N2")} kWh
            </div>
            <div class=""info-line"">
                <label>Betalingsmodel:</label> {billingMethodDisplay}
            </div>
            <div class=""info-line"">
                <label>Betalt:</label> {Paid}
            </div>
        </div>

        <div>
            <h2>Periodedata</h2>
            <table>
                <thead>
                    <tr>
                        <th>Periode Start</th>
                        <th>Periode Slut</th>
                        <th>Forbrug</th>
                        <th>Beløb</th>
                    </tr>
                </thead>
                <tbody>
                    {table}
                    </tbody>
            </table>
        </div>

        <div class=""total-amount"">
            <label>Total Beløb:</label> kr {Invoice.TotalAmount.ToString("N2")}
        </div>

        <div style=""margin-top: 20px; font-size: small;"">
            <p>Prisdata er hentet fra https://stromligning.dk/api/docs/. Forbrugsdata er autogeneret.</p>
        </div>
    </div>
</div>
</body>
</html>";

            return new HtmlContent(content);
        }
    }
}
