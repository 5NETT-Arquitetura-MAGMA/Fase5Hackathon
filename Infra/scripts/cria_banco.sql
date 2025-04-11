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
	 (N'20250411011712_AddSpecialityColumn',N'9.0.4');


CREATE TABLE Users (
	Id uniqueidentifier NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PhoneNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	EmailAddress nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Login] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type] int NOT NULL,
	DoctorConsultationStatus int NOT NULL,
	PatientConsultationStatus int NOT NULL,
	CreationTime datetime2 NOT NULL,
	UpdateTime datetime2 NULL,
	Specialty nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id)
);


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



ALTER TABLE MedicalConsultations ADD CONSTRAINT FK_MedicalConsultations_Users_DoctorId FOREIGN KEY (DoctorId) REFERENCES Users(Id);
ALTER TABLE MedicalConsultations ADD CONSTRAINT FK_MedicalConsultations_Users_PatientId FOREIGN KEY (PatientId) REFERENCES Users(Id);

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



ALTER TABLE DoctorSchedules ADD CONSTRAINT FK_DoctorSchedules_Users_DoctorId FOREIGN KEY (DoctorId) REFERENCES Users(Id) ON DELETE CASCADE;