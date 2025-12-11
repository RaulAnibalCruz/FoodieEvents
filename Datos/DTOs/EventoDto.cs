namespace Datos.Dtos;

public class EventoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int CapacidadMaxima { get; set; }
    public decimal Precio { get; set; }
    public string? Lugar { get; set; }
    public string? UrlStreaming { get; set; }
    public string Modalidad { get; set; } = null!;
    public int ChefId { get; set; }
}