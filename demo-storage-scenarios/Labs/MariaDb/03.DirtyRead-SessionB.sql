# step 2
START TRANSACTION;
UPDATE User SET IsActive = 0 WHERE Code = 'Admin';

# step 3 in 03.DirtyRead-SessionA.sql
# step 4 in 03.DirtyRead-SessionA.sql
# step 5
COMMIT;

# step 6 in 03.DirtyRead-SessionA.sql

# step 7 recovery to default setting.
UPDATE User SET IsActive = 1 WHERE Code = 'Admin';
