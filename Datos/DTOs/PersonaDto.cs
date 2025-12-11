namespace Datos.Dtos;

public class PersonaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string TipoPersona { get; set; } = null!;

    // Chef
    public string? Especialidad { get; set; }
    public string? Nacionalidad { get; set; }
    public int? AniosExperiencia { get; set; }

    // Participante
    public string? DocumentoIdentidad { get; set; }
    public string? RestriccionesAlimentarias { get; set; }

    // InvitadoEspecial
    public bool EsVIP { get; set; }
}