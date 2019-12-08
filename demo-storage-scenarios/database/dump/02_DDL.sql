CREATE USER IF NOT EXISTS 'dbowner'@'%' IDENTIFIED BY 'pass.123';
GRANT All privileges ON *.* TO 'dbowner'@'%';

CREATE DATABASE IF NOT EXISTS TestDB1;
USE TestDB1;

-- User --

DROP TABLE IF EXISTS `User`;
CREATE TABLE `User` (
  `Id`            int AUTO_INCREMENT NOT NULL,
  `Code`          varchar(20)        NOT NULL,
  `Password`      varchar(70)        NOT NULL,
  `IsActive`      bit                NOT NULL,
  UNIQUE (`Code`),
  PRIMARY KEY (`Id`)
);

CREATE INDEX IX_User_Code ON User (Code);

-- -- Create User --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_User_CreateUser`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_User_CreateUser`(
  IN  IN_Code         varchar(20),
  IN  IN_Password     varchar(70),
  IN  IN_IsActive     bit,
  OUT OUT_ReturnValue int
)
BEGIN
  SET OUT_ReturnValue = 0;
  INSERT INTO User (Code, Password, IsActive) values (IN_Code, IN_Password, IN_IsActive);
  SELECT LAST_INSERT_ID() as ID;
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get Users --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_User_GetUsers`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_User_GetUsers`(OUT OUT_ReturnValue int)
BEGIN
  SET OUT_ReturnValue = 0;
  SELECT Id, Code, Password, IsActive FROM User;
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get User --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_User_GetUser`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_User_GetUser`(
  IN  IN_Id           int,
  OUT OUT_ReturnValue int
)
BEGIN
    SET OUT_ReturnValue = 0;
    SELECT Id, Code, Password, IsActive FROM User WHERE Id = IN_Id LIMIT 1;
    SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get User By Condition --
DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_User_GetUsersByCondition`;;

CREATE DEFINER=`root`@`%` PROCEDURE `sp_User_GetUsersByCondition`(
  IN  IN_Code         varchar(20),
  OUT OUT_ReturnValue INT
)
BEGIN
  SET OUT_ReturnValue = 0;
  SELECT Id, Code, Password, IsActive FROM User
  WHERE (IN_Code IS NULL OR Code LIKE CONCAT(IN_Code,'%'));
  SET OUT_ReturnValue = 1;
END;;

DELIMITER ;
-- Update User --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_User_UpdateUser`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_User_UpdateUser`(
  IN  IN_Id    int,
  IN  IN_Password varchar(70),
  IN  IN_IsActive    bit,
  OUT OUT_ReturnValue int
)
BEGIN
    SET OUT_ReturnValue = 0;
    UPDATE User SET PASSWORD = IN_Password, IsActive = IN_IsActive WHERE Id = IN_Id;
    SET OUT_ReturnValue = 1;
  END ;;
DELIMITER ;

-- Delete User --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_User_DeleteUser`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_User_DeleteUser`(
  IN  IN_Id    int,
  OUT OUT_ReturnValue int
)
BEGIN
  SET OUT_ReturnValue = 0;
  DELETE FROM User WHERE Id = IN_Id;
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Product --

DROP TABLE IF EXISTS `Product`;
CREATE TABLE `Product` (
  `Id`            int AUTO_INCREMENT NOT NULL,
  `Name`          varchar(70)        NOT NULL,
  `Amount`        int                NOT NULL,
  `AccountId`     int                NOT NULL,
  PRIMARY KEY (`Id`),
  FOREIGN KEY(AccountId) references User(Id),
  INDEX IX_Product_AccountId (AccountId)
);


-- Create Product --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_Product_CreateProduct`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_Product_CreateProduct`(
  IN  IN_Name         varchar(70),
  IN  IN_Amount       int,
  IN  IN_AccountId    int,
  OUT OUT_ReturnValue int
)
BEGIN
  SET OUT_ReturnValue = 0;
  INSERT INTO Product (Name, Amount, AccountId) values (IN_Name, IN_Amount, IN_AccountId);
  SELECT LAST_INSERT_ID() as ID;
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get Products --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_Product_GetProducts`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_Product_GetProducts`(OUT OUT_ReturnValue int)
BEGIN
  SET OUT_ReturnValue = 0;
  SELECT Id, Name, Amount, AccountId FROM Product;
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get Products pagination--

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_Product_GetProductPage`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_Product_GetProductPage`(
  IN  IN_Limit         int,
  IN  IN_Offset       int,
  OUT OUT_ReturnValue int
  )
BEGIN
  SET OUT_ReturnValue = 0;

  SELECT Id, Name, Amount, AccountId FROM Product LIMIT IN_Limit OFFSET IN_Offset;
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get Product --

DELIMITER ;;

DROP PROCEDURE IF EXISTS `sp_Product_GetProduct`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_Product_GetProduct`(
  IN  IN_Id           int,
  OUT OUT_ReturnValue int
)
BEGIN
    SET OUT_ReturnValue = 0;
    SELECT Id, Name, Amount, AccountId FROM Product WHERE Id = IN_Id LIMIT 1;
    SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;

-- Get Product By Name --
DELIMITER ;;

DROP PROCEDURE IF EXISTS `sp_Product_GetProductsByName`;;

CREATE DEFINER=`root`@`%` PROCEDURE `sp_Product_GetProductsByName`(
  IN  IN_Name         varchar(20),
  OUT OUT_ReturnValue INT
)
BEGIN
  SET OUT_ReturnValue = 0;
  SELECT Id, Name, Amount, AccountId FROM Product
  WHERE (IN_Name IS NULL OR Name LIKE CONCAT(IN_Name,'%'));
  SET OUT_ReturnValue = 1;
END;;

DELIMITER ;

-- Create Multi Products --

DELIMITER ;;
DROP PROCEDURE IF EXISTS `sp_Product_CreateMultiProducts`;;
CREATE DEFINER=`root`@`%` PROCEDURE `sp_Product_CreateMultiProducts`(
  IN  IN_Names        text,
  IN  IN_Amount      int,
  IN  IN_AccountId    int,
  OUT OUT_ReturnValue int
)
BEGIN
  DECLARE strLenNames    INT DEFAULT 0;
  DECLARE SubStrLenNames INT DEFAULT 0;
  DECLARE name 			     varchar(20);
  DECLARE EXIT HANDLER FOR SQLEXCEPTION
  BEGIN
    ROLLBACK;
  END;
  SET OUT_ReturnValue = 0;
  START TRANSACTION;

  do_this:
    LOOP
      SET strLenNames = CHAR_LENGTH(IN_Names);
      SET name = SUBSTRING_INDEX(IN_Names, ',', 1);

      INSERT INTO Product (Name, Amount, AccountId) values (name, IN_Amount, IN_AccountId);

      SET SubStrLenNames = CHAR_LENGTH(SUBSTRING_INDEX(IN_Names, ',', 1))+2;
      SET IN_Names = MID(IN_Names, SubStrLenNames, strLenNames);

      IF IN_Names = '' THEN
        LEAVE do_this;
      END IF;
    END LOOP do_this;

  COMMIT;
  
  SET OUT_ReturnValue = 1;
END ;;
DELIMITER ;