# step 1
SELECT * FROM User;

# step 2
START TRANSACTION;
UPDATE User SET IsActive = 1 WHERE Code = 'Admin';
SELECT * FROM User;

# step 3
ROLLBACK;
SELECT * FROM User;