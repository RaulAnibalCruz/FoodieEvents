// CustomException.cs
namespace Biblioteca;

public class FoodieEventsException : Exception
{
    public FoodieEventsException(string mensaje) : base(mensaje) { }
}