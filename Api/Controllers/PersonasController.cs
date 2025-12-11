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

    public PersonasController(PersonaRepositorio repo) => _repo = repo;

    // GET: api/personas
    [HttpGet]
    public async Task<ActionResult<List<Persona>>> ListarTodas()
        => Ok(await _repo.ObtenerTodasAsync());

    // GET: api/personas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> ObtenerPorId(int id)
    {
        try
        {
            return Ok(await _repo.ObtenerPorIdAsync(id));
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
        var chef = new Chef(
            nombre: request.Nombre,
            email: request.Email,
            telefono: request.Telefono,
            especialidad: request.Especialidad,
            nacionalidad: request.Nacionalidad,
            aniosExperiencia: request.AñosExperiencia
        );

        await _repo.GuardarAsync(chef);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = chef.Id },
            new { mensaje = "Chef creado con éxito", id = chef.Id });
    }

    // POST: api/personas/participante
    [HttpPost("participante")]
    public async Task<ActionResult> CrearParticipante([FromBody] CrearParticipanteRequest request)
    {
        var participante = new Participante(
            nombre: request.Nombre,
            email: request.Email,
            telefono: request.Telefono,
            documento: request.DocumentoIdentidad,                    // ← CORREGIDO: "documento"
            restricciones: request.RestriccionesAlimentarias ?? ""   // ← CORREGIDO: "restricciones"
        );

        await _repo.GuardarAsync(participante);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = participante.Id },
            new { mensaje = "Participante creado con éxito", id = participante.Id });
    }

    // POST: api/personas/invitado-especial
    [HttpPost("invitado-especial")]
    public async Task<ActionResult> CrearInvitadoEspecial([FromBody] CrearInvitadoEspecialRequest request)
    {
        var invitado = new InvitadoEspecial(
            nombre: request.Nombre,
            email: request.Email,
            telefono: request.Telefono
        );

        await _repo.GuardarAsync(invitado);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = invitado.Id },
            new { mensaje = "Invitado especial creado con éxito", id = invitado.Id });
    }

    // DELETE: api/personas/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        try
        {
            await _repo.EliminarAsync(id);
            return Ok(new { mensaje = "Persona eliminada correctamente" });
        }
        catch (FoodieEventsException)
        {
            return NotFound("Persona no encontrada");
        }
    }
}

// ======================= DTOs =======================
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