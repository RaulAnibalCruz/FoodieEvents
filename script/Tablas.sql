-- Install.sql corregido y funcionando (cópialo tal cual)

DROP DATABASE IF EXISTS FoodieEventsDB;
CREATE DATABASE FoodieEventsDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE FoodieEventsDB;

-- 1. Tabla Persons (base para todos)
CREATE TABLE Persons (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(20) NOT NULL,
    PersonType ENUM('Chef', 'Participant', 'SpecialGuest') NOT NULL,
    
    -- Campos específicos de Chef
    Specialty VARCHAR(50) NULL,
    Nationality VARCHAR(50) NULL,
    YearsExperience INT NULL,
    
    -- Campos específicos de Participante
    IdentityDocument VARCHAR(50) NULL,
    DietaryRestrictions TEXT NULL,
    
    -- SpecialGuest
    IsVIP BOOLEAN DEFAULT FALSE
);

-- 2. Tabla Events
CREATE TABLE Events (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT NOT NULL,
    Type ENUM('Cata', 'Feria', 'Clase', 'CenaTematica') NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    MaxCapacity INT NOT NULL CHECK (MaxCapacity > 0),
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Location VARCHAR(200) NULL,           -- NULL para eventos virtuales
    StreamUrl VARCHAR(500) NULL,          -- para eventos virtuales
    Modality ENUM('Presential', 'Virtual') NOT NULL DEFAULT 'Presential',
    ChefId INT NOT NULL,
    
    CONSTRAINT fk_events_chef FOREIGN KEY (ChefId) REFERENCES Persons(Id) ON DELETE RESTRICT,
    CONSTRAINT chk_dates CHECK (EndDate > StartDate)
);

-- 3. Tabla Reservations (¡AQUÍ ESTABA EL ERROR!)
CREATE TABLE Reservations (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PersonId INT NULL,                          -- ← PERMITIMOS NULL
    EventId INT NOT NULL,
    ReservationDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsPaid BOOLEAN NOT NULL DEFAULT FALSE,
    PaymentMethod ENUM('Tarjeta', 'Transferencia', 'Efectivo', 'Cortesia') NULL,
    Status ENUM('Confirmada', 'Cancelada', 'EnEspera') NOT NULL DEFAULT 'EnEspera',
    
    CONSTRAINT fk_reservations_person 
        FOREIGN KEY (PersonId) REFERENCES Persons(Id) ON DELETE SET NULL,   -- ← Ahora sí tiene sentido
    CONSTRAINT fk_reservations_event 
        FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE
);

-- Índices para que vuele
CREATE INDEX idx_events_dates ON Events(StartDate, EndDate);
CREATE INDEX idx_reservations_event ON Reservations(EventId);
CREATE INDEX idx_reservations_person ON Reservations(PersonId);