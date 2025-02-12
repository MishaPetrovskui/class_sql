CREATE TABLE Product (
  ProductID INT AUTO_INCREMENT KEY,
  Name VARCHAR(100),
  Description VARCHAR(255)
);

CREATE TABLE Departments (
  ID INT AUTO_INCREMENT KEY,
  Name VARCHAR(100)
  Description VARCHAR(255)
);
DESCRIBE Departments;
CREATE TABLE Doctors (
  ID INT AUTO_INCREMENT KEY,
  Name VARCHAR(100),
  Birthday DATE,
  DepartmentID INT,
  FOREIGN KEY (DepartmentID) REFERENCES Departments (ID)
);

DESCRIBE Doctors;
