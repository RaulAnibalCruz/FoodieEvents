using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// ReporteEventosPopulares.cs


public class ReporteEventosPopulares : Reporte
{
    private readonly List<Evento> _eventos;
    public ReporteEventosPopulares(List<Evento> eventos) 
        : base("Top 10 Eventos Más Populares") => _eventos = eventos;

    public override string Generar()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(Titulo);
        sb.AppendLine(new string('=', 50));

        var top = _eventos
            .OrderByDescending(e => e.Reservas.Count(r => r.Estado == EstadoReserva.Confirmada))
            .Take(10);

        foreach (var e in top)
        {
            int confirmadas = e.Reservas.Count(r => r.Estado == EstadoReserva.Confirmada);
            sb.AppendLine($"{e.Nombre} | {confirmadas}/{e.CapacidadMaxima} | {e.Tipo}");
        }
        return sb.ToString();
    }
}