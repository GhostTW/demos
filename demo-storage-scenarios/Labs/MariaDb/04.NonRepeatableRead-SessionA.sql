# step 1
SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED;
SELECT @@tx_isolation;
START TRANSACTION;
SELECT * FROM User;

# step 2 in 04.NonRepeatableRead-SessionB.sql

# step 3 read non-repeatable data.
UPDATE User SET Password = '1' WHERE Code = 'Test001' AND Password = '1E867FA1A3A64AB5E1EE21BD76F05912';
COMMIT;
SELECT * FROM User;

# step 4
SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;
SELECT @@tx_isolation;
UPDATE User SET Password = '1E867FA1A3A64AB5E1EE21BD76F05912' WHERE Code = 'Test001';
START TRANSACTION;
SELECT * FROM User;

# step 5 in 04.NonRepeatableRead-SessionB.sql
# step 6 get data.
SELECT * FROM User;
UPDATE User SET Password = '2' WHERE Code = 'Test001' AND Password = '1E867FA1A3A64AB5E1EE21BD76F05912';
COMMIT;

# step 7
START TRANSACTION;
SELECT * FROM User LOCK IN SHARE MODE;

# step 9
COMMIT;

