// Api/Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("Default");

// Inyectamos los repositorios
builder.Services.AddScoped(_ => new Datos.Repositorios.PersonaRepositorio(connString));
builder.Services.AddScoped(_ => new Datos.Repositorios.EventoRepositorio(connString));
builder.Services.AddScoped(_ => new Datos.Repositorios.ReservaRepositorio(connString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();