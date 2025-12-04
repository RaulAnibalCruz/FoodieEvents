// Chef.cs
namespace Biblioteca;

public class Chef : Persona
{
    public string Especialidad { get; private set; }
    public string Nacionalidad { get; private set; }
    public int AñosExperiencia { get; private set; }

    public Chef(string nombre, string email, string telefono, 
                string especialidad, string nacionalidad, int añosExperiencia)
        : base(nombre, email, telefono)
    {
        Validador.ValidarTextoRequerido(especialidad, "Especialidad");
        Validador.ValidarTextoRequerido(nacionalidad, "Nacionalidad");
        Validador.ValidarNumeroPositivo(añosExperiencia, "Años de experiencia");

        Especialidad = especialidad;
        Nacionalidad = nacionalidad;
        AñosExperiencia = añosExperiencia;
    }

    public override void Registrarse()
        => Console.WriteLine($"Chef {Nombre} registrado como organizador.");

    public override string ObtenerInformacionContacto()
        => base.ObtenerInformacionContacto() + $" | Chef de {Especialidad}";
}