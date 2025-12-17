SET QUOTED_IDENTIFIER ON;
GO

-- Check if Employees table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employees](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [FirstName] [nvarchar](100) NOT NULL,
        [LastName] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Phone] [nvarchar](20) NULL,
        [Address] [nvarchar](500) NULL,
        [City] [nvarchar](100) NULL,
        [IdentityNumber] [nvarchar](11) NOT NULL,
        [HireDate] [datetime2](7) NOT NULL,
        [TerminationDate] [datetime2](7) NULL,
        [Salary] [decimal](18, 2) NOT NULL,
        [SalesQuota] [decimal](18, 2) NOT NULL DEFAULT (10000),
        [CommissionRate] [decimal](5, 4) NOT NULL DEFAULT (0.1000),
        [Role] [nvarchar](50) NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [PasswordHash] [nvarchar](500) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT (CAST(1 AS bit)),
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
        [UpdatedDate] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT (CAST(0 AS bit)),
        [CreatedBy] [nvarchar](max) NULL,
        [UpdatedBy] [nvarchar](max) NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees_Username] ON [dbo].[Employees]
    (
        [Username] ASC
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees_IdentityNumber] ON [dbo].[Employees]
    (
        [IdentityNumber] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Employees_Email] ON [dbo].[Employees]
    (
        [Email] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Employees_Role] ON [dbo].[Employees]
    (
        [Role] ASC
    );

    CREATE NONCLUSTERED INDEX [IX_Employees_IsActive] ON [dbo].[Employees]
    (
        [IsActive] ASC
    );

    PRINT 'Employees table created successfully';
END
ELSE
BEGIN
    PRINT 'Employees table already exists';
END
GO
