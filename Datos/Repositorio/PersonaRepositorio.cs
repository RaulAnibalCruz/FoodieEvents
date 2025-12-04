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

        if (dto == null) throw new FoodieEventsException("Persona no encontrada");

        return dto.PersonType switch
        {
            "Chef" => new Chef(dto.Nombre, dto.Email, dto.Telefono, 
                              dto.Specialty!, dto.Nationality!, dto.YearsExperience!.Value),
            "Participant" => new Participante(dto.Nombre, dto.Email, dto.Telefono, 
                                            dto.IdentityDocument!, dto.DietaryRestrictions ?? ""),
            "SpecialGuest" => new InvitadoEspecial(dto.Nombre, dto.Email, dto.Telefono),
            _ => throw new FoodieEventsException("Tipo de persona desconocido")
        };
    }

    // Acá irán Insertar, Actualizar, etc.
}