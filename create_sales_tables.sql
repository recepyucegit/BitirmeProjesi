SET QUOTED_IDENTIFIER ON;
GO

-- Check if Sales table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sales]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sales](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [SaleDate] [datetime2](7) NOT NULL,
        [CustomerId] [int] NOT NULL,
        [EmployeeId] [int] NOT NULL,
        [TotalAmount] [decimal](18, 2) NOT NULL,
        [DiscountAmount] [decimal](18, 2) NOT NULL DEFAULT (0),
        [NetAmount] [decimal](18, 2) NOT NULL,
        [CommissionAmount] [decimal](18, 2) NOT NULL,
        [PaymentMethod] [nvarchar](50) NOT NULL DEFAULT ('Cash'),
        [Notes] [nvarchar](1000) NULL,
        [Status] [nvarchar](50) NOT NULL DEFAULT ('Completed'),
        [InvoiceNumber] [nvarchar](50) NOT NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT (CAST(0 AS bit)),
        [CreatedBy] [nvarchar](max) NULL,
        [UpdatedBy] [nvarchar](max) NULL,
        CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Foreign Keys
    ALTER TABLE [dbo].[Sales] ADD CONSTRAINT [FK_Sales_Customers_CustomerId]
        FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customers] ([Id]);

    ALTER TABLE [dbo].[Sales] ADD CONSTRAINT [FK_Sales_Employees_EmployeeId]
        FOREIGN KEY([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]);

    -- Indexes
    CREATE NONCLUSTERED INDEX [IX_Sales_SaleDate] ON [dbo].[Sales]
    (
        [SaleDate] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Sales_CustomerId] ON [dbo].[Sales]
    (
        [CustomerId] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Sales_EmployeeId] ON [dbo].[Sales]
    (
        [EmployeeId] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Sales_Status] ON [dbo].[Sales]
    (
        [Status] ASC
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Sales_InvoiceNumber] ON [dbo].[Sales]
    (
        [InvoiceNumber] ASC
    );

    PRINT 'Sales table created successfully';
END
ELSE
BEGIN
    PRINT 'Sales table already exists';
END
GO

-- Check if SaleDetails table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaleDetails]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SaleDetails](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [SaleId] [int] NOT NULL,
        [ProductId] [int] NOT NULL,
        [Quantity] [int] NOT NULL,
        [UnitPrice] [decimal](18, 2) NOT NULL,
        [DiscountRate] [decimal](5, 2) NOT NULL DEFAULT (0),
        [DiscountAmount] [decimal](18, 2) NOT NULL DEFAULT (0),
        [TotalPrice] [decimal](18, 2) NOT NULL,
        [NetPrice] [decimal](18, 2) NOT NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT (CAST(0 AS bit)),
        [CreatedBy] [nvarchar](max) NULL,
        [UpdatedBy] [nvarchar](max) NULL,
        CONSTRAINT [PK_SaleDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Foreign Keys
    ALTER TABLE [dbo].[SaleDetails] ADD CONSTRAINT [FK_SaleDetails_Sales_SaleId]
        FOREIGN KEY([SaleId]) REFERENCES [dbo].[Sales] ([Id]) ON DELETE CASCADE;

    ALTER TABLE [dbo].[SaleDetails] ADD CONSTRAINT [FK_SaleDetails_Products_ProductId]
        FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]);

    -- Indexes
    CREATE NONCLUSTERED INDEX [IX_SaleDetails_SaleId] ON [dbo].[SaleDetails]
    (
        [SaleId] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_SaleDetails_ProductId] ON [dbo].[SaleDetails]
    (
        [ProductId] ASC
    );

    PRINT 'SaleDetails table created successfully';
END
ELSE
BEGIN
    PRINT 'SaleDetails table already exists';
END
GO
