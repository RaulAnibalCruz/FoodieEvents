using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// Participante.cs

public class Participante : Persona
{
    public string DocumentoIdentidad { get; private set; }
    public string RestriccionesAlimentarias { get; private set; }

    public Participante(string nombre, string email, string telefono,
                        string documento, string restricciones = "")
        : base(nombre, email, telefono)
    {
        Validador.ValidarTextoRequerido(documento, "Documento de identidad");
        DocumentoIdentidad = documento.Trim();
        RestriccionesAlimentarias = restricciones.Trim();
    }

    public override void Registrarse()
        => Console.WriteLine($"Participante {Nombre} registrado con éxito.");

    public bool TieneRestriccion(string alergeno)
        => !string.IsNullOrEmpty(RestriccionesAlimentarias) && 
           RestriccionesAlimentarias.Contains(alergeno, StringComparison.OrdinalIgnoreCase);
}