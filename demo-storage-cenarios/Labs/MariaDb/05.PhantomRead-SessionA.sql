# step 1
SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;
SELECT @@tx_isolation;
UPDATE User SET IsActive = 1 WHERE Code = 'Admin';

START TRANSACTION;
SELECT * FROM User;

# step 2 in 05.PhantomRead-SessionB.sql

# step 3
SELECT * FROM User;
COMMIT;

# step 4
SET SESSION TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SELECT @@tx_isolation;
START TRANSACTION;
SELECT * FROM User;

# step 5 in 05.PhantomRead-SessionB.sql
# step 6 get data.
COMMIT;
SELECT * FROM User;
