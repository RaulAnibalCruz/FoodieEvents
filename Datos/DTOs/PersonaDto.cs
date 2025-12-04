// Datos/Dtos/PersonaDto.cs
namespace Datos.Dtos;

public class PersonaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string PersonType { get; set; } = null!;
    
    // Chef
    public string? Specialty { get; set; }
    public string? Nationality { get; set; }
    public int? YearsExperience { get; set; }
    
    // Participante
    public string? IdentityDocument { get; set; }
    public string? DietaryRestrictions { get; set; }
    
    // Invitado especial
    public bool IsVIP { get; set; }
}