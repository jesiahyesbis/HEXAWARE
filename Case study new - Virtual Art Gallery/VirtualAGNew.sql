create database VirtualAGNew;

use VirtualAGNew;

CREATE TABLE Artist (
    ArtistID INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Biography NVARCHAR(MAX),
    BirthDate DATE,
    Nationality NVARCHAR(50),
    Website NVARCHAR(255),
    ContactInformation NVARCHAR(255)
);

CREATE TABLE Artwork (
    ArtworkID INT PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    CreationDate DATE,
    Medium NVARCHAR(100),
    ImageURL NVARCHAR(255),
    ArtistID INT FOREIGN KEY REFERENCES Artist(ArtistID)
);

CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    DateOfBirth DATE,
    ProfilePicture NVARCHAR(255)
);

CREATE TABLE Gallery (
    GalleryID INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Location NVARCHAR(255),
    Curator INT FOREIGN KEY REFERENCES Artist(ArtistID),
    OpeningHours NVARCHAR(100)
);


CREATE TABLE User_Favorite_Artwork (
    UserID INT FOREIGN KEY REFERENCES [User](UserID),
    ArtworkID INT FOREIGN KEY REFERENCES Artwork(ArtworkID),
    PRIMARY KEY (UserID, ArtworkID)
);

CREATE TABLE Artwork_Gallery (
    ArtworkID INT FOREIGN KEY REFERENCES Artwork(ArtworkID),
    GalleryID INT FOREIGN KEY REFERENCES Gallery(GalleryID),
    PRIMARY KEY (ArtworkID, GalleryID)
);

Select * from Artwork;
Select * from Artist;
Select * from Gallery;
Select * from [User];

Insert into Artist(ArtistID,Name,Biography,BirthDate,Nationality,Website,ContactInformation) 
values(1,'VincentVan Gogh','Dutch Post_Impressionist painter','1853-03-30','Dutch','www.vangogallery.com','info@vangogh.com');



