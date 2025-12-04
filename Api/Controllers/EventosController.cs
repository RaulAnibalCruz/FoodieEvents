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
    private readonly PersonaRepositorio _personaRepo;

    public EventosController(EventoRepositorio repo, PersonaRepositorio personaRepo)
    {
        _repo = repo;
        _personaRepo = personaRepo;
    }

    [HttpGet]
    public async Task<ActionResult> Listar()
        => Ok(await _repo.ObtenerTodosAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult> Obtener(int id)
    {
        try
        {
            var evento = await _repo.ObtenerPorIdCompletoAsync(id);
            return Ok(evento);
        }
        catch (FoodieEventsException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] CrearEventoRequest request)
    {
        try
        {
            var chef = await _personaRepo.ObtenerPorIdAsync(request.ChefId) as Chef
                       ?? throw new FoodieEventsException("Chef no encontrado");

            Evento evento = request.Modalidad?.ToLower() == "virtual"
                ? new EventoVirtual(
                    request.Nombre,
                    request.Descripcion,
                    request.Tipo,
                    request.FechaInicio,
                    request.FechaFin,
                    request.Capacidad,
                    request.Precio,
                    chef,
                    request.UrlStreaming!)
                : new EventoPresencial(
                    request.Nombre,
                    request.Descripcion,
                    request.Tipo,
                    request.FechaInicio,
                    request.FechaFin,
                    request.Capacidad,
                    request.Precio,
                    chef,
                    request.Lugar!);

            await _repo.InsertarAsync(evento);

            return CreatedAtAction(nameof(Obtener), new { id = evento.Id },
                new { mensaje = "Evento creado con éxito", id = evento.Id });
        }
        catch (FoodieEventsException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        try
        {
            await _repo.EliminarAsync(id);
            return Ok("Evento eliminado correctamente");
        }
        catch (FoodieEventsException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

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
    public int ChefId { get; set; }
}