
IF DB_ID(N'it38web') IS NULL
BEGIN
    CREATE DATABASE it38web;
END;
--GO

USE it38web;
--GO


        CREATE TABLE it38web.dbo.Users (
		Id int IDENTITY(1,1) NOT NULL,
		Username nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		Email nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		PasswordHash nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		CreatedAt datetime2 DEFAULT sysutcdatetime() NOT NULL,
		CONSTRAINT PK_Users PRIMARY KEY (Id),
		CONSTRAINT UQ_Users_Email UNIQUE (Email)
	);
--GO


    CREATE TABLE it38web.dbo.Notes (
		Id int IDENTITY(1,1) NOT NULL,
		Title nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		Content nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		CreatedAt datetime2 DEFAULT sysutcdatetime() NOT NULL,
		UpdatedAt datetime2 NULL,
		UserId int NOT NULL,
		CONSTRAINT PK_Notes PRIMARY KEY (Id)
	);
	
	
	
	ALTER TABLE it38web.dbo.Notes ADD CONSTRAINT FK_Notes_Users FOREIGN KEY (UserId) REFERENCES it38web.dbo.Users(Id) ON DELETE CASCADE;
--GO


    
	CREATE TABLE it38web.dbo.NoteShares (
		Id int IDENTITY(1,1) NOT NULL,
		NoteId int NOT NULL,
		UserId int NOT NULL,
		CanEdit bit DEFAULT 0 NOT NULL,
		SharedAt datetime2 DEFAULT sysutcdatetime() NOT NULL,
		CONSTRAINT PK_NoteShares PRIMARY KEY (Id),
		CONSTRAINT UQ_NoteShares_Note_User UNIQUE (NoteId,UserId)
	);
	
	
	
	ALTER TABLE it38web.dbo.NoteShares ADD CONSTRAINT FK_NoteShares_Notes FOREIGN KEY (NoteId) REFERENCES it38web.dbo.Notes(Id) ON DELETE CASCADE;
	ALTER TABLE it38web.dbo.NoteShares ADD CONSTRAINT FK_NoteShares_Users FOREIGN KEY (UserId) REFERENCES it38web.dbo.Users(Id);
--GO
        
INSERT INTO it38web.dbo.Users (Username,Email,PasswordHash,CreatedAt) VALUES
	 (N'Hoan',N'leviethoan@gmail.com',N'29GcUBM3bmsaWfz1x9Na9A==.0DcYruE5bFuWqzFLntIVVi0KlF5w+vLPODuczWq4CRg=','2026-09-14 15:55:18.424'),
	 (N'Quyen',N'quyenphan123@gmail.com',N'8KpzUHq/DVcBx0iM/4wv0A==.Zdba/6VSfWRwNBwllOi08JSZEwn848VHO13Ja7//SgY=','2026-09-14 16:42:15.585'),
	 (N'Long',N'ngoclong@gmail.com',N'XouC98wLzKLPz5OAQYr0iA==.HUX7L2ijjcUZciQD3Mf/fOFyMFSHBzZjC5ymQcGrMKA=','2026-09-15 17:30:52.969');
--GO        
INSERT INTO it38web.dbo.Notes (Title,Content,CreatedAt,UpdatedAt,UserId) VALUES
	 (N'Lorem ipsum dolor sit amet',N'Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum','2026-09-15 16:44:04.088','2026-09-15 17:33:17.908',1),
	 (N'Lorem ipsum dolor sit amet update',N'Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum','2026-09-15 16:47:21.327',NULL,1),
	 (N'Lorem ipsum',N'Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum','2026-09-15 16:47:24.960',NULL,1),
	 (N'update Chưa có ghi chú nào',N'Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum','2026-09-15 16:47:27.977','2026-09-15 17:33:09.159',1),
	 (N'Lorem ipsum dolor sit amet 123',N'Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum','2026-09-15 16:47:30.659',NULL,1);
--GO
INSERT INTO it38web.dbo.NoteShares (NoteId,UserId,CanEdit,SharedAt) VALUES
	 (1,2,0,'2026-09-15 16:48:16.647'),
	 (2,2,1,'2026-09-15 17:09:03.902'),
	 (1,1,1,'2026-09-15 17:31:33.148'),
	 (2,1,1,'2026-09-15 17:32:41.758');
--GO

SELECT Id, Username, Email, CreatedAt
FROM it38web.dbo.Users;

SELECT Id, Title, UserId, CreatedAt, UpdatedAt
FROM it38web.dbo.Notes;

SELECT *
FROM it38web.dbo.NoteShares;
 -- GO
