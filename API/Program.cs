using Api.Models;
using API.Data;
using API.HostedServices;
using API.Models;
using API.Models.InvoiceStrategy;
using API.Models.NotificationStrategy;
using API.Models.TimeframeStrategy;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;
using Common.Models;
using Common.Models.TemplateGenerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// DB Config
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Auth
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<DataContext>();


builder.Services.AddScoped(typeof(ICommonRepository<>), typeof(CommonRepository<>));
builder.Services.AddScoped<IConsumptionRepository, ConsumptionRepository>();
builder.Services.AddScoped<IConsumerRepository, ConsumerRepository>();
builder.Services.AddScoped<IInvoicePreferenceRepository, InvoicePreferenceRepository>();
builder.Services.AddScoped<IPriceInfoRepository, PriceInfoRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

builder.Services.AddScoped<IPriceInfoService, PriceInfoService>();
builder.Services.AddScoped<IConsumerService, ConsumerService>();
builder.Services.AddScoped<IConsumptionService, ConsumptionService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddTransient<TimeframeContext>();
builder.Services.AddTransient<TemplateFactory>();

builder.Services.AddTransient<FixedPriceInvoiceStrategy>();
builder.Services.AddTransient<MarketPriceInvoiceStrategy>();
builder.Services.AddTransient<InvoiceStrategyContext>();

builder.Services.AddTransient<EmailNotificationStrategy>();
builder.Services.AddTransient<SmsNotificationStrategy>();
builder.Services.AddTransient<EboksNotificationStrategy>();
builder.Services.AddTransient<PostalNotificationStrategy>();

builder.Services.AddScoped<INotificationStrategyFactory, NotificationStrategyFactory>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddSingleton<IPdfGenerationQueue, PdfGenerationQueue>();
builder.Services.AddSingleton<IPdfGeneratedNotifier, PdfGeneratedNotifier>();

builder.Services.AddHostedService<PdfGenerationService>();
builder.Services.AddHostedService<EventSubscriptionService>();

// Build
var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<AppUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
