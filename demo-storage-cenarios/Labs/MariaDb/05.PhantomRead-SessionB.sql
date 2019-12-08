# step 2
START TRANSACTION;
INSERT INTO User VALUES(4,'testP0', '1E867FA1A3A64AB5E1EE21BD76F05912', 1);
COMMIT;

# step 3 in 04.NonRepeatableRead-SessionA.sql
# step 4 in 04.NonRepeatableRead-SessionA.sql
# step 5
START TRANSACTION;
INSERT INTO User VALUES(5,'testP1', '1E867FA1A3A64AB5E1EE21BD76F05912', 1);
COMMIT;
SELECT * FROM User;
# step 6 in 04.NonRepeatableRead-SessionA.sql
# step 7
DELETE FROM User WHERE Id >=4;
SELECT * FROM User;
COMMIT;