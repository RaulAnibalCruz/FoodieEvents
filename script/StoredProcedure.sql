DELIMITER //

CREATE PROCEDURE ObtenerEventosMasPopulares(IN cantidad INT)
BEGIN
    SELECT 
        e.Id,
        e.Nombre AS NombreEvento,
        e.Tipo,
        e.Modalidad,
        e.FechaInicio,
        COUNT(r.Id) AS AsistentesConfirmados
    FROM Eventos e
    LEFT JOIN Reservas r ON e.Id = r.EventoId AND r.Estado = 'Confirmada'
    GROUP BY e.Id
    ORDER BY AsistentesConfirmados DESC
    LIMIT cantidad;
END //

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

CREATE PROCEDURE CancelarReserva(IN p_ReservaId INT)
BEGIN
    UPDATE Reservas SET Estado = 'Cancelada'
    WHERE Id = p_ReservaId AND Estado = 'Confirmada';

    IF ROW_COUNT() = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La reserva no existe o ya estaba cancelada';
    END IF;
END //

CREATE PROCEDURE ObtenerChefsConMasEventos()
BEGIN
    SELECT p.Id, p.Nombre, p.Especialidad, COUNT(e.Id) AS CantidadEventos
    FROM Personas p
    LEFT JOIN Eventos e ON p.Id = e.ChefId
    WHERE p.TipoPersona = 'Chef'
    GROUP BY p.Id
    ORDER BY CantidadEventos DESC
    LIMIT 5;
END //

CREATE PROCEDURE ParticipantesFrecuentes(IN minimo_eventos INT)
BEGIN
    SELECT p.Id, p.Nombre, p.Email, COUNT(r.Id) AS EventosAsistidos
    FROM Personas p
    INNER JOIN Reservas r ON p.Id = r.PersonaId
    WHERE p.TipoPersona IN ('Participante', 'InvitadoEspecial') AND r.Estado = 'Confirmada'
    GROUP BY p.Id
    HAVING EventosAsistidos >= minimo_eventos
    ORDER BY EventosAsistidos DESC;
END //

DELIMITER ;