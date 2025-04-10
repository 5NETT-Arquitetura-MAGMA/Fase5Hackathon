CREATE DATABASE [fiap-hackathon];
GO

USE [fiap-hackathon];
GO
CREATE LOGIN [hackathon-adm] WITH PASSWORD = '7zkpNS2RFr0v';
GO
CREATE USER [hackathon-adm] FOR LOGIN [hackathon-adm];
GO

ALTER ROLE db_owner ADD MEMBER [hackathon-adm];
GO