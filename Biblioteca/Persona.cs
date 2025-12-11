// Biblioteca/Persona.cs


namespace Biblioteca;

public abstract class Persona
{
    public int Id { get; protected set; }
    public string Nombre { get; protected set; } = null!;
    public string Email { get; protected set; } = null!;
    public string Telefono { get; protected set; } = null!;

    protected Persona(string nombre, string email, string telefono)
    {
        Validador.ValidarTextoRequerido(nombre, "Nombre");
        Validador.ValidarEmail(email);
        Validador.ValidarTelefono(telefono);

        Nombre = nombre.Trim();
        Email = email.Trim().ToLowerInvariant();
        Telefono = telefono.Trim();
    }

    // ← MÉTODO VIRTUAL (¡OBLIGATORIO!)
    public virtual void Registrarse()
    {
        Console.WriteLine($"{Nombre} se ha registrado en el sistema.");
    }

    public virtual string ObtenerInformacionContacto()
        => $"{Nombre} - {Email} - {Telefono}";
}