/* Run With Bash INIT Files 2nd one
Command:

sudo mysql < SqlInit.sql > output.txt

*/
CREATE DATABASE DoorAccess;
USE DoorAccess;
CREATE TABLE Log(id INT(20) PRIMARY KEY AUTO_INCREMENT NOT NULL, UserID VARCHAR(255), date DATE);
CREATE TABLE Users(id VARCHAR(20) PRIMARY KEY NOT NULL, fname VARCHAR(255) NOT NULL, lname VARCHAR(255) NOT NULL, Tier INT(3) NOT NULL);