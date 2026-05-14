IF DB_ID(N'InventoryStockManagementDb') IS NULL
BEGIN
    CREATE DATABASE InventoryStockManagementDb;
END
GO

USE InventoryStockManagementDb;
GO

IF OBJECT_ID(N'dbo.StockTransactions', N'U') IS NOT NULL
    DROP TABLE dbo.StockTransactions;
GO

IF OBJECT_ID(N'dbo.ProductVariantOptions', N'U') IS NOT NULL
    DROP TABLE dbo.ProductVariantOptions;
GO

IF OBJECT_ID(N'dbo.ProductVariants', N'U') IS NOT NULL
    DROP TABLE dbo.ProductVariants;
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL
    DROP TABLE dbo.Products;
GO

CREATE TABLE dbo.Products
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    ProductCode NVARCHAR(50) NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    CreatedDate DATETIMEOFFSET NOT NULL,
    UpdatedDate DATETIMEOFFSET NOT NULL,
    CreatedUser UNIQUEIDENTIFIER NOT NULL,
    IsFavourite BIT NOT NULL,
    Active BIT NOT NULL,
    HSNCode NVARCHAR(100) NULL,
    TotalStock DECIMAL(18, 2) NOT NULL,
    CONSTRAINT UQ_Products_ProductCode UNIQUE (ProductCode)
);
GO

CREATE TABLE dbo.ProductVariants
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ProductVariants PRIMARY KEY,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_ProductVariants_Products_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id) ON DELETE CASCADE
);
GO

CREATE TABLE dbo.ProductVariantOptions
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ProductVariantOptions PRIMARY KEY,
    ProductVariantId UNIQUEIDENTIFIER NOT NULL,
    Value NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_ProductVariantOptions_ProductVariants_ProductVariantId
        FOREIGN KEY (ProductVariantId) REFERENCES dbo.ProductVariants(Id) ON DELETE CASCADE
);
GO

CREATE TABLE dbo.StockTransactions
(
    Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_StockTransactions PRIMARY KEY,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    TransactionType INT NOT NULL,
    Quantity DECIMAL(18, 2) NOT NULL,
    Notes NVARCHAR(250) NULL,
    TransactionDate DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_StockTransactions_Products_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id)
);
GO

CREATE INDEX IX_ProductVariants_ProductId
    ON dbo.ProductVariants(ProductId);
GO

CREATE INDEX IX_ProductVariantOptions_ProductVariantId
    ON dbo.ProductVariantOptions(ProductVariantId);
GO

CREATE INDEX IX_StockTransactions_ProductId
    ON dbo.StockTransactions(ProductId);
GO

CREATE INDEX IX_Products_ProductName
    ON dbo.Products(ProductName);
GO
