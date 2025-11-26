-- =============================================
-- CSDL WEBSITE BAN TRA SUA - SQL SERVER
-- Chi de xem ERD tren SSMS, khong ket noi voi website
-- =============================================

USE master;
GO

-- Xoa database neu da ton tai
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MilkTeaShop')
BEGIN
    ALTER DATABASE MilkTeaShop SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MilkTeaShop;
END
GO

-- Tao database moi
CREATE DATABASE MilkTeaShop;
GO

USE MilkTeaShop;
GO

-- =============================================
-- BANG DANH MUC
-- =============================================
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- =============================================
-- BANG SAN PHAM
-- =============================================
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18,2) NOT NULL,
    ImageUrl NVARCHAR(500),
    CategoryId INT NOT NULL,
    IsActive BIT DEFAULT 1,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

-- =============================================
-- BANG NGUOI DUNG
-- =============================================
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(500),
    PasswordHash NVARCHAR(500) NOT NULL,
    Points INT DEFAULT 0,
    Role NVARCHAR(20) DEFAULT 'Customer', -- Admin hoac Customer
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- =============================================
-- BANG DON HANG
-- =============================================
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NULL, -- NULL neu khach van lai
    CustomerName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    Note NVARCHAR(500),
    TotalAmount DECIMAL(18,2) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending, Processing, Completed, Cancelled
    PaymentMethod NVARCHAR(50) DEFAULT 'COD', -- COD, Banking
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- =============================================
-- BANG CHI TIET DON HANG
-- =============================================
CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    Size NVARCHAR(10) DEFAULT 'M', -- M, L
    Sugar INT DEFAULT 100, -- 0, 30, 50, 70, 100
    Ice INT DEFAULT 100, -- 0, 30, 50, 70, 100
    Topping NVARCHAR(100),
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- =============================================
-- BANG KHUYEN MAI
-- =============================================
CREATE TABLE Promotions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Code NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    DiscountPercent INT NOT NULL,
    MinOrderAmount DECIMAL(18,2) DEFAULT 0,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    IsActive BIT DEFAULT 1
);

-- =============================================
-- BANG GIO HANG (Session-based trong website, day chi mo phong)
-- =============================================
CREATE TABLE CartItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Size NVARCHAR(10) DEFAULT 'M',
    Sugar INT DEFAULT 100,
    Ice INT DEFAULT 100,
    Topping NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_CartItems_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- =============================================
-- THEM DU LIEU MAU
-- =============================================

-- Danh muc
INSERT INTO Categories (Name) VALUES 
(N'Tra Sua Truyen Thong'),
(N'Tra Sua Trai Cay'),
(N'Tra Sua Dac Biet');

-- San pham
INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId) VALUES
(N'Tra Sua Tran Chau', N'Tra sua truyen thong voi tran chau den', 35000, '/images/trasua1.jpg', 1),
(N'Tra Sua Matcha', N'Tra sua matcha Nhat Ban', 40000, '/images/trasua2.jpg', 1),
(N'Tra Sua Dau', N'Tra sua vi dau tuoi', 38000, '/images/trasua3.jpg', 2),
(N'Tra Sua Dao', N'Tra sua dao thom ngon', 38000, '/images/trasua4.jpg', 2),
(N'Tra Sua Socola', N'Tra sua socola dam da', 42000, '/images/trasua5.jpg', 3),
(N'Tra Sua Kem Cheese', N'Tra sua phu kem cheese', 45000, '/images/trasua6.jpg', 3);

-- Nguoi dung
INSERT INTO Users (FullName, Email, PhoneNumber, Address, PasswordHash, Points, Role) VALUES
(N'Administrator', 'admin@gmail.com', '0123456789', N'Ha Noi', 'hashed_password_admin', 0, 'Admin'),
(N'Nguyen Van A', 'nguyenvana@gmail.com', '0987654321', N'123 Nguyen Trai, Ha Noi', 'hashed_password_1', 150, 'Customer'),
(N'Tran Thi B', 'tranthib@gmail.com', '0912345678', N'456 Le Loi, TP HCM', 'hashed_password_2', 200, 'Customer'),
(N'Le Van C', 'levanc@gmail.com', '0909123456', N'789 Tran Phu, Da Nang', 'hashed_password_3', 100, 'Customer');

-- Don hang
INSERT INTO Orders (UserId, CustomerName, Phone, Address, Note, TotalAmount, OrderDate, Status, PaymentMethod) VALUES
(2, N'Nguyen Van A', '0987654321', N'123 Nguyen Trai, Ha Noi', N'Giao truoc 5h chieu', 108000, GETDATE()-5, 'Completed', 'COD'),
(3, N'Tran Thi B', '0912345678', N'456 Le Loi, TP HCM', N'', 156000, GETDATE()-3, 'Completed', 'Banking'),
(4, N'Le Van C', '0909123456', N'789 Tran Phu, Da Nang', N'Goi truoc khi giao', 75000, GETDATE()-1, 'Processing', 'COD'),
(NULL, N'Khach Van Lai', '0901234567', N'999 Hai Ba Trung, Ha Noi', N'', 80000, GETDATE(), 'Pending', 'COD');

-- Chi tiet don hang
INSERT INTO OrderDetails (OrderId, ProductId, ProductName, Price, Quantity, Size, Sugar, Ice, Topping) VALUES
(1, 1, N'Tra Sua Tran Chau', 35000, 2, 'M', 100, 100, N'Tran chau'),
(1, 3, N'Tra Sua Dau', 38000, 1, 'L', 70, 50, N''),
(2, 5, N'Tra Sua Socola', 42000, 2, 'M', 100, 100, N'Pudding'),
(2, 6, N'Tra Sua Kem Cheese', 45000, 1, 'L', 50, 70, N'Thach'),
(3, 2, N'Tra Sua Matcha', 40000, 1, 'M', 70, 100, N''),
(3, 1, N'Tra Sua Tran Chau', 35000, 1, 'M', 100, 100, N'Tran chau'),
(4, 4, N'Tra Sua Dao', 38000, 2, 'M', 100, 100, N'');

-- Khuyen mai
INSERT INTO Promotions (Code, Description, DiscountPercent, MinOrderAmount, StartDate, EndDate) VALUES
(N'TRASUA50', N'Giam 50% cho don hang tren 100k', 50, 100000, GETDATE()-30, GETDATE()+30),
(N'NEWCUSTOMER', N'Giam 20% cho khach hang moi', 20, 50000, GETDATE()-10, GETDATE()+60),
(N'SUMMER2024', N'Khuyen mai mua he', 30, 80000, GETDATE()-5, GETDATE()+90);

-- Gio hang (du lieu mau)
INSERT INTO CartItems (UserId, ProductId, Quantity, Size, Sugar, Ice, Topping) VALUES
(2, 1, 2, 'M', 100, 100, N'Tran chau'),
(2, 3, 1, 'L', 70, 50, N''),
(3, 5, 1, 'M', 100, 100, N'Pudding');

GO

-- =============================================
-- TAO INDEX DE TIM KIEM NHANH
-- =============================================
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_OrderDetails_OrderId ON OrderDetails(OrderId);
CREATE INDEX IX_OrderDetails_ProductId ON OrderDetails(ProductId);
CREATE INDEX IX_CartItems_UserId ON CartItems(UserId);
GO

-- =============================================
-- HOAN TAT
-- =============================================
PRINT 'Da tao thanh cong CSDL MilkTeaShop!';
PRINT 'Co the xem ERD tren SQL Server Management Studio';
GO
