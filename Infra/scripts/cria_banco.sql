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



CREATE FUNCTION dbo.CleanLogin (@login VARCHAR(255))
RETURNS VARCHAR(255)
AS
BEGIN
    -- Remover espaços
    SET @login = REPLACE(@login, ' ', '');

    -- Remover caracteres não alfanuméricos
    WHILE PATINDEX('%[^a-zA-Z0-9]%', @login) > 0
    BEGIN
        SET @login = STUFF(@login, PATINDEX('%[^a-zA-Z0-9]%', @login), 1, '');
    END

    RETURN @login;
END;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [EmailAddress] nvarchar(max) NOT NULL,
    [Login] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Type] int NOT NULL,
    [DoctorConsultationStatus] int NOT NULL,
    [PatientConsultationStatus] int NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [DoctorSchedules] (
    [Id] uniqueidentifier NOT NULL,
    [DayOfWeek] int NOT NULL,
    [StartTime] time NULL,
    [EndTime] time NULL,
    [DoctorId] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_DoctorSchedules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DoctorSchedules_Users_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [MedicalConsultations] (
    [Id] uniqueidentifier NOT NULL,
    [DoctorId] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [ScheduledDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [Justification] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_MedicalConsultations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MedicalConsultations_Users_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MedicalConsultations_Users_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_DoctorSchedules_DoctorId_DayOfWeek] ON [DoctorSchedules] ([DoctorId], [DayOfWeek]);

CREATE INDEX [IX_MedicalConsultations_DoctorId] ON [MedicalConsultations] ([DoctorId]);

CREATE INDEX [IX_MedicalConsultations_PatientId] ON [MedicalConsultations] ([PatientId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250411011312_InitialMigration', N'9.0.4');

ALTER TABLE [Users] ADD [Specialty] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250411011712_AddSpecialityColumn', N'9.0.4');

ALTER TABLE [Users] ADD [SecurityHash] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250418130650_AddSecurityHash', N'9.0.4');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'DoctorConsultationStatus');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Users] DROP COLUMN [DoctorConsultationStatus];

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'PatientConsultationStatus');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] DROP COLUMN [PatientConsultationStatus];

ALTER TABLE [Users] ADD [DoctorConsultationStatusId] uniqueidentifier NULL;

ALTER TABLE [Users] ADD [PatientConsultationStatusId] uniqueidentifier NULL;

CREATE INDEX [IX_Users_DoctorConsultationStatusId] ON [Users] ([DoctorConsultationStatusId]);

CREATE INDEX [IX_Users_PatientConsultationStatusId] ON [Users] ([PatientConsultationStatusId]);

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_MedicalConsultations_DoctorConsultationStatusId] FOREIGN KEY ([DoctorConsultationStatusId]) REFERENCES [MedicalConsultations] ([Id]);

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_MedicalConsultations_PatientConsultationStatusId] FOREIGN KEY ([PatientConsultationStatusId]) REFERENCES [MedicalConsultations] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250428234338_fixScheduleRelations', N'9.0.4');

CREATE TABLE [DoctorOffDays] (
    [Id] uniqueidentifier NOT NULL,
    [OffDate] datetime2 NOT NULL,
    [DoctorId] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [UpdateTime] datetime2 NULL,
    CONSTRAINT [PK_DoctorOffDays] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DoctorOffDays_Users_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_DoctorOffDays_DoctorId] ON [DoctorOffDays] ([DoctorId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250502213918_AddDoctorOffDays', N'9.0.4');

ALTER TABLE [MedicalConsultations] ADD [ScheduleTime] time NOT NULL DEFAULT '00:00:00';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250508234452_AddScheduleTimeColumn', N'9.0.4');

COMMIT;
GO

