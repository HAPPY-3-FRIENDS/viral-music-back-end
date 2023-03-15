CREATE DATABASE ViralMusic
GO

USE ViralMusic
GO

------------------------------ CREATE TABLE ------------------------------
CREATE TABLE [User] (
    Username VARCHAR(16) NOT NULL,
    Password VARCHAR(16) NOT NULL,
    Fullname NVARCHAR(100) NULL,
    Avatar TEXT NULL,
    RoleId INT NOT NULL
)
GO

CREATE TABLE [Role] (
    Id INT IDENTITY(1, 1) NOT NULL,
    RoleName NVARCHAR(16) NOT NULL
)
GO

CREATE TABLE [Playlist] (
   Id INT IDENTITY(1, 1) NOT NULL,
   Name NVARCHAR(100) NOT NULL,
   Image TEXT NULL,
   Owner VARCHAR(16) NOT NULL
)
GO

CREATE TABLE [Track] (
    Id INT IDENTITY(1, 1) NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Image TEXT NULL,
    Source TEXT NOT NULL,
    CreatedDate DATETIME
)
GO

CREATE TABLE [Track_In_Playlist] (
    Id INT IDENTITY(1, 1) NOT NULL,
    TrackId INT NOT NULL,
    PlaylistId INT NOT NULL
)
GO

CREATE TABLE [Genre] (
    Id INT IDENTITY(1, 1) NOT NULL,
    Name NVARCHAR(100) NOT NULL
)
GO

CREATE TABLE [Track_Genre] (
    Id INT IDENTITY(1, 1) NOT NULL,
    GenreId INT NOT NULL,
    TrackId INT NOT NULL
)
GO

CREATE TABLE [Artist] (
    Id INT IDENTITY(1, 1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Profile NTEXT NULL,
    Avatar TEXT NULL
)
GO

CREATE TABLE [Track_Artist] (
    Id INT IDENTITY(1, 1) NOT NULL,
    ArtistId INT NOT NULL,
    TrackId INT NOT NULL
)
GO

------------------------------ CREATE CONSTRAINT ------------------------------
--- PRIMARY KEY ---
ALTER TABLE [User] ADD CONSTRAINT PK_User PRIMARY KEY (Username);
ALTER TABLE [Role] ADD CONSTRAINT PK_Role PRIMARY KEY (Id);
ALTER TABLE [Playlist] ADD CONSTRAINT PK_Playlist PRIMARY KEY (Id);
ALTER TABLE [Track] ADD CONSTRAINT PK_Track PRIMARY KEY (Id);
ALTER TABLE [Track_In_Playlist] ADD CONSTRAINT PK_Track_In_Playlist PRIMARY KEY (Id);
ALTER TABLE [Genre] ADD CONSTRAINT PK_Genre PRIMARY KEY (Id);
ALTER TABLE [Track_Genre] ADD CONSTRAINT PK_Track_Genre PRIMARY KEY (Id);
ALTER TABLE [Artist] ADD CONSTRAINT PK_Artist PRIMARY KEY (Id);
ALTER TABLE [Track_Artist] ADD CONSTRAINT PK_Track_Artist PRIMARY KEY (Id);

--- FOREIGN KEY ---
ALTER TABLE [User] ADD CONSTRAINT FK_User_Role
    FOREIGN KEY (RoleId) REFERENCES [Role](Id);

ALTER TABLE [Playlist] ADD CONSTRAINT FK_Playlist_User
    FOREIGN KEY (Owner) REFERENCES [User](Username);

ALTER TABLE [Track_In_Playlist] ADD CONSTRAINT FK_TrackInPlaylist_Track
    FOREIGN KEY (TrackId) REFERENCES [Track](Id);

ALTER TABLE [Track_In_Playlist] ADD CONSTRAINT FK_TrackInPlaylist_Playlist
    FOREIGN KEY (PlaylistId) REFERENCES [Playlist](Id);

ALTER TABLE [Track_Genre] ADD CONSTRAINT FK_TrackGenre_Track
    FOREIGN KEY (TrackId) REFERENCES [Track](Id);

ALTER TABLE [Track_Genre] ADD CONSTRAINT FK_TrackGenre_Genre
    FOREIGN KEY (GenreId) REFERENCES [Genre](Id);

ALTER TABLE [Track_Artist] ADD CONSTRAINT FK_TrackArtist_Track
    FOREIGN KEY (TrackId) REFERENCES [Track](Id);

ALTER TABLE [Track_Artist] ADD CONSTRAINT FK_TrackArtist_Artist
    FOREIGN KEY (ArtistId) REFERENCES [Artist](Id);