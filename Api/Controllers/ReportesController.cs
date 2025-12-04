// Api/Controllers/ReportesController.cs
using Dapper;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly IConfiguration _config;

    public ReportesController(IConfiguration config)
        => _config = config;

    [HttpGet("top-eventos")]
    public async Task<ActionResult> ObtenerTopEventos([FromQuery] int cantidad = 5)
    {
        using var conn = new MySqlConnection(_config.GetConnectionString("Default"));
        
        var resultados = await conn.QueryAsync(
            "ObtenerEventosMasPopulares",
            new { cantidad },
            commandType: System.Data.CommandType.StoredProcedure);

        return Ok(resultados);
    }
}