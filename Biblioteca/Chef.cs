// Biblioteca/Chef.cs
namespace Biblioteca;

public class Chef : Persona
{
    public string Especialidad { get; private set; } = null!;
    public string Nacionalidad { get; private set; } = null!;
    public int AniosExperiencia { get; private set; }

    public Chef(string nombre, string email, string telefono,
                string especialidad, string nacionalidad, int aniosExperiencia)
        : base(nombre, email, telefono)
    {
        Validador.ValidarTextoRequerido(especialidad, "Especialidad");
        Validador.ValidarTextoRequerido(nacionalidad, "Nacionalidad");
        Validador.ValidarNumeroPositivo(aniosExperiencia, "Años de experiencia");

        Especialidad = especialidad;
        Nacionalidad = nacionalidad;
        AniosExperiencia = aniosExperiencia;
    }

    public override string ObtenerInformacionContacto()
        => base.ObtenerInformacionContacto() + $" | Chef de {Especialidad}";
}