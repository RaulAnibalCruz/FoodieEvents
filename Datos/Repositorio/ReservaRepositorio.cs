// Datos/Repositorios/ReservaRepositorio.cs
using Biblioteca;
using Dapper;
using MySqlConnector;
using System.Data;

namespace Datos.Repositorios;

public class ReservaRepositorio
{
    private readonly string _connectionString;

    public ReservaRepositorio(string connectionString)
        => _connectionString = connectionString;

    public async Task CrearConValidacionAsync(int personaId, int eventoId, bool pagada, string metodoPago)
    {
        using var conn = new MySqlConnection(_connectionString);

        await conn.ExecuteAsync(
            "CrearReserva",
            new { p_PersonaId = personaId, p_EventoId = eventoId, p_Pagada = pagada, p_MetodoPago = metodoPago },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<List<dynamic>> ObtenerTodasAsync()
    {
        using var conn = new MySqlConnection(_connectionString);
        return (await conn.QueryAsync(@"
            SELECT r.Id, 
                   p.Nombre AS Persona, 
                   e.Nombre AS Evento, 
                   r.FechaReserva, 
                   r.Estado, 
                   r.Pagada
            FROM Reservas r
            JOIN Personas p ON r.PersonaId = p.Id
            JOIN Eventos e ON r.EventoId = e.Id
            ORDER BY r.FechaReserva DESC")).AsList();
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        var filas = await conn.ExecuteAsync("DELETE FROM Reservas WHERE Id = @Id", new { Id = id });
        if (filas == 0) throw new FoodieEventsException("Reserva no encontrada");
    }
}