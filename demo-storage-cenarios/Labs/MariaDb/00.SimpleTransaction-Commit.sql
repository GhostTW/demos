# step 1
SELECT * FROM User;

# step 2
START TRANSACTION;
UPDATE User SET IsActive = 0 WHERE Code = 'Admin';
SELECT * FROM User;

# step 3
COMMIT;
SELECT * FROM User;