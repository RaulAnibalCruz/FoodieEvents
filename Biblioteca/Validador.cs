// Validador.cs
using System.Text.RegularExpressions;

namespace Biblioteca;

public static class Validador
{
    public static void ValidarTextoRequerido(string valor, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new FoodieEventsException($"{nombreCampo} es obligatorio.");
    }

    public static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
            throw new FoodieEventsException("El correo electrónico no tiene un formato válido.");
    }

    public static void ValidarTelefono(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono) || !Regex.IsMatch(telefono, @"^\d{8,15}$"))
            throw new FoodieEventsException("El teléfono debe contener solo números y tener entre 8 y 15 dígitos.");
    }

    public static void ValidarFechas(DateTime inicio, DateTime fin)
    {
        if (inicio >= fin)
            throw new FoodieEventsException("La fecha de fin debe ser posterior a la fecha de inicio.");
    }

    public static void ValidarNumeroPositivo(int valor, string nombreCampo)
    {
        if (valor <= 0)
            throw new FoodieEventsException($"{nombreCampo} debe ser mayor que cero.");
    }

    public static void ValidarPrecio(decimal precio)
    {
        if (precio < 0)
            throw new FoodieEventsException("El precio no puede ser negativo.");
    }
}