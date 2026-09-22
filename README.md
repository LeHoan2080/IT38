CREATE TABLE it13webcsdl.dbo.Users (
	Id int IDENTITY(1,1) NOT NULL,
	Username nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PasswordHash nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT sysutcdatetime() NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT UQ_Users_Email UNIQUE (Email)
);

-- it13webcsdl.dbo.Notes definition

-- Drop table

-- DROP TABLE it13webcsdl.dbo.Notes;

CREATE TABLE it13webcsdl.dbo.Notes (
	Id int IDENTITY(1,1) NOT NULL,
	Title nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Content nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT sysutcdatetime() NOT NULL,
	UpdatedAt datetime2 NULL,
	UserId int NOT NULL,
	CONSTRAINT PK_Notes PRIMARY KEY (Id)
);


-- it13webcsdl.dbo.Notes foreign keys

ALTER TABLE it13webcsdl.dbo.Notes ADD CONSTRAINT FK_Notes_Users FOREIGN KEY (UserId) REFERENCES it13webcsdl.dbo.Users(Id) ON DELETE CASCADE;

-- it13webcsdl.dbo.NoteShares definition

-- Drop table

-- DROP TABLE it13webcsdl.dbo.NoteShares;

CREATE TABLE it13webcsdl.dbo.NoteShares (
	Id int IDENTITY(1,1) NOT NULL,
	NoteId int NOT NULL,
	UserId int NOT NULL,
	CanEdit bit DEFAULT 0 NOT NULL,
	SharedAt datetime2 DEFAULT sysutcdatetime() NOT NULL,
	CONSTRAINT PK_NoteShares PRIMARY KEY (Id),
	CONSTRAINT UQ_NoteShares_Note_User UNIQUE (NoteId,UserId)
);


-- it13webcsdl.dbo.NoteShares foreign keys

ALTER TABLE it13webcsdl.dbo.NoteShares ADD CONSTRAINT FK_NoteShares_Notes FOREIGN KEY (NoteId) REFERENCES it13webcsdl.dbo.Notes(Id) ON DELETE CASCADE;
ALTER TABLE it13webcsdl.dbo.NoteShares ADD CONSTRAINT FK_NoteShares_Users FOREIGN KEY (UserId) REFERENCES it13webcsdl.dbo.Users(Id);
