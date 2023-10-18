SELECT * from Compounds; --1.6

SELECT Mt.ID, Name, Formular, St.Class, MolWeight, State FROM Compounds as Mt --1.7
INNER JOIN Classes as St 
ON Mt.Class = St.ID;

SELECT * from Compounds as Mt --1.8
INNER JOIN Classes as St 
ON Mt.Class = St.ID
WHERE St.Class = N'Алкен';

SELECT Formular from Compounds WHERE ID = 6; --1.9

INSERT into Compounds (Name,Formular,Class,MolWeight,State) values (N'Бензол',N'C6H6',4,78.11,N'Рідкий'); --1.10
--SELECT * from Compounds;


INSERT into Compounds (Name,Formular,Class,MolWeight,State) --1.11
values 
(N'Глюкоза',N'C6H12O6',5,180.16,N'Твердий'),
(N'Етиловий Спирт',N'C2H5OH',6,46.07,N'Твердий');
--SELECT * from Compounds;

UPDATE Compounds set Name = 'CHANGED!', MolWeight = 0, Class = 1, Formular = 'CHANGED!', State = 'CHANGED!'; --1.12
--SELECT * from Compounds;

DELETE FROM Compounds WHERE ID = 6; --1.13
SELECT * FROM Compounds;


INSERT INTO Compounds (Name, Formular, Class, MolWeight, State) --1.14 // AddIfThereNot
VALUES
  (N'Метан', N'CH4', 1, 16.04, N'Газоподібний'),
  (N'Етен', N'C2H4', 2, 28.05, N'Газоподібний');

 -- Delete from Compounds where Name = N'Метан' OR Name = N'Етен' 
 
 --Update one staff
UPDATE Compounds set Name = 'D!', MolWeight = 0, Class = 1, Formular = 'D!', State = 'D!' WHERE ID = 8 ; --1.12
 SELECT * FROM Compounds;

 SELECT * FROM Classes;