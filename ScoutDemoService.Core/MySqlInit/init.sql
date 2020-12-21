CREATE SCHEMA IF NOT EXISTS scoutdemo;
CREATE USER IF NOT EXISTS 'scoutdemo'@'%' IDENTIFIED WITH mysql_native_password BY 'scoutdemopassword1234!';
GRANT ALL ON scoutdemo.* TO 'scoutdemo'@'%';

DROP TABLE IF EXISTS `scoutdemo`.`people`;
CREATE TABLE `scoutdemo`.`people` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `firstname` varchar(45) DEFAULT NULL,
  `lastname`varchar(45) DEFAULT NULL,
  `emailaddress`varchar(45) DEFAULT NULL,
  `address1`varchar(45) DEFAULT NULL,
  `address2`varchar(45) DEFAULT NULL,
  `city`varchar(45) DEFAULT NULL,
  `state`varchar(45) DEFAULT NULL,
  `zipcode`varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;