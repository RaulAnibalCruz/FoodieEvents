// Api/Controllers/ReservasController.cs
using Biblioteca;
using Datos.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/reservas")]
public class ReservasController : ControllerBase
{
    private readonly ReservaRepositorio _reservaRepo;
    private readonly PersonaRepositorio _personaRepo;

    public ReservasController(ReservaRepositorio reservaRepo, PersonaRepositorio personaRepo)
    {
        _reservaRepo = reservaRepo;
        _personaRepo = personaRepo;
    }

    [HttpGet]
    public async Task<ActionResult> Listar()
        => Ok(await _reservaRepo.ObtenerTodasAsync());

    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] CrearReservaRequest request)
    {
        try
        {
            var persona = await _personaRepo.ObtenerPorIdAsync(request.PersonaId);

            if (persona is InvitadoEspecial)
            {
                await _reservaRepo.CrearConValidacionAsync(request.PersonaId, request.EventoId, true, "Cortesia");
                return Ok("¡Invitado especial: Reserva confirmada GRATIS!");
            }

            await _reservaRepo.CrearConValidacionAsync(request.PersonaId, request.EventoId, request.Pagado, request.MetodoPago);
            return Ok("Reserva creada con éxito");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        try
        {
            await _reservaRepo.EliminarAsync(id);
            return Ok("Reserva cancelada");
        }
        catch (FoodieEventsException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

public class CrearReservaRequest
{
    public int PersonaId { get; set; }
    public int EventoId { get; set; }
    public bool Pagado { get; set; } = false;
    public string MetodoPago { get; set; } = "Tarjeta";
}