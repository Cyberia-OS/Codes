CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    GroupName NVARCHAR(20) NOT NULL
);

CREATE TABLE Subjects (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Grades (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    SubjectId INT NOT NULL,
    Grade INT NOT NULL CHECK (Grade BETWEEN 1 AND 5),
    GradeDate VARCHAR(10) NOT NULL,
    CONSTRAINT FK_Grades_Students FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Grades_Subjects FOREIGN KEY (SubjectId) REFERENCES Subjects(Id) ON DELETE CASCADE
);

INSERT INTO Students (FullName, GroupName) VALUES ('Иван Иванов', 'ПВ-111'), ('Анна Петрова', 'ПВ-112');
INSERT INTO Subjects (Name) VALUES ('Программирование ADO.NET'), ('Базы данных');
INSERT INTO Grades (StudentId, SubjectId, Grade, GradeDate) VALUES (1, 1, 5, '2026-10-14'), (2, 2, 4, '2026-10-14');
