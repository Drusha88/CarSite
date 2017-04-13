CREATE TABLE Users
(
    [UserID] INT NOT NULL PRIMARY KEY IDENTITY,
    [Login] NVARCHAR (30) NOT NULL,
    [Email] NVARCHAR (30) NOT NULL,
    [Password] NVARCHAR (30) NOT NULL,
    [PasswordConfirm] NVARCHAR (30) NOT NULL,
    [Confirm] BIT NOT NULL,
    [Ticket] NVARCHAR (40) NULL,
);

