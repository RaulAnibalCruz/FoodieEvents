// Api/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Swagger CORRECTO para .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Inyección de repositorios
var connString = builder.Configuration.GetConnectionString("Default") 
                 ?? throw new InvalidOperationException("Falta connection string");

builder.Services.AddScoped(_ => new Datos.Repositorios.PersonaRepositorio(connString));
builder.Services.AddScoped(_ => new Datos.Repositorios.EventoRepositorio(connString));
builder.Services.AddScoped(_ => new Datos.Repositorios.ReservaRepositorio(connString));

var app = builder.Build();

// Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();