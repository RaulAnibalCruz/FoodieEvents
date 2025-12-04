DELIMITER //

-- =============================================
-- 1. Obtener los eventos más populares
-- =============================================
CREATE PROCEDURE ObtenerEventosMasPopulares(IN cantidad INT)
BEGIN
    SELECT 
        e.Id,
        e.Name AS NombreEvento,
        e.Type AS TipoEvento,
        e.Modality AS Modalidad,
        e.StartDate AS Fecha,
        COUNT(r.Id) AS AsistentesConfirmados
    FROM Events e
    LEFT JOIN Reservations r ON e.Id = r.EventId 
        AND r.Status = 'Confirmada'
    GROUP BY e.Id, e.Name, e.Type, e.Modality, e.StartDate
    ORDER BY AsistentesConfirmados DESC
    LIMIT cantidad;
END //


-- =============================================
-- 2. Crear una reserva con control de cupo
-- =============================================
CREATE PROCEDURE CrearReserva(
    IN p_PersonId INT,
    IN p_EventId INT,
    IN p_Pagado BOOLEAN,
    IN p_MetodoPago ENUM('Tarjeta', 'Transferencia', 'Efectivo', 'Cortesia')
)
BEGIN
    DECLARE cupos_ocupados INT;
    DECLARE capacidad_maxima INT;

    -- Contar reservas confirmadas
    SELECT COUNT(*) INTO cupos_ocupados
    FROM Reservations
    WHERE EventId = p_EventId AND Status = 'Confirmada';

    -- Obtener capacidad del evento
    SELECT MaxCapacity INTO capacidad_maxima
    FROM Events WHERE Id = p_EventId;

    -- Validar disponibilidad
    IF cupos_ocupados >= capacidad_maxima THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Lo siento, el evento ya está completo. No hay más cupos disponibles.';
    END IF;

    -- Insertar la reserva
    INSERT INTO Reservations (PersonId, EventId, IsPaid, PaymentMethod, Status)
    VALUES (p_PersonId, p_EventId, p_Pagado, p_MetodoPago, 'Confirmada');

    SELECT 'Reserva creada exitosamente' AS Mensaje;
END //


-- =============================================
-- 3. Cancelar una reserva
-- =============================================
CREATE PROCEDURE CancelarReserva(IN p_ReservaId INT)
BEGIN
    UPDATE Reservations 
    SET Status = 'Cancelada'
    WHERE Id = p_ReservaId AND Status = 'Confirmada';

    IF ROW_COUNT() = 0 THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'La reserva no existe o ya estaba cancelada.';
    ELSE
        SELECT 'Reserva cancelada correctamente' AS Mensaje;
    END IF;
END //


-- =============================================
-- 4. Top 5 chefs con más eventos organizados
-- =============================================
CREATE PROCEDURE ObtenerChefsConMasEventos()
BEGIN
    SELECT 
        p.Id,
        p.Name AS NombreChef,
        p.Specialty AS Especialidad,
        COUNT(e.Id) AS CantidadEventos
    FROM Persons p
    LEFT JOIN Events e ON p.Id = e.ChefId
    WHERE p.PersonType = 'Chef'
    GROUP BY p.Id, p.Name, p.Specialty
    ORDER BY CantidadEventos DESC
    LIMIT 5;
END //


-- =============================================
-- 5. Participantes que asistieron a más de N eventos
-- =============================================
CREATE PROCEDURE ParticipantesFrecuentes(IN minimo_eventos INT)
BEGIN
    SELECT 
        p.Id,
        p.Name AS Nombre,
        p.Email,
        COUNT(r.Id) AS EventosAsistidos
    FROM Persons p
    INNER JOIN Reservations r ON p.Id = r.PersonId
    WHERE p.PersonType IN ('Participant', 'SpecialGuest')
      AND r.Status = 'Confirmada'
    GROUP BY p.Id, p.Name, p.Email
    HAVING EventosAsistidos >= minimo_eventos
    ORDER BY EventosAsistidos DESC;
END //

DELIMITER ;