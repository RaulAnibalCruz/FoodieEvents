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

    [HttpPost]
    public async Task<ActionResult> CrearReserva([FromBody] CrearReservaRequest request)
    {
        try
        {
            var persona = await _personaRepo.ObtenerPorIdAsync(request.PersonaId);

            // Invitado especial entra GRATIS automáticamente
            if (persona is InvitadoEspecial)
            {
                await _reservaRepo.CrearConValidacionAsync(
                    request.PersonaId, request.EventoId, true, "Cortesia");
                return Ok("Invitado especial: ¡Reserva confirmada GRATIS!");
            }

            await _reservaRepo.CrearConValidacionAsync(
                request.PersonaId, request.EventoId, request.Pagado, request.MetodoPago);

            return Ok("Reserva creada con éxito");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class CrearReservaRequest
{
    public int PersonaId { get; set; }
    public int EventoId { get; set; }
    public bool Pagado { get; set; }
    public string MetodoPago { get; set; } = "Tarjeta";
}