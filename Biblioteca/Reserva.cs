using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biblioteca;
namespace Biblioteca;
// Reserva.cs


public class Reserva
{
    public int Id { get; private set; }
    public Persona Persona { get; private set; }
    public Evento Evento { get; private set; }
    public DateTime FechaReserva { get; private set; } = DateTime.Now;
    public bool Pagado { get; private set; }
    public MetodoPago? MetodoPago { get; private set; }
    public EstadoReserva Estado { get; private set; } = EstadoReserva.EnEspera;

    public Reserva(Persona persona, Evento evento, bool pagado = false, MetodoPago? metodo = null)
    {
        Persona = persona ?? throw new ArgumentNullException(nameof(persona));
        Evento = evento ?? throw new ArgumentNullException(nameof(evento));

        if (evento.EstaCompleto())
            throw new FoodieEventsException("El evento está completo. No se pueden realizar más reservas.");

        // Invitados especiales no pagan
        if (persona is InvitadoEspecial)
        {
            Pagado = true;
            MetodoPago = Biblioteca.MetodoPago.Cortesia;
            Estado = EstadoReserva.Confirmada;
        }
        else
        {
            Pagado = pagado;
            MetodoPago = metodo;
            if (pagado) Estado = EstadoReserva.Confirmada;
        }
    }

    public void ConfirmarPago(MetodoPago metodo)
    {
        Pagado = true;
        MetodoPago = metodo;
        Estado = EstadoReserva.Confirmada;
    }

    public void Cancelar()
    {
        if (Estado != EstadoReserva.Cancelada)
            Estado = EstadoReserva.Cancelada;
    }
}