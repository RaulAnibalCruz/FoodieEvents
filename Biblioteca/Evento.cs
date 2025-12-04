using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// Evento.cs


public abstract class Evento
{
    public int Id { get; protected set; }
    public string Nombre { get; private set; }
    public string Descripcion { get; private set; }
    public TipoEvento Tipo { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }
    public int CapacidadMaxima { get; private set; }
    public decimal Precio { get; private set; }
    public Chef ChefOrganizador { get; private set; }
    public List<Reserva> Reservas { get; private set; } = new();

    protected Evento(string nombre, string descripcion, TipoEvento tipo,
                     DateTime inicio, DateTime fin, int capacidad, decimal precio, Chef chef)
    {
        Validador.ValidarTextoRequerido(nombre, "Nombre del evento");
        Validador.ValidarTextoRequerido(descripcion, "Descripción");
        Validador.ValidarFechas(inicio, fin);
        Validador.ValidarNumeroPositivo(capacidad, "Capacidad máxima");
        Validador.ValidarPrecio(precio);
        ChefOrganizador = chef ?? throw new FoodieEventsException("Todo evento debe tener un chef organizador.");

        Nombre = nombre;
        Descripcion = descripcion;
        Tipo = tipo;
        FechaInicio = inicio;
        FechaFin = fin;
        CapacidadMaxima = capacidad;
        Precio = precio;
    }

    public bool EstaCompleto() => Reservas.Count(r => r.Estado == EstadoReserva.Confirmada) >= CapacidadMaxima;
    public abstract string ObtenerUbicacion();
}