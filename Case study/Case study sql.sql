CREATE DATABASE VirtualArtGallery;


USE VirtualArtGallery;


-- Create Artist table
CREATE TABLE Artist (
    ArtistID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Biography NVARCHAR(MAX),
    BirthDate DATE,
    Nationality NVARCHAR(50),
    Website NVARCHAR(255),
    ContactInformation NVARCHAR(255)
);

-- Create User table
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

-- Create Artwork table
CREATE TABLE Artwork (
    ArtworkID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    CreationDate DATE,
    Medium NVARCHAR(100),
    ImageURL NVARCHAR(255),
    ArtistID INT FOREIGN KEY REFERENCES Artist(ArtistID)
);

-- Create Gallery table
CREATE TABLE Gallery (
    GalleryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Location NVARCHAR(255),
    Curator INT FOREIGN KEY REFERENCES Artist(ArtistID),
    OpeningHours NVARCHAR(100)
);

-- Junction table for User Favorite Artworks
CREATE TABLE User_Favorite_Artwork (
    UserID INT FOREIGN KEY REFERENCES [User](UserID),
    ArtworkID INT FOREIGN KEY REFERENCES Artwork(ArtworkID),
    PRIMARY KEY (UserID, ArtworkID)
);

-- Junction table for Artwork Gallery
CREATE TABLE Artwork_Gallery (
    ArtworkID INT FOREIGN KEY REFERENCES Artwork(ArtworkID),
    GalleryID INT FOREIGN KEY REFERENCES Gallery(GalleryID),
    PRIMARY KEY (ArtworkID, GalleryID)
);

-- Insert sample data
INSERT INTO Artist (Name, Biography, BirthDate, Nationality, Website, ContactInformation)
VALUES 
    ('Vincent van Gogh', 'Dutch Post-Impressionist painter', '1853-03-30', 'Dutch', 'www.vangoghgallery.com', 'info@vangogh.com'),
    ('Pablo Picasso', 'Spanish painter, sculptor, printmaker', '1881-10-25', 'Spanish', 'www.picasso.org', 'contact@picasso.org');

INSERT INTO [User] (Username, Password, Email, FirstName, LastName, DateOfBirth)
VALUES 
    ('artlover', 'password123', 'artlover@example.com', 'John', 'Doe', '1990-05-15'),
    ('galleryowner', 'securepass', 'owner@example.com', 'Jane', 'Smith', '1985-11-22');

INSERT INTO Artwork (Title, Description, CreationDate, Medium, ImageURL, ArtistID)
VALUES 
    ('Starry Night', 'A famous painting of the night sky', '1889-06-01', 'Oil on canvas', 'images/starrynight.jpg', 1),
    ('The Weeping Woman', 'Portrait of Dora Maar', '1937-10-01', 'Oil on canvas', 'images/weepingwoman.jpg', 2);

INSERT INTO Gallery (Name, Description, Location, Curator, OpeningHours)
VALUES 
    ('Modern Art Gallery', 'Collection of modern art masterpieces', 'New York', 1, '9:00 AM - 6:00 PM'),
    ('Classic Art Space', 'Traditional art exhibitions', 'Paris', 2, '10:00 AM - 8:00 PM');

INSERT INTO User_Favorite_Artwork (UserID, ArtworkID) VALUES (1, 1), (1, 2);
INSERT INTO Artwork_Gallery (ArtworkID, GalleryID) VALUES (1, 1), (2, 2);

select * from Gallery;