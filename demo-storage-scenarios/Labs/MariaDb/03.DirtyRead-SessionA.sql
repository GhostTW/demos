# step 1
SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SELECT @@tx_isolation;
SELECT * FROM User;

# step 2 in 03.DirtyRead-SessionB.sql

# step 3 read dirty data.
SELECT * FROM User;

# step 4
SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED;
SELECT @@tx_isolation;
SELECT * FROM User;

# step 5 in 03.DirtyRead-SessionB.sql
# step 6 get committed data.
SELECT * FROM User;

# recovery to default setting.
SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;
