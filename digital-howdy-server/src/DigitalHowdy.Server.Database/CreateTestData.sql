INSERT INTO Organization (Name)
VALUES ('NTNU');
INSERT INTO Organization (Name)
VALUES ('DigitalHowdy');

INSERT INTO Visitor(Name, Phone, OrganizationId)
VALUES ('Bob Bobsson', '12345678', 1);
INSERT INTO Visitor(Name, Phone, OrganizationId)
VALUES ('Bernard Bernardsen', '23456789', 2);
INSERT INTO Visitor(Name, Phone, OrganizationId)
VALUES ('Sara Bernardsen', '56784567', 2);

INSERT INTO Employee(Name, Phone, Email)
VALUES('Anton Antonsen', '11223344', 'antant@test.com');
INSERT INTO Employee(Name, Phone, Email)
VALUES('Bernt Berntsen', '22334455', 'berber@test.com');
INSERT INTO Employee(Name, Phone, Email)
VALUES('Daniel Danielsen', '33445566', 'dandan@test.com');
INSERT INTO Employee(Name, Phone, Email)
VALUES('Erik Eriksen', '44556677', 'erieri@test.com');

INSERT INTO Visit (VisitorId, EmployeeId, StartDate, Reference)
VALUES (1, 3, '2020-10-21 11:22:33', 123456);
INSERT INTO Visit (VisitorId, EmployeeId, StartDate, Reference)
VALUES (2, 2, '2020-10-23 12:34:56', 654321);
INSERT INTO Visit (VisitorId, EmployeeId, StartDate, Reference)
VALUES (3, 1, '2022-04-05 22:14:53', 192837);
INSERT INTO Admin (Username, HashedPassword)
VALUES ('antonant', 'nurwheofwhegfuhhoghfwer');
INSERT INTO Admin (Username, HashedPassword)
VALUES ('berntber', 'ofwhefwepurf8ruy29try34o9ghreh');
INSERT INTO Admin (Username, HashedPassword)
VALUES ('danieldan', 'rgwqeoifwhgfgiuwhwighwiguh');

INSERT INTO EventLog(Date, Description)
VALUES ('2020-10-21 11:22:33', 'User antonant checked in Sara Bernardsen0.');
INSERT INTO EventLog(Date, Description)
VALUES ('2020-10-22 10:40:33', 'User berntber logged in.');
INSERT INTO EventLog(Date, Description)
VALUES ('2020-10-26 10:40:33', 'User antonant created user danieldan.');