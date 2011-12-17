
-- database creation

USE [master]
GO

/****** Object:  Database [filesync]    Script Date: 11/06/2011 15:44:17 ******/
CREATE DATABASE [filesync] ON  PRIMARY 
( NAME = N'filesync', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MBDEV02\MSSQL\DATA\filesync.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'filesync_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MBDEV02\MSSQL\DATA\filesync_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [filesync] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [filesync].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [filesync] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [filesync] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [filesync] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [filesync] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [filesync] SET ARITHABORT OFF 
GO

ALTER DATABASE [filesync] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [filesync] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [filesync] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [filesync] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [filesync] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [filesync] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [filesync] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [filesync] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [filesync] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [filesync] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [filesync] SET  DISABLE_BROKER 
GO

ALTER DATABASE [filesync] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [filesync] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [filesync] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [filesync] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [filesync] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [filesync] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [filesync] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [filesync] SET  READ_WRITE 
GO

ALTER DATABASE [filesync] SET RECOVERY FULL 
GO

ALTER DATABASE [filesync] SET  MULTI_USER 
GO

ALTER DATABASE [filesync] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [filesync] SET DB_CHAINING OFF 
GO

-- creating structure of all tables (columns, primary keys)

USE [filesync]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

-- Contents table

/****** Object:  Table [dbo].[Contents]    Script Date: 11/05/2011 12:16:03 ******/

CREATE TABLE [dbo].[Contents](
	[content_id] [int] IDENTITY(1,1) NOT NULL,
	[content_data] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Contents] PRIMARY KEY CLUSTERED 
(
	[content_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Dirs table

/****** Object:  Table [dbo].[Dirs]    Script Date: 11/06/2011 15:48:27 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Dirs](
	[dir_id] [int] IDENTITY(1,1) NOT NULL,
	[dir_rootdirid] [int] NULL,
	[dir_name] [varchar](50) NOT NULL,
	[user_ownerid] [int] NOT NULL,
	[dir_description] [varchar](max) NULL,
 CONSTRAINT [PK_Dirs] PRIMARY KEY CLUSTERED 
(
	[dir_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Users table

/****** Object:  Table [dbo].[Users]    Script Date: 11/06/2011 15:49:07 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_login] [varchar](50) NOT NULL,
	[user_pass] [varchar](max) NOT NULL,
	[user_fullname] [varchar](50) NULL,
	[user_email] [varchar](50) NULL,
	[user_lastlogin] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Machines table

/****** Object:  Table [dbo].[Machines]    Script Date: 11/06/2011 15:51:45 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Machines](
	[machine_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[machine_name] [varchar](50) NOT NULL,
	[machine_description] [varchar](max) NULL,
 CONSTRAINT [PK_Machines] PRIMARY KEY CLUSTERED 
(
	[machine_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- DirUsers table

/****** Object:  Table [dbo].[DirUsers]    Script Date: 11/06/2011 16:14:35 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DirUsers](
	[user_id] [int] NOT NULL,
	[dir_id] [int] NOT NULL,
	[right_id] [int] NOT NULL,
 CONSTRAINT [PK_DirUsers] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC,
	[dir_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Rights table

/****** Object:  Table [dbo].[Rights]    Script Date: 11/06/2011 16:16:41 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rights](
	[right_id] [int] IDENTITY(1,1) NOT NULL,
	[right_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Rights] PRIMARY KEY CLUSTERED 
(
	[right_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Types table

/****** Object:  Table [dbo].[Types]    Script Date: 11/06/2011 16:13:36 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Types](
	[type_id] [int] IDENTITY(1,1) NOT NULL,
	[type_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Types] PRIMARY KEY CLUSTERED 
(
	[type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- Files table

/****** Object:  Table [dbo].[Files]    Script Date: 11/06/2011 16:13:16 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Files](
	[file_id] [int] IDENTITY(1,1) NOT NULL,
	[dir_id] [int] NOT NULL,
	[type_id] [int] NOT NULL,
	[content_id] [int] NOT NULL,
	[file_name] [varchar](50) NOT NULL,
	[file_size] [bigint] NOT NULL,
	[file_hash] [varchar](max) NOT NULL,
	[file_uploaded] [datetime] NOT NULL,
	[file_modified] [datetime] NOT NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[file_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- MachineDirs table

/****** Object:  Table [dbo].[MachineDirs]    Script Date: 11/06/2011 16:13:11 ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MachineDirs](
	[machine_id] [int] NOT NULL,
	[dir_id] [int] NOT NULL,
	[dir_realpath] [varchar](max) NOT NULL,
 CONSTRAINT [PK_MachineDirs] PRIMARY KEY CLUSTERED 
(
	[machine_id] ASC,
	[dir_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- setting default values for some fields

SET ANSI_PADDING OFF
GO

-- Dirs

ALTER TABLE [dbo].[Dirs] ADD  CONSTRAINT [DF_Dirs_dir_rootdirid]  DEFAULT (NULL) FOR [dir_rootdirid]
GO

ALTER TABLE [dbo].[Dirs] ADD  CONSTRAINT [DF_Dirs_dir_description]  DEFAULT (NULL) FOR [dir_description]
GO

-- DirUsers

ALTER TABLE [dbo].[DirUsers] ADD  CONSTRAINT [DF_DirUsers_right_id]  DEFAULT ((1)) FOR [right_id]
GO

-- Machines

ALTER TABLE [dbo].[Machines] ADD  CONSTRAINT [DF_Machines_machine_description]  DEFAULT (NULL) FOR [machine_description]
GO

-- Users

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_email]  DEFAULT (NULL) FOR [user_email]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_user_lastlogin]  DEFAULT (NULL) FOR [user_lastlogin]
GO

-- creating foreign key relations between tables

SET ANSI_PADDING OFF
GO

-- MachineDirs <-> Dirs

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[MachineDirs]  WITH CHECK ADD  CONSTRAINT [FK_MachineDirs_Dirs] FOREIGN KEY([dir_id])
REFERENCES [dbo].[Dirs] ([dir_id])
GO

ALTER TABLE [dbo].[MachineDirs] CHECK CONSTRAINT [FK_MachineDirs_Dirs]
GO

-- MachineDirs <-> Machines

ALTER TABLE [dbo].[MachineDirs]  WITH CHECK ADD  CONSTRAINT [FK_MachineDirs_Machines] FOREIGN KEY([machine_id])
REFERENCES [dbo].[Machines] ([machine_id])
GO

ALTER TABLE [dbo].[MachineDirs] CHECK CONSTRAINT [FK_MachineDirs_Machines]
GO

-- Files <-> Contents

ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_Contents] FOREIGN KEY([content_id])
REFERENCES [dbo].[Contents] ([content_id])
GO

ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_Files_Contents]
GO

-- Files <-> Dirs

ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_Dirs] FOREIGN KEY([dir_id])
REFERENCES [dbo].[Dirs] ([dir_id])
GO

ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_Files_Dirs]
GO

-- Files <-> Types

ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_Types] FOREIGN KEY([type_id])
REFERENCES [dbo].[Types] ([type_id])
GO

ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_Files_Types]
GO

-- DirUsers <-> Dirs

ALTER TABLE [dbo].[DirUsers]  WITH CHECK ADD  CONSTRAINT [FK_DirUsers_Dirs] FOREIGN KEY([dir_id])
REFERENCES [dbo].[Dirs] ([dir_id])
GO

ALTER TABLE [dbo].[DirUsers] CHECK CONSTRAINT [FK_DirUsers_Dirs]
GO

-- DirUsers <-> Rights

ALTER TABLE [dbo].[DirUsers]  WITH CHECK ADD  CONSTRAINT [FK_DirUsers_Rights] FOREIGN KEY([right_id])
REFERENCES [dbo].[Rights] ([right_id])
GO

ALTER TABLE [dbo].[DirUsers] CHECK CONSTRAINT [FK_DirUsers_Rights]
GO

-- DirUsers <-> Users

ALTER TABLE [dbo].[DirUsers]  WITH CHECK ADD  CONSTRAINT [FK_DirUsers_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO

ALTER TABLE [dbo].[DirUsers] CHECK CONSTRAINT [FK_DirUsers_Users]
GO

-- Machines <-> Users

ALTER TABLE [dbo].[Machines]  WITH CHECK ADD  CONSTRAINT [FK_Machines_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO

ALTER TABLE [dbo].[Machines] CHECK CONSTRAINT [FK_Machines_Users]
GO

-- Dirs <-> Dirs

ALTER TABLE [dbo].[Dirs]  WITH CHECK ADD  CONSTRAINT [FK_Dirs_Dirs] FOREIGN KEY([dir_rootdirid])
REFERENCES [dbo].[Dirs] ([dir_id])
GO

ALTER TABLE [dbo].[Dirs] CHECK CONSTRAINT [FK_Dirs_Dirs]
GO

-- Dirs <-> Users

ALTER TABLE [dbo].[Dirs]  WITH CHECK ADD  CONSTRAINT [FK_Dirs_Users] FOREIGN KEY([user_ownerid])
REFERENCES [dbo].[Users] ([user_id])
GO

ALTER TABLE [dbo].[Dirs] CHECK CONSTRAINT [FK_Dirs_Users]
GO
