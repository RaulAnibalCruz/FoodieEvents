DROP DATABASE IF EXISTS FoodieEventsDB;
CREATE DATABASE FoodieEventsDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE FoodieEventsDB;

-- TABLA PERSONAS
CREATE TABLE Personas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NOT NULL,
    TipoPersona ENUM('Chef', 'Participante', 'InvitadoEspecial') NOT NULL,
    Especialidad VARCHAR(50) NULL,
    Nacionalidad VARCHAR(50) NULL,
    AniosExperiencia INT NULL,
    DocumentoIdentidad VARCHAR(50) NULL,
    RestriccionesAlimentarias TEXT NULL,
    EsVIP BOOLEAN DEFAULT FALSE
);

-- TABLA EVENTOS
CREATE TABLE Eventos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion TEXT NOT NULL,
    Tipo ENUM('Cata', 'Feria', 'Clase', 'CenaTematica') NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    CapacidadMaxima INT NOT NULL CHECK (CapacidadMaxima > 0),
    Precio DECIMAL(10,2) NOT NULL CHECK (Precio >= 0),
    Lugar VARCHAR(200) NULL,
    UrlStreaming VARCHAR(500) NULL,
    Modalidad ENUM('Presencial', 'Virtual') NOT NULL DEFAULT 'Presencial',
    ChefId INT NOT NULL,
    CONSTRAINT fk_eventos_chef FOREIGN KEY (ChefId) REFERENCES Personas(Id) ON DELETE RESTRICT,
    CONSTRAINT chk_fechas CHECK (FechaFin > FechaInicio)
);

-- TABLA RESERVAS
CREATE TABLE Reservas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PersonaId INT NULL,
    EventoId INT NOT NULL,
    FechaReserva DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Pagada BOOLEAN NOT NULL DEFAULT FALSE,
    MetodoPago ENUM('Tarjeta', 'Transferencia', 'Efectivo', 'Cortesia') NULL,
    Estado ENUM('Confirmada', 'Cancelada', 'EnEspera') NOT NULL DEFAULT 'EnEspera',
    CONSTRAINT fk_reservas_persona FOREIGN KEY (PersonaId) REFERENCES Personas(Id) ON DELETE SET NULL,
    CONSTRAINT fk_reservas_evento FOREIGN KEY (EventoId) REFERENCES Eventos(Id) ON DELETE CASCADE
);

-- PROCEDIMIENTO PARA CREAR RESERVA
DELIMITER //

CREATE PROCEDURE CrearReserva(
    IN p_PersonaId INT,
    IN p_EventoId INT,
    IN p_Pagada BOOLEAN,
    IN p_MetodoPago ENUM('Tarjeta', 'Transferencia', 'Efectivo', 'Cortesia')
)
BEGIN
    DECLARE ocupados INT;
    DECLARE capacidad INT;

    SELECT COUNT(*) INTO ocupados FROM Reservas 
    WHERE EventoId = p_EventoId AND Estado = 'Confirmada';

    SELECT CapacidadMaxima INTO capacidad FROM Eventos WHERE Id = p_EventoId;

    IF ocupados >= capacidad THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No hay mas cupos disponibles';
    END IF;

    INSERT INTO Reservas (PersonaId, EventoId, Pagada, MetodoPago, Estado)
    VALUES (p_PersonaId, p_EventoId, p_Pagada, p_MetodoPago, 'Confirmada');
END //

DELIMITER ;

-- DATOS DE PRUEBA (SIN Ñ NI TILDES)
INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, Especialidad, Nacionalidad, AniosExperiencia)
VALUES ('Mauro Colagreco', 'mauro@mirazur.com', '1122334455', 'Chef', 'Cocina Francesa-Argentina', 'Argentino', 25);

INSERT INTO Eventos (Nombre, Descripcion, Tipo, FechaInicio, FechaFin, CapacidadMaxima, Precio, Lugar, Modalidad, ChefId)
VALUES ('Cena 10 pasos Mirazur', 'Experiencia unica', 'CenaTematica',
        '2025-04-20 20:00:00', '2025-04-20 23:30:00', 20, 45000.00, 'Salon Buenos Aires', 'Presencial', 1);

INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, DocumentoIdentidad, RestriccionesAlimentarias)
VALUES ('Juan Perez', 'juan@gmail.com', '1155667788', 'Participante', '30123456', 'Sin gluten');

INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, EsVIP)
VALUES ('Narda Lepes', 'narda@tv.com', '1199887766', 'InvitadoEspecial', TRUE);