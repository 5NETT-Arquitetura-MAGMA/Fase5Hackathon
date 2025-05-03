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

CREATE TABLE [__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);

INSERT INTO [__EFMigrationsHistory] (MigrationId,ProductVersion) VALUES
	 (N'20250411011312_InitialMigration',N'9.0.4'),
	 (N'20250411011712_AddSpecialityColumn',N'9.0.4'),
	 (N'20250418130650_AddSecurityHash',N'9.0.4'),
	 (N'20250428234338_fixScheduleRelations',N'9.0.4'),
	 (N'20250502213918_AddDoctorOffDays',N'9.0.4');

-- Users definição

-- Drop table

-- DROP TABLE Users;

CREATE TABLE Users (
	Id uniqueidentifier NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PhoneNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	EmailAddress nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Login] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type] int NOT NULL,
	CreationTime datetime2 NOT NULL,
	UpdateTime datetime2 NULL,
	Specialty nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	SecurityHash nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'' NOT NULL,
	DoctorConsultationStatusId uniqueidentifier NULL,
	PatientConsultationStatusId uniqueidentifier NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id)
);
 CREATE NONCLUSTERED INDEX IX_Users_DoctorConsultationStatusId ON Users (  DoctorConsultationStatusId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Users_PatientConsultationStatusId ON Users (  PatientConsultationStatusId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- Users chaves estrangeiras


-- MedicalConsultations definição

-- Drop table

-- DROP TABLE MedicalConsultations;

CREATE TABLE MedicalConsultations (
	Id uniqueidentifier NOT NULL,
	DoctorId uniqueidentifier NOT NULL,
	PatientId uniqueidentifier NOT NULL,
	ScheduledDate datetime2 NOT NULL,
	Status int NOT NULL,
	Justification nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CreationTime datetime2 NOT NULL,
	UpdateTime datetime2 NULL,
	CONSTRAINT PK_MedicalConsultations PRIMARY KEY (Id)
);
 CREATE NONCLUSTERED INDEX IX_MedicalConsultations_DoctorId ON MedicalConsultations (  DoctorId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_MedicalConsultations_PatientId ON MedicalConsultations (  PatientId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- MedicalConsultations chaves estrangeiras

ALTER TABLE MedicalConsultations ADD CONSTRAINT FK_MedicalConsultations_Users_DoctorId FOREIGN KEY (DoctorId) REFERENCES Users(Id);
ALTER TABLE MedicalConsultations ADD CONSTRAINT FK_MedicalConsultations_Users_PatientId FOREIGN KEY (PatientId) REFERENCES Users(Id);
ALTER TABLE Users ADD CONSTRAINT FK_Users_MedicalConsultations_DoctorConsultationStatusId FOREIGN KEY (DoctorConsultationStatusId) REFERENCES MedicalConsultations(Id);
ALTER TABLE Users ADD CONSTRAINT FK_Users_MedicalConsultations_PatientConsultationStatusId FOREIGN KEY (PatientConsultationStatusId) REFERENCES MedicalConsultations(Id);

-- DoctorSchedules definição

-- Drop table

-- DROP TABLE DoctorSchedules;

CREATE TABLE DoctorSchedules (
	Id uniqueidentifier NOT NULL,
	DayOfWeek int NOT NULL,
	StartTime time NULL,
	EndTime time NULL,
	DoctorId uniqueidentifier NOT NULL,
	CreationTime datetime2 NOT NULL,
	UpdateTime datetime2 NULL,
	CONSTRAINT PK_DoctorSchedules PRIMARY KEY (Id)
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_DoctorSchedules_DoctorId_DayOfWeek ON DoctorSchedules (  DoctorId ASC  , DayOfWeek ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DoctorSchedules chaves estrangeiras

ALTER TABLE DoctorSchedules ADD CONSTRAINT FK_DoctorSchedules_Users_DoctorId FOREIGN KEY (DoctorId) REFERENCES Users(Id) ON DELETE CASCADE;

-- DoctorOffDays definição

-- Drop table

-- DROP TABLE DoctorOffDays;

CREATE TABLE DoctorOffDays (
	Id uniqueidentifier NOT NULL,
	OffDate datetime2 NOT NULL,
	DoctorId uniqueidentifier NOT NULL,
	CreationTime datetime2 NOT NULL,
	UpdateTime datetime2 NULL,
	CONSTRAINT PK_DoctorOffDays PRIMARY KEY (Id)
);
 CREATE NONCLUSTERED INDEX IX_DoctorOffDays_DoctorId ON DoctorOffDays (  DoctorId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DoctorOffDays chaves estrangeiras

ALTER TABLE DoctorOffDays ADD CONSTRAINT FK_DoctorOffDays_Users_DoctorId FOREIGN KEY (DoctorId) REFERENCES Users(Id) ON DELETE CASCADE;

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