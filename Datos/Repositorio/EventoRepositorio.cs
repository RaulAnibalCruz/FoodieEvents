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
            "SELECT * FROM Events WHERE Id = @Id", new { Id = id });

        if (eventoDto == null) throw new FoodieEventsException("Evento no encontrado");

        var chef = await _personaRepo.ObtenerPorIdAsync(eventoDto.ChefId) as Chef
                   ?? throw new FoodieEventsException("El chef del evento no existe");

        Evento evento = eventoDto.Modality switch
        {
            "Presencial" => new EventoPresencial(
                eventoDto.Nombre,
                eventoDto.Descripcion,
                Enum.Parse<TipoEvento>(eventoDto.Type),
                eventoDto.StartDate,
                eventoDto.EndDate,
                eventoDto.MaxCapacity,
                eventoDto.Price,
                chef,
                eventoDto.Location!),

            "Virtual" => new EventoVirtual(
                eventoDto.Nombre,
                eventoDto.Descripcion,
                Enum.Parse<TipoEvento>(eventoDto.Type),
                eventoDto.StartDate,
                eventoDto.EndDate,
                eventoDto.MaxCapacity,
                eventoDto.Price,
                chef,
                eventoDto.StreamUrl!),

            _ => throw new FoodieEventsException("Modalidad desconocida")
        };

        return evento;
    }

    public async Task InsertarAsync(Evento evento)
    {
        using var conn = new MySqlConnection(_connectionString);
        var sql = @"INSERT INTO Events 
                    (Name, Description, Type, StartDate, EndDate, MaxCapacity, Price, 
                     Location, StreamUrl, Modality, ChefId)
                    VALUES 
                    (@Name, @Description, @Type, @StartDate, @EndDate, @MaxCapacity, @Price,
                     @Location, @StreamUrl, @Modality, @ChefId);
                    SELECT LAST_INSERT_ID();";

        var parameters = new
        {
            evento.Nombre,
            evento.Descripcion,
            Type = evento.Tipo.ToString(),
            evento.FechaInicio,
            evento.FechaFin,
            evento.CapacidadMaxima,
            evento.Precio,
            Location = evento is EventoPresencial p ? p.Lugar : null,
            StreamUrl = evento is EventoVirtual v ? v.UrlStreaming : null,
            Modality = evento is EventoPresencial ? "Presencial" : "Virtual",
            ChefId = evento.ChefOrganizador.Id
        };

        var id = await conn.ExecuteScalarAsync<int>(sql, parameters);
        // Reflexión para setear el Id (porque está protected)
        typeof(Evento).GetProperty("Id")!.SetValue(evento, id);
    }
}