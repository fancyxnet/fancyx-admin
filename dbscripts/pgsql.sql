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

 Date: 14/09/2025 19:11:53
*/


-- ----------------------------
-- Table structure for api_access_log
-- ----------------------------
DROP TABLE IF EXISTS "public"."api_access_log";
CREATE TABLE "public"."api_access_log" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "path" text COLLATE "pg_catalog"."default",
  "method" varchar(16) COLLATE "pg_catalog"."default",
  "ip" varchar(32) COLLATE "pg_catalog"."default",
  "request_time" timestamp(6) NOT NULL,
  "response_time" timestamp(6),
  "duration" int8,
  "user_id" uuid,
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
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
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
INSERT INTO "public"."config" VALUES ('68758b9b-e6de-0d4c-0000-b9861878b283', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-14 22:58:35.09269', '2025-07-14 22:58:40.62024', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '文件存储驱动类型', 'StorageType', '1', 'System', '本地服务器=1，阿里云OSS=2', NULL);

-- ----------------------------
-- Table structure for dept
-- ----------------------------
DROP TABLE IF EXISTS "public"."dept";
CREATE TABLE "public"."dept" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "is_deleted" bool NOT NULL,
  "deleter_id" uuid,
  "deletion_time" timestamp(6),
  "code" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "sort" int4 NOT NULL,
  "description" varchar(512) COLLATE "pg_catalog"."default",
  "status" int4 NOT NULL,
  "curator_id" uuid,
  "email" varchar(64) COLLATE "pg_catalog"."default",
  "phone" varchar(64) COLLATE "pg_catalog"."default",
  "parent_id" uuid,
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
INSERT INTO "public"."dept" VALUES ('2caf547c-461b-4661-a2c7-2310a8707709', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 22:44:43.333833', NULL, NULL, 't', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 22:49:33.049842', '003', '前端组', 0, NULL, 1, NULL, NULL, NULL, NULL, 'b4cb13b2-9abf-4308-b6de-a75e29d860b9/2caf547c-461b-4661-a2c7-2310a8707709', 2, NULL);
INSERT INTO "public"."dept" VALUES ('b4cb13b2-9abf-4308-b6de-a75e29d860b9', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 22:15:23.024634', NULL, NULL, 't', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:30:49.61694', '001', '研发部', 0, NULL, 1, NULL, NULL, NULL, NULL, 'b4cb13b2-9abf-4308-b6de-a75e29d860b9', 1, NULL);
INSERT INTO "public"."dept" VALUES ('32b461cf-e5f8-4354-bc27-c6b5b2b8faaf', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:32:08.853433', NULL, NULL, 'f', NULL, NULL, 'web', '前端组', 0, NULL, 1, NULL, NULL, NULL, 'cf00274b-018f-433e-bfe9-58c9f4827296', 'e60121f5-d83f-4a72-aa74-8436e73f674f/cf00274b-018f-433e-bfe9-58c9f4827296/32b461cf-e5f8-4354-bc27-c6b5b2b8faaf', 3, NULL);
INSERT INTO "public"."dept" VALUES ('49144183-a3f5-4943-a2ca-2e030ac0a38f', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:32:25.92131', NULL, NULL, 'f', NULL, NULL, '.net', '后端组', 0, NULL, 1, NULL, NULL, NULL, 'cf00274b-018f-433e-bfe9-58c9f4827296', 'e60121f5-d83f-4a72-aa74-8436e73f674f/cf00274b-018f-433e-bfe9-58c9f4827296/49144183-a3f5-4943-a2ca-2e030ac0a38f', 3, NULL);
INSERT INTO "public"."dept" VALUES ('e60121f5-d83f-4a72-aa74-8436e73f674f', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:31:19.97476', '2025-09-09 23:48:11.542242', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, 'szfxkj', '深圳风汐科技有限公司', 1, NULL, 1, NULL, NULL, NULL, NULL, 'e60121f5-d83f-4a72-aa74-8436e73f674f', 1, NULL);
INSERT INTO "public"."dept" VALUES ('f0915491-8aae-4be6-8eb1-7ddfc9b7c9a2', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:32:51.532019', '2025-09-09 23:48:40.622404', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, 'rsb', '人事部', 2, NULL, 1, NULL, NULL, NULL, 'e60121f5-d83f-4a72-aa74-8436e73f674f', 'e60121f5-d83f-4a72-aa74-8436e73f674f/f0915491-8aae-4be6-8eb1-7ddfc9b7c9a2', 2, NULL);
INSERT INTO "public"."dept" VALUES ('cf00274b-018f-433e-bfe9-58c9f4827296', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:31:38.637803', '2025-09-09 23:51:05.712082', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, 'yfb', '研发部', 1, NULL, 1, NULL, NULL, NULL, 'e60121f5-d83f-4a72-aa74-8436e73f674f', 'e60121f5-d83f-4a72-aa74-8436e73f674f/cf00274b-018f-433e-bfe9-58c9f4827296', 2, NULL);
INSERT INTO "public"."dept" VALUES ('024a98a1-f929-44a4-bddb-f19a2ede463a', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 23:33:11.880968', '2025-09-09 23:52:54.519347', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, 'sales', '销售部', 3, NULL, 1, NULL, NULL, NULL, 'e60121f5-d83f-4a72-aa74-8436e73f674f', 'e60121f5-d83f-4a72-aa74-8436e73f674f/024a98a1-f929-44a4-bddb-f19a2ede463a', 2, NULL);

-- ----------------------------
-- Table structure for dict_data
-- ----------------------------
DROP TABLE IF EXISTS "public"."dict_data";
CREATE TABLE "public"."dict_data" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
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
INSERT INTO "public"."dict_data" VALUES ('68602622-e22b-e780-00f8-5c9d688361b4', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-06-28 17:28:02.919064', '2025-06-28 17:28:28.158885', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '1', 'L1', 'positionLevel', NULL, 1, 't', NULL);
INSERT INTO "public"."dict_data" VALUES ('6860262d-e22b-e780-00f8-5c9e382875ed', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-06-28 17:28:13.873205', '2025-06-28 17:28:31.396049', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2', 'L2', 'positionLevel', NULL, 2, 't', NULL);
INSERT INTO "public"."dict_data" VALUES ('68602667-e22b-e780-00f8-5c9f2187a1d8', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-06-28 17:29:11.307524', NULL, NULL, '3', 'L3', 'positionLevel', NULL, 3, 't', NULL);
INSERT INTO "public"."dict_data" VALUES ('3fa85f64-5717-4562-b3fc-2c963f66afa6', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-08 22:11:59.270718', NULL, NULL, 'string', 'string', 'string', 'string', 0, 't', NULL);
INSERT INTO "public"."dict_data" VALUES ('019929ac-fee3-7bf6-bc2b-a36667bb2452', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-08 22:13:47.369899', NULL, NULL, 'string1', 'string', 'string', 'string', 0, 't', NULL);
INSERT INTO "public"."dict_data" VALUES ('019929ad-1984-74ff-8943-afcc0dbde57d', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-08 22:13:54.180983', NULL, NULL, 'string2', 'string', 'string', 'string', 0, 't', NULL);
INSERT INTO "public"."dict_data" VALUES ('019929ad-2ba1-77a7-b434-77afdaef0411', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-08 22:13:58.817977', NULL, NULL, 'string3', 'string', 'string', 'string', 0, 't', NULL);

-- ----------------------------
-- Table structure for dict_type
-- ----------------------------
DROP TABLE IF EXISTS "public"."dict_type";
CREATE TABLE "public"."dict_type" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
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
INSERT INTO "public"."dict_type" VALUES ('6860261b-e22b-e780-00f8-5c9b126fd27d', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-06-28 17:27:55.475365', NULL, NULL, '职位职级', 'positionLevel', NULL, 't', NULL);

-- ----------------------------
-- Table structure for employee
-- ----------------------------
DROP TABLE IF EXISTS "public"."employee";
CREATE TABLE "public"."employee" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "is_deleted" bool NOT NULL,
  "deleter_id" uuid,
  "deletion_time" timestamp(6),
  "code" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "sex" int4 NOT NULL,
  "phone" varchar(16) COLLATE "pg_catalog"."default" NOT NULL,
  "id_no" varchar(32) COLLATE "pg_catalog"."default",
  "front_id_no_url" varchar(512) COLLATE "pg_catalog"."default",
  "back_id_no_url" varchar(512) COLLATE "pg_catalog"."default",
  "birthday" timestamp(6),
  "address" varchar(512) COLLATE "pg_catalog"."default",
  "email" varchar(64) COLLATE "pg_catalog"."default",
  "in_time" timestamp(6) NOT NULL,
  "out_time" timestamp(6),
  "status" int4 NOT NULL,
  "user_id" uuid,
  "dept_id" uuid,
  "position_id" uuid,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default"
)
;
COMMENT ON COLUMN "public"."employee"."code" IS '工号';
COMMENT ON COLUMN "public"."employee"."name" IS '姓名';
COMMENT ON COLUMN "public"."employee"."sex" IS '性别';
COMMENT ON COLUMN "public"."employee"."phone" IS '手机号码';
COMMENT ON COLUMN "public"."employee"."id_no" IS '身份证';
COMMENT ON COLUMN "public"."employee"."front_id_no_url" IS '身份证正面';
COMMENT ON COLUMN "public"."employee"."back_id_no_url" IS '身份证背面';
COMMENT ON COLUMN "public"."employee"."birthday" IS '生日';
COMMENT ON COLUMN "public"."employee"."address" IS '现住址';
COMMENT ON COLUMN "public"."employee"."email" IS '邮箱';
COMMENT ON COLUMN "public"."employee"."in_time" IS '入职时间';
COMMENT ON COLUMN "public"."employee"."out_time" IS '离职时间';
COMMENT ON COLUMN "public"."employee"."status" IS '状态 1正常2离职';
COMMENT ON COLUMN "public"."employee"."user_id" IS '关联用户ID';
COMMENT ON COLUMN "public"."employee"."dept_id" IS '部门ID';
COMMENT ON COLUMN "public"."employee"."position_id" IS '职位ID';
COMMENT ON COLUMN "public"."employee"."tenant_id" IS '租户ID';
COMMENT ON TABLE "public"."employee" IS '员工表';

-- ----------------------------
-- Records of employee
-- ----------------------------
INSERT INTO "public"."employee" VALUES ('6869907c-9c93-beac-0062-34172a640e0e', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-05 20:52:12.138175', '2025-07-05 21:45:13.910127', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, '001', 'tom', 1, '18211114444', NULL, NULL, NULL, NULL, NULL, NULL, '2025-08-30 16:26:31', '2025-08-30 08:22:02.3', 2, '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '6861a4ed-de18-91c8-009e-ef9b36642c3c', NULL, NULL);
INSERT INTO "public"."employee" VALUES ('68b4d449-3a66-9e98-00a5-e1af0cc71435', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-08-31 23:01:29.425016', NULL, NULL, 'f', NULL, NULL, '0002332', 'cracker', 1, '18211114445', NULL, NULL, NULL, NULL, NULL, NULL, '2025-08-30 16:00:00', NULL, 1, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for exception_log
-- ----------------------------
DROP TABLE IF EXISTS "public"."exception_log";
CREATE TABLE "public"."exception_log" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "exception_type" varchar(64) COLLATE "pg_catalog"."default",
  "message" text COLLATE "pg_catalog"."default",
  "stack_trace" text COLLATE "pg_catalog"."default",
  "inner_exception" text COLLATE "pg_catalog"."default",
  "request_path" text COLLATE "pg_catalog"."default",
  "request_method" varchar(16) COLLATE "pg_catalog"."default",
  "user_id" uuid,
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
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "type" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "sub_type" varchar(512) COLLATE "pg_catalog"."default" NOT NULL,
  "biz_no" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "content" text COLLATE "pg_catalog"."default" NOT NULL,
  "browser" varchar(512) COLLATE "pg_catalog"."default",
  "ip" varchar(32) COLLATE "pg_catalog"."default",
  "trace_id" varchar(64) COLLATE "pg_catalog"."default",
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "user_id" uuid,
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
  "id" uuid NOT NULL,
  "creator_id" uuid,
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
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "title" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "icon" varchar(64) COLLATE "pg_catalog"."default",
  "path" varchar(256) COLLATE "pg_catalog"."default",
  "component" varchar(256) COLLATE "pg_catalog"."default",
  "menu_type" int4 NOT NULL,
  "permission" varchar(128) COLLATE "pg_catalog"."default",
  "parent_id" uuid,
  "sort" int4 NOT NULL,
  "display" bool NOT NULL,
  "tenant_id" varchar(18) COLLATE "pg_catalog"."default",
  "is_external" bool NOT NULL
)
;
COMMENT ON COLUMN "public"."menu"."title" IS '显示标题/名称';
COMMENT ON COLUMN "public"."menu"."icon" IS '图标';
COMMENT ON COLUMN "public"."menu"."path" IS '路由/地址';
COMMENT ON COLUMN "public"."menu"."component" IS '组件地址';
COMMENT ON COLUMN "public"."menu"."menu_type" IS '功能类型';
COMMENT ON COLUMN "public"."menu"."permission" IS '授权码';
COMMENT ON COLUMN "public"."menu"."parent_id" IS '父级ID';
COMMENT ON COLUMN "public"."menu"."sort" IS '排序';
COMMENT ON COLUMN "public"."menu"."display" IS '是否隐藏';
COMMENT ON COLUMN "public"."menu"."tenant_id" IS '租户ID';
COMMENT ON COLUMN "public"."menu"."is_external" IS '是否外链';
COMMENT ON TABLE "public"."menu" IS '菜单表';

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO "public"."menu" VALUES ('687ae050-ae7b-483c-004d-1d8725963b8d', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-19 00:01:20.451501', NULL, NULL, '定时任务', NULL, '/system/scheduledTask', 'system/scheduledTask', 2, '', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 10, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6876ce60-0837-6d24-002e-b3b4478ba6ea', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:55:44.831206', '2025-07-15 21:56:39.208649', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '在线文档', 'antd:ApiOutlined', 'https://doc.crackerwork.cn/', '#', 2, '', NULL, 99, 't', NULL, 't');
INSERT INTO "public"."menu" VALUES ('68894682-49de-dfe4-00fd-d0ef7ff315d2', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-29 22:09:06.608163', NULL, NULL, '快速开发', 'antd:ToolOutlined', '/quickWork', NULL, 1, '', NULL, 98, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('688946ad-49de-dfe4-00fd-d0f12ed2e587', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-29 22:09:49.060841', NULL, NULL, '富文本组件', NULL, '/quickWork/rickText', 'quickWork/rickText', 2, '', '68894682-49de-dfe4-00fd-d0ef7ff315d2', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('68b4ca67-7ecb-25c0-00f2-e2386c426d7b', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-08-31 22:19:19.906074', NULL, NULL, '分配数据权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignDataScope', '3a132d1f-2026-432a-885f-bf6b10bec15c', 6, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', '3a132908-ca06-34de-164e-21c96505a036', '2024-06-15 15:49:13.507', NULL, NULL, '系统管理', 'antd:SettingOutlined', '/system', NULL, 1, 'System', NULL, 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687aafb1-3dcd-6a08-0078-e5870e6a3c24', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-18 20:33:53.818229', '2025-07-18 20:34:27.183337', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '通知管理', '', '/org/notification', 'org/notification', 2, '', '3a13a4fe-6f74-733b-a628-6125c0325481', 5, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ae0c2-ae7b-483c-004d-1d8d2615f26d', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-19 00:03:14.804374', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Add', '687ae050-ae7b-483c-004d-1d8725963b8d', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ae0d2-ae7b-483c-004d-1d8f2f556b78', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-19 00:03:30.478029', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.List', '687ae050-ae7b-483c-004d-1d8725963b8d', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ae0dd-ae7b-483c-004d-1d917c7673ab', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-19 00:03:41.928625', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Update', '687ae050-ae7b-483c-004d-1d8725963b8d', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ae0ed-ae7b-483c-004d-1d9321d51b3c', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-19 00:03:57.712663', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Delete', '687ae050-ae7b-483c-004d-1d8725963b8d', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('686a9add-aad5-2c54-003c-a0434dfa66e6', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-06 15:48:45.260116', '2025-09-10 20:58:34.823338', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '重置密码', NULL, NULL, NULL, 3, 'Sys.User.ResetPwd', '3a132d16-df35-09cb-9f50-0a83e8290575', 9, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('9a8546e3-3230-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:45:31.598', '2025-08-31 22:17:19.164998', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '分配功能权限', NULL, NULL, NULL, 3, 'Sys.Role.AssignMenu', '3a132d1f-2026-432a-885f-bf6b10bec15c', 5, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('019933b2-daf5-746f-897a-d2a3e17d74d0', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-10 20:56:23.555831', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.User.Update', '3a132d16-df35-09cb-9f50-0a83e8290575', 10, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('68758a82-b420-88a8-00fb-1d622c8e1591', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-14 22:53:54.688697', '2025-09-14 10:56:27.76522', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '新增', NULL, NULL, NULL, 3, 'Sys.Config.Add', '687586db-b420-88a8-00fb-1d3b07afeda3', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6865a9ef-4217-02ac-0070-b65a7ed7d760', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-02 21:51:43.566378', '2025-07-02 21:51:55.291719', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '访问日志', NULL, '/monitor/apiAccessLog', 'monitor/apiAccessLog', 2, '', '3a174174-857e-2328-55e6-395fcffb3774', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13a4fe-6f74-733b-a628-6125c0325481', '3a13a4f2-568e-41fe-55e7-210cc37b6d8a', '2024-07-08 22:48:47.742', NULL, NULL, '组织架构', 'antd:TeamOutlined', '/org', NULL, 1, 'Org', NULL, 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a135caa-6050-b8fa-ba75-6aaf548a7683', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:44:19.284', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.User.Add', '3a132d16-df35-09cb-9f50-0a83e8290575', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a135caa-b115-4de4-3be5-4b3cc477d8f4', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:44:39.958', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.User.List', '3a132d16-df35-09cb-9f50-0a83e8290575', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a135cab-3200-f6de-41f9-948404a81884', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:45:12.962', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.User.Delete', '3a132d16-df35-09cb-9f50-0a83e8290575', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a135cab-7acb-41cf-30c4-720eea400b2c', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:45:31.598', NULL, NULL, '分配角色', NULL, NULL, NULL, 3, 'Sys.User.AssignRole', '3a132d16-df35-09cb-9f50-0a83e8290575', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a135cab-fcbb-1f19-8416-25c218db4279', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:46:04.86', NULL, NULL, '启用/禁用', NULL, NULL, NULL, 3, 'Sys.User.SwitchEnabledStatus', '3a132d16-df35-09cb-9f50-0a83e8290575', 5, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a135cb0-3753-ae33-82fd-603c3622f661', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:50:42.004', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Role.Update', '3a132d1f-2026-432a-885f-bf6b10bec15c', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bcfc-f11b-7dce-a264-0f34b21085f1', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 14:38:03.056', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Org.PositionGroup.Add', '3a13bcf2-3701-be8e-4ec8-ad5f77536101', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bcfd-52bb-db4a-d508-eea8536c8bdc', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 14:38:28.028', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Org.PositionGroup.List', '3a13bcf2-3701-be8e-4ec8-ad5f77536101', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bcfd-90b2-02ef-4440-8062594e3d79', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 14:38:43.893', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Org.PositionGroup.Update', '3a13bcf2-3701-be8e-4ec8-ad5f77536101', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bcfd-c549-8f84-1941-1d6baccfd6b4', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 14:38:57.355', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Org.PositionGroup.Delete', '3a13bcf2-3701-be8e-4ec8-ad5f77536101', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bdb0-d210-fbaf-ce01-6d658b1b7ec9', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 17:54:31.569', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Org.Position.Add', '3a13bdaf-34ea-bf3c-c7eb-1d1cfd91d361', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bdb1-26a1-79e7-6920-b40e50de0bbe', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 17:54:53.219', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Org.Position.List', '3a13bdaf-34ea-bf3c-c7eb-1d1cfd91d361', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bdb1-742f-1ff2-87d1-b07a807c791f', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 17:55:13.072', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Org.Position.Update', '3a13bdaf-34ea-bf3c-c7eb-1d1cfd91d361', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bdb2-213b-d9fa-d067-287801312ea1', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 17:55:57.373', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Org.Position.Delete', '3a13bdaf-34ea-bf3c-c7eb-1d1cfd91d361', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13be19-204c-f634-f95f-ef8b7dcd9117', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 19:48:27.341', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Org.Dept.Add', '3a13be18-7fe2-2163-01ba-4a86ca6a7c40', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13be19-6b83-eaa2-0194-a9f17b786109', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 19:48:46.596', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Org.Dept.List', '3a13be18-7fe2-2163-01ba-4a86ca6a7c40', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13be19-a679-8375-aa20-5ab28ad85890', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 19:49:01.689', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Org.Dept.Update', '3a13be18-7fe2-2163-01ba-4a86ca6a7c40', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13be19-d4fe-0a05-3dd1-25310c7bd52a', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 19:49:13.599', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Org.Dept.Delete', '3a13be18-7fe2-2163-01ba-4a86ca6a7c40', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993c1-445d-b8d0-32e8-d0dfeff83a05', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 20:45:10.111', NULL, NULL, '注销', NULL, NULL, NULL, 3, 'Monitor.Logout', '3a174175-1893-a38e-c4a2-837cd49e79f6', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993cc-dce1-53b9-c08b-9b77b244263a', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 20:57:50.052', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.DictType.Add', '3a198c3b-bf80-dce3-f433-f9f221339227', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993ce-9de3-0234-d9b4-3c7012f7e361', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 20:59:45.015', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.DictType.List', '3a198c3b-bf80-dce3-f433-f9f221339227', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993cf-45ce-6733-402a-63161432073c', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 21:00:27.984', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.DictType.Update', '3a198c3b-bf80-dce3-f433-f9f221339227', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993d0-7b90-8d33-94c7-f9138a5711c0', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 21:01:47.281', NULL, NULL, '删除', NULL, '', NULL, 3, 'Sys.DictType.Delete', '3a198c3b-bf80-dce3-f433-f9f221339227', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993d0-db1b-1ff9-1ce2-85ca968e7e64', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 21:02:11.74', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.DictData.Add', '3a198d86-8791-5c15-2dac-dada4eeda0fd', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993d1-2120-d4d2-e8e9-51b29d3c5cd8', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 21:02:29.665', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.DictData.List', '3a198d86-8791-5c15-2dac-dada4eeda0fd', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993d2-1130-66f0-ca47-f3d73f9fb857', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 21:03:31.122', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.DictData.Update', '3a198d86-8791-5c15-2dac-dada4eeda0fd', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a1993d2-62aa-5257-9d47-0d2b1548f5f1', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-29 21:03:51.978', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.DictData.Delete', '3a198d86-8791-5c15-2dac-dada4eeda0fd', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('5c74b782-3231-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:50:42.004', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Menu.Update', '3a132d1f-e2dd-7447-ac4b-2250201a9bad', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('5c7548d7-3231-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:44:19.284', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Menu.Add', '3a132d1f-e2dd-7447-ac4b-2250201a9bad', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('5c75c0d0-3231-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:44:39.958', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Menu.List', '3a132d1f-e2dd-7447-ac4b-2250201a9bad', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('5c767046-3231-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:45:12.962', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Menu.Delete', '3a132d1f-e2dd-7447-ac4b-2250201a9bad', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('87cd2f63-3230-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:44:19.284', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Role.Add', '3a132d1f-2026-432a-885f-bf6b10bec15c', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('9a844856-3230-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:44:39.958', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Role.List', '3a132d1f-2026-432a-885f-bf6b10bec15c', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('9a84d205-3230-11ef-afb3-0242ac110003', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-06-24 21:45:12.962', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Role.Delete', '3a132d1f-2026-432a-885f-bf6b10bec15c', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687586db-b420-88a8-00fb-1d3b07afeda3', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-14 22:38:19.75711', '2025-07-14 22:39:21.580963', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '配置管理', NULL, '/system/config', 'system/config', 2, '', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 7, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a132d1f-2026-432a-885f-bf6b10bec15c', '3a132908-ca06-34de-164e-21c96505a036', '2024-06-15 16:10:04.215', NULL, NULL, '角色管理', NULL, '/system/role', 'system/role', 2, 'Sys:Role', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a132d1f-e2dd-7447-ac4b-2250201a9bad', '3a132908-ca06-34de-164e-21c96505a036', '2024-06-15 16:10:54.046', NULL, NULL, '菜单管理', NULL, '/system/menu', 'system/menu', 2, 'Sys:Menu', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bdaf-34ea-bf3c-c7eb-1d1cfd91d361', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 17:52:45.803', NULL, NULL, '职位管理', NULL, '/org/position', 'org/position', 2, 'Org:Position', '3a13a4fe-6f74-733b-a628-6125c0325481', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13be18-7fe2-2163-01ba-4a86ca6a7c40', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 19:47:46.294', NULL, NULL, '部门管理', NULL, '/org/dept', 'org/dept', 2, 'Org:Department', '3a13a4fe-6f74-733b-a628-6125c0325481', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a198c3b-bf80-dce3-f433-f9f221339227', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-28 09:41:59.313', NULL, NULL, '数据字典', NULL, '/system/dict', 'system/dictType', 2, 'Sys:Dict', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a132d16-df35-09cb-9f50-0a83e8290575', '3a132908-ca06-34de-164e-21c96505a036', '2024-06-15 16:01:03.301', '2025-06-27 22:33:01.930601', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '用户管理', '', '/system/user', 'system/user', 2, '', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6865aa1d-4217-02ac-0070-b65d05235757', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-02 21:52:29.472722', '2025-07-02 21:52:44.816966', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '异常日志', NULL, '/monitor/exceptionLog', 'monitor/exceptionLog', 2, '', '3a174174-857e-2328-55e6-395fcffb3774', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a174175-1893-a38e-c4a2-837cd49e79f6', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-01-04 11:07:31.86', '2025-07-10 22:34:55.638327', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '在线用户', NULL, '/monitor/onlineUser', 'monitor/onlineUser', 2, '', '3a174174-857e-2328-55e6-395fcffb3774', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a174174-857e-2328-55e6-395fcffb3774', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-01-04 11:06:54.207', '2025-07-10 22:39:51.771442', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '系统监控', 'antd:FundOutlined', '/monitor', NULL, 1, '', NULL, 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6876c245-dc2a-f770-00a6-b1cd03755041', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:04:05.703868', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Tenant.Add', '6876c1a4-dc2a-f770-00a6-b1c17ef49fc6', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6876c253-dc2a-f770-00a6-b1cf12c074ff', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:04:19.12469', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Tenant.List', '6876c1a4-dc2a-f770-00a6-b1c17ef49fc6', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6876c260-dc2a-f770-00a6-b1d11da64c80', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:04:32.976932', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Tenant.Update', '6876c1a4-dc2a-f770-00a6-b1c17ef49fc6', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6876c273-dc2a-f770-00a6-b1d369d976ad', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:04:51.458147', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Tenant.Delete', '6876c1a4-dc2a-f770-00a6-b1c17ef49fc6', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('6876c1a4-dc2a-f770-00a6-b1c17ef49fc6', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:01:24.861586', '2025-07-15 21:02:56.464483', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '租户管理', NULL, '/system/tenant', 'system/tenant', 2, '', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 8, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a13bcf2-3701-be8e-4ec8-ad5f77536101', '3a13bc48-e3c9-4c0b-0cc4-b6fc4e606741', '2024-07-13 14:26:20.046', '2025-07-11 00:27:56.260876', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '职位分组', '', '/org/positionGroup', 'org/positionGroup', 2, '', '3a13a4fe-6f74-733b-a628-6125c0325481', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ab06d-3dcd-6a08-0078-e59345ff0ab1', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-18 20:37:01.851781', NULL, NULL, '新增', NULL, NULL, NULL, 3, 'Sys.Notification.Add', '687aafb1-3dcd-6a08-0078-e5870e6a3c24', 1, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ab07a-3dcd-6a08-0078-e59543d0479e', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-18 20:37:14.766532', NULL, NULL, '查询', NULL, NULL, NULL, 3, 'Sys.Notification.List', '687aafb1-3dcd-6a08-0078-e5870e6a3c24', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ab088-3dcd-6a08-0078-e5976d5d9585', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-18 20:37:28.704311', NULL, NULL, '编辑', NULL, NULL, NULL, 3, 'Sys.Notification.Update', '687aafb1-3dcd-6a08-0078-e5870e6a3c24', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ab094-3dcd-6a08-0078-e599634196cf', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-18 20:37:40.555994', NULL, NULL, '删除', NULL, NULL, NULL, 3, 'Sys.Notification.Delete', '687aafb1-3dcd-6a08-0078-e5870e6a3c24', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ab0d2-3dcd-6a08-0078-e59f6c975e7a', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-18 20:38:42.951706', NULL, NULL, '我的通知', NULL, '/org/myNotification', 'org/myNotification', 2, '', '3a13a4fe-6f74-733b-a628-6125c0325481', 6, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a138b12-93b5-e723-1539-adaeb17a2ae1', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-07-03 22:00:40.118', '2025-07-02 21:53:12.229777', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '登录日志', NULL, '/system/log/login', 'system/log/loginLog', 2, '', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 10, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a138b13-fccd-7b4a-0bb5-435a9d9c4172', '3a1356b8-6f63-a393-1f8d-4ab9dc4914f4', '2024-07-03 22:02:12.559', '2025-07-02 21:53:32.960656', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '业务日志', NULL, '/system/log/business', 'system/log/businessLog', 2, '', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 11, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('687ae0fd-ae7b-483c-004d-1d956a9d485b', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-19 00:04:13.762756', NULL, NULL, '执行日志', NULL, NULL, NULL, 3, 'Sys.ScheduledTask.Log', '687ae050-ae7b-483c-004d-1d8725963b8d', 5, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('01994625-47ae-7433-b9ea-7245ee4a18cd', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-14 10:54:32.375704', NULL, NULL, '部门简单信息', NULL, NULL, NULL, 3, 'Org.Dept.GetDeptSimpleInfos', '3a13be18-7fe2-2163-01ba-4a86ca6a7c40', 5, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('68758a8f-b420-88a8-00fb-1d642276ff41', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-14 22:54:07.938236', '2025-09-14 10:56:30.597833', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '查询', NULL, NULL, NULL, 3, 'Sys.Config.List', '687586db-b420-88a8-00fb-1d3b07afeda3', 2, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('68758aa2-b420-88a8-00fb-1d6600787728', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-14 22:54:26.364848', '2025-09-14 10:56:34.232428', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '编辑', NULL, NULL, NULL, 3, 'Sys.Config.Update', '687586db-b420-88a8-00fb-1d3b07afeda3', 3, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('68758aae-b420-88a8-00fb-1d686cd73d1e', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-14 22:54:38.605153', '2025-09-14 10:56:37.026917', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '删除', NULL, NULL, NULL, 3, 'Sys.Config.Delete', '687586db-b420-88a8-00fb-1d3b07afeda3', 4, 't', NULL, 'f');
INSERT INTO "public"."menu" VALUES ('3a198d86-8791-5c15-2dac-dada4eeda0fd', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-04-28 15:43:17.394', '2025-09-14 11:02:58.269047', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '字典项', NULL, '/system/dictItem/:dictType', 'system/dictData', 2, NULL, '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', 5, 'f', NULL, 'f');

-- ----------------------------
-- Table structure for notification
-- ----------------------------
DROP TABLE IF EXISTS "public"."notification";
CREATE TABLE "public"."notification" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "title" varchar(128) COLLATE "pg_catalog"."default" NOT NULL,
  "content" varchar(512) COLLATE "pg_catalog"."default",
  "user_id" uuid NOT NULL,
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
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "is_deleted" bool NOT NULL,
  "deleter_id" uuid,
  "deletion_time" timestamp(6),
  "code" varchar(32) COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "level" int4 NOT NULL,
  "status" int4 NOT NULL,
  "description" varchar(512) COLLATE "pg_catalog"."default",
  "group_id" uuid,
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
INSERT INTO "public"."position" VALUES ('01992e66-a6f5-72ed-95fb-af8159cc3bf1', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 20:15:03.454315', NULL, NULL, 't', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 20:22:06.037352', '990021021', '测试工程师', 3, 1, '1221221', NULL, NULL);
INSERT INTO "public"."position" VALUES ('686a9bac-aad5-2c54-003c-a0450445a76f', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-06 15:52:12.404982', '2025-09-09 23:56:23.899262', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, '001', '前端开发工程师', 1, 1, NULL, '685f2cd3-7ef6-c114-0022-88b50a861368', NULL);

-- ----------------------------
-- Table structure for position_group
-- ----------------------------
DROP TABLE IF EXISTS "public"."position_group";
CREATE TABLE "public"."position_group" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "group_name" varchar(64) COLLATE "pg_catalog"."default" NOT NULL,
  "remark" varchar(512) COLLATE "pg_catalog"."default",
  "parent_id" uuid,
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
INSERT INTO "public"."position_group" VALUES ('685f2cd3-7ef6-c114-0022-88b50a861368', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-06-27 23:44:19.081278', '2025-06-28 15:57:31.895397', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '软件', '软件研发岗位', NULL, '685f2cd3-7ef6-c114-0022-88b50a861368', 1, NULL, 1);

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS "public"."role";
CREATE TABLE "public"."role" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "is_deleted" bool NOT NULL,
  "deleter_id" uuid,
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
INSERT INTO "public"."role" VALUES ('685ed8f3-7da6-d870-0090-fdd663a31f65', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-06-27 17:46:27.287301', NULL, NULL, 't', NULL, NULL, 'test', 'test', 0, NULL, 'f');
INSERT INTO "public"."role" VALUES ('3a172369-28a4-e37e-b78a-8c3eaec17359', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2024-12-29 15:05:53.128', NULL, NULL, 'f', NULL, NULL, '系统管理员', '系统默认创建，拥有最高权限', 0, NULL, 't');
INSERT INTO "public"."role" VALUES ('687047ef-1649-9660-0098-4182062f682c', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-10 23:08:31.94067', '2025-09-14 11:04:21.533056', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, '数据权限', '测试菜单权限', 4, NULL, 't');

-- ----------------------------
-- Table structure for role_dept
-- ----------------------------
DROP TABLE IF EXISTS "public"."role_dept";
CREATE TABLE "public"."role_dept" (
  "id" uuid NOT NULL,
  "role_id" uuid NOT NULL,
  "dept_id" uuid NOT NULL,
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
  "id" uuid NOT NULL,
  "menu_id" uuid NOT NULL,
  "role_id" uuid NOT NULL,
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
INSERT INTO "public"."role_menu" VALUES ('0199461e-991c-7fb9-8c36-eb42e4ea280c', '3a13bcf2-3701-be8e-4ec8-ad5f77536101', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7060-bb6b-4e6049446ec7', '687ab06d-3dcd-6a08-0078-e59345ff0ab1', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7084-ad79-1e50f5e5ecad', '3a13bcfc-f11b-7dce-a264-0f34b21085f1', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7126-a189-ea4d36ddf10f', '3a13bdb0-d210-fbaf-ce01-6d658b1b7ec9', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7179-afc6-0f8f5440c82f', '686a9a99-aad5-2c54-003c-a0410adc5b1a', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-71b3-bc57-86a281f57978', '3a132d16-df35-09cb-9f50-0a83e8290575', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-733d-bc26-2abdea0c429f', '3a135caa-b115-4de4-3be5-4b3cc477d8f4', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-73de-bee6-c2c7f290f1d0', '3a13bdb2-213b-d9fa-d067-287801312ea1', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-746f-afc6-1677ea65dc5a', '3a13bcfd-52bb-db4a-d508-eea8536c8bdc', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7493-ac23-2abb08ba9eb2', '3a13be4c-d335-53c6-15e1-b2a16d9f94e4', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7494-b6b6-9244d5a802c3', '3a13be4b-8355-c505-5dd3-15fbe89e2639', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-74d4-89c5-7de4906fe8b2', '3a13bcfd-90b2-02ef-4440-8062594e3d79', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-74f5-8a23-9992fdb173f5', '3a13bcfd-c549-8f84-1941-1d6baccfd6b4', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-750d-b80d-1933188320b3', '3a13be18-7fe2-2163-01ba-4a86ca6a7c40', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7600-a361-63596128af03', '3a13be19-d4fe-0a05-3dd1-25310c7bd52a', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-77ff-9b6f-d288ded5996c', '687ab094-3dcd-6a08-0078-e599634196cf', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7846-908c-bb6899ad0dbe', '3a13be19-204c-f634-f95f-ef8b7dcd9117', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7851-b155-81fde95c3769', '3a13bdb1-26a1-79e7-6920-b40e50de0bbe', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7886-bd1f-81a87059f3e2', '3a13be19-a679-8375-aa20-5ab28ad85890', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-789e-a96b-856fb03b096e', '687ab07a-3dcd-6a08-0078-e59543d0479e', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7a6d-9fdb-f3df17402952', '3a13be4c-2e0e-af01-4432-a4892ff622ab', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7c6b-8874-4800a059cbc8', '3a13be4b-3b01-f611-e5da-8b3a3dc41cfc', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7ca6-9414-77b68e1236c2', '3a13bdb1-742f-1ff2-87d1-b07a807c791f', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7cdf-a48d-f2062890d9a8', '3a13be19-6b83-eaa2-0194-a9f17b786109', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7d10-895d-f953d17b3647', '3a13be49-5f19-8ebd-5dda-1cf390060a09', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7d67-85da-65c26115c7ef', '3a132d0c-0a70-b4c5-1ffd-1088c23ae02a', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7ddf-917e-b46814c43554', '687ab088-3dcd-6a08-0078-e5976d5d9585', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7e25-ae97-c4d9a26b39db', '3a13a4fe-6f74-733b-a628-6125c0325481', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7e27-853a-e89fb2239beb', '687aafb1-3dcd-6a08-0078-e5870e6a3c24', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."role_menu" VALUES ('0199461e-9922-7f85-8047-f41a9534f852', '3a13bdaf-34ea-bf3c-c7eb-1d1cfd91d361', '687047ef-1649-9660-0098-4182062f682c', NULL);

-- ----------------------------
-- Table structure for scheduled_tasks
-- ----------------------------
DROP TABLE IF EXISTS "public"."scheduled_tasks";
CREATE TABLE "public"."scheduled_tasks" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "task_key" varchar(100) COLLATE "pg_catalog"."default" NOT NULL,
  "task_description" varchar(512) COLLATE "pg_catalog"."default",
  "cron_expression" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "is_active" bool NOT NULL
)
;

-- ----------------------------
-- Records of scheduled_tasks
-- ----------------------------
INSERT INTO "public"."scheduled_tasks" VALUES ('01992e98-634b-7a88-8fa3-c6906d74d47c', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-09 21:09:22.920617', NULL, NULL, 'NotificationJob', '站内信', '0 0/1 * * * ?', 't');

-- ----------------------------
-- Table structure for task_execution_logs
-- ----------------------------
DROP TABLE IF EXISTS "public"."task_execution_logs";
CREATE TABLE "public"."task_execution_logs" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "task_key" varchar(100) COLLATE "pg_catalog"."default",
  "status" int4 NOT NULL,
  "result" text COLLATE "pg_catalog"."default",
  "node_id" varchar(128) COLLATE "pg_catalog"."default",
  "execution_time" timestamp(6) NOT NULL,
  "cost" int4 NOT NULL
)
;

-- ----------------------------
-- Records of task_execution_logs
-- ----------------------------

-- ----------------------------
-- Table structure for tenant
-- ----------------------------
DROP TABLE IF EXISTS "public"."tenant";
CREATE TABLE "public"."tenant" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
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
INSERT INTO "public"."tenant" VALUES ('6876c329-dc2a-f770-00a6-b1e306495609', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:07:53.183795', '2025-07-15 21:12:18.009625', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '重庆小卖部', 'cq_market', NULL, 'cq.market.crackerwork.cn');
INSERT INTO "public"."tenant" VALUES ('6876c45d-e5f2-35dc-00d1-48ee38eafd16', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-07-15 21:13:01.684058', NULL, NULL, '湖南小卖部', 'hn_market', NULL, 'hn.market.crackerwork.cn');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS "public"."user";
CREATE TABLE "public"."user" (
  "id" uuid NOT NULL,
  "creator_id" uuid,
  "creation_time" timestamp(6) NOT NULL,
  "last_modification_time" timestamp(6),
  "last_modifier_id" uuid,
  "is_deleted" bool NOT NULL,
  "deleter_id" uuid,
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
  "dept_id" uuid,
  "post_id" uuid
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
INSERT INTO "public"."user" VALUES ('5dcacf4e-9c8e-4ec5-8f46-d600d4660af0', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-14 10:48:23.739324', NULL, NULL, 'f', NULL, NULL, 'cracker', '079ce55e9611f57ae3e79e3581daa5ac', 'nxL4hldjtPXu2y6kSmjsQw==', 'avatar/male.png', 'cracker', 1, 't', NULL, NULL, 'f0915491-8aae-4be6-8eb1-7ddfc9b7c9a2', NULL);
INSERT INTO "public"."user" VALUES ('975bc910-8f41-4d12-9297-81fcb1b7bbec', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-10 20:31:32.231556', '2025-09-14 10:49:57.364795', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, 'test', '78d44c72c56afd42a662755ac5706d61', 'iO6QDCj8w9iDHJf97wxTvw==', 'avatar/male.png', 'test3232323', 1, 't', NULL, '18211114444', 'f0915491-8aae-4be6-8eb1-7ddfc9b7c9a2', '686a9bac-aad5-2c54-003c-a0450445a76f');
INSERT INTO "public"."user" VALUES ('3a172a37-55d5-ee9b-dc92-e07386eadc7c', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2024-12-30 22:48:48.458', '2025-09-14 18:41:56.108293', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', 'f', NULL, NULL, 'admin', 'a2fa8ec90f15197c7a4e6e00525b198a', 'vHQZvbz+ng+B4NrSAEYl6g==', 'file/myavatar.jpg', '风汐', 2, 't', 'cq_market', '18273403759', 'e60121f5-d83f-4a72-aa74-8436e73f674f', NULL);
INSERT INTO "public"."user" VALUES ('8530014e-65fb-41ae-854e-1e09ceaff644', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-10 21:13:06.186342', NULL, NULL, 't', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '2025-09-10 21:23:03.792467', 'tom12', '57d3c078d6e3d14b48472f8a247acbdd', '32zAlXGRsZ4NQ0LYbCzMZw==', 'avatar/male.png', '32112', 1, 't', NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for user_role
-- ----------------------------
DROP TABLE IF EXISTS "public"."user_role";
CREATE TABLE "public"."user_role" (
  "id" uuid NOT NULL,
  "user_id" uuid NOT NULL,
  "role_id" uuid NOT NULL,
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
INSERT INTO "public"."user_role" VALUES ('683cc8f7-ba08-3990-009e-23375861fdc5', '683cc901-ba08-3990-009e-23392649a8a4', '683cc8d0-ba08-3990-009e-233317cce095', '');
INSERT INTO "public"."user_role" VALUES ('685f2a6f-5e26-60bc-00f7-9ead1bc87fd4', '3a172a37-55d5-ee9b-dc92-e07386eadc7c', '3a172369-28a4-e37e-b78a-8c3eaec17359', NULL);
INSERT INTO "public"."user_role" VALUES ('68705965-5ecb-c374-0046-1a565d226a73', '00de38c4-17bd-415f-bf1c-2e0873eb177e', '687047ef-1649-9660-0098-4182062f682c', NULL);
INSERT INTO "public"."user_role" VALUES ('019933ba-fe4a-7149-9e6c-6cb0056b63d0', '975bc910-8f41-4d12-9297-81fcb1b7bbec', '687047ef-1649-9660-0098-4182062f682c', NULL);

-- ----------------------------
-- Primary Key structure for table api_access_log
-- ----------------------------
ALTER TABLE "public"."api_access_log" ADD CONSTRAINT "public_api_access_log_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Primary Key structure for table config
-- ----------------------------
ALTER TABLE "public"."config" ADD CONSTRAINT "public_sys_config_pkey" PRIMARY KEY ("id");

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
-- Primary Key structure for table employee
-- ----------------------------
ALTER TABLE "public"."employee" ADD CONSTRAINT "public_org_employee_pkey" PRIMARY KEY ("id");

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
-- Indexes structure for table scheduled_tasks
-- ----------------------------
CREATE UNIQUE INDEX "uk_task_key" ON "public"."scheduled_tasks" USING btree (
  "task_key" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table scheduled_tasks
-- ----------------------------
ALTER TABLE "public"."scheduled_tasks" ADD CONSTRAINT "public_scheduled_tasks_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Indexes structure for table task_execution_logs
-- ----------------------------
CREATE INDEX "idx_execution_time" ON "public"."task_execution_logs" USING btree (
  "execution_time" "pg_catalog"."timestamp_ops" ASC NULLS LAST
);
CREATE INDEX "idx_task_key" ON "public"."task_execution_logs" USING btree (
  "task_key" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table task_execution_logs
-- ----------------------------
ALTER TABLE "public"."task_execution_logs" ADD CONSTRAINT "public_task_execution_logs_pkey" PRIMARY KEY ("id");

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
