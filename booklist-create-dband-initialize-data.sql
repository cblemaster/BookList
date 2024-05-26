USE master
GO

DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'BookList')
BEGIN
    SET @SQL = N'USE BookList;

                 ALTER DATABASE BookList SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE master;

                 DROP DATABASE BookList;';
    EXEC (@SQL);
END;

CREATE DATABASE BookList
GO

USE BookList
GO

CREATE TABLE Author (
	Id				    int IDENTITY(1,1)					NOT NULL,
	[Name]				varchar(100)                        NOT NULL,
	IsFavorite			bit									NOT NULL,
	CONSTRAINT PK_Author PRIMARY KEY(Id),
	CONSTRAINT UC_Author_Name UNIQUE([Name]),
)
GO

CREATE TABLE Genre (
	Id					int IDENTITY(1,1)					NOT NULL,
	[Name]				varchar(100)						NOT NULL,
	IsFavorite			bit									NOT NULL,
	CONSTRAINT PK_Genre PRIMARY KEY(Id),
	CONSTRAINT UC_Genre_Name UNIQUE([Name]),
)
GO

CREATE TABLE Book (
	Id					int IDENTITY(1,1)					NOT NULL,
	Title				varchar(100)						NOT NULL,
	Subtitle			varchar(100)						NULL,
	IsFavorite			bit									NOT NULL,
	GenreId				int									NOT NULL,
	Publisher			varchar(100)						NULL,
	[PageCount]			int									NULL,
	[Description]		varchar(255)						NULL,
	CONSTRAINT PK_Book PRIMARY KEY(Id),
	CONSTRAINT FK_Book_Genre FOREIGN KEY(GenreId) REFERENCES Genre(Id),
)
GO

CREATE TABLE BookAuthor (
	BookId				int									NOT NULL,
	AuthorId			int									NOT NULL,
	CONSTRAINT PK_BookAuthor PRIMARY KEY(BookId, AuthorId),
	CONSTRAINT FK_BookAuthor_Book FOREIGN KEY(BookId) REFERENCES Book(Id),
	CONSTRAINT FK_BookAuthor_Author FOREIGN KEY(AuthorId) REFERENCES Author(Id),
)
GO

INSERT INTO Genre([Name],IsFavorite) VALUES('Non Fiction',1);
INSERT INTO Genre([Name],IsFavorite) VALUES('Classics',1);
INSERT INTO Genre([Name],IsFavorite) VALUES('Western',0);
INSERT INTO Genre([Name],IsFavorite) VALUES('Romance',0);
INSERT INTO Genre([Name],IsFavorite) VALUES('Science Fiction',1);
INSERT INTO Genre([Name],IsFavorite) VALUES('Fantasy',1);
INSERT INTO Genre([Name],IsFavorite) VALUES('Mystery',0);
INSERT INTO Genre([Name],IsFavorite) VALUES('Gothic',0);
INSERT INTO Genre([Name],IsFavorite) VALUES('Legal Thriller',0);
INSERT INTO Genre([Name],IsFavorite) VALUES('Military Fiction',0);

INSERT INTO Author([Name],IsFavorite) VALUES('Arthur C. Clarke',1);
INSERT INTO Author([Name],IsFavorite) VALUES('Neal Stephenson',2);
INSERT INTO Author([Name],IsFavorite) VALUES('John Steinbeck',1);
INSERT INTO Author([Name],IsFavorite) VALUES('Jon Krakauer',1);
INSERT INTO Author([Name],IsFavorite) VALUES('John Grisham',0);
INSERT INTO Author([Name],IsFavorite) VALUES('Tom Clancy',0);
INSERT INTO Author([Name],IsFavorite) VALUES('Edgar Allen Poe',1);
INSERT INTO Author([Name],IsFavorite) VALUES('George RR Martin',1);
INSERT INTO Author([Name],IsFavorite) VALUES('Stephen Baxter',0);
INSERT INTO Author([Name],IsFavorite) VALUES('George Orwell',0);

INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('Rendevous with Rama',0,(SELECT Id FROM Genre WHERE Genre.Name='Science Fiction'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('Fall',1,(SELECT Id FROM Genre WHERE Genre.Name='Science Fiction'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('Seven Eves',1,(SELECT Id FROM Genre WHERE Genre.Name='Science Fiction'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('The Grapes of Wrath',1,(SELECT Id FROM Genre WHERE Genre.Name='Classics'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('Into Thin Air',1,(SELECT Id FROM Genre WHERE Genre.Name='Non Fiction'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('A Time to Kill',0,(SELECT Id FROM Genre WHERE Genre.Name='Legal Thriller'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('The Hunt for Red October',0,(SELECT Id FROM Genre WHERE Genre.Name='Military Fiction'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('The Narrative of Arthur Gordon Pym of Nantucket',0,(SELECT Id FROM Genre WHERE Genre.Name='Gothic'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('A Storm of Swords',1,(SELECT Id FROM Genre WHERE Genre.Name='Fantasy'));
INSERT INTO Book([Title],IsFavorite,GenreId) VALUES ('The Light of Other Days',0,(SELECT Id FROM Genre WHERE Genre.Name='Science Fiction'));

INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='Rendevous with Rama'),(SELECT Id FROM Author WHERE Author.Name='Arthur C. Clarke'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='Fall'),(SELECT Id FROM Author WHERE Author.Name='Neal Stephenson'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='Seven Eves'),(SELECT Id FROM Author WHERE Author.Name='Neal Stephenson'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='The Grapes of Wrath'),(SELECT Id FROM Author WHERE Author.Name='John Steinbeck'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='Into Thin Air'),(SELECT Id FROM Author WHERE Author.Name='Jon Krakauer'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='The Hunt for Red October'),(SELECT Id FROM Author WHERE Author.Name='Tom Clancy'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='The Narrative of Arthur Gordon Pym of Nantucket'),(SELECT Id FROM Author WHERE Author.Name='Edgar Allen Poe'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='A Storm of Swords'),(SELECT Id FROM Author WHERE Author.Name='George RR Martin'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='A Time to Kill'),(SELECT Id FROM Author WHERE Author.Name='John Grisham'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='The Light of Other Days'),(SELECT Id FROM Author WHERE Author.Name='Arthur C. Clarke'));
INSERT INTO BookAuthor(BookId,AuthorId) VALUES((SELECT Id FROM Book WHERE Book.Title='The Light of Other Days'),(SELECT Id FROM Author WHERE Author.Name='Stephen Baxter'));



