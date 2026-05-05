using Microsoft.EntityFrameworkCore;
using MovieCollection.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionInfrastructure;
using MovieCollectionInfrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddDbContext<MovieCollectionDbContext>(options =>
//    options.UseNpgsql(
//        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContextFactory<MovieCollectionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddTransient<IMovieCollectionService, MovieCollectionService>();
builder.Services.AddTransient<MovieCollectionRepository, MovieCollectionRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieCollectionDbContext>();
    dbContext.Database.Migrate();
    //await SeedData.Initialize(scope);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
