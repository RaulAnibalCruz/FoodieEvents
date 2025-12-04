// Datos/Dtos/EventoDto.cs
namespace Datos.Dtos;

public class EventoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxCapacity { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public string? StreamUrl { get; set; }
    public string Modality { get; set; } = null!;
    public int ChefId { get; set; }
}