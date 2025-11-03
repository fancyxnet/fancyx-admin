/*
 Navicat Premium Data Transfer

 Source Server         : 本地mysql
 Source Server Type    : MySQL
 Source Server Version : 80042 (8.0.42)
 Source Host           : localhost:3306
 Source Schema         : fancyx-admin

 Target Server Type    : MySQL
 Target Server Version : 80042 (8.0.42)
 File Encoding         : 65001

 Date: 03/11/2025 20:50:10
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for api_access_log
-- ----------------------------
DROP TABLE IF EXISTS `api_access_log`;
CREATE TABLE `api_access_log`  (
  `id` bigint NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `path` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `method` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `request_time` datetime(6) NOT NULL,
  `response_time` datetime(6) NULL DEFAULT NULL,
  `duration` bigint NULL DEFAULT NULL,
  `user_id` bigint NULL DEFAULT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `request_body` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `response_body` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `query_string` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `trace_id` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `operate_type` json NULL,
  `operate_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of api_access_log
-- ----------------------------

-- ----------------------------
-- Table structure for cap.published
-- ----------------------------
DROP TABLE IF EXISTS `cap.published`;
CREATE TABLE `cap.published`  (
  `Id` bigint NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Retries` int NULL DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime NULL DEFAULT NULL,
  `StatusName` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_Version_ExpiresAt_StatusName`(`Version` ASC, `ExpiresAt` ASC, `StatusName` ASC) USING BTREE,
  INDEX `IX_ExpiresAt_StatusName`(`ExpiresAt` ASC, `StatusName` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.published
-- ----------------------------

-- ----------------------------
-- Table structure for cap.received
-- ----------------------------
DROP TABLE IF EXISTS `cap.received`;
CREATE TABLE `cap.received`  (
  `Id` bigint NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Group` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Retries` int NULL DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime NULL DEFAULT NULL,
  `StatusName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_Version_ExpiresAt_StatusName`(`Version` ASC, `ExpiresAt` ASC, `StatusName` ASC) USING BTREE,
  INDEX `IX_ExpiresAt_StatusName`(`ExpiresAt` ASC, `StatusName` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.received
-- ----------------------------

-- ----------------------------
-- Table structure for config
-- ----------------------------
DROP TABLE IF EXISTS `config`;
CREATE TABLE `config`  (
  `id` bigint NOT NULL,
  `name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `key` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `group_key` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of config
-- ----------------------------

-- ----------------------------
-- Table structure for dept
-- ----------------------------
DROP TABLE IF EXISTS `dept`;
CREATE TABLE `dept`  (
  `id` bigint NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sort` int NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `status` int NOT NULL,
  `curator_id` bigint NULL DEFAULT NULL,
  `email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `phone` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `parent_id` bigint NULL DEFAULT NULL,
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tree_level` int NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of dept
-- ----------------------------
INSERT INTO `dept` VALUES (4503701543874727936, '001', '深圳风汐科技有限公司', 0, NULL, 1, NULL, NULL, NULL, NULL, '4503701543874727936', 1, NULL, 631737765623021571, '2025-10-28 20:22:03.839099', NULL, NULL, 1, 631737765623021571, '2025-10-28 20:22:06.879039');

-- ----------------------------
-- Table structure for dict_data
-- ----------------------------
DROP TABLE IF EXISTS `dict_data`;
CREATE TABLE `dict_data`  (
  `id` bigint NOT NULL,
  `value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `label` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `sort` int NOT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of dict_data
-- ----------------------------

-- ----------------------------
-- Table structure for dict_type
-- ----------------------------
DROP TABLE IF EXISTS `dict_type`;
CREATE TABLE `dict_type`  (
  `id` bigint NOT NULL,
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of dict_type
-- ----------------------------

-- ----------------------------
-- Table structure for exception_log
-- ----------------------------
DROP TABLE IF EXISTS `exception_log`;
CREATE TABLE `exception_log`  (
  `id` bigint NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `exception_type` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `message` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `stack_trace` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `inner_exception` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `request_path` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `request_method` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `user_id` bigint NULL DEFAULT NULL,
  `user_name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `trace_id` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `is_handled` tinyint(1) NOT NULL,
  `handled_time` datetime(6) NULL DEFAULT NULL,
  `handled_by` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of exception_log
-- ----------------------------

-- ----------------------------
-- Table structure for log_record
-- ----------------------------
DROP TABLE IF EXISTS `log_record`;
CREATE TABLE `log_record`  (
  `id` bigint NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `type` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sub_type` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `biz_no` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `trace_id` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `user_id` bigint NULL DEFAULT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of log_record
-- ----------------------------

-- ----------------------------
-- Table structure for login_log
-- ----------------------------
DROP TABLE IF EXISTS `login_log`;
CREATE TABLE `login_log`  (
  `id` bigint NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `address` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `operation_msg` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `is_success` tinyint(1) NOT NULL,
  `session_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of login_log
-- ----------------------------

-- ----------------------------
-- Table structure for menu
-- ----------------------------
DROP TABLE IF EXISTS `menu`;
CREATE TABLE `menu`  (
  `id` bigint NOT NULL,
  `title` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `icon` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `path` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `component` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `menu_type` int NOT NULL,
  `permission` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `parent_id` bigint NULL DEFAULT NULL,
  `sort` int NOT NULL,
  `display` tinyint(1) NOT NULL,
  `is_external` tinyint(1) NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `keep_alive` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否保活',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES (4491762374256627712, '系统管理', 'antd:SettingOutlined', '/system', NULL, 1, 'System', NULL, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627713, '组织架构', 'antd:TeamOutlined', '/org', NULL, 1, 'Org', NULL, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627714, '在线文档', 'antd:ApiOutlined', 'https://doc.crackerwork.cn/', '#', 2, NULL, NULL, 99, 1, 1, NULL, '2025-10-10 21:51:44.000000', '2025-10-26 10:57:05.055614', 631737765623021571, b'0');
INSERT INTO `menu` VALUES (4491762374256627715, '快速开发', 'antd:ToolOutlined', '/quickWork', NULL, 1, '', NULL, 98, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627717, '重置密码', NULL, NULL, NULL, 3, 'Sys.User.ResetPwd', 4491762374256627763, 9, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627718, '分配功能权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignMenu', 4491762374256627751, 5, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627719, '编辑', NULL, NULL, NULL, 3, 'Sys.User.Update', 4491762374256627763, 10, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627720, '新增', NULL, NULL, NULL, 3, 'Sys.Config.Add', 4491762374256627750, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627721, '访问日志', NULL, '/monitor/apiAccessLog', 'monitor/apiAccessLog', 2, '', 4491762374256627754, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627722, '新增', NULL, NULL, NULL, 3, 'Sys.User.Add', 4491762374256627763, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627723, '查询', NULL, NULL, NULL, 3, 'Sys.User.List', 4491762374256627763, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627724, '分配角色', NULL, NULL, NULL, 3, 'Sys.User.AssignRole', 4491762374256627763, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627725, '启用/禁用', NULL, NULL, NULL, 3, 'Sys.User.SwitchEnabledStatus', 4491762374256627763, 5, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627726, '编辑', NULL, NULL, NULL, 3, 'Sys.Role.Update', 4491762374256627751, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627727, '新增', NULL, NULL, NULL, 3, 'Org.PositionGroup.Add', 4491762374256627771, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627728, '查询', NULL, NULL, NULL, 3, 'Org.PositionGroup.List', 4491762374256627771, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627729, '编辑', NULL, NULL, NULL, 3, 'Org.PositionGroup.Update', 4491762374256627771, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627730, '删除', NULL, NULL, NULL, 3, 'Org.PositionGroup.Delete', 4491762374256627771, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627731, '新增', NULL, NULL, NULL, 3, 'Org.Position.Add', 4491762374256627760, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627732, '编辑', NULL, NULL, NULL, 3, 'Org.Position.Update', 4491762374256627760, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627733, '删除', NULL, NULL, NULL, 3, 'Org.Position.Delete', 4491762374256627760, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627734, '新增', NULL, NULL, NULL, 3, 'Org.Dept.Add', 4491762374256627761, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627735, '查询', NULL, NULL, NULL, 3, 'Org.Dept.List', 4491762374256627761, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627736, '编辑', NULL, NULL, NULL, 3, 'Org.Dept.Update', 4491762374256627761, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627737, '删除', NULL, NULL, NULL, 3, 'Org.Dept.Delete', 4491762374256627761, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627738, '注销', NULL, NULL, NULL, 3, 'Monitor.Logout', 4491762374256627765, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627739, '新增', NULL, NULL, NULL, 3, 'Sys.DictType.Add', 4491762374256627762, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627740, '富文本组件', NULL, '/quickWork/rickText', 'quickWork/rickText', 2, '', 4491762374256627715, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627744, '编辑', NULL, NULL, NULL, 3, 'Sys.DictData.Update', 4491762374256627789, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627745, '删除', NULL, NULL, NULL, 3, 'Sys.DictData.Delete', 4491762374256627789, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627746, '编辑', NULL, NULL, NULL, 3, 'Sys.Menu.Update', 4491762374256627752, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627747, '删除', NULL, NULL, NULL, 3, 'Sys.Menu.Delete', 4491762374256627752, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627748, '查询', NULL, NULL, NULL, 3, 'Sys.Role.List', 4491762374256627751, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627749, '删除', NULL, NULL, NULL, 3, 'Sys.Role.Delete', 4491762374256627751, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627750, '配置管理', NULL, '/system/config', 'system/config', 2, '', 4491762374256627712, 7, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627751, '角色管理', NULL, '/system/role', 'system/role', 2, 'Sys:Role', 4491762374256627712, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627752, '菜单管理', NULL, '/system/menu', 'system/menu', 2, 'Sys:Menu', 4491762374256627712, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627753, '分配数据权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignDataScope', 4491762374256627751, 6, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627754, '系统监控', 'antd:FundOutlined', '/monitor', NULL, 1, '', NULL, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627755, '通知管理', '', '/org/notification', 'org/notification', 2, '', 4491762374256627713, 5, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627756, '删除', NULL, NULL, NULL, 3, 'Sys.User.Delete', 4491762374256627763, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627757, '查询', NULL, NULL, NULL, 3, 'Org.Position.List', 4491762374256627760, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627758, '查询', NULL, NULL, NULL, 3, 'Sys.DictType.List', 4491762374256627762, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627759, '新增', NULL, NULL, NULL, 3, 'Sys.Role.Add', 4491762374256627751, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627760, '职位管理', NULL, '/org/position', 'org/position', 2, 'Org:Position', 4491762374256627713, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627761, '部门管理', NULL, '/org/dept', 'org/dept', 2, 'Org:Department', 4491762374256627713, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627762, '数据字典', NULL, '/system/dict', 'system/dictType', 2, 'Sys:Dict', 4491762374256627712, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627763, '用户管理', '', '/system/user', 'system/user', 2, '', 4491762374256627712, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627764, '异常日志', NULL, '/monitor/exceptionLog', 'monitor/exceptionLog', 2, '', 4491762374256627754, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627765, '在线用户', NULL, '/monitor/onlineUser', 'monitor/onlineUser', 2, '', 4491762374256627754, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627770, '租户管理', NULL, '/system/tenant', 'system/tenant', 2, '', 4491762374256627712, 8, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627771, '职位分组', '', '/org/positionGroup', 'org/positionGroup', 2, NULL, 4491762374256627713, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', '2025-11-02 20:53:47.382446', 631737765623021571, b'1');
INSERT INTO `menu` VALUES (4491762374256627772, '新增', NULL, NULL, NULL, 3, 'Sys.Notification.Add', 4491762374256627755, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627773, '查询', NULL, NULL, NULL, 3, 'Sys.Notification.List', 4491762374256627755, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627774, '编辑', NULL, NULL, NULL, 3, 'Sys.Notification.Update', 4491762374256627755, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627775, '删除', NULL, NULL, NULL, 3, 'Sys.Notification.Delete', 4491762374256627755, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627776, '我的通知', NULL, '/org/myNotification', 'org/myNotification', 2, '', 4491762374256627713, 6, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627777, '登录日志', NULL, '/system/log/login', 'system/log/loginLog', 2, '', 4491762374256627712, 10, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627778, '业务日志', NULL, '/system/log/business', 'system/log/businessLog', 2, '', 4491762374256627712, 11, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627780, '删除', NULL, '', NULL, 3, 'Sys.DictType.Delete', 4491762374256627762, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627781, '新增', NULL, NULL, NULL, 3, 'Sys.DictData.Add', 4491762374256627789, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627782, '查询', NULL, NULL, NULL, 3, 'Sys.DictData.List', 4491762374256627789, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627783, '新增', NULL, NULL, NULL, 3, 'Sys.Menu.Add', 4491762374256627752, 1, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627784, '查询', NULL, NULL, NULL, 3, 'Sys.Menu.List', 4491762374256627752, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627785, '部门简单信息', NULL, NULL, NULL, 3, 'Org.Dept.GetDeptSimpleInfos', 4491762374256627761, 5, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627786, '查询', NULL, NULL, NULL, 3, 'Sys.Config.List', 4491762374256627750, 2, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627787, '编辑', NULL, NULL, NULL, 3, 'Sys.Config.Update', 4491762374256627750, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627788, '删除', NULL, NULL, NULL, 3, 'Sys.Config.Delete', 4491762374256627750, 4, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4491762374256627789, '字典项', NULL, '/system/dictItem/:dictType', 'system/dictData', 2, NULL, 4491762374256627712, 5, 0, 0, NULL, '2025-10-10 21:51:44.000000', '2025-10-26 10:56:54.927941', 631737765623021571, b'0');
INSERT INTO `menu` VALUES (4491762374256627791, '编辑', NULL, NULL, NULL, 3, 'Sys.DictType.Update', 4491762374256627762, 3, 1, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4504417542584406016, '分配功能', NULL, NULL, NULL, 3, 'Sys.Tenant.AssignTenantMenu', 4491762374256627770, 5, 1, 0, 631737765623021571, '2025-10-30 19:47:11.203231', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4504424227461926912, '新增', NULL, NULL, NULL, 3, 'Sys.Tenant.Add', 4491762374256627770, 1, 1, 0, 631737765623021571, '2025-10-30 20:13:45.002466', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4504424322014121984, '列表', NULL, NULL, NULL, 3, 'Sys.Tenant.List', 4491762374256627770, 2, 1, 0, 631737765623021571, '2025-10-30 20:14:07.545010', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4504424382105915392, '编辑', NULL, NULL, NULL, 3, 'Sys.Tenant.Update', 4491762374256627770, 3, 1, 0, 631737765623021571, '2025-10-30 20:14:21.872420', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4504424443569246208, '删除', NULL, NULL, NULL, 3, 'Sys.Tenant.Delete', 4491762374256627770, 4, 1, 0, 631737765623021571, '2025-10-30 20:14:36.526863', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4505453783849373696, '查询', NULL, NULL, NULL, 3, 'Monitor.OnlineUser', 4491762374256627765, 1, 1, 0, 631737765623021571, '2025-11-02 16:24:50.373224', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4505453882075779072, '查询', NULL, NULL, NULL, 3, 'Monitor.ExceptionLogList', 4491762374256627764, 1, 1, 0, 631737765623021571, '2025-11-02 16:25:13.792930', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4505453985779945472, '处理异常', NULL, NULL, NULL, 3, 'Monitor.ExceptionLog.HandleException', 4491762374256627764, 2, 1, 0, 631737765623021571, '2025-11-02 16:25:38.517629', NULL, NULL, b'0');
INSERT INTO `menu` VALUES (4505454091476406272, '查询', NULL, NULL, NULL, 3, 'Monitor.ApiAccessLogList', 4491762374256627721, 1, 1, 0, 631737765623021571, '2025-11-02 16:26:03.717594', NULL, NULL, b'0');

-- ----------------------------
-- Table structure for notification
-- ----------------------------
DROP TABLE IF EXISTS `notification`;
CREATE TABLE `notification`  (
  `id` bigint NOT NULL,
  `title` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `content` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `user_id` bigint NOT NULL,
  `is_readed` tinyint(1) NOT NULL,
  `readed_time` datetime(6) NULL DEFAULT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of notification
-- ----------------------------

-- ----------------------------
-- Table structure for position
-- ----------------------------
DROP TABLE IF EXISTS `position`;
CREATE TABLE `position`  (
  `id` bigint NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `level` int NOT NULL,
  `status` int NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `group_id` bigint NULL DEFAULT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of position
-- ----------------------------

-- ----------------------------
-- Table structure for position_group
-- ----------------------------
DROP TABLE IF EXISTS `position_group`;
CREATE TABLE `position_group`  (
  `id` bigint NOT NULL,
  `group_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `parent_id` bigint NULL DEFAULT NULL,
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tree_level` int NOT NULL,
  `sort` int NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of position_group
-- ----------------------------
INSERT INTO `position_group` VALUES (4505452973992185856, '前端分组', NULL, NULL, '4505452973992185856', 1, 0, 'platform', 631737765623021571, '2025-11-02 16:21:37.309496', NULL, NULL);
INSERT INTO `position_group` VALUES (4505472754346627072, '233232', '232323', NULL, '4505472754346627072', 1, 0, 'mi', 4505462198378172416, '2025-11-02 17:40:13.300295', NULL, NULL);

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role`  (
  `id` bigint NOT NULL,
  `role_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `dept_power_type` int NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  `is_platform_role` int NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (631737765623021568, '管理员', '', 'mi', 1, 0, NULL, '2025-09-14 11:04:21.533056', '2025-11-02 17:41:29.308653', 4505462198378172416, 0, NULL, NULL, 0);
INSERT INTO `role` VALUES (631737765623021569, '系统管理员', '', 'platform', 1, 0, NULL, '2025-09-14 11:04:21.533056', '2025-11-02 16:22:39.520604', 631737765623021571, 0, NULL, NULL, 0);

-- ----------------------------
-- Table structure for role_dept
-- ----------------------------
DROP TABLE IF EXISTS `role_dept`;
CREATE TABLE `role_dept`  (
  `role_id` bigint NOT NULL,
  `dept_id` bigint NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`role_id`, `dept_id`) USING BTREE,
  INDEX `IX_role_dept_role_id`(`role_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role_dept
-- ----------------------------

-- ----------------------------
-- Table structure for role_menu
-- ----------------------------
DROP TABLE IF EXISTS `role_menu`;
CREATE TABLE `role_menu`  (
  `menu_id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`menu_id`, `role_id`) USING BTREE,
  INDEX `IX_role_menu_menu_id`(`menu_id` ASC) USING BTREE,
  INDEX `IX_role_menu_role_id`(`role_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role_menu
-- ----------------------------
INSERT INTO `role_menu` VALUES (4491762374256627712, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627712, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627713, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627713, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627714, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627715, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627717, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627717, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627718, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627718, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627719, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627719, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627720, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627721, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627722, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627722, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627723, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627723, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627724, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627724, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627725, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627725, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627726, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627726, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627727, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627727, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627728, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627728, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627729, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627729, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627730, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627730, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627731, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627731, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627732, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627732, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627733, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627733, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627734, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627734, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627735, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627735, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627736, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627736, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627737, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627737, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627738, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627739, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627740, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627744, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627745, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627746, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627747, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627748, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627748, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627749, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627749, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627750, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627751, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627751, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627752, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627753, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627753, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627754, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627755, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627755, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627756, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627756, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627757, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627757, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627758, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627759, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627759, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627760, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627760, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627761, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627761, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627762, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627763, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627763, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627764, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627765, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627770, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627771, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627771, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627772, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627772, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627773, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627773, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627774, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627774, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627775, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627775, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627776, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627776, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627777, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627778, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627780, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627781, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627782, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627783, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627784, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627785, 631737765623021568, 'mi');
INSERT INTO `role_menu` VALUES (4491762374256627785, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627786, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627787, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627788, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627789, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4491762374256627791, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4504417542584406016, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4504424227461926912, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4504424322014121984, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4504424382105915392, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4504424443569246208, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4505453783849373696, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4505453882075779072, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4505453985779945472, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4505454091476406272, 631737765623021569, 'platform');

-- ----------------------------
-- Table structure for tenant
-- ----------------------------
DROP TABLE IF EXISTS `tenant`;
CREATE TABLE `tenant`  (
  `id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '启用状态',
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tenant
-- ----------------------------
INSERT INTO `tenant` VALUES ('mi', '大米公司', NULL, 'mi.crackerwork.cn', 631737765623021571, '2025-11-01 22:43:37.466969', '2025-11-03 20:49:24.066274', 631737765623021571, b'0', 0, NULL, NULL);
INSERT INTO `tenant` VALUES ('platform', '平台', NULL, 'platform.crackerwork.cn', 631737765623021571, '2025-10-30 20:38:31.319463', '2025-11-03 20:42:35.571284', 631737765623021571, b'1', 0, NULL, NULL);

-- ----------------------------
-- Table structure for tenant_menu
-- ----------------------------
DROP TABLE IF EXISTS `tenant_menu`;
CREATE TABLE `tenant_menu`  (
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '租户ID',
  `menu_id` bigint NOT NULL COMMENT '菜单ID',
  PRIMARY KEY (`tenant_id`, `menu_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tenant_menu
-- ----------------------------
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627712);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627713);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627717);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627718);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627719);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627722);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627723);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627724);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627725);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627726);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627727);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627728);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627729);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627730);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627731);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627732);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627733);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627734);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627735);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627736);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627737);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627748);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627749);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627751);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627753);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627755);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627756);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627757);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627759);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627760);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627761);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627763);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627771);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627772);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627773);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627774);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627775);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627776);
INSERT INTO `tenant_menu` VALUES ('mi', 4491762374256627785);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627712);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627713);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627714);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627715);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627717);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627718);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627719);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627720);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627721);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627722);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627723);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627724);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627725);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627726);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627727);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627728);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627729);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627730);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627731);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627732);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627733);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627734);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627735);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627736);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627737);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627738);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627739);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627740);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627744);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627745);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627746);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627747);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627748);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627749);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627750);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627751);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627752);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627753);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627754);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627755);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627756);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627757);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627758);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627759);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627760);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627761);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627762);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627763);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627764);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627765);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627770);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627771);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627772);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627773);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627774);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627775);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627776);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627777);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627778);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627780);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627781);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627782);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627783);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627784);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627785);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627786);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627787);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627788);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627789);
INSERT INTO `tenant_menu` VALUES ('platform', 4491762374256627791);
INSERT INTO `tenant_menu` VALUES ('platform', 4504417542584406016);
INSERT INTO `tenant_menu` VALUES ('platform', 4504424227461926912);
INSERT INTO `tenant_menu` VALUES ('platform', 4504424322014121984);
INSERT INTO `tenant_menu` VALUES ('platform', 4504424382105915392);
INSERT INTO `tenant_menu` VALUES ('platform', 4504424443569246208);
INSERT INTO `tenant_menu` VALUES ('platform', 4505453783849373696);
INSERT INTO `tenant_menu` VALUES ('platform', 4505453882075779072);
INSERT INTO `tenant_menu` VALUES ('platform', 4505453985779945472);
INSERT INTO `tenant_menu` VALUES ('platform', 4505454091476406272);

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user`  (
  `id` bigint NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password_salt` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `avatar` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `nick_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sex` int NOT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `phone` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `dept_id` bigint NULL DEFAULT NULL,
  `post_id` bigint NULL DEFAULT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES (631737765623021571, 'admin', 'a2fa8ec90f15197c7a4e6e00525b198a', 'vHQZvbz+ng+B4NrSAEYl6g==', 'file/myavatar.jpg', '风汐', 2, 1, 'platform', '18211114444', NULL, NULL, NULL, '2025-09-10 21:13:06.186342', '2025-11-03 20:42:35.619666', 631737765623021571, 0, NULL, NULL);
INSERT INTO `user` VALUES (4502836036707553280, 'test', '31b34585be7edab0062adaf1c5e8ca54', 'FHvandmgkN55J7rydt4RXA==', 'avatar/male.png', 'test_hdh', 1, 1, NULL, NULL, NULL, NULL, 631737765623021571, '2025-10-26 11:02:50.819162', '2025-10-26 11:03:39.408462', 631737765623021571, 1, 631737765623021571, '2025-10-26 11:08:14.994318');
INSERT INTO `user` VALUES (4505462198378172416, 'miadmin', 'a2fa8ec90f15197c7a4e6e00525b198a', 'vHQZvbz+ng+B4NrSAEYl6g==', 'avatar/male.png', 'miadmin', 1, 0, 'mi', NULL, NULL, NULL, 631737765623021571, '2025-11-02 16:58:16.553187', '2025-11-03 20:49:24.109932', 631737765623021571, 0, NULL, NULL);

-- ----------------------------
-- Table structure for user_role
-- ----------------------------
DROP TABLE IF EXISTS `user_role`;
CREATE TABLE `user_role`  (
  `user_id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`user_id`, `role_id`) USING BTREE,
  INDEX `IX_user_role_role_id`(`role_id` ASC) USING BTREE,
  INDEX `IX_user_role_user_id`(`user_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user_role
-- ----------------------------
INSERT INTO `user_role` VALUES (631737765623021571, 631737765623021569, 'platform');
INSERT INTO `user_role` VALUES (4505462198378172416, 631737765623021568, 'mi');

SET FOREIGN_KEY_CHECKS = 1;
