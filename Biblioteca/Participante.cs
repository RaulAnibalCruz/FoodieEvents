// Biblioteca/Participante.cs
namespace Biblioteca;

public class Participante : Persona
{
    public string DocumentoIdentidad { get; private set; } = null!;
    public string RestriccionesAlimentarias { get; private set; } = "";

    public Participante(string nombre, string email, string telefono,
                        string documento, string restricciones = "")
        : base(nombre, email, telefono)
    {
        Validador.ValidarTextoRequerido(documento, "Documento");
        DocumentoIdentidad = documento.Trim();
        RestriccionesAlimentarias = restricciones.Trim();
    }
}