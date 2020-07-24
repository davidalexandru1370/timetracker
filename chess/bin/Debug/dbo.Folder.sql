CREATE TABLE [dbo].[Folder] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Path] VARCHAR (450) NULL,
    [Timeore] INT      NULL,
    [Timeminute] INT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    
);

