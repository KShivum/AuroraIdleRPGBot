using AuroraAPi;
using Config.Net;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);


// Sets up config settings
bool configExisted = File.Exists("./config.ini");
IAPISettings settings = new ConfigurationBuilder<IAPISettings>().UseIniFile("./config.ini").Build();
if (!configExisted)
{
    settings.DatabaseHost = "localhost";
    settings.DatabaseUser = "postgres";
    settings.DatabasePassword = "postgres";
    settings.DatabaseName = "postgres";
    Console.Error.WriteLine("Config did not exist");
    Environment.Exit(1);
}

// Add services to the container.
builder.Services.AddDbContext<AuroraContext>(options =>
    options.UseNpgsql(
        $@"Host={settings.DatabaseHost};Username={settings.DatabaseUser};Password={settings.DatabasePassword};Database={settings.DatabaseName}"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();