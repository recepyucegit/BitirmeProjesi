SET QUOTED_IDENTIFIER ON;
GO

-- Check if Suppliers table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Suppliers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Suppliers](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [CompanyName] [nvarchar](200) NOT NULL,
        [ContactName] [nvarchar](100) NULL,
        [ContactTitle] [nvarchar](100) NULL,
        [Email] [nvarchar](100) NULL,
        [Phone] [nvarchar](20) NULL,
        [Address] [nvarchar](500) NULL,
        [City] [nvarchar](100) NULL,
        [Country] [nvarchar](100) NULL,
        [PostalCode] [nvarchar](20) NULL,
        [TaxNumber] [nvarchar](50) NULL,
        [IsActive] [bit] NOT NULL DEFAULT (CAST(1 AS bit)),
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT (CAST(0 AS bit)),
        [CreatedBy] [nvarchar](max) NULL,
        [UpdatedBy] [nvarchar](max) NULL,
        CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE NONCLUSTERED INDEX [IX_Suppliers_TaxNumber] ON [dbo].[Suppliers]
    (
        [TaxNumber] ASC
    ) WHERE ([TaxNumber] IS NOT NULL);

    CREATE NONCLUSTERED INDEX [IX_Suppliers_CompanyName] ON [dbo].[Suppliers]
    (
        [CompanyName] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Suppliers_IsActive] ON [dbo].[Suppliers]
    (
        [IsActive] ASC
    );

    PRINT 'Suppliers table created successfully';
END
ELSE
BEGIN
    PRINT 'Suppliers table already exists';
END

-- Add SupplierId column to Products table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'SupplierId')
BEGIN
    ALTER TABLE [dbo].[Products] ADD [SupplierId] [int] NULL;
    PRINT 'SupplierId column added to Products table';
END
ELSE
BEGIN
    PRINT 'SupplierId column already exists in Products table';
END

-- Add Foreign Key constraint if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Products_Suppliers_SupplierId')
BEGIN
    ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY([SupplierId])
    REFERENCES [dbo].[Suppliers] ([Id])
    ON DELETE NO ACTION;

    ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Suppliers_SupplierId];
    PRINT 'Foreign Key constraint added';
END
ELSE
BEGIN
    PRINT 'Foreign Key constraint already exists';
END

-- Insert migration history record if it doesn't exist
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251209095911_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251209095911_InitialCreate', N'7.0.0');
    PRINT 'Migration history updated';
END
ELSE
BEGIN
    PRINT 'Migration history already contains this migration';
END
