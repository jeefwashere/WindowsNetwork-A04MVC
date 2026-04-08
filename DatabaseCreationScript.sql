IF DB_ID('UserManagementSystem') IS NOT NULL
BEGIN
	CREATE DATABASE UserManagementSystem;
END
GO;

IF OBJECT_ID('User','U') IS NULL
BEGIN
	CREATE TABLE [User] (
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	[Username] NVARCHAR(100) NOT NULL,
	[HashedPassword] NVARCHAR(255) NOT NULL,
	[StreetAddress] NVARCHAR(50) NOT NULL,
	[City] NVARCHAR(50) NOT NULL,
	[Province] NVARCHAR(20) NOT NULL,
	[Country] NVARCHAR(20) NOT NULL,
	[PostalCode] NVARCHAR(6) NOT NULL,
	CreatedAt Timestamp,
	UpdatedAt Timestamp
	)
END
GO;

IF OBJECT_ID('UserItem','U') IS NULL
BEGIN
	CREATE TABLE [UserItem] (
	ItemID INT IDENTITY(1,1) PRIMARY KEY,
	[Itemname] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(255) NOT NULL,
	[Quantity] INT NOT NULL,
	OwnerID INT NOT NULL,
	CreatedAt Timestamp,
	UpdatedAt Timestamp

	CONSTRAINT fk_owner FOREIGN KEY (OwnerID) REFERENCES [User](UserID)
	);
END
GO;