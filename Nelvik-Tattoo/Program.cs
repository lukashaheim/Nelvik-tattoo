using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nelvik_Tattoo.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// DATABASE + IDENTITY
// ---------------------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Login alias "/MyPlace"
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/MyPlace");
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/MyPlace";
});

// Policy for owner
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOnly", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User?.Identity?.IsAuthenticated == true &&
            ctx.User.Identity!.Name == "owner@nelviktattoo.no"));
});

var app = builder.Build();

// ---------------------------
// CREATE DATABASE + SEED USERS
// ---------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<ApplicationDbContext>();
    var env = services.GetRequiredService<IWebHostEnvironment>();

    // Opprett tabeller
    ApplicationDbInitializer.Initialize(db, env);

    // Opprett owner-brukeren
    await OwnerUserSeeder.SeedAsync(services);
}

// ---------------------------
// PIPELINE
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

// FAQ route
app.MapControllerRoute(
    name: "faq",
    pattern: "Faq",
    defaults: new { controller = "Home", action = "Faq" }
);

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

// Blokker Register
app.MapGet("/Identity/Account/Register", () => Results.NotFound());
app.MapPost("/Identity/Account/Register", () => Results.NotFound());

app.Run();
