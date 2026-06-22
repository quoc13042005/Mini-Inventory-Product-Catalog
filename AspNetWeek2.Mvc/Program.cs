using AspNetWeek2.Mvc.Data;
using AspNetWeek2.Mvc.Options;
using AspNetWeek2.Mvc.Repositories;
using AspNetWeek2.Mvc.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Step 12: Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lab05-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddControllersWithViews();

// Step 14: ProblemDetails
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["timestamp"] = DateTimeOffset.UtcNow;
    };
});

// Step 13: Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Application is running."), tags: new[] { "live" })
    .AddDbContextCheck<AppDbContext>("database", tags: new[] { "ready" });

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Step 11: Exception Handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseStaticFiles();
app.UseRouting();

// Map Health Checks
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

// Map API Error Demo
app.MapGet("/api/products/{id:int}", async (int id, AppDbContext db, HttpContext http) =>
{
    var product = await db.Products.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    if (product == null || product.IsDeleted)
    {
        return Results.Problem(
            type: "https://example.com/problems/product-not-found",
            title: "Product not found",
            detail: $"The product with id {id} was not found.",
            statusCode: StatusCodes.Status404NotFound,
            instance: http.Request.Path);
    }
    return Results.Ok(product);
});

app.MapDefaultControllerRoute();

app.Run();
