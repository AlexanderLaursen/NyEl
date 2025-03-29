using System.Text.Json;
using System.Text;
using Common.Models;
using API.Data;
using Common.Enums;
using System.Text.Json.Serialization;
using Common.Dtos.User;

namespace DatabaseSeeder
{
    public class Seeder
    {
        private DataContext dbContext;
        public Seeder(DataContext dBContext)
        {
            dbContext = dBContext;
        }

        public async Task<bool> PingApiAsync(string url)
        {
            string pingUrl = url + "/ping";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    Console.WriteLine($"Pinging API at: {pingUrl}");
                    var response = await httpClient.GetAsync(pingUrl);
                    Console.WriteLine($"Ping response status code: {response.StatusCode}");
                    if (response.IsSuccessStatusCode)
                    {

                        Console.WriteLine($"Ping successful!");
                        Console.WriteLine("");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Ping failed.");
                        Console.WriteLine("");
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error pinging API: {ex.Message}");
                    Console.WriteLine("");
                    return false;
                }
            }
        }

        public void RegisterUser(string email, string password, string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var registrationData = new { email = email, password = password };
                    var jsonPayload = JsonSerializer.Serialize(registrationData);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var registerUrl = url + "/register";

                    Console.WriteLine($"Registering user: {email}");
                    var response = httpClient.PostAsync(registerUrl, content).Result;
                    Console.WriteLine($"Registration response status code for {email}: {response.StatusCode}");
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine($"Error: {errorContent}");
                    }
                    Console.WriteLine("");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error registering user {email}: {ex.Message}");
                    Console.WriteLine("");
                }
            }
        }

        public void ConsumerInvoicePreference()
        {
            Console.WriteLine("Checking ConsumerInvoicePreferences...");
            if (!dbContext.ConsumerInvoicePreferences.Any())
            {
                Console.WriteLine("No data found. Adding dummy ConsumerInvoicePreferences...");
                var consumer1 = dbContext.Consumers.FirstOrDefault(c => c.FirstName == "Foo");
                var consumer2 = dbContext.Consumers.FirstOrDefault(c => c.FirstName == "Alexander");
                var emailPreference = dbContext.InvoicePreferences.FirstOrDefault(ip => ip.Name == "Email");
                var smsPreference = dbContext.InvoicePreferences.FirstOrDefault(ip => ip.Name == "Sms");
                var eboksPreference = dbContext.InvoicePreferences.FirstOrDefault(ip => ip.Name == "Eboks");

                if (consumer1 != null && consumer2 != null && emailPreference != null && smsPreference != null && eboksPreference != null)
                {
                    var consumerInvoicePreferences = new List<ConsumerInvoicePreference>
            {
                new ConsumerInvoicePreference { ConsumerId = consumer1.Id, InvoiceNotificationPreferenceId = emailPreference.Id },
                new ConsumerInvoicePreference { ConsumerId = consumer1.Id, InvoiceNotificationPreferenceId = eboksPreference.Id },
                new ConsumerInvoicePreference { ConsumerId = consumer2.Id, InvoiceNotificationPreferenceId = smsPreference.Id },
                new ConsumerInvoicePreference { ConsumerId = consumer2.Id, InvoiceNotificationPreferenceId = eboksPreference.Id }
            };
                    dbContext.ConsumerInvoicePreferences.AddRange(consumerInvoicePreferences);
                    dbContext.SaveChanges();
                    Console.WriteLine("ConsumerInvoicePreferences data added successfully!");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Warning: Could not find required Consumer or InvoicePreference data to create ConsumerInvoicePreferences.");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("Database already contains ConsumerInvoicePreferences!");
                Console.WriteLine("");
            }
        }

        public void Consumer()
        {
            Console.WriteLine("Checking Consumers...");
            if (!dbContext.Consumers.Any())
            {
                Console.WriteLine("No data found. Adding dummy Consumers...");
                var fixedPriceBillingModel = dbContext.BillingModels.FirstOrDefault(b => b.Name == "FixedPrice");
                var hourlyBillingModel = dbContext.BillingModels.FirstOrDefault(b => b.Name == "Hourly");
                var user1 = dbContext.Users.FirstOrDefault(u => u.UserName == "foo@bar.com");
                var user2 = dbContext.Users.FirstOrDefault(u => u.UserName == "alexander@laursen.com");

                if (fixedPriceBillingModel != null && hourlyBillingModel != null && user1 != null && user2 != null)
                {
                    var consumers = new List<Consumer>
            {
                new Consumer
                {
                    FirstName = "Foo",
                    LastName = "Bar",
                    PhoneNumber = "12345678",
                    Email = "foo@bar.com",
                    CPR = 0101901111,
                    BillingModelId = fixedPriceBillingModel.Id,
                    UserId = user1.Id,
                    Address = "Foo Street 1",
                    City = "Bar City",
                    ZipCode = 1234
                },
                new Consumer
                {
                    FirstName = "Alexander",
                    LastName = "Laursen",
                    PhoneNumber = "87654321",
                    Email = "alexander@laursen.com",
                    CPR = 2007955555,
                    BillingModelId = hourlyBillingModel.Id,
                    UserId = user2.Id,
                    Address = "Anders And Vej 28",
                    City = "København",
                    ZipCode = 2450
                }
            };
                    dbContext.Consumers.AddRange(consumers);
                    dbContext.SaveChanges();
                    Console.WriteLine("Consumers data added successfully!");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Warning: Could not find required BillingModel or AppUser data to create Consumers.");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("Database already contains Consumers!");
                Console.WriteLine("");
            }
        }

        public void BillingModel()
        {
            Console.WriteLine("Checking BillingModels...");
            if (!dbContext.BillingModels.Any())
            {
                Console.WriteLine("No data found. Adding dummy data...");
                var billingModels = new List<BillingModel>
        {
            new BillingModel { Name = "FixedPrice", BillingModelType = BillingModelType.FixedPrice },
            new BillingModel { Name = "MarkedPrice", BillingModelType = BillingModelType.MarkedPrice },
        };
                dbContext.BillingModels.AddRange(billingModels);
                dbContext.SaveChanges();
                Console.WriteLine("BillingModels data added successfully!");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Database already contains BillingModels!");
                Console.WriteLine("");
            }
        }

        public void InvoicePreference()
        {
            Console.WriteLine("Checking InvoicePreferences...");
            if (!dbContext.InvoicePreferences.Any())
            {
                Console.WriteLine("No data found. Adding dummy InvoicePreferences...");
                var invoicePreferences = new List<InvoicePreference>
        {
            new InvoicePreference { Name = "Email", InvoicePreferenceType = InvoicePreferenceType.Email },
            new InvoicePreference { Name = "Sms", InvoicePreferenceType = InvoicePreferenceType.Sms },
            new InvoicePreference { Name = "Eboks", InvoicePreferenceType = InvoicePreferenceType.Eboks }
        };
                dbContext.InvoicePreferences.AddRange(invoicePreferences);
                dbContext.SaveChanges();
                Console.WriteLine("InvoicePreferences data added successfully!");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Database already contains InvoicePreferences!");
                Console.WriteLine("");
            }
        }

        public void SeedFixedPrice()
        {
            Console.WriteLine("Checking FixedPriceInfo...");
            if (!dbContext.FixedPriceInfos.Any())
            {
                Console.WriteLine("No data found in FixedPriceInfo. Adding dummy data...");
                DateTime date = DateTime.Parse("2025-01-01T00:00:00.000Z");
                var fixedPriceInfo = new FixedPriceInfo { Timestamp = date, FixedPrice = 3m };

                dbContext.FixedPriceInfos.Add(fixedPriceInfo);
                dbContext.SaveChanges();
                Console.WriteLine("FixedPriceInfo data added successfully!");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Database already contains FixedPriceInfo!");
                Console.WriteLine("");
            }
        }

        public void SeedPriceInfoFromjson(string priceData)
        {
            Console.WriteLine("Checking PriceInfo...");
            if (!dbContext.PriceInfos.Any())
            {
                Console.WriteLine($"No data found in PriceInfo. Reading data from '{priceData}'...");
                try
                {
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    string fullPath = Path.Combine(basePath, priceData);

                    if (File.Exists(fullPath))
                    {
                        string jsonString = File.ReadAllText(fullPath);
                        var rawPriceInfos = JsonSerializer.Deserialize<List<RawPriceInfo>>(jsonString);

                        if (rawPriceInfos != null && rawPriceInfos.Any())
                        {
                            Console.WriteLine($"Read {rawPriceInfos.Count} raw price entries from the file. Processing and filtering...");

                            var uniquePriceInfos = new HashSet<PriceInfo>(new PriceInfoComparer());

                            foreach (var rawInfo in rawPriceInfos)
                            {
                                if (DateTime.TryParse(rawInfo.Date, out DateTime timestamp))
                                {
                                    uniquePriceInfos.Add(new PriceInfo { Timestamp = timestamp, PricePerKwh = rawInfo.Price });
                                }
                                else
                                {
                                    Console.WriteLine($"Warning: Could not parse date '{rawInfo.Date}'. Skipping entry.");
                                }
                            }

                            Console.WriteLine($"Found {uniquePriceInfos.Count} unique price entries after processing and filtering.");

                            if (uniquePriceInfos.Any())
                            {
                                Console.WriteLine("Adding unique PriceInfo data to the database...");
                                dbContext.PriceInfos.AddRange(uniquePriceInfos);
                                dbContext.SaveChanges();
                                Console.WriteLine("Unique PriceInfo data added successfully from JSON file!");
                                Console.WriteLine("");
                            }
                            else
                            {
                                Console.WriteLine("No unique PriceInfo data found after processing and filtering.");
                                Console.WriteLine("");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No valid raw PriceInfo data found in '{fullPath}'.");
                            Console.WriteLine("");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: The file '{fullPath}' was not found.");
                        Console.WriteLine("");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: Could not deserialize JSON from '{priceData}'. Please ensure the file format is correct. Error: {ex.Message}");
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred while seeding PriceInfo: {ex.Message}");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("Database already contains PriceInfo!");
                Console.WriteLine("");
            }
        }

        public class PriceInfoComparer : IEqualityComparer<PriceInfo>
        {
            public bool Equals(PriceInfo x, PriceInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
                return x.Timestamp == y.Timestamp;
            }

            public int GetHashCode(PriceInfo obj)
            {
                return obj.Timestamp.GetHashCode();
            }
        }

        public class RawPriceInfo
        {
            [JsonPropertyName("date")]
            public string Date { get; set; }

            [JsonPropertyName("price")]
            public decimal Price { get; set; }
        }

        public void SeedConsumptionReadingsFromFiles(string file1, string file2)
        {
            Console.WriteLine("Checking ConsumptionReadings...");
            if (!dbContext.ConsumptionReadings.Any())
            {
                Console.WriteLine("No data found in ConsumptionReadings. Reading data from files...");

                var allConsumptionReadings = new List<ConsumptionReading>();

                var consumer1 = dbContext.Consumers.FirstOrDefault(c => c.FirstName == "Foo");
                var consumer2 = dbContext.Consumers.FirstOrDefault(c => c.FirstName == "Alexander");

                // Read from Consumption_1.json
                ReadConsumptionFile(file1, allConsumptionReadings, consumer1.Id);

                // Read from Consumption_2.json
                ReadConsumptionFile(file2, allConsumptionReadings, consumer2.Id);

                if (allConsumptionReadings.Any())
                {
                    Console.WriteLine($"Read {allConsumptionReadings.Count} consumption readings from files. Adding to database...");
                    dbContext.ConsumptionReadings.AddRange(allConsumptionReadings);
                    dbContext.SaveChanges();
                    Console.WriteLine("ConsumptionReadings data added successfully from JSON files!");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("No valid consumption readings found in the specified files.");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("Database already contains ConsumptionReadings!");
                Console.WriteLine("");
            }
        }

        static void ReadConsumptionFile(string relativePath, List<ConsumptionReading> consumptionReadingsList, int consumerId)
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = Path.Combine(basePath, relativePath);

                if (File.Exists(fullPath))
                {
                    string jsonString = File.ReadAllText(fullPath);
                    var readingsFromFile = JsonSerializer.Deserialize<List<ConsumptionReading>>(jsonString);

                    if (readingsFromFile != null && readingsFromFile.Any())
                    {
                        Console.WriteLine($"Read {readingsFromFile.Count} consumption readings from '{relativePath}'.");
                        foreach (var reading in readingsFromFile)
                        {
                            reading.ConsumerId = consumerId;
                        }
                        consumptionReadingsList.AddRange(readingsFromFile);
                    }
                    else
                    {
                        Console.WriteLine($"No valid consumption readings found in '{relativePath}'.");
                    }
                }
                else
                {
                    Console.WriteLine($"Warning: The file '{relativePath}' was not found.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{relativePath}' was not found.");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: Could not deserialize JSON from '{relativePath}'. Please ensure the file format is correct. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while reading '{relativePath}': {ex.Message}");
            }
        }

        public async Task<UserIdDto> GetUserIdByEmailAsync(string email)
        {
            string baseUrl = "https://localhost:7231/api/v1/users";
            string requestUrl = $"{baseUrl}/{email}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var userIdDto = JsonSerializer.Deserialize<UserIdDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        return userIdDto;
                    }
                    else
                    {
                        Console.WriteLine($"Error: HTTP request failed with status code {response.StatusCode}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: HTTP request exception: {ex.Message}");
                    return null;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error: JSON deserialization exception: {ex.Message}");
                    return null;
                }

            }
        }
    }
}