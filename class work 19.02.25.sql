-- INIT database
CREATE TABLE Students (
  ID INT AUTO_INCREMENT KEY,
  Name VARCHAR(100),
  GroupName VARCHAR(16),
  AVGGrade DOUBLE,
  Birthday DATE,
  EnterYear INT
);

INSERT INTO Students (Name, GroupName, AVGGrade, Birthday, EnterYear) 
VALUES 
    ('Олексій', 'ІТ-21', 4.7, '2002-05-14', 2020), 
    ('Марія', 'ІТ-21', 4.9, '2003-08-22', 2021), 
    ('Іван', 'ІТ-22', 3.8, '2001-11-10', 2023), 
    ('Андрій', 'ІТ-23', 4.2, '2002-03-30', 2022), 
    ('Софія', 'ІТ-22', 4.5, '2001-07-19',2019);
    
SELECT * FROM Students ORDER BY Name;
SELECT * FROM Students ORDER BY AVGGrade DESC;
SELECT * FROM Students WHERE (2025 - EnterYear) IN (2, 4);
SELECT * FROM Students WHERE AVGGrade BETWEEN 3.0 AND 4.5;




CREATE TABLE Faculties (
    Id INT AUTO_INCREMENT KEY,
    Name VARCHAR(255),
    Dean VARCHAR(255)
);

INSERT INTO Faculties (Name, Dean) VALUES
('Факультет Інформатики', 'Петренко Олег'),
('Факультет Економіки', 'Іваненко Марія');

SELECT * FROM Faculties;

CREATE TABLE Departments (
    Id INT AUTO_INCREMENT KEY,
    Name VARCHAR(255),
    Financing DECIMAL
);

INSERT INTO Departments (Name, Financing) VALUES
('Кафедра Програмування', 120000.50),
('Кафедра Маркетингу', 95000.75);

SELECT * FROM Departments;

CREATE TABLE Teachers (
    Id INT AUTO_INCREMENT KEY,
    Name VARCHAR(255),
    Surname VARCHAR(255),
    Position VARCHAR(255),
    EmploymentDate DATE,
    IsAssistant BOOLEAN,
    IsProfessor BOOLEAN,
    Salary DECIMAL,
    Premium DECIMAL
);

INSERT INTO Teachers (Name, Surname, Position, EmploymentDate, IsAssistant, IsProfessor, Salary, Premium) VALUES
('Олександр', 'Сидоренко', 'Доцент', '2010-09-01', TRUE, FALSE, 15000.00, 2000.00),
('Тетяна', 'Ковальчук', 'Професор', '2005-08-15', FALSE, TRUE, 25000.00, 5000.00);

SELECT * FROM Teachers;

CREATE TABLE Groups1 (
    Id INT AUTO_INCREMENT KEY,
    Name VARCHAR(50),
    Rating DECIMAL,
    Year1 INT
);

INSERT INTO Groups1 (Name, Rating, Year1) VALUES
('ІТ-21', 4.5, 2021),
('ЕК-22', 3.8, 2022),
('ІТ-22', 4.3, 2023),
('ЕК-23', 3.5, 2024),
('NET', 5.0, 2025);

SELECT * FROM Groups1;
