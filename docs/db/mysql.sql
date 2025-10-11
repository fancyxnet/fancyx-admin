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

 Date: 10/10/2025 22:14:15
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
INSERT INTO `api_access_log` VALUES (4497201445628022784, 1, '2025-10-10 21:52:59.483278', '/api/menu/list', 'GET', '::1', '2025-10-10 21:52:58.447714', NULL, NULL, 1, 'admin', '{\"dto\":{\"Title\":null,\"Path\":null}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', 'fa173a3d69f363d833d7f8e7d02e2f81', '[2]', NULL, NULL);
INSERT INTO `api_access_log` VALUES (4497201446169088000, 1, '2025-10-10 21:52:59.747542', '/api/menu/list', 'GET', '::1', '2025-10-10 21:52:58.447719', NULL, NULL, 1, 'admin', '{\"dto\":{\"Title\":null,\"Path\":null}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', 'aa4c8cdd197bb0c5c953f37908983579', '[2]', NULL, NULL);
INSERT INTO `api_access_log` VALUES (4497201450602467328, 1, '2025-10-10 21:53:00.803968', '/api/menu/list', 'GET', '::1', '2025-10-10 21:52:59.588571', NULL, NULL, 1, 'admin', '{\"dto\":{\"Title\":null,\"Path\":null}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '2649227b734884dec299785a893ebab0', '[2]', NULL, NULL);
INSERT INTO `api_access_log` VALUES (4497201574045028352, 631737765623021571, '2025-10-10 21:53:30.236928', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:29.466124', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', 'cd596bb20365c756e412d86c6e55561c', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201574179246080, 631737765623021571, '2025-10-10 21:53:30.269259', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:29.537492', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '82c8ad33777297b2fa11c729e57223ee', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201574435098624, 631737765623021571, '2025-10-10 21:53:30.329834', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:29.558783', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', 'f8839f64f1457cea143895135b0cf5b1', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201574573510656, 631737765623021571, '2025-10-10 21:53:30.362514', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:29.583176', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '027042aa75fd25a616fa4c06e9eeb5e1', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201583184416768, 631737765623021571, '2025-10-10 21:53:32.415393', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:31.472974', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', 'f03bb59bdfcefc8ca9dfc0874efe96d9', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201583410909184, 631737765623021571, '2025-10-10 21:53:32.470060', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:31.498294', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '7cab5e500dd5fadc5cacabfafdb14d8f', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201583775813632, 631737765623021571, '2025-10-10 21:53:32.556706', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:31.521938', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '613942ea5d0fcaa39f85384152967984', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201583951974400, 631737765623021571, '2025-10-10 21:53:32.598468', '/api/user/list', 'GET', '::1', '2025-10-10 21:53:31.543356', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"UserName\":null,\"DeptId\":null,\"PageSize\":10,\"Current\":1,\"SortProperty\":null,\"IsAsecending\":false}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '08efc6230bc05ee249c094a7c2d9a39c', NULL, '用户分页列表', NULL);
INSERT INTO `api_access_log` VALUES (4497201588297273344, 631737765623021571, '2025-10-10 21:53:33.634446', '/api/menu/list', 'GET', '::1', '2025-10-10 21:53:32.436728', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"Title\":null,\"Path\":null}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', '92c0a1c330aa831ef9d161db2cd02cef', '[2]', NULL, NULL);
INSERT INTO `api_access_log` VALUES (4497201588435685376, 631737765623021571, '2025-10-10 21:53:33.667170', '/api/menu/list', 'GET', '::1', '2025-10-10 21:53:32.464140', NULL, NULL, 631737765623021571, 'admin', '{\"dto\":{\"Title\":null,\"Path\":null}}', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '?current=1&pageSize=10', 'ed133f2f827e51ae694b74e90eb3bdb2', '[2]', NULL, NULL);

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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
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
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
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
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of login_log
-- ----------------------------
INSERT INTO `login_log` VALUES (4497201522736107520, 'admin', '::1', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36', '登录成功', 1, '4497201521070968832', NULL, 0, '2025-10-10 21:53:18.004749');

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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `is_external` tinyint(1) NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES (4491762374256627712, '系统管理', 'antd:SettingOutlined', '/system', NULL, 1, 'System', NULL, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627713, '组织架构', 'antd:TeamOutlined', '/org', NULL, 1, 'Org', NULL, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627714, '在线文档', 'antd:ApiOutlined', 'https://doc.crackerwork.cn/', '#', 2, '', NULL, 99, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627715, '快速开发', 'antd:ToolOutlined', '/quickWork', NULL, 1, '', NULL, 98, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627717, '重置密码', NULL, NULL, NULL, 3, 'Sys.User.ResetPwd', 4491762374256627763, 9, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627718, '分配功能权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignMenu', 4491762374256627751, 5, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627719, '编辑', NULL, NULL, NULL, 3, 'Sys.User.Update', 4491762374256627763, 10, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627720, '新增', NULL, NULL, NULL, 3, 'Sys.Config.Add', 4491762374256627750, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627721, '访问日志', NULL, '/monitor/apiAccessLog', 'monitor/apiAccessLog', 2, '', 4491762374256627754, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627722, '新增', NULL, NULL, NULL, 3, 'Sys.User.Add', 4491762374256627763, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627723, '查询', NULL, NULL, NULL, 3, 'Sys.User.List', 4491762374256627763, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627724, '分配角色', NULL, NULL, NULL, 3, 'Sys.User.AssignRole', 4491762374256627763, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627725, '启用/禁用', NULL, NULL, NULL, 3, 'Sys.User.SwitchEnabledStatus', 4491762374256627763, 5, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627726, '编辑', NULL, NULL, NULL, 3, 'Sys.Role.Update', 4491762374256627751, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627727, '新增', NULL, NULL, NULL, 3, 'Org.PositionGroup.Add', 4491762374256627771, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627728, '查询', NULL, NULL, NULL, 3, 'Org.PositionGroup.List', 4491762374256627771, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627729, '编辑', NULL, NULL, NULL, 3, 'Org.PositionGroup.Update', 4491762374256627771, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627730, '删除', NULL, NULL, NULL, 3, 'Org.PositionGroup.Delete', 4491762374256627771, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627731, '新增', NULL, NULL, NULL, 3, 'Org.Position.Add', 4491762374256627760, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627732, '编辑', NULL, NULL, NULL, 3, 'Org.Position.Update', 4491762374256627760, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627733, '删除', NULL, NULL, NULL, 3, 'Org.Position.Delete', 4491762374256627760, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627734, '新增', NULL, NULL, NULL, 3, 'Org.Dept.Add', 4491762374256627761, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627735, '查询', NULL, NULL, NULL, 3, 'Org.Dept.List', 4491762374256627761, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627736, '编辑', NULL, NULL, NULL, 3, 'Org.Dept.Update', 4491762374256627761, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627737, '删除', NULL, NULL, NULL, 3, 'Org.Dept.Delete', 4491762374256627761, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627738, '注销', NULL, NULL, NULL, 3, 'Monitor.Logout', 4491762374256627765, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627739, '新增', NULL, NULL, NULL, 3, 'Sys.DictType.Add', 4491762374256627762, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627740, '富文本组件', NULL, '/quickWork/rickText', 'quickWork/rickText', 2, '', 4491762374256627715, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627744, '编辑', NULL, NULL, NULL, 3, 'Sys.DictData.Update', 4491762374256627789, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627745, '删除', NULL, NULL, NULL, 3, 'Sys.DictData.Delete', 4491762374256627789, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627746, '编辑', NULL, NULL, NULL, 3, 'Sys.Menu.Update', 4491762374256627752, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627747, '删除', NULL, NULL, NULL, 3, 'Sys.Menu.Delete', 4491762374256627752, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627748, '查询', NULL, NULL, NULL, 3, 'Sys.Role.List', 4491762374256627751, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627749, '删除', NULL, NULL, NULL, 3, 'Sys.Role.Delete', 4491762374256627751, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627750, '配置管理', NULL, '/system/config', 'system/config', 2, '', 4491762374256627712, 7, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627751, '角色管理', NULL, '/system/role', 'system/role', 2, 'Sys:Role', 4491762374256627712, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627752, '菜单管理', NULL, '/system/menu', 'system/menu', 2, 'Sys:Menu', 4491762374256627712, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627753, '分配数据权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignDataScope', 4491762374256627751, 6, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627754, '系统监控', 'antd:FundOutlined', '/monitor', NULL, 1, '', NULL, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627755, '通知管理', '', '/org/notification', 'org/notification', 2, '', 4491762374256627713, 5, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627756, '删除', NULL, NULL, NULL, 3, 'Sys.User.Delete', 4491762374256627763, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627757, '查询', NULL, NULL, NULL, 3, 'Org.Position.List', 4491762374256627760, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627758, '查询', NULL, NULL, NULL, 3, 'Sys.DictType.List', 4491762374256627762, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627759, '新增', NULL, NULL, NULL, 3, 'Sys.Role.Add', 4491762374256627751, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627760, '职位管理', NULL, '/org/position', 'org/position', 2, 'Org:Position', 4491762374256627713, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627761, '部门管理', NULL, '/org/dept', 'org/dept', 2, 'Org:Department', 4491762374256627713, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627762, '数据字典', NULL, '/system/dict', 'system/dictType', 2, 'Sys:Dict', 4491762374256627712, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627763, '用户管理', '', '/system/user', 'system/user', 2, '', 4491762374256627712, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627764, '异常日志', NULL, '/monitor/exceptionLog', 'monitor/exceptionLog', 2, '', 4491762374256627754, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627765, '在线用户', NULL, '/monitor/onlineUser', 'monitor/onlineUser', 2, '', 4491762374256627754, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627770, '租户管理', NULL, '/system/tenant', 'system/tenant', 2, '', 4491762374256627712, 8, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627771, '职位分组', '', '/org/positionGroup', 'org/positionGroup', 2, '', 4491762374256627713, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627772, '新增', NULL, NULL, NULL, 3, 'Sys.Notification.Add', 4491762374256627755, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627773, '查询', NULL, NULL, NULL, 3, 'Sys.Notification.List', 4491762374256627755, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627774, '编辑', NULL, NULL, NULL, 3, 'Sys.Notification.Update', 4491762374256627755, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627775, '删除', NULL, NULL, NULL, 3, 'Sys.Notification.Delete', 4491762374256627755, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627776, '我的通知', NULL, '/org/myNotification', 'org/myNotification', 2, '', 4491762374256627713, 6, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627777, '登录日志', NULL, '/system/log/login', 'system/log/loginLog', 2, '', 4491762374256627712, 10, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627778, '业务日志', NULL, '/system/log/business', 'system/log/businessLog', 2, '', 4491762374256627712, 11, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627780, '删除', NULL, '', NULL, 3, 'Sys.DictType.Delete', 4491762374256627762, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627781, '新增', NULL, NULL, NULL, 3, 'Sys.DictData.Add', 4491762374256627789, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627782, '查询', NULL, NULL, NULL, 3, 'Sys.DictData.List', 4491762374256627789, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627783, '新增', NULL, NULL, NULL, 3, 'Sys.Menu.Add', 4491762374256627752, 1, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627784, '查询', NULL, NULL, NULL, 3, 'Sys.Menu.List', 4491762374256627752, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627785, '部门简单信息', NULL, NULL, NULL, 3, 'Org.Dept.GetDeptSimpleInfos', 4491762374256627761, 5, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627786, '查询', NULL, NULL, NULL, 3, 'Sys.Config.List', 4491762374256627750, 2, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627787, '编辑', NULL, NULL, NULL, 3, 'Sys.Config.Update', 4491762374256627750, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627788, '删除', NULL, NULL, NULL, 3, 'Sys.Config.Delete', 4491762374256627750, 4, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627789, '字典项', NULL, '/system/dictItem/:dictType', 'system/dictData', 2, NULL, 4491762374256627712, 5, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627791, '编辑', NULL, NULL, NULL, 3, 'Sys.DictType.Update', 4491762374256627762, 3, 1, NULL, 0, NULL, '2025-10-10 21:51:44.000000', NULL, NULL);

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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of position_group
-- ----------------------------

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role`  (
  `id` bigint NOT NULL,
  `role_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `dept_power_type` int NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (631737765623021569, '系统管理员', '系统默认创建，拥有最高权限', NULL, 1, 0, NULL, '2025-09-14 11:04:21.533056', NULL, NULL, 0, NULL, NULL);

-- ----------------------------
-- Table structure for role_dept
-- ----------------------------
DROP TABLE IF EXISTS `role_dept`;
CREATE TABLE `role_dept`  (
  `id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `dept_id` bigint NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`id`) USING BTREE,
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
  `id` bigint NOT NULL,
  `menu_id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `IX_role_menu_menu_id`(`menu_id` ASC) USING BTREE,
  INDEX `IX_role_menu_role_id`(`role_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of role_menu
-- ----------------------------

-- ----------------------------
-- Table structure for tenant
-- ----------------------------
DROP TABLE IF EXISTS `tenant`;
CREATE TABLE `tenant`  (
  `id` bigint NOT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `IX_tenant_tenant_id`(`tenant_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of tenant
-- ----------------------------

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
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
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
INSERT INTO `user` VALUES (631737765623021571, 'admin', 'a2fa8ec90f15197c7a4e6e00525b198a', 'vHQZvbz+ng+B4NrSAEYl6g==', 'file/myavatar.jpg', '风汐', 2, 1, NULL, '18211114444', NULL, NULL, NULL, '2025-09-10 21:13:06.186342', NULL, NULL, 0, NULL, NULL);

-- ----------------------------
-- Table structure for user_role
-- ----------------------------
DROP TABLE IF EXISTS `user_role`;
CREATE TABLE `user_role`  (
  `id` bigint NOT NULL,
  `user_id` bigint NOT NULL,
  `role_id` bigint NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `IX_user_role_role_id`(`role_id` ASC) USING BTREE,
  INDEX `IX_user_role_user_id`(`user_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user_role
-- ----------------------------
INSERT INTO `user_role` VALUES (631737765623021570, 631737765623021571, 631737765623021569, NULL);

SET FOREIGN_KEY_CHECKS = 1;
