using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        // Alias: /MyPlace peker til Identity sin login-side
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/MyPlace");
    });


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/MyPlace";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOnly", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User?.Identity?.IsAuthenticated == true &&
            ctx.User.Identity!.Name == "owner@nelviktattoo.no")); // <- eier-epost
});

var app = builder.Build();

// ---------------------------
// Create database correctly
// ---------------------------
using (var scope = app.Services.CreateScope())
await OwnerUserSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

    ApplicationDbInitializer.Initialize(db, env);
}


// ---------------------------
// Pipeline
// ---------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    name: "faq",
    pattern: "Faq",
    defaults: new { controller = "Home", action = "Faq" });

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

// blokker direkte tilgang til register-siden
app.MapGet("/Identity/Account/Register", () => Results.NotFound());
app.MapPost("/Identity/Account/Register", () => Results.NotFound());

// Blokker direkte tilgang til den gamle login-adressen
app.MapGet("/Identity/Account/Login", () => Results.NotFound());
app.MapPost("/Identity/Account/Login", () => Results.NotFound());


app.Run();