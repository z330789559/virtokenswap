/*
 Navicat Premium Data Transfer

 Source Server         : yf-saas
 Source Server Type    : MySQL
 Source Server Version : 80032
 Source Host           : 113.31.116.160:3306
 Source Schema         : orderdb

 Target Server Type    : MySQL
 Target Server Version : 80032
 File Encoding         : 65001

 Date: 07/06/2023 22:44:42
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for Goods
-- ----------------------------
DROP TABLE IF EXISTS `Goods`;
CREATE TABLE `Goods` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Price` double NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for OrderDetails
-- ----------------------------
DROP TABLE IF EXISTS `OrderDetails`;
CREATE TABLE `OrderDetails` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Index` int NOT NULL DEFAULT '0',
  `GoodsId` int NOT NULL DEFAULT '0',
  `GoodsName` varchar(100) NOT NULL DEFAULT '',
  `UnitPrice` decimal(10,2) NOT NULL DEFAULT '0.00',
  `OrderId` int NOT NULL DEFAULT '0',
  `Quantity` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for Orders
-- ----------------------------
DROP TABLE IF EXISTS `Orders`;
CREATE TABLE `Orders` (
  `OrderId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL DEFAULT '0',
  `CreateTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `TotalPrice` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`OrderId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for Result
-- ----------------------------
DROP TABLE IF EXISTS `Result`;
CREATE TABLE `Result` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `VirtualToken1` int NOT NULL DEFAULT '0',
  `VirtualToken2` int NOT NULL DEFAULT '0',
  `BestReturn` decimal(10,2) NOT NULL DEFAULT '0.00',
  `BestVariance` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Weight1` decimal(10,2) NOT NULL DEFAULT '0.00',
  `Weight2` decimal(10,2) NOT NULL DEFAULT '0.00',
  `ShapeRatio` decimal(10,2) NOT NULL DEFAULT '0.00',
  `RiskWeight` decimal(10,2) NOT NULL DEFAULT '0.00',
  `FreeRiskWeight` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for User
-- ----------------------------
DROP TABLE IF EXISTS `User`;
CREATE TABLE `User` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `Asset` double NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for VirtualToken
-- ----------------------------
DROP TABLE IF EXISTS `VirtualToken`;
CREATE TABLE `VirtualToken` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL DEFAULT '',
  `DayReturn` varchar(2048) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_as_ci NOT NULL DEFAULT '',
  `Price` varchar(2048) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_as_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

SET FOREIGN_KEY_CHECKS = 1;
