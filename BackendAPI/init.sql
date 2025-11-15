-- File: init.sql

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AttendanceDb')
BEGIN
    CREATE DATABASE AttendanceDb;
END
GO

-- Switch to the new database context
USE AttendanceDb;
GO

-- Create Schools table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Schools')
BEGIN
    CREATE TABLE Schools (
        SchoolId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL
    );
END
GO

-- Create Grades table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Grades')
BEGIN
    CREATE TABLE Grades (
        GradeId INT PRIMARY KEY IDENTITY(1,1),
        SchoolId INT NOT NULL,
        Name NVARCHAR(50) NOT NULL, -- e.g., "1°A", "2°B"
        FOREIGN KEY (SchoolId) REFERENCES Schools(SchoolId)
    );
END
GO

-- Create Students table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
BEGIN
    CREATE TABLE Students (
        StudentId INT PRIMARY KEY IDENTITY(1,1),
        GradeId INT NOT NULL,
        FullName NVARCHAR(200) NOT NULL,
        FOREIGN KEY (GradeId) REFERENCES Grades(GradeId)
    );
END
GO

-- Create Attendance table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Attendance')
BEGIN
    CREATE TABLE Attendance (
        AttendanceId INT PRIMARY KEY IDENTITY(1,1),
        StudentId INT NOT NULL,
        ClassDate DATE NOT NULL,
        IsPresent BIT NOT NULL,
        FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
    );
END
GO

--- SEED TEST DATA (Idempotent) ---

-- Seed a test school
IF NOT EXISTS (SELECT 1 FROM Schools WHERE Name = 'Central High School')
BEGIN
    INSERT INTO Schools (Name) VALUES ('Central High School');
END
GO

-- Seed test grades
DECLARE @SchoolId INT = (SELECT SchoolId FROM Schools WHERE Name = 'Central High School');

IF NOT EXISTS (SELECT 1 FROM Grades WHERE Name = '1°A' AND SchoolId = @SchoolId)
BEGIN
    INSERT INTO Grades (SchoolId, Name) VALUES (@SchoolId, '1°A');
END
GO

IF NOT EXISTS (SELECT 1 FROM Grades WHERE Name = '1°B' AND SchoolId = @SchoolId)
BEGIN
    INSERT INTO Grades (SchoolId, Name) VALUES (@SchoolId, '1°B');
END
GO

-- Seed test students
DECLARE @GradeA INT = (SELECT GradeId FROM Grades WHERE Name = '1°A');
DECLARE @GradeB INT = (SELECT GradeId FROM Grades WHERE Name = '1°B');

IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Juan Perez')
BEGIN
    INSERT INTO Students (GradeId, FullName) VALUES (@GradeA, 'Juan Perez');
END
GO

IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Maria Lopez')
BEGIN
    INSERT INTO Students (GradeId, FullName) VALUES (@GradeA, 'Maria Lopez');
END
GO

IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Carlos Sanchez')
BEGIN
    INSERT INTO Students (GradeId, FullName) VALUES (@GradeB, 'Carlos Sanchez');
END
GO
IF NOT EXISTS (SELECT 1 FROM Grades WHERE Name = '1°A' AND SchoolId = @SchoolId)
BEGIN
    INSERT INTO Grades (SchoolId, Name) VALUES (@SchoolId, '1°A');
END

IF NOT EXISTS (SELECT 1 FROM Grades WHERE Name = '1°B' AND SchoolId = @SchoolId)
BEGIN
    INSERT INTO Grades (SchoolId, Name) VALUES (@SchoolId, '1°B');
END
GO

-- Seed test students
DECLARE @GradeA INT = (SELECT GradeId FROM Grades WHERE Name = '1°A');
DECLARE @GradeB INT = (SELECT GradeId FROM Grades WHERE Name = '1°B');

IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Juan Perez')
BEGIN
    INSERT INTO Students (GradeId, FullName) VALUES (@GradeA, 'Juan Perez');
END

IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Maria Lopez')
BEGIN
    INSERT INTO Students (GradeId, FullName) VALUES (@GradeA, 'Maria Lopez');
END

IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Carlos Sanchez')
BEGIN
    INSERT INTO Students (GradeId, FullName) VALUES (@GradeB, 'Carlos Sanchez');
END
GO