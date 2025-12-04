// Datos/Repositorios/ReservaRepositorio.cs
using Biblioteca;
using Dapper;
using MySqlConnector;

namespace Datos.Repositorios;

public class ReservaRepositorio
{
    private readonly string _connectionString;

    public ReservaRepositorio(string connectionString)
        => _connectionString = connectionString;

    public async Task CrearConValidacionAsync(int personId, int eventId, bool pagado, string metodoPago)
    {
        using var conn = new MySqlConnection(_connectionString);

        var parametros = new
        {
            p_PersonId = personId,
            p_EventId = eventId,
            p_Pagado = pagado,
            p_MetodoPago = metodoPago
        };

        await conn.ExecuteAsync(
            "CrearReserva",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure);
    }

        public async Task<List<dynamic>> ObtenerTodasAsync()
    {
    using var conn = new MySqlConnection(_connectionString);
    return (await conn.QueryAsync(@"
    SELECT r.Id, p.Name AS Persona, e.Name AS Evento, r.ReservationDate, r.Status, r.IsPaid
    FROM Reservations r
    JOIN Persons p ON r.PersonId = p.Id
    JOIN Events e ON r.EventId = e.Id
    ORDER BY r.ReservationDate DESC")).AsList();
    }
    public async Task EliminarAsync(int id)
    {
    using var conn = new MySqlConnection(_connectionString);
    var filas = await conn.ExecuteAsync("DELETE FROM Reservations WHERE Id = @Id", new { Id = id });
    if (filas == 0) throw new FoodieEventsException("Reserva no encontrada");
    }
}