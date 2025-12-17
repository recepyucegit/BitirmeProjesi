SET QUOTED_IDENTIFIER ON;
GO

-- Check if Customers table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Customers](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [FirstName] [nvarchar](100) NOT NULL,
        [LastName] [nvarchar](100) NOT NULL,
        [IdentityNumber] [nvarchar](11) NOT NULL,
        [Email] [nvarchar](100) NULL,
        [Phone] [nvarchar](20) NULL,
        [Address] [nvarchar](500) NULL,
        [City] [nvarchar](100) NULL,
        [PostalCode] [nvarchar](10) NULL,
        [CustomerType] [nvarchar](50) NOT NULL DEFAULT ('Individual'),
        [IsActive] [bit] NOT NULL DEFAULT (CAST(1 AS bit)),
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT (CAST(0 AS bit)),
        [CreatedBy] [nvarchar](max) NULL,
        [UpdatedBy] [nvarchar](max) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_IdentityNumber] ON [dbo].[Customers]
    (
        [IdentityNumber] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Customers_Email] ON [dbo].[Customers]
    (
        [Email] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Customers_Phone] ON [dbo].[Customers]
    (
        [Phone] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Customers_City] ON [dbo].[Customers]
    (
        [City] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Customers_CustomerType] ON [dbo].[Customers]
    (
        [CustomerType] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Customers_IsActive] ON [dbo].[Customers]
    (
        [IsActive] ASC
    );

    PRINT 'Customers table created successfully';
END
ELSE
BEGIN
    PRINT 'Customers table already exists';
END
GO
