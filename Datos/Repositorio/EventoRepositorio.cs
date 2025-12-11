// Datos/Repositorios/EventoRepositorio.cs
using Biblioteca;
using Dapper;
using MySqlConnector;
using Datos.Dtos;

namespace Datos.Repositorios;

public class EventoRepositorio
{
    private readonly string _connectionString;
    private readonly PersonaRepositorio _personaRepo;

    public EventoRepositorio(string connectionString)
    {
        _connectionString = connectionString;
        _personaRepo = new PersonaRepositorio(connectionString);
    }

    public async Task<Evento> ObtenerPorIdCompletoAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);

        var eventoDto = await conn.QueryFirstOrDefaultAsync<EventoDto>(
            "SELECT * FROM Eventos WHERE Id = @Id", new { Id = id });

        if (eventoDto == null) 
            throw new FoodieEventsException("Evento no encontrado");

        var chef = await _personaRepo.ObtenerPorIdAsync(eventoDto.ChefId) as Chef
                   ?? throw new FoodieEventsException("El chef del evento no existe");

        Evento evento = eventoDto.Modalidad switch
        {
            "Presencial" => new EventoPresencial(
                eventoDto.Nombre,
                eventoDto.Descripcion,
                Enum.Parse<TipoEvento>(eventoDto.Tipo),
                eventoDto.FechaInicio,
                eventoDto.FechaFin,
                eventoDto.CapacidadMaxima,
                eventoDto.Precio,
                chef,
                eventoDto.Lugar!),

            "Virtual" => new EventoVirtual(
                eventoDto.Nombre,
                eventoDto.Descripcion,
                Enum.Parse<TipoEvento>(eventoDto.Tipo),
                eventoDto.FechaInicio,
                eventoDto.FechaFin,
                eventoDto.CapacidadMaxima,
                eventoDto.Precio,
                chef,
                eventoDto.UrlStreaming!),

            _ => throw new FoodieEventsException("Modalidad desconocida")
        };

        typeof(Evento).GetProperty("Id")!.SetValue(evento, eventoDto.Id);
        return evento;
    }

    public async Task InsertarAsync(Evento evento)
    {
        using var conn = new MySqlConnection(_connectionString);
        var sql = @"INSERT INTO Eventos 
                    (Nombre, Descripcion, Tipo, FechaInicio, FechaFin, CapacidadMaxima, Precio, 
                     Lugar, UrlStreaming, Modalidad, ChefId)
                    VALUES 
                    (@Nombre, @Descripcion, @Tipo, @FechaInicio, @FechaFin, @CapacidadMaxima, @Precio,
                     @Lugar, @UrlStreaming, @Modalidad, @ChefId);
                    SELECT LAST_INSERT_ID();";

        var parameters = new
        {
            evento.Nombre,
            evento.Descripcion,
            Tipo = evento.Tipo.ToString(),
            evento.FechaInicio,
            evento.FechaFin,
            evento.CapacidadMaxima,
            evento.Precio,
            Lugar = evento is EventoPresencial p ? p.Lugar : null,
            UrlStreaming = evento is EventoVirtual v ? v.UrlStreaming : null,
            Modalidad = evento is EventoPresencial ? "Presencial" : "Virtual",
            ChefId = evento.ChefOrganizador.Id
        };

        var id = await conn.ExecuteScalarAsync<int>(sql, parameters);
        typeof(Evento).GetProperty("Id")!.SetValue(evento, id);
    }

    public async Task<List<EventoDto>> ObtenerTodosAsync()
    {
        using var conn = new MySqlConnection(_connectionString);
        return (await conn.QueryAsync<EventoDto>(
            "SELECT * FROM Eventos ORDER BY FechaInicio DESC")).AsList();
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        var filas = await conn.ExecuteAsync("DELETE FROM Eventos WHERE Id = @Id", new { Id = id });
        if (filas == 0) throw new FoodieEventsException("Evento no encontrado");
    }
}