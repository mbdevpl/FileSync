
-- removes all custom content from database, preserving order of cascading delete

DELETE FROM MachineDirs;
DELETE FROM Machines;
DELETE FROM Files;
DELETE FROM Contents;
DELETE FROM DirUsers;
DELETE FROM Dirs;
DELETE FROM Users WHERE user_id > 3;
