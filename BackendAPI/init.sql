-- Postgres automatically creates the DB 'AttendanceDb' based on the Env Var

-- Create Tables
CREATE TABLE IF NOT EXISTS Schools (
    SchoolId SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Grades (
    GradeId SERIAL PRIMARY KEY,
    SchoolId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    FOREIGN KEY (SchoolId) REFERENCES Schools(SchoolId)
);

CREATE TABLE IF NOT EXISTS Students (
    StudentId SERIAL PRIMARY KEY,
    GradeId INT NOT NULL,
    FullName VARCHAR(200) NOT NULL,
    FOREIGN KEY (GradeId) REFERENCES Grades(GradeId)
);

CREATE TABLE IF NOT EXISTS Attendance (
    AttendanceId SERIAL PRIMARY KEY,
    StudentId INT NOT NULL,
    ClassDate DATE NOT NULL,
    IsPresent BOOLEAN NOT NULL,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);

-- Seed Data (using DO block to handle "if not exists" logic simply)

INSERT INTO Schools (Name) VALUES ('Central High School') ON CONFLICT DO NOTHING;

-- Note: For simple seeding, we assume IDs 1, 2, etc. for simplicity in this script
-- Or we look them up. Here is a simple lookup approach:

DO $$
DECLARE 
    v_SchoolId INT;
    v_GradeA_Id INT;
    v_GradeB_Id INT;
BEGIN
    SELECT SchoolId INTO v_SchoolId FROM Schools WHERE Name = 'Central High School';

    -- Seed Grades
    IF NOT EXISTS (SELECT 1 FROM Grades WHERE Name = '1°A' AND SchoolId = v_SchoolId) THEN
        INSERT INTO Grades (SchoolId, Name) VALUES (v_SchoolId, '1°A') RETURNING GradeId INTO v_GradeA_Id;
    ELSE
        SELECT GradeId INTO v_GradeA_Id FROM Grades WHERE Name = '1°A';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM Grades WHERE Name = '1°B' AND SchoolId = v_SchoolId) THEN
        INSERT INTO Grades (SchoolId, Name) VALUES (v_SchoolId, '1°B') RETURNING GradeId INTO v_GradeB_Id;
    ELSE
         SELECT GradeId INTO v_GradeB_Id FROM Grades WHERE Name = '1°B';
    END IF;

    -- Seed Students
    IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Juan Perez') THEN
        INSERT INTO Students (GradeId, FullName) VALUES (v_GradeA_Id, 'Juan Perez');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Maria Lopez') THEN
        INSERT INTO Students (GradeId, FullName) VALUES (v_GradeA_Id, 'Maria Lopez');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM Students WHERE FullName = 'Carlos Sanchez') THEN
        INSERT INTO Students (GradeId, FullName) VALUES (v_GradeB_Id, 'Carlos Sanchez');
    END IF;
END $$;