// Api/Controllers/EventosController.cs
using Biblioteca;
using Datos.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/eventos")]
public class EventosController : ControllerBase
{
    private readonly EventoRepositorio _repo;

    public EventosController(EventoRepositorio repo)
        => _repo = repo;

    [HttpPost]
    public async Task<ActionResult> CrearEvento([FromBody] CrearEventoRequest request)
    {
        try
        {
            var chef = new Chef(request.ChefNombre, request.ChefEmail, request.ChefTelefono,
                               request.ChefEspecialidad, request.ChefNacionalidad, request.ChefAñosExperiencia);

            Evento evento = request.Modalidad == "Presencial"
                ? new EventoPresencial(request.Nombre, request.Descripcion, request.Tipo,
                                     request.FechaInicio, request.FechaFin, request.Capacidad,
                                     request.Precio, chef, request.Lugar!)
                : new EventoVirtual(request.Nombre, request.Descripcion, request.Tipo,
                                   request.FechaInicio, request.FechaFin, request.Capacidad,
                                   request.Precio, chef, request.UrlStreaming!);

            await _repo.InsertarAsync(evento);
            return Ok(new { mensaje = "Evento creado con éxito", eventoId = evento.Id });
        }
        catch (FoodieEventsException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

// DTO para no exponer las clases de dominio directamente
public class CrearEventoRequest
{
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public TipoEvento Tipo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Capacidad { get; set; }
    public decimal Precio { get; set; }
    public string Modalidad { get; set; } = "Presencial";
    public string? Lugar { get; set; }
    public string? UrlStreaming { get; set; }

    // Datos del chef
    public string ChefNombre { get; set; } = null!;
    public string ChefEmail { get; set; } = null!;
    public string ChefTelefono { get; set; } = null!;
    public string ChefEspecialidad { get; set; } = null!;
    public string ChefNacionalidad { get; set; } = null!;
    public int ChefAñosExperiencia { get; set; }
}