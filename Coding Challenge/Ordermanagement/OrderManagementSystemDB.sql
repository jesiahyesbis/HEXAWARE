create database OrderManagementDB;
use OrderManagementDB;


CREATE TABLE Users (
    userId INT PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL,
    role VARCHAR(10) CHECK (role IN ('Admin', 'User')) NOT NULL
);


CREATE TABLE Products (
    productId INT PRIMARY KEY,
    productName VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    price DECIMAL(10, 2) NOT NULL,
    quantityInStock INT NOT NULL,
    type VARCHAR(20) CHECK (type IN ('Electronics', 'Clothing')) NOT NULL
);


CREATE TABLE Electronics (
    productId INT PRIMARY KEY,
    brand VARCHAR(50) NOT NULL,
    warrantyPeriod INT NOT NULL,
    FOREIGN KEY (productId) REFERENCES Products(productId)
);


CREATE TABLE Clothing (
    productId INT PRIMARY KEY,
    size VARCHAR(10) NOT NULL,
    color VARCHAR(20) NOT NULL,
    FOREIGN KEY (productId) REFERENCES Products(productId)
);


CREATE TABLE Orders (
    orderId INT PRIMARY KEY IDENTITY(1,1),
    userId INT NOT NULL,
    orderDate DATETIME NOT NULL,
    FOREIGN KEY (userId) REFERENCES Users(userId)
);


CREATE TABLE OrderItems (
    orderItemId INT PRIMARY KEY IDENTITY(1,1),
    orderId INT NOT NULL,
    productId INT NOT NULL,
    FOREIGN KEY (orderId) REFERENCES Orders(orderId),
    FOREIGN KEY (productId) REFERENCES Products(productId)
);

Select * from Products;
Select * from Electronics;
Select * from Users;
Select * from Orders;
Select * from OrderItems;