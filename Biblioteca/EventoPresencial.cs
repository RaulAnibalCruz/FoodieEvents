using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// EventoPresencial.cs


public class EventoPresencial : Evento
{
    public string Lugar { get; private set; }

    public EventoPresencial(string nombre, string descripcion, TipoEvento tipo,
                            DateTime inicio, DateTime fin, int capacidad, decimal precio,
                            Chef chef, string lugar)
        : base(nombre, descripcion, tipo, inicio, fin, capacidad, precio, chef)
    {
        Validador.ValidarTextoRequerido(lugar, "Lugar del evento");
        Lugar = lugar;
    }

    public override string ObtenerUbicacion() => Lugar;
}