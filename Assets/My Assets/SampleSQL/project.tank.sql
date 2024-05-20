-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        11.3.2-MariaDB - mariadb.org binary distribution
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  12.6.0.6765
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- project_tank 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `project_tank` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `project_tank`;

-- 테이블 project_tank.tanks 구조 내보내기
CREATE TABLE IF NOT EXISTS `tanks` (
  `tank_id` int(11) NOT NULL AUTO_INCREMENT,
  `tank_name` varchar(255) NOT NULL,
  `tank_nation` varchar(50) DEFAULT NULL,
  `tank_price` int(11) DEFAULT NULL,
  `description` text DEFAULT NULL,
  PRIMARY KEY (`tank_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 project_tank.tanks:~0 rows (대략적) 내보내기
INSERT INTO `tanks` (`tank_id`, `tank_name`, `tank_nation`, `tank_price`, `description`) VALUES
	(1, 'M1', 'USA', 5000, 'Made in USA'),
	(2, 'T-72', 'RUSSIA', 5000, 'Made in Russia');

-- 테이블 project_tank.users 구조 내보내기
CREATE TABLE IF NOT EXISTS `users` (
  `uid` varchar(36) NOT NULL,
  `user_email` varchar(255) DEFAULT NULL,
  `user_password` varchar(64) DEFAULT NULL,
  `user_nickname` varchar(20) DEFAULT NULL,
  `user_level` int(11) DEFAULT 1,
  `silver` int(11) DEFAULT 50000,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 project_tank.users:~4 rows (대략적) 내보내기
INSERT INTO `users` (`uid`, `user_email`, `user_password`, `user_nickname`, `user_level`, `silver`) VALUES
	('0221c910-5eb1-4166-b182-efb1bf5487c9', 'test3', 'fd61a03af4f77d870fc21e05e7e80678095c92d808cfb3b5c279ee04c74aca13', 'test3', 1, 50000),
	('50edbfb6-86ae-4643-8547-1659263df4c3', 'qwerty', '65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5', 'Qwerty', 1, 50000),
	('7aa425d5-0a91-4677-95b5-01d010526225', 'test', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08', 'test', 1, 50000),
	('bd495cc0-b688-414e-b34a-e80890b61229', 'test2', '60303ae22b998861bce3b28f33eec1be758a213c86c93c076dbe9f558c11c752', 'test2', 1, 50000);

-- 테이블 project_tank.user_owned_tanks 구조 내보내기
CREATE TABLE IF NOT EXISTS `user_owned_tanks` (
  `user_tank_id` int(11) NOT NULL AUTO_INCREMENT,
  `uid` varchar(36) DEFAULT NULL,
  `tank_id` int(11) DEFAULT NULL,
  `item_slot_1` int(11) unsigned DEFAULT 0,
  `item_slot_2` int(11) DEFAULT 0,
  `item_slot_3` int(11) DEFAULT 0,
  `camo_slot` int(11) DEFAULT 0,
  PRIMARY KEY (`user_tank_id`),
  KEY `uid` (`uid`),
  KEY `tank_id` (`tank_id`),
  CONSTRAINT `user_owned_tanks_ibfk_1` FOREIGN KEY (`uid`) REFERENCES `users` (`uid`),
  CONSTRAINT `user_owned_tanks_ibfk_2` FOREIGN KEY (`tank_id`) REFERENCES `tanks` (`tank_id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 project_tank.user_owned_tanks:~5 rows (대략적) 내보내기
INSERT INTO `user_owned_tanks` (`user_tank_id`, `uid`, `tank_id`, `item_slot_1`, `item_slot_2`, `item_slot_3`, `camo_slot`) VALUES
	(3, '50edbfb6-86ae-4643-8547-1659263df4c3', 2, 0, 0, 0, 0),
	(5, '50edbfb6-86ae-4643-8547-1659263df4c3', 1, 0, 0, 0, 5),
	(9, 'bd495cc0-b688-414e-b34a-e80890b61229', 1, 0, 0, 0, 0),
	(10, '7aa425d5-0a91-4677-95b5-01d010526225', 1, 0, 0, 0, 0),
	(11, '0221c910-5eb1-4166-b182-efb1bf5487c9', 1, 0, 0, 0, 0);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
