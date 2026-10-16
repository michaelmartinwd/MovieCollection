using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using MovieCollection.Components;
using MovieCollection.SeedData;
using MovieCollectionAPI;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionInfrastructure;
using MovieCollectionInfrastructure.Repository;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<MovieCollectionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddTransient<MovieCollectionRepository, MovieCollectionRepository>();
builder.Services.AddTransient<MovieLibraryAPI, MovieLibraryAPI>();
builder.Services.AddTransient<IMovieCollectionService, MovieCollectionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<Seed>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddHttpClient<IClaudeService, ClaudeService>();


builder.Services
    .AddAuthentication("DemoCookie")
    .AddCookie("DemoCookie", options =>
    {
        options.Cookie.Name = "MovieCollection.Demo";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieCollectionDbContext>();
    dbContext.Database.Migrate();
    //await Seed.Initialize(scope);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/demo/start", async (
    HttpContext http,
    IUserService userService) =>
{
    if (http.User.Identity?.IsAuthenticated == true)
    {
        return Results.Redirect("/");
    }

    var user = await userService.CreateDemoUserAsync();

    var claims = new List<Claim>
    {
        new Claim(
            ClaimTypes.NameIdentifier,
            user.Id.ToString())
    };

    var identity = new ClaimsIdentity(
        claims,
        "DemoCookie");

    await http.SignInAsync(
        "DemoCookie",
        new ClaimsPrincipal(identity));

    return Results.Redirect("/");
});

app.Run();
