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
}