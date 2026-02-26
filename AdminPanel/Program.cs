using MVC.Services.Interfaces;
using MVC.Services;
using AdminPanel.Services.Interfaces;
using AdminPanel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CommonApiService>();
builder.Services.AddScoped<AdminPanel.Services.Interfaces.IInvoiceService, AdminPanel.Services.InvoiceService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<AdminPanel.Services.Interfaces.IConsumerService, AdminPanel.Services.ConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=AdminLogin}/{id?}")
    .WithStaticAssets();


app.Run();
