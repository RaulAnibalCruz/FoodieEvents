namespace Biblioteca;
// Enums.cs  ← Archivo correcto y completo


public enum TipoEvento 
{ 
    Cata, Feria, Clase, CenaTematica 
}

public enum ModalidadEvento 
{ 
    Presencial, Virtual 
}

public enum MetodoPago 
{ 
    Tarjeta, 
    Transferencia, 
    Efectivo, 
    Cortesia        // ¡AQUÍ ESTABA FALTANDO!
}

public enum EstadoReserva 
{ 
    EnEspera, 
    Confirmada, 
    Cancelada 
}