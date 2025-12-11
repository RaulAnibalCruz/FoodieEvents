# FoodieEvents – Sistema de Gestión de Eventos Gastronómicos  
**Sprint 2 – Arquitectura en capas con C# y Dapper sobre MySQL**

**Autor:** Raúl Cruz  
**Fecha:** Diciembre 2025  

---

### Descripción del proyecto
Aplicación completa para la gestión de eventos gastronómicos que incluye chefs/organizadores, participantes, invitados especiales y reservas.  
Implementado con **arquitectura limpia en capas**, **Dapper** como micro-ORM y **MySQL** como motor de base de datos.

---

### Proyectos

| Proyecto                | Responsabilidad                                      | Tecnologías principales                     |
|-------------------------|------------------------------------------------------|---------------------------------------------|
| **Biblioteca**          | Dominio del negocio                                  | Clases de dominio, enums, validaciones      |
|                         | • Entidades: `Persona`, `Chef`, `Participante`, `InvitadoEspecial`, `Evento`, `Reserva`<br>• Enumeraciones: `TipoEvento`, `MetodoPago`, `EstadoReserva`<br>• `Validador` (validación centralizada)<br>• `FoodieEventsException` | C# 13, records, pattern matching           |
| **Datos**               | Capa de acceso a datos                               | Dapper, MySqlConnector                      |
|                         | • DTOs (`PersonaDto`, `EventoDto`)<br>• Repositorios (`PersonaRepositorio`, `EventoRepositorio`, `ReservaRepositorio`) | Consultas parametrizadas y Stored Procedures |
| **Api**                 | Capa de presentación (Web API)                       | ASP.NET Core 9.0, Swagger/OpenAPI           |
|                         | • Controladores: `PersonasController`, `EventosController`, `ReservasController`, `ReportesController` | Minimal APIs + Controllers                  |

---

### Características implementadas

- Herencia y polimorfismo completo (`Persona` → `Chef`, `Participante`, `InvitadoEspecial`)
- Validaciones centralizadas (`Validador`) y excepción propia
- Arquitectura estrictamente en capas (Dominio ↔ Datos ↔ API)
- Persistencia con **Dapper** + **Stored Procedures** 
- CRUD completo sobre Personas, Eventos y Reservas
- Invitados especiales obtienen acceso gratuito automáticamente
- Control de cupo mediante procedimiento almacenado
- Reportes en tiempo real (eventos más populares, chefs con más eventos, participantes frecuentes)
- API REST documentada con **Swagger**

---

### Base de datos (MySQL)d

- Tablas: `Personas`, `Eventos`, `Reservas` 
- 5 procedimientos almacenados:
  - `ObtenerEventosMasPopulares`
  - `CrearReserva` (control de cupo)
  - `CancelarReserva`
  - `ObtenerChefsConMasEventos`
  - `ParticipantesFrecuentes`

**Scripts incluidos en la carpeta `sql/`**  
Ejecutar en orden:
```sql
mysql -u root -p
source Install.sql

### Compilar y ejecutar desde la raíz del proyecto (en la carpeta Api):
dotnet restore
dotnet build
dotnet run

### Acceder a Swagger:
https://localhost:7XXX/swagger

Endpoints principales (Swagger)

Recurso,Métodos disponibles
/api/personas,"GET (todos), GET {id}, POST chef, POST participante, POST invitado-especial, DELETE {id}"
/api/eventos,"GET (todos), GET {id}, POST, DELETE {id}"
/api/reservas,"GET (todos), POST, DELETE {id}"
/api/reportes,"GET top-eventos, etc."
