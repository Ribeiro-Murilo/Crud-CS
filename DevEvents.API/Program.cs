using DevEvents.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DevEvents.API.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("DevEventsCs");

//builder.Services.AddDbContext <DevEventsDbContext>(o=> o.UseInMemoryDatabase("DevEventsDb"));
//builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseSqlServer(connectionString));
// builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseInMemoryDatabase("DevEventsDb"));

var connectionString = builder.Configuration.GetConnectionString("DevEventsCs");

builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseSqlServer(connectionString));

//não necessario por o .Assembly
builder.Services.AddAutoMapper(typeof(DevEventsProfile).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevEvents.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name= "Murilo",
            Email = "muriloribeiro@gmail.com",
            Url = new Uri("https://google.com")
        }
    });

    var xmlFile = "DevEvents.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
