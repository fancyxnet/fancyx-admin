-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: fancyx-admin
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
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250921033330_InitCreated','9.0.1');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `api_access_log`
--

DROP TABLE IF EXISTS `api_access_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `api_access_log` (
  `id` char(36) NOT NULL,
  `creator_id` char(36) DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `path` text,
  `method` varchar(16) DEFAULT NULL,
  `ip` varchar(32) DEFAULT NULL,
  `request_time` datetime(6) NOT NULL,
  `response_time` datetime(6) DEFAULT NULL,
  `duration` bigint DEFAULT NULL,
  `user_id` char(36) DEFAULT NULL,
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
-- Table structure for table `cap.published`
--

DROP TABLE IF EXISTS `cap.published`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cap.published` (
  `Id` bigint NOT NULL,
  `Version` varchar(20) DEFAULT NULL,
  `Name` varchar(200) NOT NULL,
  `Content` longtext,
  `Retries` int DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime DEFAULT NULL,
  `StatusName` varchar(40) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Version_ExpiresAt_StatusName` (`Version`,`ExpiresAt`,`StatusName`),
  KEY `IX_ExpiresAt_StatusName` (`ExpiresAt`,`StatusName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cap.published`
--

LOCK TABLES `cap.published` WRITE;
/*!40000 ALTER TABLE `cap.published` DISABLE KEYS */;
INSERT INTO `cap.published` VALUES (5199077414245773313,'v1','exception_log_event','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077414245773313\",\"cap-corr-id\":\"5199077414245773313\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"exception_log_event\",\"cap-msg-type\":\"ExceptionLogMessage\",\"cap-senttime\":\"09/21/2025 11:39:32\"},\"Value\":{\"ExceptionType\":\"Refit.ApiException\",\"Message\":\"Response status code does not indicate success: 404 (Not Found).\",\"StackTrace\":\"   at Refit.RequestBuilderImplementation.\\u003C\\u003Ec__DisplayClass15_0\\u00602.\\u003C\\u003CBuildCancellableTaskFuncForMethod\\u003Eb__0\\u003Ed.MoveNext() in c:\\\\temp\\\\releaser\\\\refit\\\\Refit\\\\RequestBuilderImplementation.cs:line 384\\r\\n--- End of stack trace from previous location ---\\r\\n   at Refit.Implementation.Generated.FancyxErpRemoteITestApi.global::Fancyx.Erp.Remote.ITestApi.Hello() in E:\\\\fancyx-admin\\\\fancyx-server\\\\src\\\\Fancyx.Erp\\\\obj\\\\Debug\\\\net9.0\\\\InterfaceStubGeneratorV2\\\\Refit.Generator.InterfaceStubGeneratorV2\\\\ITestApi.g.cs:line 46\\r\\n   at Fancyx.Erp.Controllers.RemoteDemoController.HelloAsync() in E:\\\\fancyx-admin\\\\fancyx-server\\\\src\\\\Fancyx.Erp\\\\Controllers\\\\RemoteDemoController.cs:line 22\\r\\n   at lambda_method533(Closure, Object)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeActionMethodAsync\\u003Eg__Awaited|12_0(ControllerActionInvoker invoker, ValueTask\\u00601 actionResultValueTask)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeNextActionFilterAsync\\u003Eg__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State\\u0026 next, Scope\\u0026 scope, Object\\u0026 state, Boolean\\u0026 isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeInnerFilterAsync\\u003Eg__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.\\u003CInvokeNextExceptionFilterAsync\\u003Eg__Awaited|26_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\",\"InnerException\":null,\"RequestPath\":\"/api/RemoteDemo\",\"RequestMethod\":\"GET\",\"TraceId\":\"30754ae8949e3d635361bade983d2f10\",\"Ip\":\"::1\",\"UserAgent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"UserId\":null,\"UserName\":null,\"TenantId\":null}}',0,'2025-09-21 11:39:32','2025-09-22 11:39:32','Succeeded'),(5199077417901936641,'v1','login_log_event','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077417901936641\",\"cap-corr-id\":\"5199077417901936641\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"login_log_event\",\"cap-msg-type\":\"LoginLog\",\"cap-senttime\":\"09/21/2025 11:53:31\"},\"Value\":{\"UserName\":\"admin\",\"Ip\":\"::1\",\"Address\":null,\"Browser\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"OperationMsg\":\"\\u767B\\u5F55\\u6210\\u529F\",\"IsSuccess\":true,\"SessionId\":\"4490165206487928832\",\"TenantId\":null,\"CreatorId\":null,\"CreationTime\":\"0001-01-01T00:00:00\",\"Id\":\"00000000-0000-0000-0000-000000000000\"}}',0,'2025-09-21 11:53:32','2025-09-22 11:53:32','Succeeded'),(5199077417901936643,'v1','login_log_event','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077417901936643\",\"cap-corr-id\":\"5199077417901936643\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"login_log_event\",\"cap-msg-type\":\"LoginLog\",\"cap-senttime\":\"09/21/2025 11:53:34\"},\"Value\":{\"UserName\":\"admin\",\"Ip\":\"::1\",\"Address\":null,\"Browser\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"OperationMsg\":\"\\u767B\\u5F55\\u6210\\u529F\",\"IsSuccess\":true,\"SessionId\":\"4490165227455254528\",\"TenantId\":null,\"CreatorId\":null,\"CreationTime\":\"0001-01-01T00:00:00\",\"Id\":\"00000000-0000-0000-0000-000000000000\"}}',0,'2025-09-21 11:53:35','2025-09-22 11:53:35','Succeeded'),(5199077417901936645,'v1','login_log_event','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077417901936645\",\"cap-corr-id\":\"5199077417901936645\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"login_log_event\",\"cap-msg-type\":\"LoginLog\",\"cap-senttime\":\"09/21/2025 11:53:36\"},\"Value\":{\"UserName\":\"admin\",\"Ip\":\"::1\",\"Address\":null,\"Browser\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"OperationMsg\":\"\\u767B\\u5F55\\u6210\\u529F\",\"IsSuccess\":true,\"SessionId\":\"4490165234552016896\",\"TenantId\":null,\"CreatorId\":null,\"CreationTime\":\"0001-01-01T00:00:00\",\"Id\":\"00000000-0000-0000-0000-000000000000\"}}',0,'2025-09-21 11:53:36','2025-09-22 11:53:36','Succeeded');
/*!40000 ALTER TABLE `cap.published` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cap.received`
--

DROP TABLE IF EXISTS `cap.received`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cap.received` (
  `Id` bigint NOT NULL,
  `Version` varchar(20) DEFAULT NULL,
  `Name` varchar(400) NOT NULL,
  `Group` varchar(200) DEFAULT NULL,
  `Content` longtext,
  `Retries` int DEFAULT NULL,
  `Added` datetime NOT NULL,
  `ExpiresAt` datetime DEFAULT NULL,
  `StatusName` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Version_ExpiresAt_StatusName` (`Version`,`ExpiresAt`,`StatusName`),
  KEY `IX_ExpiresAt_StatusName` (`ExpiresAt`,`StatusName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cap.received`
--

LOCK TABLES `cap.received` WRITE;
/*!40000 ALTER TABLE `cap.received` DISABLE KEYS */;
INSERT INTO `cap.received` VALUES (5199077414220226561,'v1','exception_log_event','cap.queue.fancyx.admin.v1','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077414245773313\",\"cap-corr-id\":\"5199077414245773313\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"exception_log_event\",\"cap-msg-type\":\"ExceptionLogMessage\",\"cap-senttime\":\"09/21/2025 11:39:32\",\"cap-msg-group\":\"cap.queue.fancyx.admin.v1\",\"cap-exec-instance-id\":\"DESKTOP-N11U69U\",\"cap-exception\":\"SubscriberNotFoundException--\\u003EMessage (Name:exception_log_event,Group:cap.queue.fancyx.admin.v1) can not be found subscriber.\\r\\n see: https://github.com/dotnetcore/CAP/issues/63\"},\"Value\":{\"ExceptionType\":\"Refit.ApiException\",\"Message\":\"Response status code does not indicate success: 404 (Not Found).\",\"StackTrace\":\"   at Refit.RequestBuilderImplementation.\\u003C\\u003Ec__DisplayClass15_0\\u00602.\\u003C\\u003CBuildCancellableTaskFuncForMethod\\u003Eb__0\\u003Ed.MoveNext() in c:\\\\temp\\\\releaser\\\\refit\\\\Refit\\\\RequestBuilderImplementation.cs:line 384\\r\\n--- End of stack trace from previous location ---\\r\\n   at Refit.Implementation.Generated.FancyxErpRemoteITestApi.global::Fancyx.Erp.Remote.ITestApi.Hello() in E:\\\\fancyx-admin\\\\fancyx-server\\\\src\\\\Fancyx.Erp\\\\obj\\\\Debug\\\\net9.0\\\\InterfaceStubGeneratorV2\\\\Refit.Generator.InterfaceStubGeneratorV2\\\\ITestApi.g.cs:line 46\\r\\n   at Fancyx.Erp.Controllers.RemoteDemoController.HelloAsync() in E:\\\\fancyx-admin\\\\fancyx-server\\\\src\\\\Fancyx.Erp\\\\Controllers\\\\RemoteDemoController.cs:line 22\\r\\n   at lambda_method533(Closure, Object)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeActionMethodAsync\\u003Eg__Awaited|12_0(ControllerActionInvoker invoker, ValueTask\\u00601 actionResultValueTask)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeNextActionFilterAsync\\u003Eg__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State\\u0026 next, Scope\\u0026 scope, Object\\u0026 state, Boolean\\u0026 isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeInnerFilterAsync\\u003Eg__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.\\u003CInvokeNextExceptionFilterAsync\\u003Eg__Awaited|26_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\",\"InnerException\":null,\"RequestPath\":\"/api/RemoteDemo\",\"RequestMethod\":\"GET\",\"TraceId\":\"30754ae8949e3d635361bade983d2f10\",\"Ip\":\"::1\",\"UserAgent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"UserId\":null,\"UserName\":null,\"TenantId\":null}}',51,'2025-09-21 11:39:33','2025-10-06 11:39:33','Failed'),(5199077414245773314,'v1','exception_log_event','cap.queue.fancyx.erp.v1','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077414245773313\",\"cap-corr-id\":\"5199077414245773313\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"exception_log_event\",\"cap-msg-type\":\"ExceptionLogMessage\",\"cap-senttime\":\"09/21/2025 11:39:32\",\"cap-msg-group\":\"cap.queue.fancyx.erp.v1\",\"cap-exec-instance-id\":\"DESKTOP-N11U69U\",\"cap-exception\":\"SubscriberNotFoundException--\\u003EMessage (Name:exception_log_event,Group:cap.queue.fancyx.erp.v1) can not be found subscriber.\\r\\n see: https://github.com/dotnetcore/CAP/issues/63\"},\"Value\":{\"ExceptionType\":\"Refit.ApiException\",\"Message\":\"Response status code does not indicate success: 404 (Not Found).\",\"StackTrace\":\"   at Refit.RequestBuilderImplementation.\\u003C\\u003Ec__DisplayClass15_0\\u00602.\\u003C\\u003CBuildCancellableTaskFuncForMethod\\u003Eb__0\\u003Ed.MoveNext() in c:\\\\temp\\\\releaser\\\\refit\\\\Refit\\\\RequestBuilderImplementation.cs:line 384\\r\\n--- End of stack trace from previous location ---\\r\\n   at Refit.Implementation.Generated.FancyxErpRemoteITestApi.global::Fancyx.Erp.Remote.ITestApi.Hello() in E:\\\\fancyx-admin\\\\fancyx-server\\\\src\\\\Fancyx.Erp\\\\obj\\\\Debug\\\\net9.0\\\\InterfaceStubGeneratorV2\\\\Refit.Generator.InterfaceStubGeneratorV2\\\\ITestApi.g.cs:line 46\\r\\n   at Fancyx.Erp.Controllers.RemoteDemoController.HelloAsync() in E:\\\\fancyx-admin\\\\fancyx-server\\\\src\\\\Fancyx.Erp\\\\Controllers\\\\RemoteDemoController.cs:line 22\\r\\n   at lambda_method533(Closure, Object)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeActionMethodAsync\\u003Eg__Awaited|12_0(ControllerActionInvoker invoker, ValueTask\\u00601 actionResultValueTask)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeNextActionFilterAsync\\u003Eg__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State\\u0026 next, Scope\\u0026 scope, Object\\u0026 state, Boolean\\u0026 isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.\\u003CInvokeInnerFilterAsync\\u003Eg__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.\\u003CInvokeNextExceptionFilterAsync\\u003Eg__Awaited|26_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\",\"InnerException\":null,\"RequestPath\":\"/api/RemoteDemo\",\"RequestMethod\":\"GET\",\"TraceId\":\"30754ae8949e3d635361bade983d2f10\",\"Ip\":\"::1\",\"UserAgent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"UserId\":null,\"UserName\":null,\"TenantId\":null}}',51,'2025-09-21 11:39:33','2025-10-06 11:39:33','Failed'),(5199077417901936642,'v1','login_log_event','cap.queue.fancyx.admin.v1','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077417901936641\",\"cap-corr-id\":\"5199077417901936641\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"login_log_event\",\"cap-msg-type\":\"LoginLog\",\"cap-senttime\":\"09/21/2025 11:53:31\",\"cap-msg-group\":\"cap.queue.fancyx.admin.v1\",\"cap-exec-instance-id\":\"DESKTOP-N11U69U\"},\"Value\":{\"UserName\":\"admin\",\"Ip\":\"::1\",\"Address\":null,\"Browser\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"OperationMsg\":\"\\u767B\\u5F55\\u6210\\u529F\",\"IsSuccess\":true,\"SessionId\":\"4490165206487928832\",\"TenantId\":null,\"CreatorId\":null,\"CreationTime\":\"2025-09-21T11:53:32.863333+08:00\",\"Id\":\"08ddf8c2-6f4a-414a-86c9-7fd0fe2323e5\"}}',0,'2025-09-21 11:53:32','2025-09-22 11:53:33','Succeeded'),(5199077417901936644,'v1','login_log_event','cap.queue.fancyx.admin.v1','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077417901936643\",\"cap-corr-id\":\"5199077417901936643\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"login_log_event\",\"cap-msg-type\":\"LoginLog\",\"cap-senttime\":\"09/21/2025 11:53:34\",\"cap-msg-group\":\"cap.queue.fancyx.admin.v1\",\"cap-exec-instance-id\":\"DESKTOP-N11U69U\"},\"Value\":{\"UserName\":\"admin\",\"Ip\":\"::1\",\"Address\":null,\"Browser\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"OperationMsg\":\"\\u767B\\u5F55\\u6210\\u529F\",\"IsSuccess\":true,\"SessionId\":\"4490165227455254528\",\"TenantId\":null,\"CreatorId\":null,\"CreationTime\":\"2025-09-21T11:53:35.1456306+08:00\",\"Id\":\"08ddf8c2-7083-4637-8e7d-807ea345925e\"}}',0,'2025-09-21 11:53:35','2025-09-22 11:53:35','Succeeded'),(5199077417901936646,'v1','login_log_event','cap.queue.fancyx.admin.v1','{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"5199077417901936645\",\"cap-corr-id\":\"5199077417901936645\",\"cap-corr-seq\":\"0\",\"cap-msg-name\":\"login_log_event\",\"cap-msg-type\":\"LoginLog\",\"cap-senttime\":\"09/21/2025 11:53:36\",\"cap-msg-group\":\"cap.queue.fancyx.admin.v1\",\"cap-exec-instance-id\":\"DESKTOP-N11U69U\"},\"Value\":{\"UserName\":\"admin\",\"Ip\":\"::1\",\"Address\":null,\"Browser\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36\",\"OperationMsg\":\"\\u767B\\u5F55\\u6210\\u529F\",\"IsSuccess\":true,\"SessionId\":\"4490165234552016896\",\"TenantId\":null,\"CreatorId\":null,\"CreationTime\":\"2025-09-21T11:53:37.181539+08:00\",\"Id\":\"08ddf8c2-71bb-429f-8840-3ba8b3ec5282\"}}',0,'2025-09-21 11:53:37','2025-09-22 11:53:37','Succeeded');
/*!40000 ALTER TABLE `cap.received` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `config`
--

DROP TABLE IF EXISTS `config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `config` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `key` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `group_key` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `dept`
--

DROP TABLE IF EXISTS `dept`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dept` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sort` int NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `status` int NOT NULL,
  `curator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `email` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `phone` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `parent_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tree_level` int NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `dict_data`
--

DROP TABLE IF EXISTS `dict_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dict_data` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `label` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `sort` int NOT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `dict_type`
--

DROP TABLE IF EXISTS `dict_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dict_type` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `dict_type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `exception_log`
--

DROP TABLE IF EXISTS `exception_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `exception_log` (
  `id` char(36) NOT NULL,
  `creator_id` char(36) DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `exception_type` varchar(64) DEFAULT NULL,
  `message` text,
  `stack_trace` text,
  `inner_exception` text,
  `request_path` text,
  `request_method` varchar(16) DEFAULT NULL,
  `user_id` char(36) DEFAULT NULL,
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
-- Table structure for table `log_record`
--

DROP TABLE IF EXISTS `log_record`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `log_record` (
  `id` char(36) NOT NULL,
  `creator_id` char(36) DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `type` varchar(64) NOT NULL,
  `sub_type` varchar(512) NOT NULL,
  `biz_no` varchar(64) NOT NULL,
  `content` text NOT NULL,
  `browser` varchar(512) DEFAULT NULL,
  `ip` varchar(32) DEFAULT NULL,
  `trace_id` varchar(64) DEFAULT NULL,
  `tenant_id` varchar(18) DEFAULT NULL,
  `user_id` char(36) DEFAULT NULL,
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
-- Table structure for table `login_log`
--

DROP TABLE IF EXISTS `login_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `login_log` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ip` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `address` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `browser` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `operation_msg` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `is_success` tinyint(1) NOT NULL,
  `session_id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `login_log`
--

LOCK TABLES `login_log` WRITE;
/*!40000 ALTER TABLE `login_log` DISABLE KEYS */;
INSERT INTO `login_log` VALUES ('08ddf8c2-6f4a-414a-86c9-7fd0fe2323e5','admin','::1',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36','登录成功',1,'4490165206487928832',NULL,NULL,'2025-09-21 11:53:32.863333'),('08ddf8c2-7083-4637-8e7d-807ea345925e','admin','::1',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36','登录成功',1,'4490165227455254528',NULL,NULL,'2025-09-21 11:53:35.145630'),('08ddf8c2-71bb-429f-8840-3ba8b3ec5282','admin','::1',NULL,'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36','登录成功',1,'4490165234552016896',NULL,NULL,'2025-09-21 11:53:37.181539');
/*!40000 ALTER TABLE `login_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu`
--

DROP TABLE IF EXISTS `menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `title` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `icon` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `path` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `component` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `menu_type` int NOT NULL,
  `permission` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `parent_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `sort` int NOT NULL,
  `display` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `is_external` tinyint(1) NOT NULL,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `notification`
--

DROP TABLE IF EXISTS `notification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notification` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `title` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `content` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `is_readed` tinyint(1) NOT NULL,
  `readed_time` datetime(6) DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `position`
--

DROP TABLE IF EXISTS `position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `position` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `code` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `level` int NOT NULL,
  `status` int NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `group_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `position_group`
--

DROP TABLE IF EXISTS `position_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `position_group` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `group_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `parent_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `tree_path` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tree_level` int NOT NULL,
  `sort` int NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `role_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `is_enabled` tinyint(1) NOT NULL,
  `dept_power_type` int NOT NULL,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `deletion_time` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES ('3a172369-28a4-e37e-b78a-8c3eaec17359','系统管理员','系统默认创建，拥有最高权限',NULL,1,0,NULL,'2025-09-14 11:04:21.533056',NULL,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_dept`
--

DROP TABLE IF EXISTS `role_dept`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_dept` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `role_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `dept_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`),
  KEY `IX_role_dept_role_id` (`role_id`),
  CONSTRAINT `FK_role_dept_role_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE CASCADE
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
-- Table structure for table `role_menu`
--

DROP TABLE IF EXISTS `role_menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_menu` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `menu_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `role_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`),
  KEY `IX_role_menu_menu_id` (`menu_id`),
  KEY `IX_role_menu_role_id` (`role_id`),
  CONSTRAINT `FK_role_menu_menu_menu_id` FOREIGN KEY (`menu_id`) REFERENCES `menu` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_role_menu_role_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE CASCADE
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
-- Table structure for table `tenant`
--

DROP TABLE IF EXISTS `tenant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenant` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `tenant_id` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
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
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `user_name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password_salt` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `avatar` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `nick_name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `sex` int NOT NULL,
  `is_enabled` tinyint(1) NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `phone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `dept_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `post_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creator_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `creation_time` datetime(6) NOT NULL,
  `last_modification_time` datetime(6) DEFAULT NULL,
  `last_modifier_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `deleter_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `deletion_time` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('3a172a37-55d5-ee9b-dc92-e07386eadc7c','admin','a2fa8ec90f15197c7a4e6e00525b198a','vHQZvbz+ng+B4NrSAEYl6g==','file/myavatar.jpg','风汐',2,1,NULL,'18211114444',NULL,NULL,NULL,'2025-09-10 21:13:06.186342',NULL,NULL,0,NULL,NULL);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_role`
--

DROP TABLE IF EXISTS `user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_role` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `role_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `tenant_id` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id`),
  KEY `IX_user_role_role_id` (`role_id`),
  KEY `IX_user_role_user_id` (`user_id`),
  CONSTRAINT `FK_user_role_role_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_user_role_user_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_role`
--

LOCK TABLES `user_role` WRITE;
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` VALUES ('83cc8f7-ba08-3990-009e-23375861fdc5','3a172a37-55d5-ee9b-dc92-e07386eadc7c','3a172369-28a4-e37e-b78a-8c3eaec17359',NULL);
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'fancyx-admin'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-21 12:14:20
