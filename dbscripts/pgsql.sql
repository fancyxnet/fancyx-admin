/*
 Navicat Premium Data Transfer

 Source Server         : 本地pgsql
 Source Server Type    : PostgreSQL
 Source Server Version : 160009 (160009)
 Source Host           : localhost:5432
 Source Catalog        : fancyx-admin
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 160009 (160009)
 File Encoding         : 65001

 Date: 25/09/2025 21:51:40
*/


-- ----------------------------
-- Table structure for api_access_log
-- ----------------------------
DROP TABLE IF EXISTS "public"."api_access_log";
CREATE TABLE "public"."api_access_log" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "path" text COLLATE "pg_catalog"."default",
  "method" varchar(16) COLLATE "pg_catalog"."default",
  "ip" varchar(32) COLLATE "pg_catalog"."default",
  "request_time" timestamp(6) NOT NULL,
  "response_time" timestamp(6),
  "duration" int4,
  "user_id" int8,
  "user_name" varchar(32) COLLATE "pg_catalog"."default",
  "request_body" text COLLATE "pg_catalog"."default",
  "response_body" text COLLATE "pg_catalog"."default",
  "browser" varchar(512) COLLATE "pg_catalog"."default",
  "query_string" text COLLATE "pg_catalog"."default",
  "trace_id" varchar(64) COLLATE "pg_catalog"."default",
  "operate_type" int4[],
  "operate_name" varchar(64) COLLATE "pg_catalog"."default",
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of api_access_log
-- ----------------------------

-- ----------------------------
-- Table structure for config
-- ----------------------------
DROP TABLE IF EXISTS "public"."config";
CREATE TABLE "public"."config" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "name" varchar(256) COLLATE "pg_catalog"."default" NOT NULL,
  "key" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "value" varchar(1024) COLLATE "pg_catalog"."default" NOT NULL,
  "group_key" varchar(64) COLLATE "pg_catalog"."default",
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."config"."name" IS '配置名称';
COMMENT ON COLUMN "public"."config"."key" IS '配置键名';
COMMENT ON COLUMN "public"."config"."value" IS '配置键值';
COMMENT ON COLUMN "public"."config"."group_key" IS '组别';
COMMENT ON COLUMN "public"."config"."remark" IS '备注';
COMMENT ON COLUMN "public"."config"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."config" IS '系统配置';

-- ----------------------------
-- Records of config
-- ----------------------------
INSERT INTO "public"."config" VALUES (1, NULL, '2025-07-14 22:58:35.09269', '2025-09-15 20:24:41.824287', NULL, '文件存储驱动类型', 'StorageType', '1', 'System', '本地服务器=1，阿里云OSS=2', NULL);

-- ----------------------------
-- Table structure for customer
-- ----------------------------
DROP TABLE IF EXISTS "public"."customer";
CREATE TABLE "public"."customer" (
  "id" int8 NOT NULL,
  "code" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "code_slim" varchar(16) COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(256) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "contact_name" varchar(64) COLLATE "pg_catalog"."default",
  "contact_phone" varchar(64) COLLATE "pg_catalog"."default",
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "is_deleted" bool NOT NULL,
  "deleter_id" varchar COLLATE "pg_catalog"."default",
  "deletion_time" timestamp(6)
)
;
COMMENT ON COLUMN "public"."customer"."code" IS '编号';
COMMENT ON COLUMN "public"."customer"."code_slim" IS '简码';
COMMENT ON COLUMN "public"."customer"."name" IS '名称';
COMMENT ON COLUMN "public"."customer"."remark" IS '备注';
COMMENT ON COLUMN "public"."customer"."contact_name" IS '联系人';
COMMENT ON COLUMN "public"."customer"."contact_phone" IS '联系电话';
COMMENT ON TABLE "public"."customer" IS '客户信息表';

-- ----------------------------
-- Records of customer
-- ----------------------------

-- ----------------------------
-- Table structure for dept
-- ----------------------------
DROP TABLE IF EXISTS "public"."dept";
CREATE TABLE "public"."dept" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" varchar COLLATE "pg_catalog"."default",
  "is_deleted" bool NOT NULL,
  "deleter_id" int8,
  "deletion_time" timestamp(6),
  "code" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "sort" int4 NOT NULL,
  "description" varchar(512) COLLATE "pg_catalog"."default",
  "status" int4 NOT NULL,
  "curator_id" int8,
  "email" varchar(64) COLLATE "pg_catalog"."default",
  "phone" varchar(64) COLLATE "pg_catalog"."default",
  "parent_id" int8,
  "tree_path" varchar(1024) COLLATE "pg_catalog"."default" NOT NULL,
  "tree_level" int4 NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."dept"."code" IS '部门编号';
COMMENT ON COLUMN "public"."dept"."name" IS '部门名称';
COMMENT ON COLUMN "public"."dept"."sort" IS '排序';
COMMENT ON COLUMN "public"."dept"."description" IS '描述';
COMMENT ON COLUMN "public"."dept"."status" IS '状态：1正常2停用';
COMMENT ON COLUMN "public"."dept"."curator_id" IS '负责人';
COMMENT ON COLUMN "public"."dept"."email" IS '邮箱';
COMMENT ON COLUMN "public"."dept"."phone" IS '电话';
COMMENT ON COLUMN "public"."dept"."parent_id" IS '父ID';
COMMENT ON COLUMN "public"."dept"."tree_path" IS '树形路径';
COMMENT ON COLUMN "public"."dept"."tree_level" IS '树形层级';
COMMENT ON COLUMN "public"."dept"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."dept" IS '部门表';

-- ----------------------------
-- Records of dept
-- ----------------------------

-- ----------------------------
-- Table structure for dict_data
-- ----------------------------
DROP TABLE IF EXISTS "public"."dict_data";
CREATE TABLE "public"."dict_data" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "value" varchar(256) COLLATE "pg_catalog"."default" NOT NULL,
  "label" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "dict_type" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "sort" int4 NOT NULL,
  "is_enabled" bool NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."dict_data"."value" IS '字典值';
COMMENT ON COLUMN "public"."dict_data"."label" IS '显示文本';
COMMENT ON COLUMN "public"."dict_data"."dict_type" IS '字典类型';
COMMENT ON COLUMN "public"."dict_data"."remark" IS '备注';
COMMENT ON COLUMN "public"."dict_data"."sort" IS '排序值';
COMMENT ON COLUMN "public"."dict_data"."is_enabled" IS '是否开启';
COMMENT ON COLUMN "public"."dict_data"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."dict_data" IS '字典数据表';

-- ----------------------------
-- Records of dict_data
-- ----------------------------

-- ----------------------------
-- Table structure for dict_type
-- ----------------------------
DROP TABLE IF EXISTS "public"."dict_type";
CREATE TABLE "public"."dict_type" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "name" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "dict_type" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "is_enabled" bool NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."dict_type"."name" IS '字典名称';
COMMENT ON COLUMN "public"."dict_type"."dict_type" IS '字典类型';
COMMENT ON COLUMN "public"."dict_type"."remark" IS '备注';
COMMENT ON COLUMN "public"."dict_type"."is_enabled" IS '是否开启';
COMMENT ON COLUMN "public"."dict_type"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."dict_type" IS '字典类型表';

-- ----------------------------
-- Records of dict_type
-- ----------------------------

-- ----------------------------
-- Table structure for exception_log
-- ----------------------------
DROP TABLE IF EXISTS "public"."exception_log";
CREATE TABLE "public"."exception_log" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "exception_type" varchar(64) COLLATE "pg_catalog"."default",
  "message" text COLLATE "pg_catalog"."default",
  "stack_trace" text COLLATE "pg_catalog"."default",
  "inner_exception" text COLLATE "pg_catalog"."default",
  "request_path" text COLLATE "pg_catalog"."default",
  "request_method" varchar(16) COLLATE "pg_catalog"."default",
  "user_id" int8,
  "user_name" varchar(16) COLLATE "pg_catalog"."default",
  "ip" varchar(32) COLLATE "pg_catalog"."default",
  "browser" varchar(512) COLLATE "pg_catalog"."default",
  "trace_id" varchar(64) COLLATE "pg_catalog"."default",
  "is_handled" bool NOT NULL,
  "handled_time" timestamp(6),
  "handled_by" varchar(255) COLLATE "pg_catalog"."default",
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of exception_log
-- ----------------------------

-- ----------------------------
-- Table structure for log_record
-- ----------------------------
DROP TABLE IF EXISTS "public"."log_record";
CREATE TABLE "public"."log_record" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "type" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "sub_type" varchar(512) COLLATE "pg_catalog"."default" NOT NULL,
  "biz_no" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "content" text COLLATE "pg_catalog"."default" NOT NULL,
  "browser" varchar(512) COLLATE "pg_catalog"."default",
  "ip" varchar(32) COLLATE "pg_catalog"."default",
  "trace_id" varchar(64) COLLATE "pg_catalog"."default",
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "user_id" int8,
  "user_name" varchar(32) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of log_record
-- ----------------------------

-- ----------------------------
-- Table structure for login_log
-- ----------------------------
DROP TABLE IF EXISTS "public"."login_log";
CREATE TABLE "public"."login_log" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "user_name" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "ip" varchar(32) COLLATE "pg_catalog"."default",
  "address" varchar(256) COLLATE "pg_catalog"."default",
  "os" varchar(64) COLLATE "pg_catalog"."default",
  "browser" varchar(512) COLLATE "pg_catalog"."default",
  "operation_msg" varchar(128) COLLATE "pg_catalog"."default",
  "is_success" bool NOT NULL,
  "session_id" varchar(36) COLLATE "pg_catalog"."default",
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."login_log"."user_name" IS '账号';
COMMENT ON COLUMN "public"."login_log"."ip" IS 'IP';
COMMENT ON COLUMN "public"."login_log"."address" IS '登录地址';
COMMENT ON COLUMN "public"."login_log"."os" IS '系统';
COMMENT ON COLUMN "public"."login_log"."browser" IS '浏览器';
COMMENT ON COLUMN "public"."login_log"."operation_msg" IS '操作信息';
COMMENT ON COLUMN "public"."login_log"."is_success" IS '是否成功';
COMMENT ON COLUMN "public"."login_log"."session_id" IS '会话ID';
COMMENT ON COLUMN "public"."login_log"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."login_log" IS '登录日志';

-- ----------------------------
-- Records of login_log
-- ----------------------------

-- ----------------------------
-- Table structure for menu
-- ----------------------------
DROP TABLE IF EXISTS "public"."menu";
CREATE TABLE "public"."menu" (
  "creator_id" varchar COLLATE "pg_catalog"."default",
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "title" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "icon" varchar(64) COLLATE "pg_catalog"."default",
  "path" varchar(256) COLLATE "pg_catalog"."default",
  "component" varchar(256) COLLATE "pg_catalog"."default",
  "menu_type" int4 NOT NULL,
  "permission" varchar(128) COLLATE "pg_catalog"."default",
  "sort" int4 NOT NULL,
  "display" bool NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "is_external" bool NOT NULL,
  "id" int8 NOT NULL,
  "parent_id" int8
)
;
COMMENT ON COLUMN "public"."menu"."title" IS '显示标题/名称';
COMMENT ON COLUMN "public"."menu"."icon" IS '图标';
COMMENT ON COLUMN "public"."menu"."path" IS '路由/地址';
COMMENT ON COLUMN "public"."menu"."component" IS '组件地址';
COMMENT ON COLUMN "public"."menu"."menu_type" IS '功能类型';
COMMENT ON COLUMN "public"."menu"."permission" IS '授权码';
COMMENT ON COLUMN "public"."menu"."sort" IS '排序';
COMMENT ON COLUMN "public"."menu"."display" IS '是否隐藏';
COMMENT ON COLUMN "public"."menu"."tenant_id" IS '租户ID';
COMMENT ON COLUMN "public"."menu"."is_external" IS '是否外链';
COMMENT ON TABLE "public"."menu" IS '菜单表';

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-06 15:48:45.260116', NULL, NULL, '重置密码', NULL, NULL, NULL, 3, 'Sys.User.ResetPwd', 9, 't', NULL, 'f', 4491762374256627717, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:45:31.598', NULL, NULL, '分配功能权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignMenu', 5, 't', NULL, 'f', 4491762374256627718, 4491762374256627751);
INSERT INTO "public"."menu" VALUES (NULL, '2025-09-10 20:56:23.555831', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.User.Update', 10, 't', NULL, 'f', 4491762374256627719, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-14 22:53:54.688697', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Config.Add', 1, 't', NULL, 'f', 4491762374256627720, 4491762374256627750);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-02 21:51:43.566378', NULL, NULL, '访问日志', NULL, '/monitor/apiAccessLog', 'monitor/apiAccessLog', 2, '', 3, 't', NULL, 'f', 4491762374256627721, 4491762374256627754);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:44:19.284', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.User.Add', 1, 't', NULL, 'f', 4491762374256627722, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:44:39.958', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.User.List', 2, 't', NULL, 'f', 4491762374256627723, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:45:31.598', NULL, NULL, '分配角色', NULL, NULL, NULL, 3, 'Sys.User.AssignRole', 4, 't', NULL, 'f', 4491762374256627724, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:46:04.86', NULL, NULL, '启用/禁用', NULL, NULL, NULL, 3, 'Sys.User.SwitchEnabledStatus', 5, 't', NULL, 'f', 4491762374256627725, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:50:42.004', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Role.Update', 3, 't', NULL, 'f', 4491762374256627726, 4491762374256627751);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 14:38:03.056', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Org.PositionGroup.Add', 1, 't', NULL, 'f', 4491762374256627727, 4491762374256627771);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 14:38:28.028', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Org.PositionGroup.List', 2, 't', NULL, 'f', 4491762374256627728, 4491762374256627771);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 14:38:43.893', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Org.PositionGroup.Update', 3, 't', NULL, 'f', 4491762374256627729, 4491762374256627771);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 14:38:57.355', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Org.PositionGroup.Delete', 4, 't', NULL, 'f', 4491762374256627730, 4491762374256627771);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 17:54:31.569', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Org.Position.Add', 1, 't', NULL, 'f', 4491762374256627731, 4491762374256627760);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 17:55:13.072', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Org.Position.Update', 3, 't', NULL, 'f', 4491762374256627732, 4491762374256627760);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 17:55:57.373', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Org.Position.Delete', 4, 't', NULL, 'f', 4491762374256627733, 4491762374256627760);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 19:48:27.341', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Org.Dept.Add', 1, 't', NULL, 'f', 4491762374256627734, 4491762374256627761);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 19:48:46.596', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Org.Dept.List', 2, 't', NULL, 'f', 4491762374256627735, 4491762374256627761);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 19:49:01.689', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Org.Dept.Update', 3, 't', NULL, 'f', 4491762374256627736, 4491762374256627761);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 19:49:13.599', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Org.Dept.Delete', 4, 't', NULL, 'f', 4491762374256627737, 4491762374256627761);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 20:45:10.111', NULL, NULL, '注销', NULL, NULL, NULL, 3, 'Monitor.Logout', 1, 't', NULL, 'f', 4491762374256627738, 4491762374256627765);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 20:57:50.052', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.DictType.Add', 1, 't', NULL, 'f', 4491762374256627739, 4491762374256627762);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-29 22:09:49.060841', NULL, NULL, '富文本组件', NULL, '/quickWork/rickText', 'quickWork/rickText', 2, '', 1, 't', NULL, 'f', 4491762374256627740, 4491762374256627715);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-19 00:03:14.804374', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Add', 1, 't', NULL, 'f', 4491762374256627741, 4491762374256627790);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-19 00:03:30.478029', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.List', 1, 't', NULL, 'f', 4491762374256627742, 4491762374256627790);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-19 00:03:41.928625', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Update', 3, 't', NULL, 'f', 4491762374256627743, 4491762374256627790);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 21:03:31.122', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.DictData.Update', 3, 't', NULL, 'f', 4491762374256627744, 4491762374256627789);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 21:03:51.978', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.DictData.Delete', 1, 't', NULL, 'f', 4491762374256627745, 4491762374256627789);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:50:42.004', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Menu.Update', 3, 't', NULL, 'f', 4491762374256627746, 4491762374256627752);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:45:12.962', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Menu.Delete', 4, 't', NULL, 'f', 4491762374256627747, 4491762374256627752);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:44:39.958', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Role.List', 2, 't', NULL, 'f', 4491762374256627748, 4491762374256627751);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:45:12.962', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Role.Delete', 4, 't', NULL, 'f', 4491762374256627749, 4491762374256627751);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-14 22:38:19.75711', NULL, NULL, '配置管理', NULL, '/system/config', 'system/config', 2, '', 7, 't', NULL, 'f', 4491762374256627750, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-15 16:10:04.215', NULL, NULL, '角色管理', NULL, '/system/role', 'system/role', 2, 'Sys:Role', 2, 't', NULL, 'f', 4491762374256627751, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-15 16:10:54.046', NULL, NULL, '菜单管理', NULL, '/system/menu', 'system/menu', 2, 'Sys:Menu', 3, 't', NULL, 'f', 4491762374256627752, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2025-08-31 22:19:19.906074', NULL, NULL, '分配数据权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignDataScope', 6, 't', NULL, 'f', 4491762374256627753, 4491762374256627751);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-15 15:49:13.507', NULL, NULL, '系统管理', 'antd:SettingOutlined', '/system', NULL, 1, 'System', 2, 't', NULL, 'f', 4491762374256627712, NULL);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-08 22:48:47.742', NULL, NULL, '组织架构', 'antd:TeamOutlined', '/org', NULL, 1, 'Org', 1, 't', NULL, 'f', 4491762374256627713, NULL);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-15 21:55:44.831206', NULL, NULL, '在线文档', 'antd:ApiOutlined', 'https://doc.crackerwork.cn/', '#', 2, '', 99, 't', NULL, 't', 4491762374256627714, NULL);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-29 22:09:06.608163', NULL, NULL, '快速开发', 'antd:ToolOutlined', '/quickWork', NULL, 1, '', 98, 't', NULL, 'f', 4491762374256627715, NULL);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-19 00:03:57.712663', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Delete', 4, 't', NULL, 'f', 4491762374256627716, 4491762374256627790);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-28 09:41:59.313', NULL, NULL, '数据字典', NULL, '/system/dict', 'system/dictType', 2, 'Sys:Dict', 4, 't', NULL, 'f', 4491762374256627762, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-15 16:01:03.301', NULL, NULL, '用户管理', '', '/system/user', 'system/user', 2, '', 1, 't', NULL, 'f', 4491762374256627763, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-02 21:52:29.472722', NULL, NULL, '异常日志', NULL, '/monitor/exceptionLog', 'monitor/exceptionLog', 2, '', 2, 't', NULL, 'f', 4491762374256627764, 4491762374256627754);
INSERT INTO "public"."menu" VALUES (NULL, '2025-01-04 11:07:31.86', NULL, NULL, '在线用户', NULL, '/monitor/onlineUser', 'monitor/onlineUser', 2, '', 1, 't', NULL, 'f', 4491762374256627765, 4491762374256627754);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-15 21:04:05.703868', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Tenant.Add', 1, 't', NULL, 'f', 4491762374256627766, 4491762374256627770);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-15 21:04:19.12469', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Tenant.List', 2, 't', NULL, 'f', 4491762374256627767, 4491762374256627770);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-15 21:04:32.976932', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Tenant.Update', 3, 't', NULL, 'f', 4491762374256627768, 4491762374256627770);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-15 21:04:51.458147', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Tenant.Delete', 4, 't', NULL, 'f', 4491762374256627769, 4491762374256627770);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-15 21:01:24.861586', NULL, NULL, '租户管理', NULL, '/system/tenant', 'system/tenant', 2, '', 8, 't', NULL, 'f', 4491762374256627770, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 14:26:20.046', NULL, NULL, '职位分组', '', '/org/positionGroup', 'org/positionGroup', 2, '', 1, 't', NULL, 'f', 4491762374256627771, 4491762374256627713);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-18 20:37:01.851781', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Notification.Add', 1, 't', NULL, 'f', 4491762374256627772, 4491762374256627755);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-18 20:37:14.766532', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Notification.List', 2, 't', NULL, 'f', 4491762374256627773, 4491762374256627755);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-18 20:37:28.704311', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Notification.Update', 3, 't', NULL, 'f', 4491762374256627774, 4491762374256627755);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-18 20:37:40.555994', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Notification.Delete', 4, 't', NULL, 'f', 4491762374256627775, 4491762374256627755);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-18 20:38:42.951706', NULL, NULL, '我的通知', NULL, '/org/myNotification', 'org/myNotification', 2, '', 6, 't', NULL, 'f', 4491762374256627776, 4491762374256627713);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-03 22:00:40.118', NULL, NULL, '登录日志', NULL, '/system/log/login', 'system/log/loginLog', 2, '', 10, 't', NULL, 'f', 4491762374256627777, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-03 22:02:12.559', NULL, NULL, '业务日志', NULL, '/system/log/business', 'system/log/businessLog', 2, '', 11, 't', NULL, 'f', 4491762374256627778, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-19 00:04:13.762756', NULL, NULL, '执行日志', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Log', 5, 't', NULL, 'f', 4491762374256627779, 4491762374256627790);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 21:01:47.281', NULL, NULL, '删除', NULL, '', NULL, 3, 'Sys.DictType.Delete', 4, 't', NULL, 'f', 4491762374256627780, 4491762374256627762);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 21:02:11.74', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.DictData.Add', 1, 't', NULL, 'f', 4491762374256627781, 4491762374256627789);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 21:02:29.665', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.DictData.List', 2, 't', NULL, 'f', 4491762374256627782, 4491762374256627789);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:44:19.284', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Menu.Add', 1, 't', NULL, 'f', 4491762374256627783, 4491762374256627752);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:44:39.958', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Menu.List', 2, 't', NULL, 'f', 4491762374256627784, 4491762374256627752);
INSERT INTO "public"."menu" VALUES (NULL, '2025-09-14 10:54:32.375704', NULL, NULL, '部门简单信息', NULL, NULL, NULL, 3, 'Org.Dept.GetDeptSimpleInfos', 5, 't', NULL, 'f', 4491762374256627785, 4491762374256627761);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-14 22:54:07.938236', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Config.List', 2, 't', NULL, 'f', 4491762374256627786, 4491762374256627750);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-14 22:54:26.364848', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Config.Update', 3, 't', NULL, 'f', 4491762374256627787, 4491762374256627750);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-14 22:54:38.605153', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Config.Delete', 4, 't', NULL, 'f', 4491762374256627788, 4491762374256627750);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-28 15:43:17.394', NULL, NULL, '字典项', NULL, '/system/dictItem/:dictType', 'system/dictData', 2, NULL, 5, 'f', NULL, 'f', 4491762374256627789, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-19 00:01:20.451501', NULL, NULL, '定时任务', NULL, '/system/scheduledTask', 'system/scheduledTask', 2, '', 10, 't', NULL, 'f', 4491762374256627790, 4491762374256627712);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 21:00:27.984', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.DictType.Update', 3, 't', NULL, 'f', 4491762374256627791, 4491762374256627762);
INSERT INTO "public"."menu" VALUES (NULL, '2025-01-04 11:06:54.207', NULL, NULL, '系统监控', 'antd:FundOutlined', '/monitor', NULL, 1, '', 3, 't', NULL, 'f', 4491762374256627754, NULL);
INSERT INTO "public"."menu" VALUES (NULL, '2025-07-18 20:33:53.818229', NULL, NULL, '通知管理', '', '/org/notification', 'org/notification', 2, '', 5, 't', NULL, 'f', 4491762374256627755, 4491762374256627713);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:45:12.962', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.User.Delete', 3, 't', NULL, 'f', 4491762374256627756, 4491762374256627763);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 17:54:53.219', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Org.Position.List', 2, 't', NULL, 'f', 4491762374256627757, 4491762374256627760);
INSERT INTO "public"."menu" VALUES (NULL, '2025-04-29 20:59:45.015', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.DictType.List', 2, 't', NULL, 'f', 4491762374256627758, 4491762374256627762);
INSERT INTO "public"."menu" VALUES (NULL, '2024-06-24 21:44:19.284', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Role.Add', 1, 't', NULL, 'f', 4491762374256627759, 4491762374256627751);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 17:52:45.803', NULL, NULL, '职位管理', NULL, '/org/position', 'org/position', 2, 'Org:Position', 2, 't', NULL, 'f', 4491762374256627760, 4491762374256627713);
INSERT INTO "public"."menu" VALUES (NULL, '2024-07-13 19:47:46.294', NULL, NULL, '部门管理', NULL, '/org/dept', 'org/dept', 2, 'Org:Department', 3, 't', NULL, 'f', 4491762374256627761, 4491762374256627713);

-- ----------------------------
-- Table structure for notification
-- ----------------------------
DROP TABLE IF EXISTS "public"."notification";
CREATE TABLE "public"."notification" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "title" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "content" varchar(512) COLLATE "pg_catalog"."default",
  "user_id" int8 NOT NULL,
  "is_readed" bool NOT NULL,
  "readed_time" timestamp(6),
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."notification"."title" IS '通知标题';
COMMENT ON COLUMN "public"."notification"."content" IS '通知内容';
COMMENT ON COLUMN "public"."notification"."user_id" IS '通知员工';
COMMENT ON COLUMN "public"."notification"."is_readed" IS '是否已读(true已读false未读)';
COMMENT ON COLUMN "public"."notification"."readed_time" IS '已读时间';
COMMENT ON COLUMN "public"."notification"."tenant_id" IS '租户ID';

-- ----------------------------
-- Records of notification
-- ----------------------------

-- ----------------------------
-- Table structure for position
-- ----------------------------
DROP TABLE IF EXISTS "public"."position";
CREATE TABLE "public"."position" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" varchar COLLATE "pg_catalog"."default",
  "is_deleted" bool NOT NULL,
  "deleter_id" int8,
  "deletion_time" timestamp(6),
  "code" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "level" int4 NOT NULL,
  "status" int4 NOT NULL,
  "description" varchar(512) COLLATE "pg_catalog"."default",
  "group_id" int8,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."position"."code" IS '职位编号';
COMMENT ON COLUMN "public"."position"."name" IS '职位名称';
COMMENT ON COLUMN "public"."position"."level" IS '职级';
COMMENT ON COLUMN "public"."position"."status" IS '状态：1正常2停用';
COMMENT ON COLUMN "public"."position"."description" IS '描述';
COMMENT ON COLUMN "public"."position"."group_id" IS '职位分组';
COMMENT ON COLUMN "public"."position"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."position" IS '职位表';

-- ----------------------------
-- Records of position
-- ----------------------------

-- ----------------------------
-- Table structure for position_group
-- ----------------------------
DROP TABLE IF EXISTS "public"."position_group";
CREATE TABLE "public"."position_group" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "group_name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "parent_id" int8,
  "tree_path" varchar(1024) COLLATE "pg_catalog"."default" NOT NULL,
  "sort" int4 NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "tree_level" int4 NOT NULL
)
;
COMMENT ON COLUMN "public"."position_group"."group_name" IS '分组名';
COMMENT ON COLUMN "public"."position_group"."remark" IS '备注';
COMMENT ON COLUMN "public"."position_group"."parent_id" IS '父ID';
COMMENT ON COLUMN "public"."position_group"."tree_path" IS '层级路径';
COMMENT ON COLUMN "public"."position_group"."sort" IS '排序值';
COMMENT ON COLUMN "public"."position_group"."tenant_id" IS '租户ID';
COMMENT ON COLUMN "public"."position_group"."tree_level" IS '层级';
COMMENT ON TABLE "public"."position_group" IS '职位分组';

-- ----------------------------
-- Records of position_group
-- ----------------------------

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS "public"."role";
CREATE TABLE "public"."role" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" varchar COLLATE "pg_catalog"."default",
  "is_deleted" bool NOT NULL,
  "deleter_id" int8,
  "deletion_time" timestamp(6),
  "role_name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "dept_power_type" int4 NOT NULL DEFAULT 0,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "is_enabled" bool NOT NULL
)
;
COMMENT ON COLUMN "public"."role"."role_name" IS '角色名';
COMMENT ON COLUMN "public"."role"."remark" IS '备注';
COMMENT ON COLUMN "public"."role"."dept_power_type" IS '部门权限类型';
COMMENT ON COLUMN "public"."role"."tenant_id" IS '租户ID';
COMMENT ON COLUMN "public"."role"."is_enabled" IS '是否启用';
COMMENT ON TABLE "public"."role" IS '角色表';

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO "public"."role" VALUES (1, NULL, '2025-09-25 21:09:23', NULL, NULL, 'f', NULL, NULL, '系统管理员', NULL, 0, NULL, 't');

-- ----------------------------
-- Table structure for role_dept
-- ----------------------------
DROP TABLE IF EXISTS "public"."role_dept";
CREATE TABLE "public"."role_dept" (
  "id" int8 NOT NULL,
  "role_id" int8 NOT NULL,
  "dept_id" int8 NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."role_dept"."role_id" IS '角色ID';
COMMENT ON COLUMN "public"."role_dept"."dept_id" IS '部门ID';
COMMENT ON COLUMN "public"."role_dept"."tenant_id" IS '租户ID';

-- ----------------------------
-- Records of role_dept
-- ----------------------------

-- ----------------------------
-- Table structure for role_menu
-- ----------------------------
DROP TABLE IF EXISTS "public"."role_menu";
CREATE TABLE "public"."role_menu" (
  "id" int8 NOT NULL,
  "menu_id" int8 NOT NULL,
  "role_id" int8 NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."role_menu"."menu_id" IS '菜单ID';
COMMENT ON COLUMN "public"."role_menu"."role_id" IS '角色ID';
COMMENT ON COLUMN "public"."role_menu"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."role_menu" IS '角色菜单表';

-- ----------------------------
-- Records of role_menu
-- ----------------------------

-- ----------------------------
-- Table structure for tenant
-- ----------------------------
DROP TABLE IF EXISTS "public"."tenant";
CREATE TABLE "public"."tenant" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "domain" varchar(256) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."tenant"."name" IS '租户名称';
COMMENT ON COLUMN "public"."tenant"."tenant_id" IS '租户标识';
COMMENT ON COLUMN "public"."tenant"."remark" IS '备注';
COMMENT ON COLUMN "public"."tenant"."domain" IS '租户域名';

-- ----------------------------
-- Records of tenant
-- ----------------------------

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS "public"."user";
CREATE TABLE "public"."user" (
  "id" int8 NOT NULL,
  "creator_id" int8,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" int8,
  "is_deleted" bool NOT NULL,
  "deleter_id" varchar COLLATE "pg_catalog"."default",
  "deletion_time" timestamp(6),
  "user_name" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "password" varchar(512) COLLATE "pg_catalog"."default" NOT NULL,
  "password_salt" varchar(256) COLLATE "pg_catalog"."default" NOT NULL,
  "avatar" varchar(256) COLLATE "pg_catalog"."default",
  "nick_name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "sex" int4 NOT NULL,
  "is_enabled" bool NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "phone" varchar(11) COLLATE "pg_catalog"."default",
  "dept_id" int8,
  "post_id" int8
)
;
COMMENT ON COLUMN "public"."user"."user_name" IS '用户名';
COMMENT ON COLUMN "public"."user"."password" IS '密码';
COMMENT ON COLUMN "public"."user"."password_salt" IS '密码盐';
COMMENT ON COLUMN "public"."user"."avatar" IS '头像';
COMMENT ON COLUMN "public"."user"."nick_name" IS '昵称';
COMMENT ON COLUMN "public"."user"."sex" IS '性别';
COMMENT ON COLUMN "public"."user"."is_enabled" IS '是否启用';
COMMENT ON COLUMN "public"."user"."tenant_id" IS '租户ID';
COMMENT ON COLUMN "public"."user"."phone" IS '手机号码';
COMMENT ON COLUMN "public"."user"."dept_id" IS '部门ID';
COMMENT ON COLUMN "public"."user"."post_id" IS '岗位ID';
COMMENT ON TABLE "public"."user" IS '用户表';

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO "public"."user" VALUES (1, NULL, '2024-12-30 22:48:48.458', '2025-09-14 18:41:56.108293', NULL, 'f', NULL, NULL, 'admin', 'a2fa8ec90f15197c7a4e6e00525b198a', 'vHQZvbz+ng+B4NrSAEYl6g==', 'file/myavatar.jpg', '风汐', 2, 't', 'cq_market', '18273403759', NULL, NULL);

-- ----------------------------
-- Table structure for user_role
-- ----------------------------
DROP TABLE IF EXISTS "public"."user_role";
CREATE TABLE "public"."user_role" (
  "id" int8 NOT NULL,
  "user_id" int8 NOT NULL,
  "role_id" int8 NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."user_role"."user_id" IS '用户ID';
COMMENT ON COLUMN "public"."user_role"."role_id" IS '角色ID';
COMMENT ON COLUMN "public"."user_role"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."user_role" IS '用户角色关联表';

-- ----------------------------
-- Records of user_role
-- ----------------------------
INSERT INTO "public"."user_role" VALUES (1, 1, 1, NULL);

-- ----------------------------
-- Function structure for convert_all_uuid_columns_to_int8_safe
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."convert_all_uuid_columns_to_int8_safe"();
CREATE OR REPLACE FUNCTION "public"."convert_all_uuid_columns_to_int8_safe"()
  RETURNS "pg_catalog"."void" AS $BODY$
DECLARE
    table_record RECORD;
    column_record RECORD;
BEGIN
    FOR table_record IN 
        SELECT table_name, table_schema
        FROM information_schema.columns 
        WHERE data_type = 'uuid' 
          AND table_schema NOT IN ('information_schema', 'pg_catalog')
        GROUP BY table_name, table_schema
    LOOP
        FOR column_record IN
            SELECT column_name
            FROM information_schema.columns 
            WHERE table_name = table_record.table_name 
              AND table_schema = table_record.table_schema
              AND data_type = 'uuid'
        LOOP
            RAISE NOTICE '处理表: %.%，列: %', 
                table_record.table_schema, 
                table_record.table_name, 
                column_record.column_name;
                
            -- 这里只生成转换脚本，不实际执行
            RAISE NOTICE '转换脚本: ALTER TABLE %.% ALTER COLUMN % TYPE BIGINT USING hashtext(%)::bigint;',
                table_record.table_schema,
                table_record.table_name,
                column_record.column_name,
                column_record.column_name;
        END LOOP;
    END LOOP;
    
    RAISE NOTICE '注意: 这只是一个预览，实际转换需要手动执行上述脚本';
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- ----------------------------
-- Primary Key structure for table api_access_log
-- ----------------------------
ALTER TABLE "public"."api_access_log" ADD CONSTRAINT "public_api_access_log_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table config
-- ----------------------------
ALTER TABLE "public"."config" ADD CONSTRAINT "public_sys_config_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table customer
-- ----------------------------
ALTER TABLE "public"."customer" ADD CONSTRAINT "customer_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table dept
-- ----------------------------
ALTER TABLE "public"."dept" ADD CONSTRAINT "public_sys_dept_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table dict_data
-- ----------------------------
ALTER TABLE "public"."dict_data" ADD CONSTRAINT "public_sys_dict_data_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table dict_type
-- ----------------------------
ALTER TABLE "public"."dict_type" ADD CONSTRAINT "public_sys_dict_type_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table exception_log
-- ----------------------------
ALTER TABLE "public"."exception_log" ADD CONSTRAINT "public_exception_log_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table log_record
-- ----------------------------
ALTER TABLE "public"."log_record" ADD CONSTRAINT "public_log_record_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table login_log
-- ----------------------------
ALTER TABLE "public"."login_log" ADD CONSTRAINT "public_sys_login_log_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table menu
-- ----------------------------
ALTER TABLE "public"."menu" ADD CONSTRAINT "public_sys_menu_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table notification
-- ----------------------------
ALTER TABLE "public"."notification" ADD CONSTRAINT "public_sys_notification_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table position
-- ----------------------------
ALTER TABLE "public"."position" ADD CONSTRAINT "public_org_position_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table position_group
-- ----------------------------
ALTER TABLE "public"."position_group" ADD CONSTRAINT "public_org_position_group_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table role
-- ----------------------------
ALTER TABLE "public"."role" ADD CONSTRAINT "public_sys_role_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table role_dept
-- ----------------------------
ALTER TABLE "public"."role_dept" ADD CONSTRAINT "public_sys_role_dept_pkey" PRIMARY KEY ("id", "role_id", "dept_id");

-- ----------------------------
-- Primary Key structure for table role_menu
-- ----------------------------
ALTER TABLE "public"."role_menu" ADD CONSTRAINT "public_sys_role_menu_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Indexes structure for table tenant
-- ----------------------------
CREATE UNIQUE INDEX "uk_tenant_id" ON "public"."tenant" USING btree (
  "tenant_id" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table tenant
-- ----------------------------
ALTER TABLE "public"."tenant" ADD CONSTRAINT "public_sys_tenant_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table user
-- ----------------------------
ALTER TABLE "public"."user" ADD CONSTRAINT "public_sys_user_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table user_role
-- ----------------------------
ALTER TABLE "public"."user_role" ADD CONSTRAINT "public_sys_user_role_pkey" PRIMARY KEY ("id");
