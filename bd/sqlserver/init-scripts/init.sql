USE master;
GO

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ProductManagerDb')
BEGIN
    CREATE DATABASE ProductManagerDb;
    PRINT 'Database ProductManagerDb created.';
END
ELSE
BEGIN
    PRINT 'Database ProductManagerDb already exists.';
END
GO

-- Switch to the ProductManagerDb database
USE ProductManagerDb;
GO

-- Create Products table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products' AND type = 'U')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        Price DECIMAL(18,2) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT UQ_Products_Code UNIQUE (Code)
    );
    
    PRINT 'Table Products created.';
    
    -- Create index for better search performance
    CREATE INDEX IX_Products_Code ON Products(Code);
    CREATE INDEX IX_Products_Name ON Products(Name);
    CREATE INDEX IX_Products_IsActive ON Products(IsActive);
    
    PRINT 'Indexes created.';
    
    -- Insert some sample data
    INSERT INTO Products (Code, Name, Description, Price, IsActive)
    VALUES 
        ('P001', 'Laptop', 'High performance laptop', 1500.00, 1),
        ('P002', 'Smartphone', 'Latest smartphone model', 800.00, 1),
        ('P003', 'Headphones', 'Noise cancelling headphones', 200.00, 1),
        ('P004', 'Keyboard', 'Mechanical keyboard', 120.00, 1),
        ('P005', 'Mouse', 'Wireless mouse', 50.00, 1);
        
    PRINT 'Sample data inserted.';
END
ELSE
BEGIN
    PRINT 'Table Products already exists.';
END
GO
