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
            "SELECT * FROM Persons WHERE Id = @Id", new { Id = id });

        if (dto == null)
            throw new FoodieEventsException("Persona no encontrada");

        Persona persona = dto.PersonType switch
        {
            "Chef" => new Chef(dto.Nombre, dto.Email, dto.Telefono,
                              dto.Specialty!, dto.Nationality!, dto.YearsExperience!.Value),
            "Participant" => new Participante(dto.Nombre, dto.Email, dto.Telefono,
                                            dto.IdentityDocument!, dto.DietaryRestrictions ?? ""),
            "SpecialGuest" => new InvitadoEspecial(dto.Nombre, dto.Email, dto.Telefono),
            _ => throw new FoodieEventsException("Tipo de persona desconocido")
        };

        typeof(Persona).GetProperty("Id")!.SetValue(persona, dto.Id);
        return persona;
    }

    public async Task GuardarAsync(Persona persona)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql;
        var parametros = new
        {
            Nombre = persona.Nombre,
            Email = persona.Email,
            Telefono = persona.Telefono,
            Especialidad = (persona as Chef)?.Especialidad,
            Nacionalidad = (persona as Chef)?.Nacionalidad,
            AñosExperiencia = (persona as Chef)?.AñosExperiencia,
            DocumentoIdentidad = (persona as Participante)?.DocumentoIdentidad,
            RestriccionesAlimentarias = (persona as Participante)?.RestriccionesAlimentarias
        };

        // SWITCH CLÁSICO → 100% compatible y sin CS8506
        if (persona is Chef)
        {
            sql = @"
                INSERT INTO Persons (Name, Email, Phone, PersonType, Specialty, Nationality, YearsExperience)
                VALUES (@Nombre, @Email, @Telefono, 'Chef', @Especialidad, @Nacionalidad, @AñosExperiencia);
                SELECT LAST_INSERT_ID();";
        }
        else if (persona is Participante)
        {
            sql = @"
                INSERT INTO Persons (Name, Email, Phone, PersonType, IdentityDocument, DietaryRestrictions)
                VALUES (@Nombre, @Email, @Telefono, 'Participant', @DocumentoIdentidad, @RestriccionesAlimentarias);
                SELECT LAST_INSERT_ID();";
        }
        else if (persona is InvitadoEspecial)
        {
            sql = @"
                INSERT INTO Persons (Name, Email, Phone, PersonType, IsVIP)
                VALUES (@Nombre, @Email, @Telefono, 'SpecialGuest', TRUE);
                SELECT LAST_INSERT_ID();";
        }
        else
        {
            throw new NotSupportedException("Tipo de persona no soportado");
        }

        var id = await conn.ExecuteScalarAsync<int>(sql, parametros);
        typeof(Persona).GetProperty("Id")!.SetValue(persona, id);
    }

    public async Task<List<Persona>> ObtenerTodasAsync()
    {
        using var conn = new MySqlConnection(_connectionString);
        var dtos = await conn.QueryAsync<PersonaDto>("SELECT * FROM Persons");

        var lista = new List<Persona>();

        foreach (var dto in dtos)
        {
            Persona persona = dto.PersonType switch
            {
                "Chef" => new Chef(dto.Nombre, dto.Email, dto.Telefono,
                                  dto.Specialty!, dto.Nationality!, dto.YearsExperience!.Value),
                "Participant" => new Participante(dto.Nombre, dto.Email, dto.Telefono,
                                                dto.IdentityDocument!, dto.DietaryRestrictions ?? ""),
                "SpecialGuest" => new InvitadoEspecial(dto.Nombre, dto.Email, dto.Telefono),
                _ => throw new FoodieEventsException("Tipo desconocido")
            };

            typeof(Persona).GetProperty("Id")!.SetValue(persona, dto.Id);
            lista.Add(persona);
        }

        return lista;
    }

    public async Task EliminarAsync(int id)
    {
        using var conn = new MySqlConnector.MySqlConnection(_connectionString);
        var filas = await conn.ExecuteAsync("DELETE FROM Persons WHERE Id = @Id", new { Id = id });
        if (filas == 0)
            throw new FoodieEventsException("Persona no encontrada para eliminar");
    }
}