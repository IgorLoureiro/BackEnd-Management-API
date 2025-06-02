CREATE TABLE IF NOT EXISTS `User` (
   `id` INT AUTO_INCREMENT NOT NULL,
   `username` VARCHAR(255) NOT NULL,
   `password` VARCHAR(255) NOT NULL,
   `email` VARCHAR(255) NOT NULL,
   `role` ENUM("admin", "user") NOT NULL default "user",
   `passwordRecovery` VARCHAR(255) NULL,
   `otpCode` VARCHAR(255) NULL,
   `otpExpiration` DATETIME NULL,

   PRIMARY KEY(`id`)
);