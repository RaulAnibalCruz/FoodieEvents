// Biblioteca/Reserva.cs
namespace Biblioteca;

public class Reserva
{
    public int Id { get; private set; }
    public Persona Persona { get; private set; } = null!;
    public Evento Evento { get; private set; } = null!;
    public DateTime FechaReserva { get; private set; } = DateTime.Now;
    public bool Pagada { get; private set; }
    public MetodoPago? MetodoPago { get; private set; }
    public EstadoReserva Estado { get; private set; } = EstadoReserva.EnEspera;

        public Reserva(Persona persona, Evento evento, bool pagada = false, MetodoPago? metodo = null)
        {
            Persona = persona ?? throw new ArgumentNullException(nameof(persona));
            Evento = evento ?? throw new ArgumentNullException(nameof(evento));
            Pagada = pagada;
            MetodoPago = metodo;

            if (persona is InvitadoEspecial)
            {
                Pagada = true;
                MetodoPago = Biblioteca.MetodoPago.Cortesia;  
                Estado = EstadoReserva.Confirmada;
            }
            else if (pagada)
            {
                Estado = EstadoReserva.Confirmada;
            }
        }
}