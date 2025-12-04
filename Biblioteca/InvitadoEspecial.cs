using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// InvitadoEspecial.cs


public class InvitadoEspecial : Persona
{
    public bool EsVIP { get; private set; } = true;

    public InvitadoEspecial(string nombre, string email, string telefono)
        : base(nombre, email, telefono) { }

    public override void Registrarse()
        => Console.WriteLine($"Invitado especial {Nombre} registrado - Acceso gratuito.");

    public override string ObtenerInformacionContacto()
        => base.ObtenerInformacionContacto() + " | INVITADO ESPECIAL (GRATIS)";
}