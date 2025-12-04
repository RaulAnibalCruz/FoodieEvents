// Persona.cs
namespace Biblioteca;

public abstract class Persona : IRegistrable
{
    public int Id { get; protected set; }
    public string Nombre { get; protected set; }
    public string Email { get; protected set; }
    public string Telefono { get; protected set; }

    protected Persona(string nombre, string email, string telefono)
    {
        Validador.ValidarTextoRequerido(nombre, "Nombre");
        Validador.ValidarEmail(email);
        Validador.ValidarTelefono(telefono);

        Nombre = nombre.Trim();
        Email = email.Trim().ToLowerInvariant();
        Telefono = telefono.Trim();
    }

    public abstract void Registrarse();

    public virtual string ObtenerInformacionContacto()
        => $"{Nombre} - {Email} - {Telefono}";
}