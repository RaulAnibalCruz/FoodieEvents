// Api/Controllers/PersonasController.cs
using Biblioteca;
using Datos.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/personas")]
public class PersonasController : ControllerBase
{
    private readonly PersonaRepositorio _repo;

    public PersonasController(PersonaRepositorio repo)
        => _repo = repo;

    // GET: api/personas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> ObtenerPorId(int id)
    {
        try
        {
            var persona = await _repo.ObtenerPorIdAsync(id);
            return Ok(persona);
        }
        catch (FoodieEventsException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST: api/personas/chef
    [HttpPost("chef")]
    public async Task<ActionResult> CrearChef([FromBody] CrearChefRequest request)
    {
        try
        {
            var chef = new Chef(
                request.Nombre, request.Email, request.Telefono,
                request.Especialidad, request.Nacionalidad, request.AñosExperiencia);

            // Guardar en base de datos
            await _repo.GuardarAsync(chef); // ← Necesitamos este método (lo agrego abajo)

            return CreatedAtAction(nameof(ObtenerPorId), new { id = chef.Id }, 
                new { mensaje = "Chef creado con éxito", chefId = chef.Id });
        }
        catch (FoodieEventsException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/personas/participante
    [HttpPost("participante")]
    public async Task<ActionResult> CrearParticipante([FromBody] CrearParticipanteRequest request)
    {
        var participante = new Participante(
            request.Nombre, request.Email, request.Telefono,
            request.DocumentoIdentidad, request.RestriccionesAlimentarias ?? "");

        await _repo.GuardarAsync(participante);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = participante.Id },
            new { mensaje = "Participante creado", id = participante.Id });
    }

    // POST: api/personas/invitado-especial
    [HttpPost("invitado-especial")]
    public async Task<ActionResult> CrearInvitadoEspecial([FromBody] CrearInvitadoEspecialRequest request)
    {
        var invitado = new InvitadoEspecial(request.Nombre, request.Email, request.Telefono);
        await _repo.GuardarAsync(invitado);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = invitado.Id },
            new { mensaje = "Invitado especial creado - ENTRA GRATIS A TODO", id = invitado.Id });
    }
}

// DTOs para no exponer las clases de dominio directamente
public class CrearChefRequest
{
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Especialidad { get; set; } = null!;
    public string Nacionalidad { get; set; } = null!;
    public int AñosExperiencia { get; set; }
}

public class CrearParticipanteRequest
{
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string DocumentoIdentidad { get; set; } = null!;
    public string? RestriccionesAlimentarias { get; set; }
}

public class CrearInvitadoEspecialRequest
{
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefono { get; set; } = null!;
}