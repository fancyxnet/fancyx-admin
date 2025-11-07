/*
 Navicat Premium Dump SQL

 Source Server         : 本地mysql
 Source Server Type    : MySQL
 Source Server Version : 80406 (8.4.6)
 Source Host           : localhost:3306
 Source Schema         : fancyx-admin

 Target Server Type    : MySQL
 Target Server Version : 80406 (8.4.6)
 File Encoding         : 65001

 Date: 07/11/2025 17:14:01
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
  `path` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '请求路径',
  `method` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'HTTP方法 (GET, POST, PUT等)',
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'IP',
  `request_time` datetime(6) NOT NULL COMMENT '请求时间',
  `response_time` datetime(6) NULL DEFAULT NULL COMMENT '响应时间',
  `duration` bigint NULL DEFAULT NULL COMMENT '耗时(毫秒)',
  `user_id` bigint NULL DEFAULT NULL COMMENT '用户ID (可为空，未登录用户)',
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '用户名',
  `request_body` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '请求体',
  `response_body` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '响应体',
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '浏览器',
  `query_string` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '请求参数',
  `trace_id` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '跟踪ID (用于关联一次请求的所有日志)',
  `operate_type` json NULL COMMENT '操作类型',
  `operate_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '操作名称',
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = 'API访问日志' ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

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
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of cap.received
-- ----------------------------

-- ----------------------------
-- Table structure for config
-- ----------------------------
DROP TABLE IF EXISTS `config`;
CREATE TABLE `config`  (
  `id` bigint NOT NULL,
  `name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '配置名称',
  `key` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '配置键名',
  `value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '配置键值',
  `group_key` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '组别',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `key`(`key` ASC, `tenant_id` ASC) USING BTREE COMMENT 'key唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '系统配置' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of config
-- ----------------------------

-- ----------------------------
-- Table structure for customer
-- ----------------------------
DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编号',
  `code_slim` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '简码',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `contact_name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系人',
  `contact_phone` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系电话',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '客户信息' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of customer
-- ----------------------------

-- ----------------------------
-- Table structure for dept
-- ----------------------------
DROP TABLE IF EXISTS `dept`;
CREATE TABLE `dept`  (
  `id` bigint NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门编号',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门名称',
  `sort` int NOT NULL COMMENT '排序',
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '描述',
  `status` int NOT NULL COMMENT '状态：1正常2停用',
  `curator_id` bigint NULL DEFAULT NULL COMMENT '负责人',
  `email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '邮箱',
  `phone` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '电话',
  `parent_id` bigint NULL DEFAULT NULL COMMENT '父ID',
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '树形路径',
  `tree_level` int NOT NULL COMMENT '树形层级',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '部门表' ROW_FORMAT = DYNAMIC;

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
  `value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '字典值',
  `label` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '显示文本',
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '字典类型',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `sort` int NOT NULL COMMENT '排序值',
  `is_enabled` tinyint(1) NOT NULL COMMENT '是否开启',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `value`(`value` ASC, `tenant_id` ASC) USING BTREE COMMENT '字典项值唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '字典数据表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of dict_data
-- ----------------------------

-- ----------------------------
-- Table structure for dict_type
-- ----------------------------
DROP TABLE IF EXISTS `dict_type`;
CREATE TABLE `dict_type`  (
  `id` bigint NOT NULL,
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '字典名称',
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '字典类型',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL COMMENT '是否开启',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `dict_type`(`dict_type` ASC, `tenant_id` ASC) USING BTREE COMMENT '字典类型唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '字典类型表' ROW_FORMAT = DYNAMIC;

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
  `exception_type` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '异常类型',
  `message` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '异常消息',
  `stack_trace` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '异常堆栈',
  `inner_exception` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '内部异常信息',
  `request_path` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '请求路径 (如果是Web请求)',
  `request_method` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '请求方法 (GET, POST等)',
  `user_id` bigint NULL DEFAULT NULL COMMENT '用户ID',
  `user_name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '用户名',
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'IP',
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '浏览器',
  `trace_id` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '跟踪ID (用于关联一次请求的所有日志)',
  `is_handled` bit(1) NOT NULL COMMENT '是否已处理',
  `handled_time` datetime(6) NULL DEFAULT NULL COMMENT '处理时间',
  `handled_by` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '处理人',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '异常日志' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of exception_log
-- ----------------------------

-- ----------------------------
-- Table structure for gen_table
-- ----------------------------
DROP TABLE IF EXISTS `gen_table`;
CREATE TABLE `gen_table`  (
  `table_id` bigint NOT NULL COMMENT '编号',
  `table_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT '' COMMENT '表名称',
  `table_comment` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT '' COMMENT '表描述',
  `sub_table_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '关联子表的表名',
  `sub_table_fk_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '子表关联的外键名',
  `class_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT '' COMMENT '实体类名称',
  `tpl_category` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'crud' COMMENT '使用的模板（crud单表操作 tree树表操作）',
  `namespace_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '生成命名空间路径',
  `module_name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '生成模块名',
  `business_name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '生成业务名',
  `function_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '生成功能名',
  `gen_type` char(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT '0' COMMENT '生成代码方式（0zip压缩包 1自定义路径）',
  `gen_path` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT '/' COMMENT '生成路径',
  `options` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '其它生成选项',
  `remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`table_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '代码生成业务表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of gen_table
-- ----------------------------

-- ----------------------------
-- Table structure for gen_table_column
-- ----------------------------
DROP TABLE IF EXISTS `gen_table_column`;
CREATE TABLE `gen_table_column`  (
  `column_id` bigint NOT NULL COMMENT '编号',
  `table_id` bigint NULL DEFAULT NULL COMMENT '归属表编号',
  `column_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '列名称',
  `column_comment` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '列描述',
  `column_type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '列类型',
  `csharp_type` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'CSharp类型',
  `csharp_field` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'CSharp字段名',
  `is_pk` bit(1) NULL DEFAULT NULL COMMENT '是否主键（1是）',
  `is_increment` bit(1) NULL DEFAULT NULL COMMENT '是否自增（1是）',
  `is_required` bit(1) NULL DEFAULT NULL COMMENT '是否必填（1是）',
  `is_insert` bit(1) NULL DEFAULT NULL COMMENT '是否为插入字段（1是）',
  `is_edit` bit(1) NULL DEFAULT NULL COMMENT '是否编辑字段（1是）',
  `is_list` bit(1) NULL DEFAULT NULL COMMENT '是否列表字段（1是）',
  `is_query` bit(1) NULL DEFAULT NULL COMMENT '是否查询字段（1是）',
  `query_type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'EQ' COMMENT '查询方式（等于、不等于、大于、小于、范围）',
  `html_type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '显示类型（文本框、文本域、下拉框、复选框、单选框、日期控件）',
  `dict_type` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT '' COMMENT '字典类型',
  `sort` int NULL DEFAULT NULL COMMENT '排序',
  PRIMARY KEY (`column_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '代码生成业务表字段' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of gen_table_column
-- ----------------------------

-- ----------------------------
-- Table structure for inventory
-- ----------------------------
DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory`  (
  `id` bigint NOT NULL,
  `inventory_no` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '库存编号',
  `product_id` bigint NOT NULL COMMENT '产品ID',
  `quantity` int NOT NULL COMMENT '数量',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `inventory_no`(`inventory_no` ASC, `tenant_id` ASC) USING BTREE COMMENT '库存编号唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '库存' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inventory
-- ----------------------------

-- ----------------------------
-- Table structure for inventory_log
-- ----------------------------
DROP TABLE IF EXISTS `inventory_log`;
CREATE TABLE `inventory_log`  (
  `id` bigint NOT NULL,
  `biz_type` int NOT NULL COMMENT '业务类型',
  `inventory_id` bigint NOT NULL COMMENT '库存ID',
  `inventory_no` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '库存编号',
  `source` int NOT NULL COMMENT '来源',
  `source_no` int NOT NULL COMMENT '来源单号',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `change_quantity` int NOT NULL COMMENT '改变数量',
  `after_quantity` int NOT NULL COMMENT '改变后数量',
  `cost_price` decimal(10, 2) NULL DEFAULT NULL COMMENT '单价',
  `total_cost` decimal(10, 2) NULL DEFAULT NULL COMMENT '总价',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '库存日志' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inventory_log
-- ----------------------------

-- ----------------------------
-- Table structure for log_record
-- ----------------------------
DROP TABLE IF EXISTS `log_record`;
CREATE TABLE `log_record`  (
  `id` bigint NOT NULL,
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `type` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '日志类型',
  `sub_type` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '日志子类型',
  `biz_no` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '业务编号/ID',
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '操作内容',
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '浏览器',
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'IP',
  `trace_id` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '跟踪ID (用于关联一次请求的所有日志)',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `user_id` bigint NULL DEFAULT NULL COMMENT '用户ID',
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '业务日志' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of log_record
-- ----------------------------

-- ----------------------------
-- Table structure for login_log
-- ----------------------------
DROP TABLE IF EXISTS `login_log`;
CREATE TABLE `login_log`  (
  `id` bigint NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '账号',
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'IP',
  `address` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '登录地址',
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '浏览器',
  `operation_msg` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '操作信息',
  `is_success` tinyint(1) NOT NULL COMMENT '是否成功',
  `session_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '会话ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '登录日志' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of login_log
-- ----------------------------

-- ----------------------------
-- Table structure for menu
-- ----------------------------
DROP TABLE IF EXISTS `menu`;
CREATE TABLE `menu`  (
  `id` bigint NOT NULL,
  `title` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '显示标题/名称',
  `icon` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '图标',
  `path` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '路由/地址',
  `component` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '组件地址',
  `menu_type` int NOT NULL COMMENT '功能类型',
  `permission` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '授权码',
  `parent_id` bigint NULL DEFAULT NULL COMMENT '父级ID',
  `sort` int NOT NULL COMMENT '排序',
  `display` bit(1) NOT NULL DEFAULT b'1' COMMENT '是否隐藏',
  `is_external` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否外链',
  `keep_alive` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否保活',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `path`(`path` ASC) USING BTREE COMMENT '菜单路由唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '菜单表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES (7694476983298048, '使用帮助', 'antd:InfoCircleOutlined', '/help', NULL, 1, NULL, NULL, 9, b'1', b'0', b'0', NULL, '2025-11-06 14:04:16.000000', '2025-11-07 14:35:05.370783', 631737765623021571);
INSERT INTO `menu` VALUES (778412336363999232, '发起工单', NULL, NULL, NULL, 2, NULL, 7694476983298048, 1, b'1', b'0', b'0', NULL, '2025-11-06 14:07:27.000000', NULL, NULL);
INSERT INTO `menu` VALUES (778412336363999233, '关闭', NULL, NULL, NULL, 3, 'Feedback.Ticket.Close', 778412336363999232, 2, b'1', b'0', b'0', NULL, '2025-11-06 14:17:39.000000', NULL, NULL);
INSERT INTO `menu` VALUES (778412336363999234, '发起', NULL, NULL, NULL, 3, 'Feedback.Ticket.UserCreate', 778412336363999232, 1, b'1', b'0', b'0', NULL, '2025-11-06 14:17:39.000000', NULL, NULL);
INSERT INTO `menu` VALUES (778412336363999235, '评价', NULL, NULL, NULL, 3, 'Feedback.Ticket.Evaluation', 778412336363999232, 3, b'1', b'0', b'0', NULL, '2025-11-06 14:17:39.000000', NULL, NULL);
INSERT INTO `menu` VALUES (778412336363999236, '我的工单', NULL, NULL, NULL, 3, 'Feedback.Ticket.ListForUser', 778412336363999232, 3, b'1', b'0', b'0', NULL, '2025-11-06 14:17:39.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627712, '系统管理', 'antd:SettingOutlined', '/system', NULL, 1, 'System', NULL, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627713, '组织架构', 'antd:TeamOutlined', '/org', NULL, 1, 'Org', NULL, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627714, '在线文档', 'antd:ApiOutlined', 'https://doc.crackerwork.cn/', '#', 2, NULL, NULL, 99, b'1', b'1', b'0', NULL, '2025-10-10 21:51:44.000000', '2025-10-26 10:57:05.055614', 631737765623021571);
INSERT INTO `menu` VALUES (4491762374256627715, '快速开发', 'antd:ToolOutlined', '/quickWork', NULL, 1, '', NULL, 98, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627717, '重置密码', NULL, NULL, NULL, 3, 'Sys.User.ResetPwd', 4491762374256627763, 9, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627718, '分配功能权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignMenu', 4491762374256627751, 5, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627719, '编辑', NULL, NULL, NULL, 3, 'Sys.User.Update', 4491762374256627763, 10, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627720, '新增', NULL, NULL, NULL, 3, 'Sys.Config.Add', 4491762374256627750, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627721, '访问日志', NULL, '/monitor/apiAccessLog', 'monitor/apiAccessLog', 2, '', 4491762374256627754, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627722, '新增', NULL, NULL, NULL, 3, 'Sys.User.Add', 4491762374256627763, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627723, '查询', NULL, NULL, NULL, 3, 'Sys.User.List', 4491762374256627763, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627724, '分配角色', NULL, NULL, NULL, 3, 'Sys.User.AssignRole', 4491762374256627763, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627725, '启用/禁用', NULL, NULL, NULL, 3, 'Sys.User.SwitchEnabledStatus', 4491762374256627763, 5, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627726, '编辑', NULL, NULL, NULL, 3, 'Sys.Role.Update', 4491762374256627751, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627727, '新增', NULL, NULL, NULL, 3, 'Org.PositionGroup.Add', 4491762374256627771, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627728, '查询', NULL, NULL, NULL, 3, 'Org.PositionGroup.List', 4491762374256627771, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627729, '编辑', NULL, NULL, NULL, 3, 'Org.PositionGroup.Update', 4491762374256627771, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627730, '删除', NULL, NULL, NULL, 3, 'Org.PositionGroup.Delete', 4491762374256627771, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627731, '新增', NULL, NULL, NULL, 3, 'Org.Position.Add', 4491762374256627760, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627732, '编辑', NULL, NULL, NULL, 3, 'Org.Position.Update', 4491762374256627760, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627733, '删除', NULL, NULL, NULL, 3, 'Org.Position.Delete', 4491762374256627760, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627734, '新增', NULL, NULL, NULL, 3, 'Org.Dept.Add', 4491762374256627761, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627735, '查询', NULL, NULL, NULL, 3, 'Org.Dept.List', 4491762374256627761, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627736, '编辑', NULL, NULL, NULL, 3, 'Org.Dept.Update', 4491762374256627761, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627737, '删除', NULL, NULL, NULL, 3, 'Org.Dept.Delete', 4491762374256627761, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627738, '注销', NULL, NULL, NULL, 3, 'Monitor.Logout', 4491762374256627765, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627739, '新增', NULL, NULL, NULL, 3, 'Sys.DictType.Add', 4491762374256627762, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627740, '富文本组件', NULL, '/quickWork/richText', 'quickWork/richText', 2, NULL, 4491762374256627715, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', '2025-11-04 15:09:04.370687', 631737765623021571);
INSERT INTO `menu` VALUES (4491762374256627744, '编辑', NULL, NULL, NULL, 3, 'Sys.DictData.Update', 4491762374256627789, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627745, '删除', NULL, NULL, NULL, 3, 'Sys.DictData.Delete', 4491762374256627789, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627746, '编辑', NULL, NULL, NULL, 3, 'Sys.Menu.Update', 4491762374256627752, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627747, '删除', NULL, NULL, NULL, 3, 'Sys.Menu.Delete', 4491762374256627752, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627748, '查询', NULL, NULL, NULL, 3, 'Sys.Role.List', 4491762374256627751, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627749, '删除', NULL, NULL, NULL, 3, 'Sys.Role.Delete', 4491762374256627751, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627750, '配置管理', NULL, '/system/config', 'system/config', 2, '', 4491762374256627712, 7, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627751, '角色管理', NULL, '/system/role', 'system/role', 2, 'Sys:Role', 4491762374256627712, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627752, '菜单管理', NULL, '/system/menu', 'system/menu', 2, 'Sys:Menu', 4491762374256627712, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627753, '分配数据权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignDataScope', 4491762374256627751, 6, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627754, '系统监控', 'antd:FundOutlined', '/monitor', NULL, 1, '', NULL, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627755, '通知管理', '', '/org/notification', 'org/notification', 2, '', 4491762374256627713, 5, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627756, '删除', NULL, NULL, NULL, 3, 'Sys.User.Delete', 4491762374256627763, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627757, '查询', NULL, NULL, NULL, 3, 'Org.Position.List', 4491762374256627760, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627758, '查询', NULL, NULL, NULL, 3, 'Sys.DictType.List', 4491762374256627762, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627759, '新增', NULL, NULL, NULL, 3, 'Sys.Role.Add', 4491762374256627751, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627760, '职位管理', NULL, '/org/position', 'org/position', 2, 'Org:Position', 4491762374256627713, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627761, '部门管理', NULL, '/org/dept', 'org/dept', 2, 'Org:Department', 4491762374256627713, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627762, '数据字典', NULL, '/system/dict', 'system/dictType', 2, 'Sys:Dict', 4491762374256627712, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627763, '用户管理', '', '/system/user', 'system/user', 2, '', 4491762374256627712, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627764, '异常日志', NULL, '/monitor/exceptionLog', 'monitor/exceptionLog', 2, '', 4491762374256627754, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627765, '在线用户', NULL, '/monitor/onlineUser', 'monitor/onlineUser', 2, '', 4491762374256627754, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627770, '租户管理', NULL, '/system/tenant', 'system/tenant', 2, '', 4491762374256627712, 8, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627771, '职位分组', '', '/org/positionGroup', 'org/positionGroup', 2, NULL, 4491762374256627713, 1, b'1', b'0', b'1', NULL, '2025-10-10 21:51:44.000000', '2025-11-02 20:53:47.382446', 631737765623021571);
INSERT INTO `menu` VALUES (4491762374256627772, '新增', NULL, NULL, NULL, 3, 'Sys.Notification.Add', 4491762374256627755, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627773, '查询', NULL, NULL, NULL, 3, 'Sys.Notification.List', 4491762374256627755, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627774, '编辑', NULL, NULL, NULL, 3, 'Sys.Notification.Update', 4491762374256627755, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627775, '删除', NULL, NULL, NULL, 3, 'Sys.Notification.Delete', 4491762374256627755, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627776, '我的通知', NULL, '/org/myNotification', 'org/myNotification', 2, '', 4491762374256627713, 6, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627777, '登录日志', NULL, '/system/log/login', 'system/log/loginLog', 2, '', 4491762374256627712, 10, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627778, '业务日志', NULL, '/system/log/business', 'system/log/businessLog', 2, '', 4491762374256627712, 11, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627780, '删除', NULL, '', NULL, 3, 'Sys.DictType.Delete', 4491762374256627762, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627781, '新增', NULL, NULL, NULL, 3, 'Sys.DictData.Add', 4491762374256627789, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627782, '查询', NULL, NULL, NULL, 3, 'Sys.DictData.List', 4491762374256627789, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627783, '新增', NULL, NULL, NULL, 3, 'Sys.Menu.Add', 4491762374256627752, 1, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627784, '查询', NULL, NULL, NULL, 3, 'Sys.Menu.List', 4491762374256627752, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627785, '部门简单信息', NULL, NULL, NULL, 3, 'Org.Dept.GetDeptSimpleInfos', 4491762374256627761, 5, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627786, '查询', NULL, NULL, NULL, 3, 'Sys.Config.List', 4491762374256627750, 2, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627787, '编辑', NULL, NULL, NULL, 3, 'Sys.Config.Update', 4491762374256627750, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627788, '删除', NULL, NULL, NULL, 3, 'Sys.Config.Delete', 4491762374256627750, 4, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4491762374256627789, '字典项', NULL, '/system/dictItem/:dictType', 'system/dictData', 2, NULL, 4491762374256627712, 5, b'0', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', '2025-10-26 10:56:54.927941', 631737765623021571);
INSERT INTO `menu` VALUES (4491762374256627791, '编辑', NULL, NULL, NULL, 3, 'Sys.DictType.Update', 4491762374256627762, 3, b'1', b'0', b'0', NULL, '2025-10-10 21:51:44.000000', NULL, NULL);
INSERT INTO `menu` VALUES (4504417542584406016, '分配功能', NULL, NULL, NULL, 3, 'Sys.Tenant.AssignTenantMenu', 4491762374256627770, 5, b'1', b'0', b'0', 631737765623021571, '2025-10-30 19:47:11.203231', NULL, NULL);
INSERT INTO `menu` VALUES (4504424227461926912, '新增', NULL, NULL, NULL, 3, 'Sys.Tenant.Add', 4491762374256627770, 1, b'1', b'0', b'0', 631737765623021571, '2025-10-30 20:13:45.002466', NULL, NULL);
INSERT INTO `menu` VALUES (4504424322014121984, '列表', NULL, NULL, NULL, 3, 'Sys.Tenant.List', 4491762374256627770, 2, b'1', b'0', b'0', 631737765623021571, '2025-10-30 20:14:07.545010', NULL, NULL);
INSERT INTO `menu` VALUES (4504424382105915392, '编辑', NULL, NULL, NULL, 3, 'Sys.Tenant.Update', 4491762374256627770, 3, b'1', b'0', b'0', 631737765623021571, '2025-10-30 20:14:21.872420', NULL, NULL);
INSERT INTO `menu` VALUES (4504424443569246208, '删除', NULL, NULL, NULL, 3, 'Sys.Tenant.Delete', 4491762374256627770, 4, b'1', b'0', b'0', 631737765623021571, '2025-10-30 20:14:36.526863', NULL, NULL);
INSERT INTO `menu` VALUES (4505453783849373696, '查询', NULL, NULL, NULL, 3, 'Monitor.OnlineUser', 4491762374256627765, 1, b'1', b'0', b'0', 631737765623021571, '2025-11-02 16:24:50.373224', NULL, NULL);
INSERT INTO `menu` VALUES (4505453882075779072, '查询', NULL, NULL, NULL, 3, 'Monitor.ExceptionLogList', 4491762374256627764, 1, b'1', b'0', b'0', 631737765623021571, '2025-11-02 16:25:13.792930', NULL, NULL);
INSERT INTO `menu` VALUES (4505453985779945472, '处理异常', NULL, NULL, NULL, 3, 'Monitor.ExceptionLog.HandleException', 4491762374256627764, 2, b'1', b'0', b'0', 631737765623021571, '2025-11-02 16:25:38.517629', NULL, NULL);
INSERT INTO `menu` VALUES (4505454091476406272, '查询', NULL, NULL, NULL, 3, 'Monitor.ApiAccessLogList', 4491762374256627721, 1, b'1', b'0', b'0', 631737765623021571, '2025-11-02 16:26:03.717594', NULL, NULL);
INSERT INTO `menu` VALUES (4507238275102543872, '代码生成', NULL, '/quickWork/gen', 'quickWork/gen', 2, NULL, 4491762374256627715, 2, b'1', b'0', b'1', 631737765623021571, '2025-11-07 14:35:46.242069', '2025-11-07 14:36:46.745280', 631737765623021571);
INSERT INTO `menu` VALUES (4507240594183557120, '预览', NULL, NULL, NULL, 3, 'Sys.Gen.GenCode', 4507238275102543872, 1, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:44:59.154386', NULL, NULL);
INSERT INTO `menu` VALUES (4507241708169728000, '导入', NULL, NULL, NULL, 3, 'Sys.Gen.ImportTable', 4507238275102543872, 2, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:49:24.749293', '2025-11-07 14:57:02.578151', 631737765623021571);
INSERT INTO `menu` VALUES (4507241809709633536, '数据库表查询', NULL, NULL, NULL, 3, 'Sys.Gen.GetTableList', 4507238275102543872, 3, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:49:48.958132', '2025-11-07 14:57:11.802979', 631737765623021571);
INSERT INTO `menu` VALUES (4507241880912138240, '同步', NULL, NULL, NULL, 3, 'Sys.Gen.GenSyncFromDb', 4507238275102543872, 4, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:50:05.934747', NULL, NULL);
INSERT INTO `menu` VALUES (4507241957160390656, '生成表查询', NULL, NULL, NULL, 3, 'Sys.Gen.GetGenTableList', 4507238275102543872, 5, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:50:24.113003', NULL, NULL);
INSERT INTO `menu` VALUES (4507242150706548736, '生成表列查询', NULL, NULL, NULL, 3, 'Sys.Gen.GetGenTableColumnList', 4507238275102543872, 6, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:51:10.258554', '2025-11-07 14:53:14.468479', 631737765623021571);
INSERT INTO `menu` VALUES (4507242216104136704, '删除', NULL, NULL, NULL, 3, 'Sys.Gen.DeleteGenTable', 4507238275102543872, 7, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:51:25.850045', NULL, NULL);
INSERT INTO `menu` VALUES (4507242290074882048, '保存生成表信息', NULL, NULL, NULL, 3, 'Sys.Gen.SaveGenTableInfo', 4507238275102543872, 8, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:51:43.486195', NULL, NULL);
INSERT INTO `menu` VALUES (4507242360648241152, '保存生成表列信息', NULL, NULL, NULL, 3, 'Sys.Gen.SaveGenColumnInfo', 4507238275102543872, 9, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:52:00.312666', NULL, NULL);
INSERT INTO `menu` VALUES (4507242531641626624, '生成表详细信息', NULL, NULL, NULL, 3, 'Sys.Gen.GetGenDetailsInfo', 4507238275102543872, 10, b'1', b'0', b'0', 631737765623021571, '2025-11-07 14:52:41.080676', NULL, NULL);

-- ----------------------------
-- Table structure for notification
-- ----------------------------
DROP TABLE IF EXISTS `notification`;
CREATE TABLE `notification`  (
  `id` bigint NOT NULL,
  `title` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '通知标题',
  `content` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '通知内容',
  `user_id` bigint NOT NULL COMMENT '通知用户',
  `is_readed` tinyint(1) NOT NULL COMMENT '是否已读(true已读false未读)',
  `readed_time` datetime(6) NULL DEFAULT NULL COMMENT '已读时间',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '站内通知' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of notification
-- ----------------------------

-- ----------------------------
-- Table structure for position
-- ----------------------------
DROP TABLE IF EXISTS `position`;
CREATE TABLE `position`  (
  `id` bigint NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '职位编号',
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '职位名称',
  `level` int NOT NULL COMMENT '职级',
  `status` int NOT NULL COMMENT '状态：1正常2停用',
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '描述',
  `group_id` bigint NULL DEFAULT NULL COMMENT '职位分组',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '职位表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of position
-- ----------------------------

-- ----------------------------
-- Table structure for position_group
-- ----------------------------
DROP TABLE IF EXISTS `position_group`;
CREATE TABLE `position_group`  (
  `id` bigint NOT NULL,
  `group_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '分组名',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `parent_id` bigint NULL DEFAULT NULL COMMENT '父ID',
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '树形路径',
  `tree_level` int NOT NULL COMMENT '树形层级',
  `sort` int NOT NULL COMMENT '排序值',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '职位分组' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of position_group
-- ----------------------------
INSERT INTO `position_group` VALUES (4505452973992185856, '前端分组', NULL, NULL, '4505452973992185856', 1, 0, 'platform', 631737765623021571, '2025-11-02 16:21:37.309496', NULL, NULL);
INSERT INTO `position_group` VALUES (4505472754346627072, '233232', '232323', NULL, '4505472754346627072', 1, 0, 'mi', 4505462198378172416, '2025-11-02 17:40:13.300295', NULL, NULL);

-- ----------------------------
-- Table structure for product
-- ----------------------------
DROP TABLE IF EXISTS `product`;
CREATE TABLE `product`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '产品编号',
  `sku_code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'SKU编号',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '产品名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `brand_id` bigint NOT NULL COMMENT '品牌ID',
  `category_id` bigint NOT NULL COMMENT '分类ID',
  `unit` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '单位,取字典',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一',
  INDEX `sku_code`(`sku_code` ASC, `tenant_id` ASC) USING BTREE COMMENT 'SKU编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '产品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product
-- ----------------------------

-- ----------------------------
-- Table structure for product_attr
-- ----------------------------
DROP TABLE IF EXISTS `product_attr`;
CREATE TABLE `product_attr`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编码',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `is_required` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否必填',
  `input_type` int NOT NULL COMMENT '多选/单选/手动录入',
  `attr_type` int NOT NULL COMMENT '指定分类/所有分类',
  `category_id` bigint NULL DEFAULT NULL COMMENT '分类ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '产品属性' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product_attr
-- ----------------------------

-- ----------------------------
-- Table structure for product_attr_value
-- ----------------------------
DROP TABLE IF EXISTS `product_attr_value`;
CREATE TABLE `product_attr_value`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编码',
  `value` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '值',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `attr_id` bigint NULL DEFAULT NULL COMMENT '属性ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '产品属性可选值' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product_attr_value
-- ----------------------------

-- ----------------------------
-- Table structure for product_bind_attr_value
-- ----------------------------
DROP TABLE IF EXISTS `product_bind_attr_value`;
CREATE TABLE `product_bind_attr_value`  (
  `id` bigint NOT NULL,
  `product_id` bigint NOT NULL COMMENT '产品ID',
  `attr_id` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '属性ID',
  `attr_value` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '属性值',
  `attr_value_id` bigint NULL DEFAULT NULL COMMENT '属性值ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '产品属性绑定值' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product_bind_attr_value
-- ----------------------------

-- ----------------------------
-- Table structure for product_brand
-- ----------------------------
DROP TABLE IF EXISTS `product_brand`;
CREATE TABLE `product_brand`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编码',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '产品品牌' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product_brand
-- ----------------------------

-- ----------------------------
-- Table structure for product_category
-- ----------------------------
DROP TABLE IF EXISTS `product_category`;
CREATE TABLE `product_category`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编码',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '产品分类' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product_category
-- ----------------------------

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role`  (
  `id` bigint NOT NULL,
  `role_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色名',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `is_enabled` bit(1) NOT NULL COMMENT '是否启用',
  `dept_power_type` int NOT NULL COMMENT '部门权限类型',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  `is_platform_role` int NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `role_name`(`role_name` ASC, `tenant_id` ASC) USING BTREE COMMENT '角色名唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '角色表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (631737765623021568, '管理员', '', 'mi', b'1', 0, NULL, '2025-09-14 11:04:21.533056', '2025-11-02 17:41:29.308653', 4505462198378172416, 0, NULL, NULL, 0);
INSERT INTO `role` VALUES (631737765623021569, '系统管理员', '', 'platform', b'1', 0, NULL, '2025-09-14 11:04:21.533056', '2025-11-02 16:22:39.520604', 631737765623021571, 0, NULL, NULL, 0);

-- ----------------------------
-- Table structure for role_dept
-- ----------------------------
DROP TABLE IF EXISTS `role_dept`;
CREATE TABLE `role_dept`  (
  `role_id` bigint NOT NULL COMMENT '角色ID',
  `dept_id` bigint NOT NULL COMMENT '部门ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  PRIMARY KEY (`role_id`, `dept_id`) USING BTREE,
  INDEX `IX_role_dept_role_id`(`role_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '角色部门关联' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of role_dept
-- ----------------------------

-- ----------------------------
-- Table structure for role_menu
-- ----------------------------
DROP TABLE IF EXISTS `role_menu`;
CREATE TABLE `role_menu`  (
  `menu_id` bigint NOT NULL COMMENT '菜单ID',
  `role_id` bigint NOT NULL COMMENT '角色ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  PRIMARY KEY (`menu_id`, `role_id`) USING BTREE,
  INDEX `IX_role_menu_menu_id`(`menu_id` ASC) USING BTREE,
  INDEX `IX_role_menu_role_id`(`role_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '角色菜单表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of role_menu
-- ----------------------------
INSERT INTO `role_menu` VALUES (7694476983298048, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (778412336363999232, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (778412336363999233, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (778412336363999234, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (778412336363999235, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (778412336363999236, 631737765623021569, 'platform');
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
INSERT INTO `role_menu` VALUES (4507238275102543872, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507240594183557120, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507241708169728000, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507241809709633536, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507241880912138240, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507241957160390656, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507242150706548736, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507242216104136704, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507242290074882048, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507242360648241152, 631737765623021569, 'platform');
INSERT INTO `role_menu` VALUES (4507242531641626624, 631737765623021569, 'platform');

-- ----------------------------
-- Table structure for supplier
-- ----------------------------
DROP TABLE IF EXISTS `supplier`;
CREATE TABLE `supplier`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编码',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '供应商' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of supplier
-- ----------------------------

-- ----------------------------
-- Table structure for tenant
-- ----------------------------
DROP TABLE IF EXISTS `tenant`;
CREATE TABLE `tenant`  (
  `id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '租户名称',
  `remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '备注',
  `domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '租户域名',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '租户' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tenant
-- ----------------------------
INSERT INTO `tenant` VALUES ('mi', '大米公司', NULL, 'mi.crackerwork.cn', b'0', 631737765623021571, '2025-11-01 22:43:37.466969', '2025-11-03 20:49:24.066274', 631737765623021571, 0, NULL, NULL);
INSERT INTO `tenant` VALUES ('platform', '平台', NULL, 'platform.crackerwork.cn', b'1', 631737765623021571, '2025-10-30 20:38:31.319463', '2025-11-03 20:42:35.571284', 631737765623021571, 0, NULL, NULL);

-- ----------------------------
-- Table structure for tenant_menu
-- ----------------------------
DROP TABLE IF EXISTS `tenant_menu`;
CREATE TABLE `tenant_menu`  (
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '租户ID',
  `menu_id` bigint NOT NULL COMMENT '菜单ID',
  PRIMARY KEY (`tenant_id`, `menu_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '租户菜单关联' ROW_FORMAT = DYNAMIC;

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
INSERT INTO `tenant_menu` VALUES ('platform', 7694476983298048);
INSERT INTO `tenant_menu` VALUES ('platform', 778412336363999232);
INSERT INTO `tenant_menu` VALUES ('platform', 778412336363999233);
INSERT INTO `tenant_menu` VALUES ('platform', 778412336363999234);
INSERT INTO `tenant_menu` VALUES ('platform', 778412336363999235);
INSERT INTO `tenant_menu` VALUES ('platform', 778412336363999236);
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
INSERT INTO `tenant_menu` VALUES ('platform', 4507238275102543872);
INSERT INTO `tenant_menu` VALUES ('platform', 4507240594183557120);
INSERT INTO `tenant_menu` VALUES ('platform', 4507241708169728000);
INSERT INTO `tenant_menu` VALUES ('platform', 4507241809709633536);
INSERT INTO `tenant_menu` VALUES ('platform', 4507241880912138240);
INSERT INTO `tenant_menu` VALUES ('platform', 4507241957160390656);
INSERT INTO `tenant_menu` VALUES ('platform', 4507242150706548736);
INSERT INTO `tenant_menu` VALUES ('platform', 4507242216104136704);
INSERT INTO `tenant_menu` VALUES ('platform', 4507242290074882048);
INSERT INTO `tenant_menu` VALUES ('platform', 4507242360648241152);
INSERT INTO `tenant_menu` VALUES ('platform', 4507242531641626624);

-- ----------------------------
-- Table structure for ticket
-- ----------------------------
DROP TABLE IF EXISTS `ticket`;
CREATE TABLE `ticket`  (
  `id` bigint NOT NULL,
  `title` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '标题',
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '内容',
  `status` int NOT NULL COMMENT '状态',
  `user_id` bigint NOT NULL COMMENT '用户ID',
  `rating` int NULL DEFAULT NULL COMMENT '评价星级',
  `rating_comment` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '评价内容',
  `assigned_user_id` bigint NULL DEFAULT NULL COMMENT '负责人',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '工单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ticket
-- ----------------------------

-- ----------------------------
-- Table structure for ticket_reply
-- ----------------------------
DROP TABLE IF EXISTS `ticket_reply`;
CREATE TABLE `ticket_reply`  (
  `id` bigint NOT NULL,
  `ticket_id` bigint NOT NULL COMMENT '工单ID',
  `sender_id` bigint NOT NULL COMMENT '发送人ID',
  `content` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '内容',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '工单回复' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ticket_reply
-- ----------------------------

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user`  (
  `id` bigint NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户名',
  `password` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '密码',
  `password_salt` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '密码盐',
  `avatar` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '头像',
  `nick_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '昵称',
  `sex` int NOT NULL COMMENT '性别',
  `is_enabled` tinyint(1) NOT NULL COMMENT '是否启用',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `phone` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '手机号码',
  `dept_id` bigint NULL DEFAULT NULL COMMENT '部门ID',
  `post_id` bigint NULL DEFAULT NULL COMMENT '职位ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `user_name`(`user_name` ASC, `tenant_id` ASC) USING BTREE COMMENT '用户名唯一',
  UNIQUE INDEX `nick_name`(`nick_name` ASC, `tenant_id` ASC) USING BTREE COMMENT '昵称唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '用户表' ROW_FORMAT = DYNAMIC;

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
  `user_id` bigint NOT NULL COMMENT '用户ID',
  `role_id` bigint NOT NULL COMMENT '角色ID',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  PRIMARY KEY (`user_id`, `role_id`) USING BTREE,
  INDEX `IX_user_role_role_id`(`role_id` ASC) USING BTREE,
  INDEX `IX_user_role_user_id`(`user_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '用户角色关联表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of user_role
-- ----------------------------
INSERT INTO `user_role` VALUES (631737765623021571, 631737765623021569, 'platform');
INSERT INTO `user_role` VALUES (4505462198378172416, 631737765623021568, 'mi');

-- ----------------------------
-- Table structure for warehouse
-- ----------------------------
DROP TABLE IF EXISTS `warehouse`;
CREATE TABLE `warehouse`  (
  `id` bigint NOT NULL,
  `code` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '编码',
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '名称',
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `is_enabled` bit(1) NOT NULL DEFAULT b'0' COMMENT '是否启用',
  `tenant_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '租户ID',
  `creator_id` bigint NULL DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) NULL DEFAULT NULL,
  `last_modifier_id` bigint NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` bigint NULL DEFAULT NULL,
  `deletion_time` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `code`(`code` ASC, `tenant_id` ASC) USING BTREE COMMENT '编码唯一'
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '仓库' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of warehouse
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
