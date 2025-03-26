using API.Data;
using Common.Enums;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using DatabaseSeeder;
using System.Text.Json;

Console.WriteLine("Starting database seeding...");

const string BASE_URL = "https://localhost:7231";
const string PRICE_DATA = "Data/PriceInfoData.json";
const string CONSUMPTION_FILE_1 = "Data/Consumption_1.json";
const string CONSUMPTION_FILE_2 = "Data/Consumption_2.json";


bool connectionEstablished = false;

var connectionString = "Server=(localdb)\\mssqllocaldb;Database=NyElDb;Trusted_Connection=True";

var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
    .UseSqlServer(connectionString);

var dbContext = new DataContext(optionsBuilder.Options);

Seeder seeder = new Seeder(dbContext);

while (!connectionEstablished)
{
    Console.WriteLine("Connecting to API...");
    connectionEstablished = await seeder.PingApiAsync(BASE_URL);
}

seeder.RegisterUser("foo@bar.com", "String!1", BASE_URL);
var user1 = await seeder.GetUserIdByEmailAsync("foo@bar.com");
var userId1 = user1.UserId;

seeder.RegisterUser("alexander@laursen.com", "String!1", BASE_URL);
var user2 = await seeder.GetUserIdByEmailAsync("alexander@laursen.com");
var userId2 = user2.UserId;

seeder.BillingModel();

seeder.InvoicePreference();

seeder.Consumer();

seeder.ConsumerInvoicePreference();

seeder.SeedPriceInfoFromjson(PRICE_DATA);



seeder.SeedConsumptionReadingsFromFiles(CONSUMPTION_FILE_1, CONSUMPTION_FILE_2, user1.UserId, user2.UserId);

// TODO seed invoices


Console.WriteLine("Database seeding complete. Press any key to exit.");
Console.ReadKey();
