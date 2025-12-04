using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// EventoVirtual.cs


public class EventoVirtual : Evento
{
    public string UrlStreaming { get; private set; }

    public EventoVirtual(string nombre, string descripcion, TipoEvento tipo,
                         DateTime inicio, DateTime fin, int capacidad, decimal precio,
                         Chef chef, string url)
        : base(nombre, descripcion, tipo, inicio, fin, capacidad, precio, chef)
    {
        Validador.ValidarTextoRequerido(url, "URL del streaming");
        UrlStreaming = url;
    }

    public override string ObtenerUbicacion() => $"Virtual: {UrlStreaming}";
}