INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, Especialidad, Nacionalidad, AniosExperiencia)
VALUES ('Mauro Colagreco', 'mauro@mirazur.com', '1122334455', 'Chef', 'Cocina Francesa-Argentina', 'Argentino', 25);

INSERT INTO Eventos (Nombre, Descripcion, Tipo, FechaInicio, FechaFin, CapacidadMaxima, Precio, Lugar, Modalidad, ChefId)
VALUES ('Cena 10 pasos Mirazur', 'Experiencia unica', 'CenaTematica',
        '2025-04-20 20:00:00', '2025-04-20 23:30:00', 20, 45000.00, 'Salon Buenos Aires', 'Presencial', 1);

INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, DocumentoIdentidad, RestriccionesAlimentarias)
VALUES ('Juan Perez', 'juan@gmail.com', '1155667788', 'Participante', '30123456', 'Sin gluten');

INSERT INTO Personas (Nombre, Email, Telefono, TipoPersona, EsVIP)
VALUES ('Narda Lepes', 'narda@tv.com', '1199887766', 'InvitadoEspecial', TRUE);