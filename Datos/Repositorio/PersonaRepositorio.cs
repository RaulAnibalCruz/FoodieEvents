// Datos/Repositorios/PersonaRepositorio.cs
using Biblioteca;
using Dapper;
using MySqlConnector;
using Datos.Dtos;

namespace Datos.Repositorios;

public class PersonaRepositorio
{
    private readonly string _connectionString;

    public PersonaRepositorio(string connectionString)
        => _connectionString = connectionString;

    public async Task<Persona> ObtenerPorIdAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);

        var dto = await conn.QueryFirstOrDefaultAsync<PersonaDto>(
            "SELECT * FROM Personas WHERE Id = @Id", new { Id = id });

        if (dto == null)
            throw new FoodieEventsException("Persona no encontrada");

        Persona persona = dto.TipoPersona switch
        {
            "Chef" => new Chef(dto.Nombre, dto.Email, dto.Telefono,
                              dto.Especialidad!, dto.Nacionalidad!, dto.AniosExperiencia!.Value),
            "Participante" => new Participante(dto.Nombre, dto.Email, dto.Telefono,
                                              dto.DocumentoIdentidad!, dto.RestriccionesAlimentarias ?? ""),
            "InvitadoEspecial" => new InvitadoEspecial(dto.Nombre, dto.Email, dto.Telefono),
            _ => throw new FoodieEventsException("Tipo de persona desconocido")
        };

        typeof(Persona).GetProperty("Id")!.SetValue(persona, dto.Id);
        return persona;
    }

    // MÉTODO  GUARDAR
    public async Task GuardarAsync(Persona persona)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql;
        var parametros = new
        {
            persona.Nombre,
            persona.Email,
            persona.Telefono,
            Especialidad = (persona as Chef)?.Especialidad,
            Nacionalidad = (persona as Chef)?.Nacionalidad,
            AniosExperiencia = (persona as Chef)?.AniosExperiencia,
            DocumentoIdentidad = (persona as Participante)?.DocumentoIdentidad,
            RestriccionesAlimentarias = (persona as Participante)?.RestriccionesAlimentarias
        };

        if (persona is Chef)
            sql = "INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, Especialidad, Nacionalidad, AniosExperiencia) " +
                  "VALUES (@Nombre, @Email, @Telefono, 'Chef', @Especialidad, @Nacionalidad, @AniosExperiencia); SELECT LAST_INSERT_ID();";
        else if (persona is Participante)
            sql = "INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, DocumentoIdentidad, RestriccionesAlimentarias) " +
                  "VALUES (@Nombre, @Email, @Telefono, 'Participante', @DocumentoIdentidad, @RestriccionesAlimentarias); SELECT LAST_INSERT_ID();";
        else if (persona is InvitadoEspecial)
            sql = "INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, EsVIP) " +
                  "VALUES (@Nombre, @Email, @Telefono, 'InvitadoEspecial', TRUE); SELECT LAST_INSERT_ID();";
        else
            throw new NotSupportedException("Tipo no soportado");

        var id = await conn.ExecuteScalarAsync<int>(sql, parametros);
        typeof(Persona).GetProperty("Id")!.SetValue(persona, id);
    }

    public async Task<List<Persona>> ObtenerTodasAsync()
    {
        using var conn = new MySqlConnection(_connectionString);
        var dtos = await conn.QueryAsync<PersonaDto>("SELECT * FROM Personas");

        var lista = new List<Persona>();
        foreach (var dto in dtos)
        {
            Persona persona = dto.TipoPersona switch
            {
                "Chef" => new Chef(dto.Nombre, dto.Email, dto.Telefono,
                                  dto.Especialidad!, dto.Nacionalidad!, dto.AniosExperiencia!.Value),
                "Participante" => new Participante(dto.Nombre, dto.Email, dto.Telefono,
                                                  dto.DocumentoIdentidad!, dto.RestriccionesAlimentarias ?? ""),
                "InvitadoEspecial" => new InvitadoEspecial(dto.Nombre, dto.Email, dto.Telefono),
                _ => throw new FoodieEventsException("Tipo desconocido")
            };
            typeof(Persona).GetProperty("Id")!.SetValue(persona, dto.Id);
            lista.Add(persona);
        }
        return lista;
    }

    // MÉTODO ELIMINAR
    public async Task EliminarAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        var filas = await conn.ExecuteAsync("DELETE FROM Personas WHERE Id = @Id", new { Id = id });
        if (filas == 0)
            throw new FoodieEventsException("Persona no encontrada para eliminar");
    }
}