# step 2
START TRANSACTION;
UPDATE User SET Password = '0' WHERE Code = 'Test001';
COMMIT;

# step 3 in 04.NonRepeatableRead-SessionA.sql
# step 4 in 04.NonRepeatableRead-SessionA.sql
# step 5
START TRANSACTION;
UPDATE User SET Password = '1' WHERE Code = 'Test001';
COMMIT;
SELECT * FROM User;
# step 6 in 04.NonRepeatableRead-SessionA.sql
# step 7 in 04.NonRepeatableRead-SessionA.sql
# step 8
START TRANSACTION;
UPDATE User SET Password = '2' WHERE Code = 'Test001';
COMMIT;

# step 10
SELECT * FROM User;
