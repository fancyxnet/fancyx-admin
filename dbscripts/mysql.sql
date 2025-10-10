-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: fadmin
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `dict_data`
--

DROP TABLE IF EXISTS `dict_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dict_data` (
  `id` bigint NOT NULL,
  `value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `label` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `sort` int NOT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dict_data`
--

LOCK TABLES `dict_data` WRITE;
/*!40000 ALTER TABLE `dict_data` DISABLE KEYS */;
/*!40000 ALTER TABLE `dict_data` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `log_record`
--

DROP TABLE IF EXISTS `log_record`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `log_record` (
  `id` bigint NOT NULL,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `type` varchar(64) NOT NULL,
  `sub_type` varchar(512) NOT NULL,
  `biz_no` varchar(64) NOT NULL,
  `content` text NOT NULL,
  `browser` varchar(512) DEFAULT NULL,
  `ip` varchar(32) DEFAULT NULL,
  `trace_id` varchar(64) DEFAULT NULL,
  `tenant_id` varchar(18) DEFAULT NULL,
  `user_id` bigint DEFAULT NULL,
  `user_name` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `log_record`
--

LOCK TABLES `log_record` WRITE;
/*!40000 ALTER TABLE `log_record` DISABLE KEYS */;
/*!40000 ALTER TABLE `log_record` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu`
--

DROP TABLE IF EXISTS `menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu` (
  `id` bigint NOT NULL,
  `title` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `icon` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `path` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `component` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `menu_type` int NOT NULL,
  `permission` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `parent_id` bigint DEFAULT NULL,
  `sort` int NOT NULL,
  `display` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `is_external` tinyint(1) NOT NULL,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu`
--

LOCK TABLES `menu` WRITE;
/*!40000 ALTER TABLE `menu` DISABLE KEYS */;
/*!40000 ALTER TABLE `menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_role`
--

DROP TABLE IF EXISTS `user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_role` (
  `id` bigint NOT NULL,
  `user_id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`),
  KEY `IX_user_role_role_id` (`role_id`),
  KEY `IX_user_role_user_id` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_role`
--

LOCK TABLES `user_role` WRITE;
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` VALUES (631737765623021570,631737765623021571,631737765623021569,NULL);
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `config`
--

DROP TABLE IF EXISTS `config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `config` (
  `id` bigint NOT NULL,
  `name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `key` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `group_key` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `config`
--

LOCK TABLES `config` WRITE;
/*!40000 ALTER TABLE `config` DISABLE KEYS */;
/*!40000 ALTER TABLE `config` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` bigint NOT NULL,
  `role_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `is_enabled` tinyint(1) NOT NULL,
  `dept_power_type` int NOT NULL,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint DEFAULT NULL,
  `deletion_time` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (631737765623021569,'系统管理员','系统默认创建，拥有最高权限',NULL,1,0,NULL,'2025-09-14 11:04:21.533056',NULL,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `position`
--

DROP TABLE IF EXISTS `position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `position` (
  `id` bigint NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `level` int NOT NULL,
  `status` int NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `group_id` bigint DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint DEFAULT NULL,
  `deletion_time` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `position`
--

LOCK TABLES `position` WRITE;
/*!40000 ALTER TABLE `position` DISABLE KEYS */;
/*!40000 ALTER TABLE `position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_menu`
--

DROP TABLE IF EXISTS `role_menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_menu` (
  `id` bigint NOT NULL,
  `menu_id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`),
  KEY `IX_role_menu_menu_id` (`menu_id`),
  KEY `IX_role_menu_role_id` (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role_menu`
--

LOCK TABLES `role_menu` WRITE;
/*!40000 ALTER TABLE `role_menu` DISABLE KEYS */;
/*!40000 ALTER TABLE `role_menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notification`
--

DROP TABLE IF EXISTS `notification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notification` (
  `id` bigint NOT NULL,
  `title` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `content` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `user_id` bigint NOT NULL,
  `is_readed` tinyint(1) NOT NULL,
  `readed_time` datetime(6) DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notification`
--

LOCK TABLES `notification` WRITE;
/*!40000 ALTER TABLE `notification` DISABLE KEYS */;
/*!40000 ALTER TABLE `notification` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `api_access_log`
--

DROP TABLE IF EXISTS `api_access_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `api_access_log` (
  `id` bigint NOT NULL,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `path` text,
  `method` varchar(16) DEFAULT NULL,
  `ip` varchar(32) DEFAULT NULL,
  `request_time` datetime(6) NOT NULL,
  `response_time` datetime(6) DEFAULT NULL,
  `duration` bigint DEFAULT NULL,
  `user_id` bigint DEFAULT NULL,
  `user_name` varchar(32) DEFAULT NULL,
  `request_body` text,
  `response_body` text,
  `browser` varchar(512) DEFAULT NULL,
  `query_string` text,
  `trace_id` varchar(64) DEFAULT NULL,
  `operate_type` json DEFAULT NULL,
  `operate_name` varchar(64) DEFAULT NULL,
  `tenant_id` varchar(18) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `api_access_log`
--

LOCK TABLES `api_access_log` WRITE;
/*!40000 ALTER TABLE `api_access_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `api_access_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` bigint NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password_salt` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `avatar` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `nick_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sex` int NOT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `dept_id` bigint DEFAULT NULL,
  `post_id` bigint DEFAULT NULL,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint DEFAULT NULL,
  `deletion_time` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (631737765623021571,'admin','a2fa8ec90f15197c7a4e6e00525b198a','vHQZvbz+ng+B4NrSAEYl6g==','file/myavatar.jpg','风汐',2,1,NULL,'18211114444',NULL,NULL,NULL,'2025-09-10 21:13:06.186342',NULL,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `position_group`
--

DROP TABLE IF EXISTS `position_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `position_group` (
  `id` bigint NOT NULL,
  `group_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `parent_id` bigint DEFAULT NULL,
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tree_level` int NOT NULL,
  `sort` int NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `position_group`
--

LOCK TABLES `position_group` WRITE;
/*!40000 ALTER TABLE `position_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `position_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dict_type`
--

DROP TABLE IF EXISTS `dict_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dict_type` (
  `id` bigint NOT NULL,
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dict_type`
--

LOCK TABLES `dict_type` WRITE;
/*!40000 ALTER TABLE `dict_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `dict_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tenant`
--

DROP TABLE IF EXISTS `tenant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenant` (
  `id` bigint NOT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_tenant_tenant_id` (`tenant_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tenant`
--

LOCK TABLES `tenant` WRITE;
/*!40000 ALTER TABLE `tenant` DISABLE KEYS */;
/*!40000 ALTER TABLE `tenant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dept`
--

DROP TABLE IF EXISTS `dept`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dept` (
  `id` bigint NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sort` int NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `status` int NOT NULL,
  `curator_id` bigint DEFAULT NULL,
  `email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `phone` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `parent_id` bigint DEFAULT NULL,
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tree_level` int NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` bigint DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint DEFAULT NULL,
  `deletion_time` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dept`
--

LOCK TABLES `dept` WRITE;
/*!40000 ALTER TABLE `dept` DISABLE KEYS */;
/*!40000 ALTER TABLE `dept` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `exception_log`
--

DROP TABLE IF EXISTS `exception_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `exception_log` (
  `id` bigint NOT NULL,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `exception_type` varchar(64) DEFAULT NULL,
  `message` text,
  `stack_trace` text,
  `inner_exception` text,
  `request_path` text,
  `request_method` varchar(16) DEFAULT NULL,
  `user_id` bigint DEFAULT NULL,
  `user_name` varchar(16) DEFAULT NULL,
  `ip` varchar(32) DEFAULT NULL,
  `browser` varchar(512) DEFAULT NULL,
  `trace_id` varchar(64) DEFAULT NULL,
  `is_handled` tinyint(1) NOT NULL,
  `handled_time` datetime(6) DEFAULT NULL,
  `handled_by` varchar(255) DEFAULT NULL,
  `tenant_id` varchar(18) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `exception_log`
--

LOCK TABLES `exception_log` WRITE;
/*!40000 ALTER TABLE `exception_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `exception_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_dept`
--

DROP TABLE IF EXISTS `role_dept`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_dept` (
  `id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `dept_id` bigint NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`),
  KEY `IX_role_dept_role_id` (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role_dept`
--

LOCK TABLES `role_dept` WRITE;
/*!40000 ALTER TABLE `role_dept` DISABLE KEYS */;
/*!40000 ALTER TABLE `role_dept` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `login_log`
--

DROP TABLE IF EXISTS `login_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `login_log` (
  `id` bigint NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `address` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `operation_msg` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `is_success` tinyint(1) NOT NULL,
  `session_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` bigint DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `login_log`
--

LOCK TABLES `login_log` WRITE;
/*!40000 ALTER TABLE `login_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `login_log` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-10-10 14:25:09
