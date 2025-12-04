using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca;
// Reporte.cs


public abstract class Reporte
{
    protected readonly string Titulo;
    protected Reporte(string titulo) => Titulo = titulo;
    public abstract string Generar();
    public virtual void Mostrar() => Console.WriteLine(Generar());
}