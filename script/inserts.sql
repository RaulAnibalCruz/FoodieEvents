-- 1. Un chef famoso
INSERT INTO Persons (Name, Email, Phone, PersonType, Specialty, Nationality, YearsExperience)
VALUES ('Mauro Colagreco', 'mauro@mirazur.com', '1122334455', 'Chef', 'Cocina Francesa-Argentina', 'Argentino', 25);

-- 2. Un evento brutal
INSERT INTO Events (Name, Description, Type, StartDate, EndDate, MaxCapacity, Price, Location, Modality, ChefId)
VALUES ('Cena 10 pasos Mirazur', 'Experiencia única con el mejor chef del mundo', 'CenaTematica',
        '2025-04-20 20:00:00', '2025-04-20 23:30:00', 20, 45000.00, 'Salón Buenos Aires', 'Presential', 1);

-- 3. Un participante normal
INSERT INTO Persons (Name, Email, Phone, PersonType, IdentityDocument, DietaryRestrictions)
VALUES ('Juan Pérez', 'juan@gmail.com', '1155667788', 'Participant', '30123456', 'Sin gluten');

-- 4. Un INVITADO ESPECIAL (entra GRATIS)
INSERT INTO Persons (Name, Email, Phone, PersonType, IsVIP)
VALUES ('Narda Lepes', 'narda@tv.com', '1199887766', 'SpecialGuest', TRUE);