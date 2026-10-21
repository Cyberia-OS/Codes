--удаление старой бд (если была)
USE master;
GO
IF DB_ID('MailingListDb') IS NOT NULL
BEGIN
    ALTER DATABASE MailingListDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MailingListDb;
END
GO

--создание бд
CREATE DATABASE MailingListDb;
GO
USE MailingListDb;
GO

--задание 1: таблицы
CREATE TABLE Countries(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
CREATE TABLE Cities(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CountryId INT NOT NULL REFERENCES Countries(Id)
);
CREATE TABLE Customers(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Gender NVARCHAR(1) NOT NULL,
    CityId INT NOT NULL REFERENCES Cities(Id)
);
CREATE TABLE Sections(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
CREATE TABLE Products(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    SectionId INT NOT NULL REFERENCES Sections(Id),
    IsPromo BIT NOT NULL DEFAULT 0,
    PromoEndDate DATE NULL
);
CREATE TABLE Subscriptions(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL REFERENCES Customers(Id),
    SectionId INT NOT NULL REFERENCES Sections(Id)
);
CREATE TABLE PromoArchive(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OriginalProductId INT NOT NULL,
    ProductName NVARCHAR(100),
    SectionName NVARCHAR(100),
    PromoEndDate DATE,
    ArchivedAt DATETIME NOT NULL DEFAULT GETDATE()
);
CREATE TABLE ArchiveLog(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Message NVARCHAR(300),
    LoggedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

--представления
CREATE VIEW vw_SectionPopularity AS
SELECT s.Id, s.Name, COUNT(sub.Id) AS SubscriberCount
FROM Sections s
LEFT JOIN Subscriptions sub ON sub.SectionId = s.Id
GROUP BY s.Id, s.Name;
GO

CREATE VIEW vw_SectionPromoCount AS
SELECT s.Id, s.Name, COUNT(p.Id) AS PromoCount
FROM Sections s
LEFT JOIN Products p ON p.SectionId = s.Id AND p.IsPromo = 1
GROUP BY s.Id, s.Name;
GO

--пользовательская функция
CREATE FUNCTION fn_DaysUntilPromoEnd(@d DATE)
RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(DAY, CAST(GETDATE() AS DATE), @d);
END
GO

--задание 2: популярность разделов
CREATE PROCEDURE sp_Top3Popular AS
SELECT TOP 3 Id, Name, SubscriberCount
FROM vw_SectionPopularity
ORDER BY SubscriberCount DESC, Name;
GO

CREATE PROCEDURE sp_MostPopular AS
SELECT TOP 1 Id, Name, SubscriberCount
FROM vw_SectionPopularity
ORDER BY SubscriberCount DESC, Name;
GO

CREATE PROCEDURE sp_Top3Unpopular AS
SELECT TOP 3 Id, Name, SubscriberCount
FROM vw_SectionPopularity
ORDER BY SubscriberCount ASC, Name;
GO

CREATE PROCEDURE sp_MostUnpopular AS
SELECT TOP 1 Id, Name, SubscriberCount
FROM vw_SectionPopularity
ORDER BY SubscriberCount ASC, Name;
GO

--задание 3: акционные товары
CREATE PROCEDURE sp_Top3ByPromo AS
SELECT TOP 3 Id, Name, PromoCount
FROM vw_SectionPromoCount
ORDER BY PromoCount DESC, Name;
GO

CREATE PROCEDURE sp_MostPromo AS
SELECT TOP 1 Id, Name, PromoCount
FROM vw_SectionPromoCount
ORDER BY PromoCount DESC, Name;
GO

CREATE PROCEDURE sp_Top3ByLeastPromo AS
SELECT TOP 3 Id, Name, PromoCount
FROM vw_SectionPromoCount
ORDER BY PromoCount ASC, Name;
GO

CREATE PROCEDURE sp_LeastPromo AS
SELECT TOP 1 Id, Name, PromoCount
FROM vw_SectionPromoCount
ORDER BY PromoCount ASC, Name;
GO

--задание 4: акции и средний возраст
CREATE PROCEDURE sp_PromosEndingIn3Days AS
SELECT p.Id, p.Name, s.Name AS Section, p.PromoEndDate,
       dbo.fn_DaysUntilPromoEnd(p.PromoEndDate) AS DaysLeft
FROM Products p
JOIN Sections s ON s.Id = p.SectionId
WHERE p.IsPromo = 1 AND dbo.fn_DaysUntilPromoEnd(p.PromoEndDate) = 3;
GO

CREATE PROCEDURE sp_ExpiredPromos AS
SELECT p.Id, p.Name, s.Name AS Section, p.PromoEndDate,
       dbo.fn_DaysUntilPromoEnd(p.PromoEndDate) AS DaysLeft
FROM Products p
JOIN Sections s ON s.Id = p.SectionId
WHERE p.IsPromo = 1 AND dbo.fn_DaysUntilPromoEnd(p.PromoEndDate) < 0;
GO

CREATE PROCEDURE sp_ArchiveExpiredPromos AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO PromoArchive(OriginalProductId, ProductName, SectionName, PromoEndDate)
        SELECT p.Id, p.Name, s.Name, p.PromoEndDate
        FROM Products p
        JOIN Sections s ON s.Id = p.SectionId
        WHERE p.IsPromo = 1 AND dbo.fn_DaysUntilPromoEnd(p.PromoEndDate) < 0;

        DECLARE @cnt INT = @@ROWCOUNT;

        DELETE FROM Products
        WHERE IsPromo = 1 AND dbo.fn_DaysUntilPromoEnd(PromoEndDate) < 0;

        COMMIT;
        SELECT @cnt AS ArchivedCount;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

CREATE PROCEDURE sp_AvgAgeBySection AS
SELECT s.Id, s.Name,
       AVG(CAST(c.Age AS FLOAT)) AS AvgAge,
       COUNT(c.Id) AS Customers
FROM Sections s
LEFT JOIN Subscriptions sub ON sub.SectionId = s.Id
LEFT JOIN Customers c ON c.Id = sub.CustomerId
GROUP BY s.Id, s.Name
ORDER BY s.Name;
GO

CREATE PROCEDURE sp_AvgAgeByCity AS
SELECT ci.Id, ci.Name AS City,
       AVG(CAST(c.Age AS FLOAT)) AS AvgAge,
       COUNT(c.Id) AS Customers
FROM Cities ci
LEFT JOIN Customers c ON c.CityId = ci.Id
GROUP BY ci.Id, ci.Name
ORDER BY ci.Name;
GO

CREATE PROCEDURE sp_AvgAgeByCountry AS
SELECT co.Id, co.Name AS Country,
       AVG(CAST(c.Age AS FLOAT)) AS AvgAge,
       COUNT(c.Id) AS Customers
FROM Countries co
LEFT JOIN Cities ci ON ci.CountryId = co.Id
LEFT JOIN Customers c ON c.CityId = ci.Id
GROUP BY co.Id, co.Name
ORDER BY co.Name;
GO

--задание 5: по полу
CREATE PROCEDURE sp_MostPopularByGender AS
WITH Cnts AS (
    SELECT c.Gender, s.Id AS SectionId, s.Name AS Section, COUNT(sub.Id) AS Cnt
    FROM Sections s
    JOIN Subscriptions sub ON sub.SectionId = s.Id
    JOIN Customers c ON c.Id = sub.CustomerId
    GROUP BY c.Gender, s.Id, s.Name
),
Ranked AS (
    SELECT *, ROW_NUMBER() OVER (PARTITION BY Gender ORDER BY Cnt DESC, Section) AS rn
    FROM Cnts
)
SELECT Gender, SectionId, Section, Cnt
FROM Ranked
WHERE rn = 1
ORDER BY Gender;
GO

CREATE PROCEDURE sp_Top3ByGender AS
WITH Cnts AS (
    SELECT c.Gender, s.Id AS SectionId, s.Name AS Section, COUNT(sub.Id) AS Cnt
    FROM Sections s
    JOIN Subscriptions sub ON sub.SectionId = s.Id
    JOIN Customers c ON c.Id = sub.CustomerId
    GROUP BY c.Gender, s.Id, s.Name
),
Ranked AS (
    SELECT *, ROW_NUMBER() OVER (PARTITION BY Gender ORDER BY Cnt DESC, Section) AS rn
    FROM Cnts
)
SELECT Gender, SectionId, Section, Cnt
FROM Ranked
WHERE rn <= 3
ORDER BY Gender, rn;
GO

CREATE PROCEDURE sp_CustomerCountByGender AS
SELECT Gender, COUNT(*) AS Customers
FROM Customers
GROUP BY Gender
ORDER BY Gender;
GO

CREATE PROCEDURE sp_CustomerCountByGenderCountry AS
SELECT co.Name AS Country, c.Gender, COUNT(*) AS Customers
FROM Customers c
JOIN Cities ci ON ci.Id = c.CityId
JOIN Countries co ON co.Id = ci.CountryId
GROUP BY co.Name, c.Gender
ORDER BY co.Name, c.Gender;
GO

--триггер: пишет в лог при архивации
CREATE TRIGGER trg_LogArchive ON PromoArchive
AFTER INSERT
AS
BEGIN
    INSERT INTO ArchiveLog(Message)
    SELECT N'Archived: ' + i.ProductName + N' (' + i.SectionName + N')'
    FROM inserted i;
END
GO

--демо-данные
INSERT INTO Countries(Name) VALUES (N'Россия'),(N'Германия'),(N'США');

INSERT INTO Cities(Name, CountryId) VALUES
    (N'Москва', 1),
    (N'Санкт-Петербург', 1),
    (N'Берлин', 2),
    (N'Мюнхен', 2),
    (N'Нью-Йорк', 3);

INSERT INTO Customers(Name, Age, Gender, CityId) VALUES
    (N'Иван', 25, N'M', 1),
    (N'Мария', 30, N'F', 1),
    (N'Пётр', 45, N'M', 2),
    (N'Анна', 22, N'F', 2),
    (N'Сергей', 50, N'M', 3),
    (N'Ольга', 35, N'F', 3),
    (N'Дмитрий', 28, N'M', 4),
    (N'Елена', 40, N'F', 4),
    (N'Алексей', 33, N'M', 5),
    (N'Татьяна', 27, N'F', 5);

INSERT INTO Sections(Name) VALUES (N'Спорт'),(N'Технологии'),(N'Мода'),(N'Кулинария');

INSERT INTO Subscriptions(CustomerId, SectionId) VALUES
    (1,1),(1,2),(2,1),(2,3),(3,1),(4,2),(5,1),(5,2),
    (6,3),(7,2),(7,4),(8,1),(9,3),(10,4),(3,2);

INSERT INTO Products(Name, SectionId, IsPromo, PromoEndDate) VALUES
    (N'Мяч', 1, 1, DATEADD(DAY, 3, CAST(GETDATE() AS DATE))),
    (N'Гантели', 1, 1, DATEADD(DAY, 10, CAST(GETDATE() AS DATE))),
    (N'Ноутбук', 2, 1, DATEADD(DAY, -2, CAST(GETDATE() AS DATE))),
    (N'Смартфон', 2, 1, DATEADD(DAY, 20, CAST(GETDATE() AS DATE))),
    (N'Платье', 3, 1, DATEADD(DAY, -5, CAST(GETDATE() AS DATE))),
    (N'Кроссовки', 3, 1, DATEADD(DAY, 3, CAST(GETDATE() AS DATE))),
    (N'Книга рецептов', 4, 1, DATEADD(DAY, 15, CAST(GETDATE() AS DATE)));
GO

PRINT N'Готово. Создана БД MailingListDb со всеми объектами и демо-данными.';
GO