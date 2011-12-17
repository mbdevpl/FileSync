
-- available types of content that users can assign to the uploaded files

INSERT INTO [filesync].[dbo].[Types]
           ([type_name])
     VALUES
           ('plain text'),
           ('formatted text'),
           ('audio'),
           ('video'),
           ('image'),
           ('archive'),
           ('executable'),
           ('other')
GO

-- available kinds of rights that users can have for a given directory

INSERT INTO [filesync].[dbo].[Rights]
           ([right_name])
     VALUES
           ('Read/Write'),
           ('Read'),
           ('Write'),
           ('Read/Write/Share'),
           ('No access')
GO

-- forbidden usernames

INSERT INTO [filesync].[dbo].[Users]
           ([user_login]
           ,[user_pass]
           ,[user_fullname]
           ,[user_email]
           ,[user_lastlogin])
     VALUES
           ('administrator','admin','Server Administrator',NULL,NULL),
           ('admin','admin','Server Administrator',NULL,NULL),
           ('adm','admin','Server Administrator',NULL,NULL)
GO
