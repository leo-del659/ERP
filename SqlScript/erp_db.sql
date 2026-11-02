/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 80046 (8.0.46)
 Source Host           : localhost:3306
 Source Schema         : erp_db

 Target Server Type    : MySQL
 Target Server Version : 80046 (8.0.46)
 File Encoding         : 65001

 Date: 02/11/2026 14:58:52
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for fin_cost
-- ----------------------------
DROP TABLE IF EXISTS `fin_cost`;
CREATE TABLE `fin_cost`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `cost_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '成本单号',
  `pr_order_id` bigint NULL DEFAULT NULL COMMENT '生产订单ID',
  `material_id` bigint NOT NULL COMMENT '成品物料ID',
  `material_cost` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '材料成本',
  `labor_cost` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '人工成本',
  `overhead_cost` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '制造费用',
  `total_cost` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '总成本',
  `unit_cost` decimal(18, 6) NULL DEFAULT 0.000000 COMMENT '单位成本',
  `calc_date` date NULL DEFAULT NULL COMMENT '核算日期',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_cost_no`(`cost_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '成本计算单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_cost
-- ----------------------------
INSERT INTO `fin_cost` VALUES (1, 'COST202610001', 1, 1, 52000.00, 8000.00, 4000.00, 64000.00, 128.000000, '2026-10-20', '2026-10-31 11:53:32');
INSERT INTO `fin_cost` VALUES (2, 'COST202610002', 2, 2, 31000.00, 5000.00, 2500.00, 38500.00, 77.000000, '2026-10-22', '2026-10-31 11:53:32');
INSERT INTO `fin_cost` VALUES (3, 'COST202611020001', 1, 1, 113250.00, 1.00, 0.00, 113251.00, 226.502000, '2026-11-02', '2026-11-02 09:42:37');
INSERT INTO `fin_cost` VALUES (4, 'COST202611020002', 1, 1, 113250.00, 0.00, 0.00, 113250.00, 226.500000, '2026-11-02', '2026-11-02 09:42:49');

-- ----------------------------
-- Table structure for fin_payable
-- ----------------------------
DROP TABLE IF EXISTS `fin_payable`;
CREATE TABLE `fin_payable`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `pay_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '应付单号',
  `supplier_id` bigint NOT NULL COMMENT '供应商ID',
  `source_order_id` bigint NULL DEFAULT NULL COMMENT '来源采购订单ID',
  `amount` decimal(18, 2) NOT NULL COMMENT '应付金额',
  `paid_amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '已付金额',
  `due_date` date NULL DEFAULT NULL COMMENT '到期日',
  `status` tinyint NULL DEFAULT 0 COMMENT '0未付/1部分付/2已付',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_pay_no`(`pay_no` ASC) USING BTREE,
  INDEX `idx_pay_supplier`(`supplier_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '应付单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_payable
-- ----------------------------
INSERT INTO `fin_payable` VALUES (1, 'PAY202610001', 1, 1, 113000.00, 50080.00, '2026-11-15', 1, '2026-10-31 11:53:32');
INSERT INTO `fin_payable` VALUES (2, 'PAY202610002', 2, 2, 56500.00, 0.00, '2026-11-20', 0, '2026-10-31 11:53:32');
INSERT INTO `fin_payable` VALUES (3, 'PAY202610003', 3, 3, 22600.00, 22600.00, '2026-10-30', 2, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for fin_payment
-- ----------------------------
DROP TABLE IF EXISTS `fin_payment`;
CREATE TABLE `fin_payment`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `payment_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '付款单号',
  `supplier_id` bigint NOT NULL COMMENT '供应商ID',
  `payable_id` bigint NULL DEFAULT NULL COMMENT '核销应付单ID',
  `amount` decimal(18, 2) NOT NULL COMMENT '付款金额',
  `payment_date` date NOT NULL COMMENT '付款日期',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_payment_no`(`payment_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '付款单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_payment
-- ----------------------------
INSERT INTO `fin_payment` VALUES (1, 'PAYF202610001', 1, 1, 50000.00, '2026-10-18', '2026-10-31 11:53:32', 0);
INSERT INTO `fin_payment` VALUES (2, 'PAYF202610002', 3, 3, 22600.00, '2026-10-25', '2026-10-31 11:53:32', 0);
INSERT INTO `fin_payment` VALUES (3, 'PM202611020001', 1, 1, 80.00, '2026-11-02', '2026-11-02 11:23:45', 0);

-- ----------------------------
-- Table structure for fin_receipt
-- ----------------------------
DROP TABLE IF EXISTS `fin_receipt`;
CREATE TABLE `fin_receipt`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `receipt_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '收款单号',
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `receivable_id` bigint NULL DEFAULT NULL COMMENT '核销应收单ID',
  `amount` decimal(18, 2) NOT NULL COMMENT '收款金额',
  `receipt_date` date NOT NULL COMMENT '收款日期',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_receipt_no`(`receipt_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '收款单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_receipt
-- ----------------------------
INSERT INTO `fin_receipt` VALUES (1, 'RECV202610001', 1, 1, 80000.00, '2026-10-15', '2026-10-31 11:53:32', 1);
INSERT INTO `fin_receipt` VALUES (2, 'RECV202610002', 2, 2, 45000.00, '2026-10-20', '2026-10-31 11:53:32', 0);
INSERT INTO `fin_receipt` VALUES (3, 'RC202611020001', 1, 1, 100.00, '2026-11-02', '2026-11-02 11:23:45', 0);

-- ----------------------------
-- Table structure for fin_receivable
-- ----------------------------
DROP TABLE IF EXISTS `fin_receivable`;
CREATE TABLE `fin_receivable`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `recv_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '应收单号',
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `source_order_id` bigint NULL DEFAULT NULL COMMENT '来源销售订单ID',
  `amount` decimal(18, 2) NOT NULL COMMENT '应收金额',
  `received_amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '已收金额',
  `due_date` date NULL DEFAULT NULL COMMENT '到期日',
  `status` tinyint NULL DEFAULT 0 COMMENT '0未收/1部分收/2已收',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_recv_no`(`recv_no` ASC) USING BTREE,
  INDEX `idx_recv_customer`(`customer_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '应收单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_receivable
-- ----------------------------
INSERT INTO `fin_receivable` VALUES (1, 'AR202610001', 1, 1, 120000.00, 80100.00, '2026-11-10', 1, '2026-10-31 11:53:32');
INSERT INTO `fin_receivable` VALUES (2, 'AR202610002', 2, 2, 45000.00, 45000.00, '2026-10-31', 2, '2026-10-31 11:53:32');
INSERT INTO `fin_receivable` VALUES (3, 'AR202610003', 3, 3, 30000.00, 0.00, '2026-11-30', 0, '2026-10-31 11:53:32');
INSERT INTO `fin_receivable` VALUES (4, 'AR202611010001', 1, 4, 10000.00, 0.00, '2026-12-01', 0, '2026-11-01 11:10:30');

-- ----------------------------
-- Table structure for fin_voucher
-- ----------------------------
DROP TABLE IF EXISTS `fin_voucher`;
CREATE TABLE `fin_voucher`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `voucher_no` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '凭证号',
  `voucher_date` date NOT NULL COMMENT '凭证日期',
  `source_module` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '来源模块：PO/SO/INV/PR',
  `source_id` bigint NULL DEFAULT NULL COMMENT '来源单据ID',
  `total_debit` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '借方合计',
  `total_credit` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '贷方合计',
  `status` tinyint NULL DEFAULT 0 COMMENT '0未审核/1已审核',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_voucher_no`(`voucher_no` ASC) USING BTREE,
  INDEX `idx_voucher_date`(`voucher_date` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '财务凭证主表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_voucher
-- ----------------------------
INSERT INTO `fin_voucher` VALUES (1, 'VCH202610001', '2026-10-10', 'PO', 1, 100000.00, 100000.00, 1, '2026-10-31 11:53:32', 0);
INSERT INTO `fin_voucher` VALUES (2, 'VCH202610002', '2026-10-15', 'SO', 1, 120000.00, 120000.00, 1, '2026-10-31 11:53:32', 0);
INSERT INTO `fin_voucher` VALUES (3, 'VCH202610003', '2026-10-20', 'INV', 1, 52000.00, 52000.00, 2, '2026-10-31 11:53:32', 0);
INSERT INTO `fin_voucher` VALUES (4, 'HR-Pay-2026-10', '2026-10-31', 'HR', 1, 8780.00, 8780.00, 3, '2026-10-31 21:56:01', 0);
INSERT INTO `fin_voucher` VALUES (5, 'V202611020001', '2026-11-02', '手工', NULL, 10000.00, 10000.00, 0, '2026-11-02 08:55:12', 0);
INSERT INTO `fin_voucher` VALUES (6, 'V202611020002', '2026-11-02', '手工', NULL, 100.00, 100.00, 0, '2026-11-02 11:23:45', 1);

-- ----------------------------
-- Table structure for fin_voucher_entry
-- ----------------------------
DROP TABLE IF EXISTS `fin_voucher_entry`;
CREATE TABLE `fin_voucher_entry`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `voucher_id` bigint NOT NULL COMMENT '凭证ID',
  `entry_no` int NOT NULL COMMENT '分录行号',
  `account_code` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '会计科目编码',
  `direction` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '方向：DR借/CR贷',
  `amount` decimal(18, 2) NOT NULL COMMENT '金额',
  `memo` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '摘要',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_entry_voucher`(`voucher_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '凭证分录' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fin_voucher_entry
-- ----------------------------
INSERT INTO `fin_voucher_entry` VALUES (1, 1, 1, '1403', 'DR', 100000.00, '采购原材料入库');
INSERT INTO `fin_voucher_entry` VALUES (2, 1, 2, '2202', 'CR', 100000.00, '应付供应商货款');
INSERT INTO `fin_voucher_entry` VALUES (3, 2, 1, '1122', 'DR', 120000.00, '销售应收款');
INSERT INTO `fin_voucher_entry` VALUES (4, 2, 2, '6001', 'CR', 120000.00, '销售收入');
INSERT INTO `fin_voucher_entry` VALUES (5, 3, 1, '5001', 'DR', 52000.00, '结转生产成本');
INSERT INTO `fin_voucher_entry` VALUES (6, 3, 2, '1405', 'CR', 52000.00, '库存商品结转');
INSERT INTO `fin_voucher_entry` VALUES (7, 4, 1, '5602.01', 'DR', 8780.00, '应付职工薪酬-工资(赵敏 2026-10)');
INSERT INTO `fin_voucher_entry` VALUES (8, 4, 2, '2211', 'CR', 8780.00, '应付职工薪酬-工资');
INSERT INTO `fin_voucher_entry` VALUES (9, 5, 1, '', 'DR', 10000.00, '');
INSERT INTO `fin_voucher_entry` VALUES (10, 5, 2, '', 'CR', 10000.00, '');
INSERT INTO `fin_voucher_entry` VALUES (11, 6, 1, '1001', 'DR', 100.00, 't');
INSERT INTO `fin_voucher_entry` VALUES (12, 6, 2, '1002', 'CR', 100.00, 't');

-- ----------------------------
-- Table structure for hr_attendance
-- ----------------------------
DROP TABLE IF EXISTS `hr_attendance`;
CREATE TABLE `hr_attendance`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `schedule_id` bigint NULL DEFAULT NULL COMMENT '排班ID',
  `work_date` date NULL DEFAULT NULL,
  `punch_in` datetime NULL DEFAULT NULL COMMENT '上班打卡',
  `punch_out` datetime NULL DEFAULT NULL COMMENT '下班打卡',
  `late_min` int NULL DEFAULT 0 COMMENT '迟到分钟',
  `early_min` int NULL DEFAULT 0 COMMENT '早退分钟',
  `absent` tinyint NULL DEFAULT 0 COMMENT '0否/1是',
  `overtime_min` int NULL DEFAULT 0 COMMENT '加班分钟',
  `status` tinyint NULL DEFAULT 0 COMMENT '0正常/1迟到/2早退/3缺勤/4加班',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_att_emp`(`emp_id` ASC, `work_date` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '打卡/考勤明细（结果传递至薪资核算）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_attendance
-- ----------------------------
INSERT INTO `hr_attendance` VALUES (1, 7, 2, '2026-10-01', '2026-10-01 08:12:00', '2026-10-01 16:05:00', 12, 0, 0, 0, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_attendance` VALUES (2, 11, 3, '2026-10-01', '2026-10-01 07:58:00', '2026-10-01 17:30:00', 0, 0, 0, 90, 4, '2026-10-31 21:56:01');
INSERT INTO `hr_attendance` VALUES (3, 7, 5, '2026-10-02', '2026-10-02 08:00:00', '2026-10-02 18:30:00', 0, 0, 0, 150, 4, '2026-10-31 21:56:01');
INSERT INTO `hr_attendance` VALUES (4, 11, 6, '2026-10-02', '2026-10-02 08:30:00', '2026-10-02 16:00:00', 30, 0, 0, 0, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_candidate
-- ----------------------------
DROP TABLE IF EXISTS `hr_candidate`;
CREATE TABLE `hr_candidate`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '候选人姓名',
  `gender` tinyint NULL DEFAULT 1 COMMENT '0女/1男',
  `phone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `email` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `education` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `source` tinyint NULL DEFAULT 1 COMMENT '1内推/2招聘网/3校招/4猎头',
  `resume_url` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `pos_id` bigint NULL DEFAULT NULL COMMENT '应聘岗位',
  `status` tinyint NULL DEFAULT 0 COMMENT '0初筛/1面试中/2已offer/3已入职/4淘汰/5人才库',
  `rating` int NULL DEFAULT 0 COMMENT '评分(0~100)',
  `talent_pool` tinyint NULL DEFAULT 0 COMMENT '0否/1是（进入人才库）',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_cand_status`(`status` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '候选人/人才库' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_candidate
-- ----------------------------
INSERT INTO `hr_candidate` VALUES (1, '钱进', 1, '13900000021', 'qianjin@mail.com', '大专', 2, '/resume/qianjin.pdf', 6, 3, 86, 0, '2026-10-31 21:56:01');
INSERT INTO `hr_candidate` VALUES (2, '冯雪', 0, '13900000022', 'fengxue@mail.com', '本科', 1, '/resume/fengxue.pdf', 2, 1, 78, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_candidate` VALUES (3, '卫东', 1, '13900000023', 'weidong@mail.com', '硕士', 3, '/resume/weidong.pdf', 9, 0, 90, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_candidate` VALUES (4, '蒋雯', 0, '13900000024', 'jiangwen@mail.com', '本科', 2, '/resume/jiangwen.pdf', 6, 5, 72, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_contract
-- ----------------------------
DROP TABLE IF EXISTS `hr_contract`;
CREATE TABLE `hr_contract`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `contract_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '合同编号',
  `contract_type` tinyint NULL DEFAULT 1 COMMENT '1固定期限/2无固定/3实习/4劳务',
  `start_date` date NULL DEFAULT NULL,
  `end_date` date NULL DEFAULT NULL,
  `sign_date` date NULL DEFAULT NULL COMMENT '签订日期',
  `probation_months` int NULL DEFAULT 0 COMMENT '试用期(月)',
  `salary` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '合同约定薪资',
  `status` tinyint NULL DEFAULT 1 COMMENT '0草稿/1生效/2续签/3变更/4解除/5到期',
  `remark` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_contract_no`(`contract_no` ASC) USING BTREE,
  INDEX `idx_contract_emp`(`emp_id` ASC) USING BTREE,
  INDEX `idx_contract_end`(`end_date` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 12 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '劳动合同（含到期预警：按 end_date 查询）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_contract
-- ----------------------------
INSERT INTO `hr_contract` VALUES (1, 1, 'HT-2015-0001', 1, '2015-04-01', '2026-03-31', '2015-03-25', 3, 45000.00, 5, '无固定期限已续签', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (2, 2, 'HT-2017-0002', 1, '2017-06-15', '2026-12-31', '2017-06-10', 2, 18000.00, 1, '次年到期，需续签预警', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (3, 3, 'HT-2019-0003', 1, '2019-03-01', '2027-02-28', '2019-02-25', 2, 11000.00, 1, '', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (4, 4, 'HT-2016-0004', 1, '2016-08-01', '2026-07-31', '2016-07-25', 2, 15000.00, 2, '已续签', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (5, 5, 'HT-2014-0005', 2, '2014-02-01', '2029-01-31', '2014-01-20', 3, 30000.00, 1, '', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (6, 6, 'HT-2018-0006', 1, '2018-07-01', '2026-06-30', '2018-06-25', 1, 12000.00, 1, '', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (7, 7, 'HT-2026-0007', 1, '2026-10-08', '2027-10-07', '2026-10-05', 3, 7000.00, 1, '试用期合同', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (8, 8, 'HT-2020-0008', 1, '2020-05-20', '2026-05-19', '2020-05-15', 2, 13000.00, 3, '', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (9, 9, 'HT-2017-0009', 1, '2017-09-10', '2027-09-09', '2017-09-05', 2, 14000.00, 1, '', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (10, 10, 'HT-2019-0010', 1, '2019-11-01', '2026-10-31', '2019-10-25', 2, 20000.00, 1, '当月到期，预警', '2026-10-31 21:56:00');
INSERT INTO `hr_contract` VALUES (11, 11, 'HT-2026-0011', 1, '2026-10-20', '2027-10-19', '2026-10-18', 3, 7500.00, 1, '', '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_department
-- ----------------------------
DROP TABLE IF EXISTS `hr_department`;
CREATE TABLE `hr_department`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `dept_no` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门编码',
  `dept_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门名称',
  `parent_id` bigint NULL DEFAULT 0 COMMENT '上级部门ID，0=顶级',
  `dept_level` int NULL DEFAULT 1 COMMENT '层级深度',
  `manager_id` bigint NULL DEFAULT NULL COMMENT '负责人(员工ID)',
  `phone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系电话',
  `address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '办公地点',
  `order_no` int NULL DEFAULT 0 COMMENT '排序',
  `status` tinyint NULL DEFAULT 1 COMMENT '0停用/1启用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `manager_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_dept_no`(`dept_no` ASC) USING BTREE,
  INDEX `idx_dept_parent`(`parent_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '部门（组织层级）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_department
-- ----------------------------
INSERT INTO `hr_department` VALUES (1, 'D00', '集团总部', 0, 1, 1, '021-66880000', '上海总部', 1, 1, '2026-10-31 21:56:00', '张伟');
INSERT INTO `hr_department` VALUES (2, 'D01', '人力资源部', 1, 2, 2, '021-66880001', '上海总部 3F', 2, 1, '2026-10-31 21:56:00', '李娜');
INSERT INTO `hr_department` VALUES (3, 'D02', '财务部', 1, 2, 4, '021-66880002', '上海总部 2F', 3, 1, '2026-10-31 21:56:00', '刘强');
INSERT INTO `hr_department` VALUES (4, 'D03', '生产中心', 1, 2, 5, '021-66880003', '松江工厂', 4, 1, '2026-10-31 21:56:00', '陈刚');
INSERT INTO `hr_department` VALUES (5, 'D04', '制造一部', 3, 3, 6, '021-66880004', '松江工厂 A栋', 5, 1, '2026-10-31 21:56:00', '杨洋');
INSERT INTO `hr_department` VALUES (6, 'D05', '销售部', 1, 2, 8, '021-66880005', '上海总部 1F', 6, 1, '2026-10-31 21:56:00', '孙莉');
INSERT INTO `hr_department` VALUES (7, 'D06', '采购部', 1, 2, 9, '021-66880006', '上海总部 2F', 7, 1, '2026-10-31 21:56:00', '周杰');
INSERT INTO `hr_department` VALUES (8, 'D07', '研发中心', 1, 2, 10, '021-66880007', '上海总部 4F', 8, 1, '2026-10-31 21:56:00', '吴婷');

-- ----------------------------
-- Table structure for hr_employee
-- ----------------------------
DROP TABLE IF EXISTS `hr_employee`;
CREATE TABLE `hr_employee`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_no` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工号',
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '姓名',
  `gender` tinyint NULL DEFAULT 1 COMMENT '0女/1男',
  `birthday` date NULL DEFAULT NULL,
  `id_card` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '身份证',
  `nation` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '民族',
  `marital` tinyint NULL DEFAULT 0 COMMENT '0未婚/1已婚',
  `education` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '学历',
  `phone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `email` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `dept_id` bigint NULL DEFAULT NULL COMMENT '所属部门',
  `pos_id` bigint NULL DEFAULT NULL COMMENT '所属岗位',
  `emp_status` tinyint NULL DEFAULT 0 COMMENT '0试用/1正式/2休假/3离职/4借调',
  `hire_date` date NULL DEFAULT NULL COMMENT '入职日期',
  `regular_date` date NULL DEFAULT NULL COMMENT '转正日期',
  `leave_date` date NULL DEFAULT NULL COMMENT '离职日期',
  `contract_count` int NULL DEFAULT 0 COMMENT '合同份数',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_emp_no`(`emp_no` ASC) USING BTREE,
  INDEX `idx_emp_dept`(`dept_id` ASC) USING BTREE,
  INDEX `idx_emp_status`(`emp_status` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 12 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '员工主档' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_employee
-- ----------------------------
INSERT INTO `hr_employee` VALUES (1, 'E0001', '张伟', 1, '1982-03-12', '310101198203120011', '汉', 1, '硕士', '13800000001', 'zhangwei@erp.com', '上海浦东', 1, 1, 1, '2015-04-01', '2015-10-01', NULL, 2, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (2, 'E0002', '李娜', 0, '1988-07-22', '310101198807220022', '汉', 1, '本科', '13800000002', 'lina@erp.com', '上海徐汇', 2, 2, 1, '2017-06-15', '2017-12-15', NULL, 2, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (3, 'E0003', '王芳', 0, '1992-11-03', '310101199211030033', '汉', 0, '本科', '13800000003', 'wangfang@erp.com', '上海闵行', 2, 3, 1, '2019-03-01', '2019-09-01', NULL, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (4, 'E0004', '刘强', 1, '1985-09-18', '310101198509180044', '汉', 1, '本科', '13800000004', 'liuqiang@erp.com', '上海静安', 3, 3, 1, '2016-08-01', '2017-02-01', NULL, 2, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (5, 'E0005', '陈刚', 1, '1980-01-25', '310101198001250055', '汉', 1, '本科', '13800000005', 'chengang@erp.com', '上海松江', 4, 4, 1, '2014-02-01', '2014-08-01', NULL, 2, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (6, 'E0006', '杨洋', 1, '1990-05-14', '310101199005140066', '汉', 1, '大专', '13800000006', 'yangyang@erp.com', '上海松江', 5, 5, 1, '2018-07-01', '2019-01-01', NULL, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (7, 'E0007', '赵敏', 0, '1998-02-09', '310101199802090077', '汉', 0, '大专', '13800000007', 'zhaomin@erp.com', '上海松江', 5, 6, 0, '2026-10-08', NULL, NULL, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (8, 'E0008', '孙莉', 0, '1993-12-30', '310101199312300088', '汉', 0, '本科', '13800000008', 'sunli@erp.com', '上海黄浦', 6, 7, 1, '2020-05-20', '2020-11-20', NULL, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (9, 'E0009', '周杰', 1, '1989-04-16', '310101198904160099', '汉', 1, '本科', '13800000009', 'zhoujie@erp.com', '上海宝山', 7, 8, 1, '2017-09-10', '2018-03-10', NULL, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (10, 'E0010', '吴婷', 0, '1991-08-21', '310101199108210010', '汉', 1, '硕士', '13800000010', 'wuting@erp.com', '上海长宁', 8, 9, 1, '2019-11-01', '2020-05-01', NULL, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_employee` VALUES (11, 'E0011', '郑浩', 1, '1996-06-28', '310101199606280011', '汉', 0, '大专', '13800000011', 'zhenghao@erp.com', '上海松江', 5, 6, 1, '2026-10-20', '2026-04-20', NULL, 1, '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_employee_change
-- ----------------------------
DROP TABLE IF EXISTS `hr_employee_change`;
CREATE TABLE `hr_employee_change`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `change_type` tinyint NOT NULL COMMENT '1入职/2转正/3调动/4晋升/5降职/6离职/7复职',
  `old_dept_id` bigint NULL DEFAULT NULL,
  `new_dept_id` bigint NULL DEFAULT NULL,
  `old_pos_id` bigint NULL DEFAULT NULL,
  `new_pos_id` bigint NULL DEFAULT NULL,
  `old_status` tinyint NULL DEFAULT NULL,
  `new_status` tinyint NULL DEFAULT NULL,
  `effect_date` date NULL DEFAULT NULL COMMENT '生效日期',
  `reason` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `approval_id` bigint NULL DEFAULT NULL COMMENT '关联 sys_approval.id（HR_EMP）',
  `operator` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '操作人',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_chg_emp`(`emp_id` ASC) USING BTREE,
  INDEX `idx_chg_type`(`change_type` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '人事异动轨迹（全周期）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_employee_change
-- ----------------------------
INSERT INTO `hr_employee_change` VALUES (1, 7, 1, NULL, 5, NULL, 6, NULL, 0, '2026-10-08', '校园招聘入职', NULL, '李娜', '2026-10-31 21:56:00');
INSERT INTO `hr_employee_change` VALUES (2, 11, 1, NULL, 5, NULL, 6, NULL, 1, '2026-10-20', '社会招聘入职', 6, '李娜', '2026-10-31 21:56:00');
INSERT INTO `hr_employee_change` VALUES (3, 1, 2, NULL, 1, NULL, 1, NULL, 1, '2015-10-01', '试用期满转正', NULL, '李娜', '2026-10-31 21:56:00');
INSERT INTO `hr_employee_change` VALUES (4, 7, 2, NULL, 5, NULL, 6, NULL, 1, '2026-10-08', '试用期转正', NULL, '李娜', '2026-10-31 21:56:00');
INSERT INTO `hr_employee_change` VALUES (5, 8, 3, NULL, 6, NULL, 7, NULL, 1, '2022-01-01', '部门调动：原客服部→销售部', NULL, 'admin', '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_employee_salary
-- ----------------------------
DROP TABLE IF EXISTS `hr_employee_salary`;
CREATE TABLE `hr_employee_salary`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `base_salary` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '核定基本工资',
  `eff_date` date NULL DEFAULT NULL COMMENT '生效日期',
  `status` tinyint NULL DEFAULT 1 COMMENT '0历史/1生效',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_es_emp`(`emp_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '员工薪资档案(定薪)' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_employee_salary
-- ----------------------------
INSERT INTO `hr_employee_salary` VALUES (1, 7, 7000.00, '2026-10-08', 1, '2026-10-31 21:56:01');
INSERT INTO `hr_employee_salary` VALUES (2, 8, 13000.00, '2020-05-20', 1, '2026-10-31 21:56:01');
INSERT INTO `hr_employee_salary` VALUES (3, 11, 7500.00, '2026-10-20', 1, '2026-10-31 21:56:01');
INSERT INTO `hr_employee_salary` VALUES (4, 2, 18000.00, '2017-06-15', 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_interview
-- ----------------------------
DROP TABLE IF EXISTS `hr_interview`;
CREATE TABLE `hr_interview`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `candidate_id` bigint NOT NULL COMMENT '候选人ID',
  `round` int NULL DEFAULT 1 COMMENT '面试轮次',
  `interview_date` date NULL DEFAULT NULL,
  `interviewers` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '面试官',
  `score` decimal(6, 1) NULL DEFAULT 0.0 COMMENT '综合评分',
  `result` tinyint NULL DEFAULT 1 COMMENT '1通过/2未过',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_iv_cand`(`candidate_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '面试记录' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_interview
-- ----------------------------
INSERT INTO `hr_interview` VALUES (1, 1, 1, '2026-10-12', '杨洋,李娜', 86.0, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_interview` VALUES (2, 1, 2, '2026-10-16', '陈刚,李娜', 88.0, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_interview` VALUES (3, 3, 1, '2026-10-14', '吴婷,李娜', 90.0, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_leave
-- ----------------------------
DROP TABLE IF EXISTS `hr_leave`;
CREATE TABLE `hr_leave`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `leave_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '请假单号',
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `leave_type` tinyint NULL DEFAULT 1 COMMENT '1年假/2事假/3病假/4调休/5婚假/6产假',
  `start_date` date NULL DEFAULT NULL,
  `end_date` date NULL DEFAULT NULL,
  `days` decimal(6, 1) NULL DEFAULT 0.0 COMMENT '请假天数',
  `reason` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `status` tinyint NULL DEFAULT 1 COMMENT '0草稿/1审批中/2通过/3驳回',
  `approval_id` bigint NULL DEFAULT NULL COMMENT '关联 sys_approval.id（HR_LEAVE）',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_leave_no`(`leave_no` ASC) USING BTREE,
  INDEX `idx_leave_emp`(`emp_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '请假单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_leave
-- ----------------------------
INSERT INTO `hr_leave` VALUES (1, 'LV-2026-0001', 7, 1, '2026-10-15', '2026-10-16', 2.0, '年假旅行', 2, 4, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_offer
-- ----------------------------
DROP TABLE IF EXISTS `hr_offer`;
CREATE TABLE `hr_offer`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `candidate_id` bigint NOT NULL COMMENT '候选人ID',
  `offer_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '录用单号',
  `salary` decimal(18, 2) NULL DEFAULT 0.00 COMMENT 'offer 薪资',
  `eff_date` date NULL DEFAULT NULL COMMENT '期望入职日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待确认/1接受/2拒绝',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_offer_no`(`offer_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '录用offer' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_offer
-- ----------------------------
INSERT INTO `hr_offer` VALUES (1, 1, 'OF-2026-0001', 7200.00, '2026-10-20', 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_overtime
-- ----------------------------
DROP TABLE IF EXISTS `hr_overtime`;
CREATE TABLE `hr_overtime`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `ot_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '加班单号',
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `ot_date` date NULL DEFAULT NULL COMMENT '加班日期',
  `hours` decimal(6, 1) NULL DEFAULT 0.0 COMMENT '加班小时',
  `reason` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `status` tinyint NULL DEFAULT 1 COMMENT '0草稿/1审批中/2通过/3驳回',
  `approval_id` bigint NULL DEFAULT NULL COMMENT '关联 sys_approval.id（HR_OVERTIME）',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_ot_no`(`ot_no` ASC) USING BTREE,
  INDEX `idx_ot_emp`(`emp_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '加班单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_overtime
-- ----------------------------
INSERT INTO `hr_overtime` VALUES (1, 'OT-2026-0001', 11, '2026-10-01', 1.5, '节前赶工', 2, 5, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_perf_detail
-- ----------------------------
DROP TABLE IF EXISTS `hr_perf_detail`;
CREATE TABLE `hr_perf_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `result_id` bigint NOT NULL COMMENT '考核结果ID',
  `ind_id` bigint NULL DEFAULT NULL COMMENT '指标ID',
  `ind_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `score` decimal(6, 2) NULL DEFAULT 0.00 COMMENT '指标得分',
  `weight` decimal(6, 2) NULL DEFAULT 0.00 COMMENT '权重%',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_pd_result`(`result_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '考核明细（按指标）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_perf_detail
-- ----------------------------
INSERT INTO `hr_perf_detail` VALUES (1, 1, 1, '销售额达成率', 95.00, 40.00, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_detail` VALUES (2, 1, 2, '订单交付准时率', 90.00, 30.00, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_detail` VALUES (3, 1, 3, '专业技能', 90.00, 20.00, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_detail` VALUES (4, 1, 4, '团队协作', 95.00, 10.00, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_detail` VALUES (5, 2, 1, '销售额达成率', 88.00, 40.00, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_detail` VALUES (6, 2, 3, '专业技能', 90.00, 20.00, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_detail` VALUES (7, 2, 4, '团队协作', 92.00, 10.00, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_perf_indicator
-- ----------------------------
DROP TABLE IF EXISTS `hr_perf_indicator`;
CREATE TABLE `hr_perf_indicator`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `ind_no` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '指标编码',
  `ind_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '指标名称',
  `category` tinyint NULL DEFAULT 1 COMMENT '1业绩/2能力/3态度',
  `weight` decimal(6, 2) NULL DEFAULT 0.00 COMMENT '权重%',
  `target` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '目标值',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_ind_no`(`ind_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '考核指标库' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_perf_indicator
-- ----------------------------
INSERT INTO `hr_perf_indicator` VALUES (1, 'K01', '销售额达成率', 1, 40.00, '>=100%', '2026-10-31 21:56:01');
INSERT INTO `hr_perf_indicator` VALUES (2, 'K02', '订单交付准时率', 1, 30.00, '>=95%', '2026-10-31 21:56:01');
INSERT INTO `hr_perf_indicator` VALUES (3, 'K03', '专业技能', 2, 20.00, '胜任岗位', '2026-10-31 21:56:01');
INSERT INTO `hr_perf_indicator` VALUES (4, 'K04', '团队协作', 3, 10.00, '良好', '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_perf_plan
-- ----------------------------
DROP TABLE IF EXISTS `hr_perf_plan`;
CREATE TABLE `hr_perf_plan`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `plan_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '考核计划单号',
  `plan_name` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '计划名称',
  `period` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '考核期间(YYYY-Qx / YYYY-MM)',
  `start_date` date NULL DEFAULT NULL,
  `end_date` date NULL DEFAULT NULL,
  `status` tinyint NULL DEFAULT 0 COMMENT '0草稿/1进行中/2已完成',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_pplan_no`(`plan_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '考核计划' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_perf_plan
-- ----------------------------
INSERT INTO `hr_perf_plan` VALUES (1, 'PF-2026-Q4', '2026年第四季度考核', '2026-Q4', '2026-10-01', '2026-12-31', 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_perf_result
-- ----------------------------
DROP TABLE IF EXISTS `hr_perf_result`;
CREATE TABLE `hr_perf_result`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `plan_id` bigint NOT NULL COMMENT '考核计划ID',
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `score` decimal(6, 2) NULL DEFAULT 0.00 COMMENT '综合得分',
  `level` tinyint NULL DEFAULT 1 COMMENT '1优秀/2良好/3合格/4待改进',
  `conclusion` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '考核结论',
  `apply_salary` tinyint NULL DEFAULT 0 COMMENT '结果是否应用于薪资(0否/1是)',
  `apply_promote` tinyint NULL DEFAULT 0 COMMENT '结果是否应用于晋升(0否/1是)',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_pr_emp`(`emp_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '考核结果（应用至薪资/晋升）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_perf_result
-- ----------------------------
INSERT INTO `hr_perf_result` VALUES (1, 1, 8, 92.50, 1, '超额完成销售目标，建议加薪', 1, 0, '2026-10-31 21:56:01');
INSERT INTO `hr_perf_result` VALUES (2, 1, 2, 88.00, 2, '人效提升明显', 1, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_position
-- ----------------------------
DROP TABLE IF EXISTS `hr_position`;
CREATE TABLE `hr_position`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `pos_no` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '岗位编码',
  `pos_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '岗位名称',
  `dept_id` bigint NULL DEFAULT NULL COMMENT '归属部门',
  `parent_id` bigint NULL DEFAULT 0 COMMENT '上级岗位ID，0=顶级',
  `pos_level` int NULL DEFAULT 1 COMMENT '职级（1~20）',
  `job_family` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '职族/序列：管理/专业/操作',
  `responsibility` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '岗位职责',
  `status` tinyint NULL DEFAULT 1 COMMENT '0停用/1启用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_pos_no`(`pos_no` ASC) USING BTREE,
  INDEX `idx_pos_dept`(`dept_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '岗位体系' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_position
-- ----------------------------
INSERT INTO `hr_position` VALUES (1, 'P00', '总经理', 1, 0, 20, '管理', '企业全面经营', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (2, 'P01', 'HR经理', 2, 0, 14, '管理', '人力资源体系搭建', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (3, 'P02', 'HR专员', 2, 0, 8, '专业', '招聘/薪酬/员工关系', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (4, 'P03', '会计', 3, 0, 10, '专业', '总账/税务', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (5, 'P04', '生产总监', 4, 0, 18, '管理', '生产运营统筹', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (6, 'P05', '车间主任', 5, 0, 12, '管理', '车间排产与现场管理', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (7, 'P06', '操作工', 5, 0, 4, '操作', '产线作业/报工', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (8, 'P07', '销售代表', 6, 0, 8, '专业', '客户开发与订单跟进', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (9, 'P08', '采购专员', 7, 0, 8, '专业', '供应商管理与采购执行', 1, '2026-10-31 21:56:00');
INSERT INTO `hr_position` VALUES (10, 'P09', '研发工程师', 8, 0, 12, '专业', '产品设计与开发', 1, '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_position_quota
-- ----------------------------
DROP TABLE IF EXISTS `hr_position_quota`;
CREATE TABLE `hr_position_quota`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `dept_id` bigint NOT NULL COMMENT '部门ID',
  `pos_id` bigint NOT NULL COMMENT '岗位ID',
  `plan_count` int NULL DEFAULT 0 COMMENT '编制数',
  `actual_count` int NULL DEFAULT 0 COMMENT '在岗数',
  `frozen_count` int NULL DEFAULT 0 COMMENT '冻结/预留数',
  `remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_quota_dp`(`dept_id` ASC, `pos_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '岗位编制控制' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_position_quota
-- ----------------------------
INSERT INTO `hr_position_quota` VALUES (1, 2, 2, 1, 1, 0, 'HR经理满编', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (2, 2, 3, 3, 2, 1, 'HR专员缺1', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (3, 4, 4, 1, 1, 0, '', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (4, 5, 5, 2, 1, 0, '车间主任缺1', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (5, 5, 6, 40, 12, 3, '操作工大量缺编，需招聘', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (6, 6, 7, 6, 1, 0, '', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (7, 7, 8, 3, 1, 0, '', '2026-10-31 21:56:00');
INSERT INTO `hr_position_quota` VALUES (8, 8, 9, 8, 1, 1, '', '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_recruit_plan
-- ----------------------------
DROP TABLE IF EXISTS `hr_recruit_plan`;
CREATE TABLE `hr_recruit_plan`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `plan_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '招聘计划(人力需求)单号',
  `dept_id` bigint NOT NULL COMMENT '需求部门',
  `pos_id` bigint NOT NULL COMMENT '需求岗位',
  `need_count` int NULL DEFAULT 1 COMMENT '需求人数',
  `reason` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '需求理由',
  `status` tinyint NULL DEFAULT 1 COMMENT '0草稿/1审批中/2通过/3关闭',
  `approval_id` bigint NULL DEFAULT NULL COMMENT '关联 sys_approval.id（HR_RECRUIT）',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_rp_no`(`plan_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '人力需求/招聘计划' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_recruit_plan
-- ----------------------------
INSERT INTO `hr_recruit_plan` VALUES (1, 'RC-2026-0001', 5, 6, 10, '制造一部产能提升，操作工缺编', 2, 7, '2026-10-31 21:56:01');
INSERT INTO `hr_recruit_plan` VALUES (2, 'RC-2026-0002', 2, 3, 1, 'HR专员离职补充', 2, NULL, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_salary
-- ----------------------------
DROP TABLE IF EXISTS `hr_salary`;
CREATE TABLE `hr_salary`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `salary_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工资单号',
  `period` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '薪资期间(YYYY-MM)',
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `base_salary` decimal(18, 2) NULL DEFAULT 0.00,
  `attend_days` decimal(6, 1) NULL DEFAULT 0.0 COMMENT '出勤天数',
  `ot_pay` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '加班费(来自 hr_attendance/overtime)',
  `leave_deduct` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '请假扣款(来自 hr_leave)',
  `perf_bonus` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '绩效奖金(来自 hr_perf_result)',
  `gross_pay` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '应发合计',
  `social_personal` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '个人社保',
  `fund_personal` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '个人公积金',
  `tax` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '个税(按 hr_tax_bracket)',
  `net_pay` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '实发合计',
  `voucher_id` bigint NULL DEFAULT NULL COMMENT '关联 fin_voucher.id（传递财务总账）',
  `status` tinyint NULL DEFAULT 0 COMMENT '0草稿/1已核算/2已发/3已生成凭证',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_salary_no`(`salary_no` ASC) USING BTREE,
  INDEX `idx_sal_emp`(`emp_id` ASC, `period` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '薪资核算主表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_salary
-- ----------------------------
INSERT INTO `hr_salary` VALUES (1, 'PAY-2026-10-E0007', '2026-10', 7, 7000.00, 22.0, 1280.00, 0.00, 500.00, 8780.00, 945.00, 630.00, 112.00, 7093.00, 4, 3, '2026-10-31 21:56:01');
INSERT INTO `hr_salary` VALUES (2, 'PAY-2026-10-E0008', '2026-10', 8, 13000.00, 22.0, 0.00, 0.00, 2000.00, 15000.00, 1752.00, 1170.00, 488.00, 11590.00, NULL, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_salary_detail
-- ----------------------------
DROP TABLE IF EXISTS `hr_salary_detail`;
CREATE TABLE `hr_salary_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `salary_id` bigint NOT NULL COMMENT '工资单ID',
  `item_id` bigint NULL DEFAULT NULL COMMENT '薪资项目ID',
  `item_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `amount` decimal(18, 2) NULL DEFAULT 0.00,
  `direction` tinyint NULL DEFAULT 1 COMMENT '1收入/-1扣减',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_sald_salary`(`salary_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 12 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '薪资明细' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_salary_detail
-- ----------------------------
INSERT INTO `hr_salary_detail` VALUES (1, 1, 1, '基本工资', 7000.00, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (2, 1, 3, '加班费', 1280.00, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (3, 1, 4, '绩效奖金', 500.00, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (4, 1, 6, '社保(个人)', 945.00, -1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (5, 1, 7, '公积金(个人)', 630.00, -1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (6, 1, 8, '个税', 112.00, -1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (7, 2, 1, '基本工资', 13000.00, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (8, 2, 4, '绩效奖金', 2000.00, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (9, 2, 6, '社保(个人)', 1752.00, -1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (10, 2, 7, '公积金(个人)', 1170.00, -1, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_detail` VALUES (11, 2, 8, '个税', 488.00, -1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_salary_item
-- ----------------------------
DROP TABLE IF EXISTS `hr_salary_item`;
CREATE TABLE `hr_salary_item`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `item_no` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '薪资项目编码',
  `item_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '项目名称',
  `item_type` tinyint NULL DEFAULT 1 COMMENT '1应发/2扣减',
  `calc_type` tinyint NULL DEFAULT 1 COMMENT '1固定/2公式/3考勤/4绩效',
  `tax_flag` tinyint NULL DEFAULT 1 COMMENT '0不计税/1计税',
  `order_no` int NULL DEFAULT 0 COMMENT '显示顺序',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_item_no`(`item_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '薪资项目（自定义）' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_salary_item
-- ----------------------------
INSERT INTO `hr_salary_item` VALUES (1, 'I01', '基本工资', 1, 1, 1, 10, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (2, 'I02', '岗位工资', 1, 1, 1, 20, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (3, 'I03', '加班费', 1, 3, 1, 30, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (4, 'I04', '绩效奖金', 1, 4, 1, 40, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (5, 'I05', '全勤奖', 1, 1, 1, 50, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (6, 'I06', '社保(个人)', 2, 1, 0, 60, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (7, 'I07', '公积金(个人)', 2, 1, 0, 70, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (8, 'I08', '个税', 2, 1, 0, 80, '2026-10-31 21:56:01');
INSERT INTO `hr_salary_item` VALUES (9, 'I09', '事假扣款', 2, 3, 0, 90, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_schedule
-- ----------------------------
DROP TABLE IF EXISTS `hr_schedule`;
CREATE TABLE `hr_schedule`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `shift_id` bigint NOT NULL COMMENT '班次ID',
  `work_date` date NULL DEFAULT NULL COMMENT '排班日期',
  `remark` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_sched_emp`(`emp_id` ASC, `work_date` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '排班表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_schedule
-- ----------------------------
INSERT INTO `hr_schedule` VALUES (1, 6, 1, '2026-10-01', '制造一部排班', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (2, 7, 1, '2026-10-01', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (3, 11, 1, '2026-10-01', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (4, 6, 1, '2026-10-02', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (5, 7, 1, '2026-10-02', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (6, 11, 1, '2026-10-02', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (7, 6, 2, '2026-10-03', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (8, 7, 2, '2026-10-03', '', '2026-10-31 21:56:00');
INSERT INTO `hr_schedule` VALUES (9, 11, 2, '2026-10-03', '', '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_shift
-- ----------------------------
DROP TABLE IF EXISTS `hr_shift`;
CREATE TABLE `hr_shift`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `shift_no` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '班次编码',
  `shift_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '班次名称',
  `start_time` time NULL DEFAULT NULL,
  `end_time` time NULL DEFAULT NULL,
  `cross_day` tinyint NULL DEFAULT 0 COMMENT '0否/1跨天',
  `rest_minutes` int NULL DEFAULT 0 COMMENT '休息分钟',
  `status` tinyint NULL DEFAULT 1 COMMENT '0停用/1启用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_shift_no`(`shift_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '班次定义' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_shift
-- ----------------------------
INSERT INTO `hr_shift` VALUES (1, 'S1', '早班', '08:00:00', '16:00:00', 0, 60, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_shift` VALUES (2, 'S2', '晚班', '16:00:00', '24:00:00', 0, 60, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_shift` VALUES (3, 'S3', '夜班', '00:00:00', '08:00:00', 1, 60, 1, '2026-10-31 21:56:00');
INSERT INTO `hr_shift` VALUES (4, 'S4', '行政班', '09:00:00', '18:00:00', 0, 60, 1, '2026-10-31 21:56:00');

-- ----------------------------
-- Table structure for hr_social_rule
-- ----------------------------
DROP TABLE IF EXISTS `hr_social_rule`;
CREATE TABLE `hr_social_rule`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `city` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '参保城市',
  `base_min` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '缴费基数下限',
  `base_max` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '缴费基数上限',
  `pension_emp_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '养老-个人比例',
  `pension_com_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '养老-企业比例',
  `medical_emp_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '医疗-个人比例',
  `medical_com_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '医疗-企业比例',
  `unemployment_emp_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '失业-个人比例',
  `unemployment_com_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '失业-企业比例',
  `injury_com_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '工伤-企业比例',
  `birth_com_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '生育-企业比例',
  `fund_emp_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '公积金-个人比例',
  `fund_com_rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '公积金-企业比例',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_social_city`(`city` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '社保/公积金缴费规则' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_social_rule
-- ----------------------------
INSERT INTO `hr_social_rule` VALUES (1, '上海', 6000.00, 30000.00, 0.0800, 0.1600, 0.0200, 0.0950, 0.0050, 0.0050, 0.0020, 0.0080, 0.0700, 0.0700, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_tax_bracket
-- ----------------------------
DROP TABLE IF EXISTS `hr_tax_bracket`;
CREATE TABLE `hr_tax_bracket`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `level` int NOT NULL COMMENT '级次',
  `threshold_low` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '应纳税所得额下限',
  `threshold_high` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '应纳税所得额上限(0=无上限)',
  `rate` decimal(8, 4) NULL DEFAULT 0.0000 COMMENT '税率',
  `quick_deduction` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '速算扣除数',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '个税累进税率表(月度)' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_tax_bracket
-- ----------------------------
INSERT INTO `hr_tax_bracket` VALUES (1, 1, 0.00, 3000.00, 0.0300, 0.00, '2026-10-31 21:56:01');
INSERT INTO `hr_tax_bracket` VALUES (2, 2, 3000.00, 12000.00, 0.1000, 210.00, '2026-10-31 21:56:01');
INSERT INTO `hr_tax_bracket` VALUES (3, 3, 12000.00, 25000.00, 0.2000, 1410.00, '2026-10-31 21:56:01');
INSERT INTO `hr_tax_bracket` VALUES (4, 4, 25000.00, 35000.00, 0.2500, 2660.00, '2026-10-31 21:56:01');
INSERT INTO `hr_tax_bracket` VALUES (5, 5, 35000.00, 55000.00, 0.3000, 4410.00, '2026-10-31 21:56:01');
INSERT INTO `hr_tax_bracket` VALUES (6, 6, 55000.00, 80000.00, 0.3500, 7160.00, '2026-10-31 21:56:01');
INSERT INTO `hr_tax_bracket` VALUES (7, 7, 80000.00, 0.00, 0.4500, 15160.00, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_train_expense
-- ----------------------------
DROP TABLE IF EXISTS `hr_train_expense`;
CREATE TABLE `hr_train_expense`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `plan_id` bigint NOT NULL COMMENT '培训计划ID',
  `expense_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '费用类型：讲师费/场地/教材/差旅',
  `amount` decimal(18, 2) NULL DEFAULT 0.00,
  `occur_date` date NULL DEFAULT NULL COMMENT '发生日期',
  `remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_te_plan`(`plan_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '培训费用' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_train_expense
-- ----------------------------
INSERT INTO `hr_train_expense` VALUES (1, 1, '讲师费', 2000.00, '2026-10-25', '内部讲师', '2026-10-31 21:56:01');
INSERT INTO `hr_train_expense` VALUES (2, 1, '教材', 1000.00, '2026-10-25', '手册印刷', '2026-10-31 21:56:01');
INSERT INTO `hr_train_expense` VALUES (3, 2, '场地', 3000.00, '2026-11-05', '外部场地', '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_train_need
-- ----------------------------
DROP TABLE IF EXISTS `hr_train_need`;
CREATE TABLE `hr_train_need`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `need_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '培训需求单号',
  `dept_id` bigint NULL DEFAULT NULL COMMENT '提出部门',
  `need_desc` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '需求描述',
  `priority` tinyint NULL DEFAULT 2 COMMENT '1高/2中/3低',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待审批/1采纳/2驳回',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_tneed_no`(`need_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '培训需求' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_train_need
-- ----------------------------
INSERT INTO `hr_train_need` VALUES (1, 'TN-2026-0001', 5, '新设备操作安全培训', 1, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_train_need` VALUES (2, 'TN-2026-0002', 2, '劳动合同法实务', 2, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_train_plan
-- ----------------------------
DROP TABLE IF EXISTS `hr_train_plan`;
CREATE TABLE `hr_train_plan`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `plan_no` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '培训计划单号',
  `plan_name` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '培训名称',
  `train_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '类型：新员工/技能/安全/管理',
  `start_date` date NULL DEFAULT NULL,
  `end_date` date NULL DEFAULT NULL,
  `trainer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '讲师',
  `budget` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '培训预算',
  `status` tinyint NULL DEFAULT 0 COMMENT '0草稿/1执行中/2完成',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_tplan_no`(`plan_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '培训计划' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_train_plan
-- ----------------------------
INSERT INTO `hr_train_plan` VALUES (1, 'TP-2026-0001', '新员工入职培训', '新员工', '2026-10-25', '2026-10-27', '李娜', 5000.00, 1, '2026-10-31 21:56:01');
INSERT INTO `hr_train_plan` VALUES (2, 'TP-2026-0002', '设备安全操作培训', '安全', '2026-11-05', '2026-11-06', '陈刚', 8000.00, 1, '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_train_record
-- ----------------------------
DROP TABLE IF EXISTS `hr_train_record`;
CREATE TABLE `hr_train_record`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `plan_id` bigint NOT NULL COMMENT '培训计划ID',
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `hours` decimal(6, 1) NULL DEFAULT 0.0 COMMENT '培训课时',
  `score` decimal(6, 1) NULL DEFAULT 0.0 COMMENT '考核成绩',
  `certificate` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '获得证书',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_tr_plan`(`plan_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '培训记录' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_train_record
-- ----------------------------
INSERT INTO `hr_train_record` VALUES (1, 1, 7, 16.0, 95.0, '入职培训合格证', '2026-10-31 21:56:01');
INSERT INTO `hr_train_record` VALUES (2, 1, 11, 16.0, 92.0, '入职培训合格证', '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for hr_welfare
-- ----------------------------
DROP TABLE IF EXISTS `hr_welfare`;
CREATE TABLE `hr_welfare`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `emp_id` bigint NOT NULL COMMENT '员工ID',
  `welfare_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '福利类型：节日/餐补/交通/体检',
  `amount` decimal(18, 2) NULL DEFAULT 0.00,
  `month` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '所属月份(YYYY-MM)',
  `remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_welfare_emp`(`emp_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '员工福利' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of hr_welfare
-- ----------------------------
INSERT INTO `hr_welfare` VALUES (1, 7, '节日福利', 500.00, '2026-10', '中秋福利', '2026-10-31 21:56:01');
INSERT INTO `hr_welfare` VALUES (2, 8, '餐补', 300.00, '2026-10', '月度餐补', '2026-10-31 21:56:01');
INSERT INTO `hr_welfare` VALUES (3, 2, '体检', 800.00, '2026-10', '年度体检', '2026-10-31 21:56:01');

-- ----------------------------
-- Table structure for inv_batch
-- ----------------------------
DROP TABLE IF EXISTS `inv_batch`;
CREATE TABLE `inv_batch`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `batch_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '批次号',
  `quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '批次数量',
  `production_date` date NULL DEFAULT NULL COMMENT '生产日期',
  `expiry_date` date NULL DEFAULT NULL COMMENT '有效期',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_batch_mat`(`material_id` ASC, `batch_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '批次库存' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_batch
-- ----------------------------
INSERT INTO `inv_batch` VALUES (1, 1, 1, 'B20261001', 100.0000, '2026-10-01', '2027-10-01');
INSERT INTO `inv_batch` VALUES (2, 2, 1, 'B20261002', 200.0000, '2026-10-02', '2027-10-02');
INSERT INTO `inv_batch` VALUES (3, 3, 2, 'B20261003', 300.0000, '2026-10-03', '2027-10-03');
INSERT INTO `inv_batch` VALUES (4, 4, 2, 'B20261004', 400.0000, '2026-10-04', '2027-10-04');

-- ----------------------------
-- Table structure for inv_check
-- ----------------------------
DROP TABLE IF EXISTS `inv_check`;
CREATE TABLE `inv_check`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `check_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '盘点单号',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `check_date` date NOT NULL COMMENT '盘点日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0盘点中/1已完成',
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_check_no`(`check_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '盘点单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_check
-- ----------------------------
INSERT INTO `inv_check` VALUES (1, 'CK202610001', 1, '2026-10-15', 1, 0);
INSERT INTO `inv_check` VALUES (2, 'CK202610002', 2, '2026-10-20', 0, 0);
INSERT INTO `inv_check` VALUES (3, 'CK202611020001', 1, '2026-11-02', 1, 0);
INSERT INTO `inv_check` VALUES (4, 'CK202611020002', 1, '2026-11-02', 1, 0);

-- ----------------------------
-- Table structure for inv_check_detail
-- ----------------------------
DROP TABLE IF EXISTS `inv_check_detail`;
CREATE TABLE `inv_check_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `check_id` bigint NOT NULL COMMENT '盘点单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `book_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '账面数量',
  `actual_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '实盘数量',
  `diff_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '差异数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_cd_check`(`check_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '盘点明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_check_detail
-- ----------------------------
INSERT INTO `inv_check_detail` VALUES (1, 1, 1, 100.0000, 98.0000, -2.0000);
INSERT INTO `inv_check_detail` VALUES (2, 1, 2, 200.0000, 200.0000, 0.0000);
INSERT INTO `inv_check_detail` VALUES (3, 2, 3, 300.0000, 305.0000, 5.0000);
INSERT INTO `inv_check_detail` VALUES (4, 2, 4, 400.0000, 400.0000, 0.0000);
INSERT INTO `inv_check_detail` VALUES (5, 3, 1, 3.0000, 0.0000, -3.0000);
INSERT INTO `inv_check_detail` VALUES (6, 3, 2, 200.0000, 0.0000, -200.0000);
INSERT INTO `inv_check_detail` VALUES (7, 4, 1, 3.0000, 3.0000, 0.0000);
INSERT INTO `inv_check_detail` VALUES (8, 4, 2, 200.0000, 200.0000, 0.0000);

-- ----------------------------
-- Table structure for inv_in_detail
-- ----------------------------
DROP TABLE IF EXISTS `inv_in_detail`;
CREATE TABLE `inv_in_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `in_order_id` bigint NOT NULL COMMENT '入库单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '入库数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_ind_order`(`in_order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '入库明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_in_detail
-- ----------------------------
INSERT INTO `inv_in_detail` VALUES (1, 1, 1, 50.0000);
INSERT INTO `inv_in_detail` VALUES (2, 1, 2, 100.0000);
INSERT INTO `inv_in_detail` VALUES (3, 2, 3, 200.0000);
INSERT INTO `inv_in_detail` VALUES (4, 4, 1, 5.0000);

-- ----------------------------
-- Table structure for inv_in_order
-- ----------------------------
DROP TABLE IF EXISTS `inv_in_order`;
CREATE TABLE `inv_in_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `in_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '入库单号',
  `in_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '类型：PO采购/PR生产/OTHER其他',
  `source_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '来源单据号',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `in_date` date NOT NULL COMMENT '入库日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待审/1已审',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_in_no`(`in_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '入库单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_in_order
-- ----------------------------
INSERT INTO `inv_in_order` VALUES (1, 'IN202610001', 'PO采购', 'PO202610001', 1, '2026-10-10', 1, '2026-10-31 11:53:32', 0);
INSERT INTO `inv_in_order` VALUES (2, 'IN202610002', 'PR生产', 'PR202610001', 1, '2026-10-18', 1, '2026-10-31 11:53:32', 0);
INSERT INTO `inv_in_order` VALUES (3, 'IN202610003', 'OTHER其他', NULL, 2, '2026-10-25', 0, '2026-10-31 11:53:32', 0);
INSERT INTO `inv_in_order` VALUES (4, 'IN202611020001', 'OTHER', NULL, 1, '2026-11-02', 1, '2026-11-02 11:27:54', 1);

-- ----------------------------
-- Table structure for inv_out_detail
-- ----------------------------
DROP TABLE IF EXISTS `inv_out_detail`;
CREATE TABLE `inv_out_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `out_order_id` bigint NOT NULL COMMENT '出库单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '出库数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_outd_order`(`out_order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '出库明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_out_detail
-- ----------------------------
INSERT INTO `inv_out_detail` VALUES (1, 1, 1, 20.0000);
INSERT INTO `inv_out_detail` VALUES (2, 1, 2, 30.0000);
INSERT INTO `inv_out_detail` VALUES (3, 2, 3, 40.0000);
INSERT INTO `inv_out_detail` VALUES (4, 4, 1, 2.0000);

-- ----------------------------
-- Table structure for inv_out_order
-- ----------------------------
DROP TABLE IF EXISTS `inv_out_order`;
CREATE TABLE `inv_out_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `out_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '出库单号',
  `out_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '类型：SO销售/PR领料/OTHER其他',
  `source_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '来源单据号',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `out_date` date NOT NULL COMMENT '出库日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待审/1已审',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_out_no`(`out_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '出库单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_out_order
-- ----------------------------
INSERT INTO `inv_out_order` VALUES (1, 'OUT202610001', 'SO销售', 'SO202610001', 1, '2026-10-12', 1, '2026-10-31 11:53:32', 0);
INSERT INTO `inv_out_order` VALUES (2, 'OUT202610002', 'PR领料', 'PR202610001', 1, '2026-10-16', 1, '2026-10-31 11:53:32', 0);
INSERT INTO `inv_out_order` VALUES (3, 'OUT202610003', 'OTHER其他', NULL, 2, '2026-10-28', 0, '2026-10-31 11:53:32', 0);
INSERT INTO `inv_out_order` VALUES (4, 'OUT202611020001', 'OTHER', NULL, 1, '2026-11-02', 1, '2026-11-02 11:27:55', 1);

-- ----------------------------
-- Table structure for inv_stock
-- ----------------------------
DROP TABLE IF EXISTS `inv_stock`;
CREATE TABLE `inv_stock`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `current_qty` decimal(18, 4) NOT NULL DEFAULT 0.0000 COMMENT '当前库存数量',
  `locked_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '锁定库存',
  `last_update` datetime NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_stock_mat_wh`(`material_id` ASC, `warehouse_id` ASC) USING BTREE,
  INDEX `idx_stock_wh`(`warehouse_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '库存实时台账' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_stock
-- ----------------------------
INSERT INTO `inv_stock` VALUES (1, 1, 1, 3.0000, 10.0000, '2026-11-02 11:27:54');
INSERT INTO `inv_stock` VALUES (2, 2, 1, 200.0000, 20.0000, '2026-10-31 11:53:32');
INSERT INTO `inv_stock` VALUES (3, 3, 2, 300.0000, 30.0000, '2026-10-31 11:53:32');
INSERT INTO `inv_stock` VALUES (4, 4, 2, 400.0000, 40.0000, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for inv_transaction
-- ----------------------------
DROP TABLE IF EXISTS `inv_transaction`;
CREATE TABLE `inv_transaction`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `trans_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '流水单号',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `warehouse_id` bigint NOT NULL COMMENT '仓库ID',
  `trans_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'IN_PO/OUT_SO/IN_PR/OUT_PR/ADJ',
  `direction` tinyint NOT NULL COMMENT '1入库/-1出库',
  `quantity` decimal(18, 4) NOT NULL COMMENT '变动数量（正数）',
  `before_qty` decimal(18, 4) NULL DEFAULT NULL COMMENT '变动前数量',
  `after_qty` decimal(18, 4) NULL DEFAULT NULL COMMENT '变动后数量',
  `source_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '来源单据号',
  `trans_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_trans_mat_wh`(`material_id` ASC, `warehouse_id` ASC) USING BTREE,
  INDEX `idx_trans_time`(`trans_time` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '库存流水' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_transaction
-- ----------------------------
INSERT INTO `inv_transaction` VALUES (1, 'T202610001', 1, 1, 'IN_PO', 1, 50.0000, 50.0000, 100.0000, 'IN202610001', '2026-10-10 09:00:00');
INSERT INTO `inv_transaction` VALUES (2, 'T202610002', 2, 1, 'IN_PO', 1, 100.0000, 100.0000, 200.0000, 'IN202610001', '2026-10-10 09:05:00');
INSERT INTO `inv_transaction` VALUES (3, 'T202610003', 1, 1, 'OUT_SO', -1, 20.0000, 100.0000, 80.0000, 'OUT202610001', '2026-10-12 10:00:00');
INSERT INTO `inv_transaction` VALUES (4, 'T202610004', 2, 1, 'OUT_SO', -1, 30.0000, 200.0000, 170.0000, 'OUT202610001', '2026-10-12 10:05:00');
INSERT INTO `inv_transaction` VALUES (5, 'T202610005', 3, 2, 'IN_PR', 1, 200.0000, 100.0000, 300.0000, 'IN202610002', '2026-10-18 14:00:00');
INSERT INTO `inv_transaction` VALUES (6, 'TX202611010001', 1, 1, 'OUT_SO', -1, 100.0000, 100.0000, 0.0000, 'DN202611010001', '2026-11-01 11:10:30');
INSERT INTO `inv_transaction` VALUES (7, 'TX202611020001', 1, 1, 'IN_OTHER', 1, 5.0000, 0.0000, 5.0000, 'IN202611020001', '2026-11-02 11:27:54');
INSERT INTO `inv_transaction` VALUES (8, 'TX202611020002', 1, 1, 'OUT_OTHER', -1, 2.0000, 5.0000, 3.0000, 'OUT202611020001', '2026-11-02 11:27:55');

-- ----------------------------
-- Table structure for inv_transfer
-- ----------------------------
DROP TABLE IF EXISTS `inv_transfer`;
CREATE TABLE `inv_transfer`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `transfer_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '调拨单号',
  `from_warehouse_id` bigint NOT NULL COMMENT '调出仓库ID',
  `to_warehouse_id` bigint NOT NULL COMMENT '调入仓库ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '调拨数量',
  `transfer_date` date NOT NULL COMMENT '调拨日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待审/1已审',
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_transfer_no`(`transfer_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '调拨单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of inv_transfer
-- ----------------------------
INSERT INTO `inv_transfer` VALUES (1, 'TR202610001', 1, 2, 1, 10.0000, '2026-10-20', 1, 0);
INSERT INTO `inv_transfer` VALUES (2, 'TR202610002', 2, 1, 3, 20.0000, '2026-10-22', 0, 0);

-- ----------------------------
-- Table structure for md_account
-- ----------------------------
DROP TABLE IF EXISTS `md_account`;
CREATE TABLE `md_account`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '科目ID',
  `account_code` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '科目编码',
  `account_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '科目名称',
  `account_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '科目类型：资产/负债/权益/成本/损益',
  `parent_id` bigint NULL DEFAULT 0 COMMENT '上级科目ID',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_account_code`(`account_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 11 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '会计科目' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of md_account
-- ----------------------------
INSERT INTO `md_account` VALUES (1, '1001', '库存现金', '资产', 0, 1);
INSERT INTO `md_account` VALUES (2, '1002', '银行存款', '资产', 0, 1);
INSERT INTO `md_account` VALUES (3, '1122', '应收账款', '资产', 0, 1);
INSERT INTO `md_account` VALUES (4, '1403', '原材料', '资产', 0, 1);
INSERT INTO `md_account` VALUES (5, '1405', '库存商品', '资产', 0, 1);
INSERT INTO `md_account` VALUES (6, '2202', '应付账款', '负债', 0, 1);
INSERT INTO `md_account` VALUES (7, '4001', '实收资本', '权益', 0, 1);
INSERT INTO `md_account` VALUES (8, '5001', '生产成本', '成本', 0, 1);
INSERT INTO `md_account` VALUES (9, '6001', '主营业务收入', '损益', 0, 1);
INSERT INTO `md_account` VALUES (10, '6401', '主营业务成本', '损益', 0, 1);

-- ----------------------------
-- Table structure for md_bom
-- ----------------------------
DROP TABLE IF EXISTS `md_bom`;
CREATE TABLE `md_bom`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT 'BOM ID',
  `parent_material_id` bigint NOT NULL COMMENT '父项物料ID（成品/半成品）',
  `child_material_id` bigint NOT NULL COMMENT '子项物料ID（原材料/半成品）',
  `qty_per_unit` decimal(18, 6) NOT NULL COMMENT '单位用量',
  `loss_rate` decimal(6, 4) NULL DEFAULT 0.0000 COMMENT '损耗率（0.02=2%）',
  `version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'V1.0' COMMENT 'BOM版本',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_bom_parent`(`parent_material_id` ASC) USING BTREE,
  INDEX `idx_bom_child`(`child_material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = 'BOM物料清单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of md_bom
-- ----------------------------
INSERT INTO `md_bom` VALUES (1, 1, 3, 2.000000, 0.0200, 'V1.0', 1, '2026-10-31 11:53:32');
INSERT INTO `md_bom` VALUES (2, 1, 4, 1.000000, 0.0100, 'V1.0', 1, '2026-10-31 11:53:32');
INSERT INTO `md_bom` VALUES (3, 2, 3, 3.000000, 0.0300, 'V1.0', 1, '2026-10-31 11:53:32');
INSERT INTO `md_bom` VALUES (4, 2, 4, 1.500000, 0.0200, 'V1.0', 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for md_customer
-- ----------------------------
DROP TABLE IF EXISTS `md_customer`;
CREATE TABLE `md_customer`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '客户ID',
  `customer_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '客户编码',
  `customer_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '客户名称',
  `contact_person` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系人',
  `phone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系电话',
  `address` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '地址',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_customer_code`(`customer_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '客户档案' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of md_customer
-- ----------------------------
INSERT INTO `md_customer` VALUES (1, 'CUST001', '北京华信科技有限公司', '张伟', '13800138001', '北京市朝阳区建国路88号', 1, '2026-10-31 11:53:32');
INSERT INTO `md_customer` VALUES (2, 'CUST002', '上海远大贸易有限公司', '李娜', '13900139002', '上海市浦东新区张江路100号', 1, '2026-10-31 11:53:32');
INSERT INTO `md_customer` VALUES (3, 'CUST003', '广州南方制造有限公司', '王强', '13700137003', '广州市天河区体育西路200号', 1, '2026-10-31 11:53:32');
INSERT INTO `md_customer` VALUES (4, 'CUST004', '深圳创新电子有限公司', '赵敏', '13600136004', '深圳市南山区科技园南路18号', 1, '2026-10-31 11:53:32');
INSERT INTO `md_customer` VALUES (5, 'CUST005', '杭州恒通商贸有限公司', '陈刚', '13500135005', '杭州市西湖区文三路50号', 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for md_location
-- ----------------------------
DROP TABLE IF EXISTS `md_location`;
CREATE TABLE `md_location`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `warehouse_id` bigint NOT NULL COMMENT '所属仓库ID',
  `location_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '库位编码',
  `location_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '库位名称',
  `area` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '区域',
  `shelf` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '货架',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_location_code`(`location_code` ASC) USING BTREE,
  INDEX `idx_loc_warehouse`(`warehouse_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '库位档案' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of md_location
-- ----------------------------
INSERT INTO `md_location` VALUES (1, 1, 'A-01-01', 'A区01排01位', 'A', '01', 1, '2026-10-31 11:53:32');
INSERT INTO `md_location` VALUES (2, 1, 'A-01-02', 'A区01排02位', 'A', '01', 1, '2026-10-31 11:53:32');
INSERT INTO `md_location` VALUES (3, 1, 'A-02-01', 'A区02排01位', 'A', '02', 1, '2026-10-31 11:53:32');
INSERT INTO `md_location` VALUES (4, 2, 'B-01-01', 'B区01排01位', 'B', '01', 1, '2026-10-31 11:53:32');
INSERT INTO `md_location` VALUES (5, 2, 'B-01-02', 'B区01排02位', 'B', '01', 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for md_material
-- ----------------------------
DROP TABLE IF EXISTS `md_material`;
CREATE TABLE `md_material`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '物料ID',
  `material_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '物料编码',
  `material_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '物料名称',
  `spec` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '规格型号',
  `unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '计量单位',
  `category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '物料分类：成品/半成品/原材料/辅料',
  `safety_stock` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '安全库存',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_material_code`(`material_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '物料主数据' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of md_material
-- ----------------------------
INSERT INTO `md_material` VALUES (1, 'M001', '智能控制器成品', 'CTRL-100', '台', '成品', 50.0000, 1, '2026-10-31 11:53:32');
INSERT INTO `md_material` VALUES (2, 'M002', '电源模块成品', 'PWR-200', '台', '成品', 80.0000, 1, '2026-10-31 11:53:32');
INSERT INTO `md_material` VALUES (3, 'M003', '控制芯片', 'CHIP-A1', '个', '原材料', 500.0000, 1, '2026-10-31 11:53:32');
INSERT INTO `md_material` VALUES (4, 'M004', '电路板', 'PCB-B2', '块', '原材料', 300.0000, 1, '2026-10-31 11:53:32');
INSERT INTO `md_material` VALUES (5, 'M005', '外壳组件', 'CASE-C3', '套', '辅料', 200.0000, 1, '2026-10-31 11:53:32');
INSERT INTO `md_material` VALUES (6, 'M006', '连接线束', 'WIRE-D4', '根', '辅料', 1000.0000, 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for md_supplier
-- ----------------------------
DROP TABLE IF EXISTS `md_supplier`;
CREATE TABLE `md_supplier`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '供应商ID',
  `supplier_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '供应商编码',
  `supplier_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '供应商名称',
  `contact_person` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系人',
  `phone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '联系电话',
  `address` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '地址',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_supplier_code`(`supplier_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '供应商档案' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of md_supplier
-- ----------------------------
INSERT INTO `md_supplier` VALUES (1, 'SUP001', '深圳华强电子元件有限公司', '刘洋', '13811110001', '深圳市福田区华强北路100号', 1, '2026-10-31 11:53:32');
INSERT INTO `md_supplier` VALUES (2, 'SUP002', '东莞精工五金制品厂', '周杰', '13822220002', '东莞市长安镇工业大道200号', 1, '2026-10-31 11:53:32');
INSERT INTO `md_supplier` VALUES (3, 'SUP003', '苏州联创包装材料有限公司', '吴敏', '13833330003', '苏州市工业园区星湖街300号', 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for md_warehouse
-- ----------------------------
DROP TABLE IF EXISTS `md_warehouse`;
CREATE TABLE `md_warehouse`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '仓库ID',
  `warehouse_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '仓库编码',
  `warehouse_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '仓库名称',
  `location` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '仓库位置',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_warehouse_code`(`warehouse_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '仓库档案' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of md_warehouse
-- ----------------------------
INSERT INTO `md_warehouse` VALUES (1, 'WH001', '原料仓', '工厂A区一层', 1, '2026-10-31 11:53:32');
INSERT INTO `md_warehouse` VALUES (2, 'WH002', '成品仓', '工厂B区一层', 1, '2026-10-31 11:53:32');
INSERT INTO `md_warehouse` VALUES (3, 'WH003', '半成品仓', '工厂A区二层', 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for mes_material_issue
-- ----------------------------
DROP TABLE IF EXISTS `mes_material_issue`;
CREATE TABLE `mes_material_issue`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `issue_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '领料单号',
  `work_order_id` bigint NOT NULL COMMENT '工单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `qty` decimal(18, 3) NOT NULL COMMENT '领用数量',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态(0待领,1已领)',
  `issue_time` datetime NULL DEFAULT NULL COMMENT '领料时间',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '领料单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_material_issue
-- ----------------------------
INSERT INTO `mes_material_issue` VALUES (1, 'MI202611010001', 1, 3, 204.000, 1, '2026-11-01 09:00:00', '', NULL);
INSERT INTO `mes_material_issue` VALUES (2, 'MI202611010002', 1, 4, 202.000, 1, '2026-11-01 09:05:00', '', NULL);
INSERT INTO `mes_material_issue` VALUES (3, 'MI202611010003', 2, 3, 371.000, 1, '2026-11-01 10:30:00', 'chip 120x3x1.03', NULL);
INSERT INTO `mes_material_issue` VALUES (4, 'MI202611010004', 2, 4, 184.000, 1, '2026-11-01 10:35:00', 'pcb 120x1.5x1.02', NULL);

-- ----------------------------
-- Table structure for mes_production_order
-- ----------------------------
DROP TABLE IF EXISTS `mes_production_order`;
CREATE TABLE `mes_production_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `prod_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '生产任务号',
  `work_order_id` bigint NOT NULL COMMENT '所属工单ID',
  `workstation_id` bigint NULL DEFAULT NULL COMMENT '工位ID',
  `plan_qty` int NOT NULL DEFAULT 0 COMMENT '计划数量',
  `done_qty` int NOT NULL DEFAULT 0 COMMENT '已产数量',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态(0待开工,1生产中,2完工)',
  `start_time` datetime NULL DEFAULT NULL COMMENT '开工时间',
  `end_time` datetime NULL DEFAULT NULL COMMENT '完工时间',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '生产任务(派工)' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_production_order
-- ----------------------------
INSERT INTO `mes_production_order` VALUES (1, 'PO202611010001', 1, 1, 200, 180, 1, '2026-11-01 08:00:00', NULL, '', NULL);
INSERT INTO `mes_production_order` VALUES (2, 'PO202611010002', 2, 1, 120, 0, 0, NULL, NULL, '', NULL);

-- ----------------------------
-- Table structure for mes_quality_check
-- ----------------------------
DROP TABLE IF EXISTS `mes_quality_check`;
CREATE TABLE `mes_quality_check`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `check_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '质检单号',
  `production_order_id` bigint NULL DEFAULT NULL COMMENT '生产任务ID',
  `work_report_id` bigint NULL DEFAULT NULL COMMENT '报工单ID',
  `check_type` tinyint NOT NULL DEFAULT 1 COMMENT '检验类型(1首检,2巡检,3终检)',
  `qty` int NOT NULL DEFAULT 0 COMMENT '检验数量',
  `pass_qty` int NOT NULL DEFAULT 0 COMMENT '合格数',
  `fail_qty` int NOT NULL DEFAULT 0 COMMENT '不良数',
  `result` tinyint NOT NULL DEFAULT 0 COMMENT '结果(0待检,1合格,2不合格)',
  `check_time` datetime NULL DEFAULT NULL COMMENT '检验时间',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '质量检验' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_quality_check
-- ----------------------------
INSERT INTO `mes_quality_check` VALUES (1, 'QC202611010001', 1, 1, 3, 182, 180, 2, 2, '2026-11-01 17:00:00', '', NULL);
INSERT INTO `mes_quality_check` VALUES (2, 'QC202611010002', 1, 1, 1, 50, 50, 0, 2, '2026-11-01 10:00:00', 'FAI 50 ok', NULL);

-- ----------------------------
-- Table structure for mes_routing
-- ----------------------------
DROP TABLE IF EXISTS `mes_routing`;
CREATE TABLE `mes_routing`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `routing_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工艺路线编码',
  `product_id` bigint NOT NULL COMMENT '产品ID',
  `version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '版本',
  `status` tinyint NOT NULL DEFAULT 1 COMMENT '状态(0停用,1启用)',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '工艺路线' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_routing
-- ----------------------------
INSERT INTO `mes_routing` VALUES (1, 'RT-CTRL-V1', 1, 'V1.0', 1, '', NULL);
INSERT INTO `mes_routing` VALUES (2, 'RT-PWR-V1', 2, 'V1.0', 1, '', NULL);

-- ----------------------------
-- Table structure for mes_routing_step
-- ----------------------------
DROP TABLE IF EXISTS `mes_routing_step`;
CREATE TABLE `mes_routing_step`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `routing_id` bigint NOT NULL COMMENT '工艺路线ID',
  `step_no` int NOT NULL DEFAULT 1 COMMENT '工序号',
  `step_name` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工序名称',
  `workstation_id` bigint NULL DEFAULT NULL COMMENT '工位ID',
  `std_hours` decimal(10, 2) NOT NULL DEFAULT 0.00 COMMENT '标准工时',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '工艺工序' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_routing_step
-- ----------------------------
INSERT INTO `mes_routing_step` VALUES (1, 1, 10, '装配', 1, 1.50, '', NULL);
INSERT INTO `mes_routing_step` VALUES (2, 1, 20, '老化测试', 2, 2.00, '', NULL);
INSERT INTO `mes_routing_step` VALUES (3, 1, 30, '检测包装', 3, 0.50, '', NULL);
INSERT INTO `mes_routing_step` VALUES (4, 2, 10, '模块组装', 1, 1.00, '', NULL);
INSERT INTO `mes_routing_step` VALUES (5, 2, 20, '成品检测', 3, 0.80, '', NULL);

-- ----------------------------
-- Table structure for mes_work_order
-- ----------------------------
DROP TABLE IF EXISTS `mes_work_order`;
CREATE TABLE `mes_work_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工单号',
  `product_id` bigint NOT NULL COMMENT '产品/成品ID',
  `plan_qty` int NOT NULL DEFAULT 0 COMMENT '计划数量',
  `done_qty` int NOT NULL DEFAULT 0 COMMENT '已产数量',
  `priority` tinyint NOT NULL DEFAULT 1 COMMENT '优先级(1普通,2紧急)',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态(0待下发,1生产中,2完工,3关闭)',
  `plan_start` date NULL DEFAULT NULL COMMENT '计划开工',
  `plan_end` date NULL DEFAULT NULL COMMENT '计划完工',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `pr_order_id` bigint NULL DEFAULT NULL COMMENT '关联生产订单ID(pr_order.id)',
  `source` tinyint NOT NULL DEFAULT 1 COMMENT '来源(1手工,2由生产订单生成)',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '生产工单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_work_order
-- ----------------------------
INSERT INTO `mes_work_order` VALUES (1, 'PR202610003', 1, 200, 180, 1, 1, '2026-11-01', '2026-11-10', '', 3, 2, NULL);
INSERT INTO `mes_work_order` VALUES (2, 'MO202611010001', 2, 120, 0, 2, 0, '2026-11-01', '2026-11-08', '', 5, 2, NULL);

-- ----------------------------
-- Table structure for mes_work_report
-- ----------------------------
DROP TABLE IF EXISTS `mes_work_report`;
CREATE TABLE `mes_work_report`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `report_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '报工单号',
  `production_order_id` bigint NOT NULL COMMENT '生产任务ID',
  `employee_id` bigint NULL DEFAULT NULL COMMENT '操作工ID',
  `good_qty` int NOT NULL DEFAULT 0 COMMENT '合格数',
  `bad_qty` int NOT NULL DEFAULT 0 COMMENT '不良数',
  `work_hours` decimal(10, 2) NOT NULL DEFAULT 0.00 COMMENT '工时',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态(0待确认,1已确认)',
  `report_time` datetime NULL DEFAULT NULL COMMENT '报工时间',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '报工单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_work_report
-- ----------------------------
INSERT INTO `mes_work_report` VALUES (1, 'WR202611010001', 1, NULL, 180, 2, 24.00, 1, '2026-11-01 16:00:00', NULL);
INSERT INTO `mes_work_report` VALUES (2, 'WR202611010002', 2, NULL, 0, 0, 0.00, 0, '2026-11-01 08:00:00', NULL);

-- ----------------------------
-- Table structure for mes_workstation
-- ----------------------------
DROP TABLE IF EXISTS `mes_workstation`;
CREATE TABLE `mes_workstation`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `code` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工位编码',
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工位名称',
  `line` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '产线',
  `remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NULL DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '工位/产线' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of mes_workstation
-- ----------------------------
INSERT INTO `mes_workstation` VALUES (1, 'WS-001', '装配一区', '装配线A', '', NULL);
INSERT INTO `mes_workstation` VALUES (2, 'WS-002', '老化测试区', '装配线A', '', NULL);
INSERT INTO `mes_workstation` VALUES (3, 'WS-003', '检测包装区', '装配线B', '', NULL);

-- ----------------------------
-- Table structure for pay_notify
-- ----------------------------
DROP TABLE IF EXISTS `pay_notify`;
CREATE TABLE `pay_notify`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `notify_no` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `channel` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `biz_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `receivable_id` bigint NULL DEFAULT NULL,
  `amount` decimal(18, 2) NOT NULL DEFAULT 0.00,
  `status` tinyint NOT NULL DEFAULT 0,
  `payload` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `update_time` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_notify_no`(`notify_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pay_notify
-- ----------------------------
INSERT INTO `pay_notify` VALUES (1, 'NOTIFY202610001', 'ALIPAY', 'AR202610001', 1, 80000.00, 1, '{\"trade_no\":\"2026101522001\",\"status\":\"SUCCESS\"}', '2026-10-31 11:53:32', '2026-10-15 10:00:00');
INSERT INTO `pay_notify` VALUES (2, 'NOTIFY202610002', 'WECHAT', 'AR202610002', 2, 45000.00, 1, '{\"transaction_id\":\"4200001234\",\"status\":\"SUCCESS\"}', '2026-10-31 11:53:32', '2026-10-20 14:30:00');

-- ----------------------------
-- Table structure for po_order
-- ----------------------------
DROP TABLE IF EXISTS `po_order`;
CREATE TABLE `po_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '采购订单号',
  `supplier_id` bigint NOT NULL COMMENT '供应商ID',
  `order_date` date NOT NULL COMMENT '下单日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待审/1已审/2部分收货/3已完成',
  `total_amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '订单总金额',
  `created_at` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_po_order_no`(`order_no` ASC) USING BTREE,
  INDEX `idx_po_supplier`(`supplier_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '采购订单主表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of po_order
-- ----------------------------
INSERT INTO `po_order` VALUES (1, 'PO202610001', 1, '2026-10-05', 3, 113000.00, '2026-10-31 11:53:32', 1);
INSERT INTO `po_order` VALUES (2, 'PO202610002', 2, '2026-10-08', 2, 56500.00, '2026-10-31 11:53:32', 0);
INSERT INTO `po_order` VALUES (3, 'PO202610003', 3, '2026-10-12', 3, 22600.00, '2026-10-31 11:53:32', 0);
INSERT INTO `po_order` VALUES (4, 'PO202611020001', 3, '2026-11-02', 1, 30000.00, '2026-11-02 08:46:10', 0);
INSERT INTO `po_order` VALUES (5, 'PO202611020002', 2, '2026-11-02', 0, 5000.00, '2026-11-02 14:40:07', 0);

-- ----------------------------
-- Table structure for po_order_detail
-- ----------------------------
DROP TABLE IF EXISTS `po_order_detail`;
CREATE TABLE `po_order_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `order_id` bigint NOT NULL COMMENT '采购订单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '采购数量',
  `unit_price` decimal(18, 6) NOT NULL COMMENT '含税单价',
  `received_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '已收货数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_pod_order`(`order_id` ASC) USING BTREE,
  INDEX `idx_pod_material`(`material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '采购订单明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of po_order_detail
-- ----------------------------
INSERT INTO `po_order_detail` VALUES (1, 1, 3, 1000.0000, 85.000000, 1000.0000);
INSERT INTO `po_order_detail` VALUES (2, 1, 4, 500.0000, 56.000000, 500.0000);
INSERT INTO `po_order_detail` VALUES (3, 2, 5, 500.0000, 45.000000, 300.0000);
INSERT INTO `po_order_detail` VALUES (4, 2, 6, 1000.0000, 34.000000, 600.0000);
INSERT INTO `po_order_detail` VALUES (5, 3, 4, 400.0000, 56.500000, 400.0000);
INSERT INTO `po_order_detail` VALUES (6, 4, 6, 100.0000, 300.000000, 0.0000);
INSERT INTO `po_order_detail` VALUES (7, 5, 4, 100.0000, 50.000000, 0.0000);

-- ----------------------------
-- Table structure for po_receipt
-- ----------------------------
DROP TABLE IF EXISTS `po_receipt`;
CREATE TABLE `po_receipt`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `receipt_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '收货单号',
  `order_id` bigint NOT NULL COMMENT '采购订单ID',
  `supplier_id` bigint NOT NULL COMMENT '供应商ID',
  `receipt_date` date NOT NULL COMMENT '收货日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待检/1合格/2不合格',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_po_receipt_no`(`receipt_no` ASC) USING BTREE,
  INDEX `idx_pr_order`(`order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '采购收货单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of po_receipt
-- ----------------------------
INSERT INTO `po_receipt` VALUES (1, 'PR202610001', 1, 1, '2026-10-10', 1, '2026-10-31 11:53:32', 1);
INSERT INTO `po_receipt` VALUES (2, 'PR202610002', 2, 2, '2026-10-15', 1, '2026-10-31 11:53:32', 0);
INSERT INTO `po_receipt` VALUES (3, 'PR202610003', 3, 3, '2026-10-18', 1, '2026-10-31 11:53:32', 0);

-- ----------------------------
-- Table structure for po_receipt_detail
-- ----------------------------
DROP TABLE IF EXISTS `po_receipt_detail`;
CREATE TABLE `po_receipt_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `receipt_id` bigint NOT NULL COMMENT '收货单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '收货数量',
  `qualified_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '合格数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_prd_receipt`(`receipt_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '采购收货明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of po_receipt_detail
-- ----------------------------
INSERT INTO `po_receipt_detail` VALUES (1, 1, 3, 1000.0000, 995.0000);
INSERT INTO `po_receipt_detail` VALUES (2, 1, 4, 500.0000, 500.0000);
INSERT INTO `po_receipt_detail` VALUES (3, 2, 5, 300.0000, 298.0000);
INSERT INTO `po_receipt_detail` VALUES (4, 2, 6, 600.0000, 600.0000);
INSERT INTO `po_receipt_detail` VALUES (5, 3, 4, 400.0000, 400.0000);

-- ----------------------------
-- Table structure for po_requisition
-- ----------------------------
DROP TABLE IF EXISTS `po_requisition`;
CREATE TABLE `po_requisition`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `req_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '请购单号',
  `req_date` date NOT NULL COMMENT '请购日期',
  `dept_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '申请部门',
  `applicant_id` bigint NULL DEFAULT NULL COMMENT '申请人ID',
  `applicant_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '申请人姓名',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态：0草稿/1已转采购订单/2已驳回',
  `po_order_id` bigint NULL DEFAULT NULL COMMENT '转生成的销售订单ID',
  `remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_req_no`(`req_no` ASC) USING BTREE,
  INDEX `idx_req_status`(`status` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '请购单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of po_requisition
-- ----------------------------
INSERT INTO `po_requisition` VALUES (1, 'REQ202610001', '2026-10-01', '生产部', 1, '张伟', 1, 1, '原材料补货', '2026-10-31 11:53:32', 0);
INSERT INTO `po_requisition` VALUES (2, 'REQ202610002', '2026-10-05', '仓储部', 2, '李娜', 1, 2, '辅料安全库存补充', '2026-10-31 11:53:32', 0);
INSERT INTO `po_requisition` VALUES (3, 'REQ202610003', '2026-10-10', '研发部', 3, '王强', 0, NULL, '样品试制材料', '2026-10-31 11:53:32', 0);
INSERT INTO `po_requisition` VALUES (4, 'REQ202611020001', '2026-11-02', '测试部', NULL, NULL, 0, NULL, 'auto', '2026-11-02 11:12:11', 2);

-- ----------------------------
-- Table structure for po_requisition_detail
-- ----------------------------
DROP TABLE IF EXISTS `po_requisition_detail`;
CREATE TABLE `po_requisition_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `req_id` bigint NOT NULL COMMENT '请购单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `req_qty` decimal(18, 4) NOT NULL COMMENT '请购数量',
  `converted_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '已转订单数量',
  `suggest_supplier_id` bigint NULL DEFAULT NULL COMMENT '建议供应商ID',
  `suggest_price` decimal(18, 6) NULL DEFAULT 0.000000 COMMENT '建议单价（取报价最优价）',
  `remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_reqd_req`(`req_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '请购单明细' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of po_requisition_detail
-- ----------------------------
INSERT INTO `po_requisition_detail` VALUES (1, 1, 3, 1000.0000, 1000.0000, 1, 85.000000, '芯片紧急采购');
INSERT INTO `po_requisition_detail` VALUES (2, 1, 4, 500.0000, 500.0000, 1, 56.000000, NULL);
INSERT INTO `po_requisition_detail` VALUES (3, 2, 5, 500.0000, 300.0000, 2, 45.000000, '外壳组件');
INSERT INTO `po_requisition_detail` VALUES (4, 2, 6, 1000.0000, 600.0000, 2, 34.000000, NULL);
INSERT INTO `po_requisition_detail` VALUES (5, 3, 3, 50.0000, 0.0000, 1, 85.000000, '样品试制');
INSERT INTO `po_requisition_detail` VALUES (6, 4, 1, 5.0000, 0.0000, NULL, 0.000000, NULL);

-- ----------------------------
-- Table structure for po_return
-- ----------------------------
DROP TABLE IF EXISTS `po_return`;
CREATE TABLE `po_return`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `return_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '退货单号',
  `receipt_id` bigint NULL DEFAULT NULL COMMENT '来源收货单ID',
  `supplier_id` bigint NOT NULL COMMENT '供应商ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '退货数量',
  `return_date` date NOT NULL COMMENT '退货日期',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_po_return_no`(`return_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '采购退货单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of po_return
-- ----------------------------
INSERT INTO `po_return` VALUES (1, 'RT202610001', 1, 1, 3, 5.0000, '2026-10-12');
INSERT INTO `po_return` VALUES (2, 'RT202610002', 2, 2, 5, 2.0000, '2026-10-16');

-- ----------------------------
-- Table structure for po_supplier_quote
-- ----------------------------
DROP TABLE IF EXISTS `po_supplier_quote`;
CREATE TABLE `po_supplier_quote`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `supplier_id` bigint NOT NULL COMMENT '供应商ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quote_price` decimal(18, 6) NOT NULL COMMENT '报价单价',
  `valid_from` date NULL DEFAULT NULL COMMENT '有效期起',
  `valid_to` date NULL DEFAULT NULL COMMENT '有效期止',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_quote_supplier`(`supplier_id` ASC) USING BTREE,
  INDEX `idx_quote_material`(`material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '供应商报价' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of po_supplier_quote
-- ----------------------------
INSERT INTO `po_supplier_quote` VALUES (1, 1, 3, 85.000000, '2026-10-01', '2026-12-31', '2026-10-31 11:53:32');
INSERT INTO `po_supplier_quote` VALUES (2, 1, 4, 56.000000, '2026-10-01', '2026-12-31', '2026-10-31 11:53:32');
INSERT INTO `po_supplier_quote` VALUES (3, 2, 5, 45.000000, '2026-10-01', '2026-12-31', '2026-10-31 11:53:32');
INSERT INTO `po_supplier_quote` VALUES (4, 2, 6, 34.000000, '2026-10-01', '2026-12-31', '2026-10-31 11:53:32');
INSERT INTO `po_supplier_quote` VALUES (5, 3, 4, 56.500000, '2026-10-01', '2026-12-31', '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for pr_material_issue
-- ----------------------------
DROP TABLE IF EXISTS `pr_material_issue`;
CREATE TABLE `pr_material_issue`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `pr_order_id` bigint NOT NULL COMMENT '生产订单ID',
  `material_id` bigint NOT NULL COMMENT '领用物料ID',
  `issue_qty` decimal(18, 4) NOT NULL COMMENT '领用数量',
  `issue_time` datetime NULL DEFAULT CURRENT_TIMESTAMP COMMENT '领用时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_issue_pr`(`pr_order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '生产领料' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_material_issue
-- ----------------------------
INSERT INTO `pr_material_issue` VALUES (1, 1, 3, 1000.0000, '2026-10-15 08:00:00');
INSERT INTO `pr_material_issue` VALUES (2, 1, 4, 500.0000, '2026-10-15 08:10:00');
INSERT INTO `pr_material_issue` VALUES (3, 2, 3, 1500.0000, '2026-10-20 08:00:00');
INSERT INTO `pr_material_issue` VALUES (4, 2, 4, 750.0000, '2026-10-20 08:10:00');

-- ----------------------------
-- Table structure for pr_material_return
-- ----------------------------
DROP TABLE IF EXISTS `pr_material_return`;
CREATE TABLE `pr_material_return`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `pr_order_id` bigint NOT NULL COMMENT '生产订单ID',
  `material_id` bigint NOT NULL COMMENT '退料物料ID',
  `return_qty` decimal(18, 4) NOT NULL COMMENT '退料数量',
  `return_time` datetime NULL DEFAULT CURRENT_TIMESTAMP COMMENT '退料时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_return_pr`(`pr_order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '生产退料' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_material_return
-- ----------------------------
INSERT INTO `pr_material_return` VALUES (1, 1, 3, 20.0000, '2026-10-18 16:00:00');
INSERT INTO `pr_material_return` VALUES (2, 2, 4, 30.0000, '2026-10-25 16:00:00');

-- ----------------------------
-- Table structure for pr_mps
-- ----------------------------
DROP TABLE IF EXISTS `pr_mps`;
CREATE TABLE `pr_mps`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `mps_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'MPS计划号',
  `material_id` bigint NOT NULL COMMENT '成品物料ID',
  `plan_qty` decimal(18, 4) NOT NULL COMMENT '计划数量',
  `plan_start_date` date NULL DEFAULT NULL COMMENT '计划开工日',
  `plan_end_date` date NULL DEFAULT NULL COMMENT '计划完工日',
  `source_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '来源：SO销售/FCST预测/STOCK安全库存',
  `source_id` bigint NULL DEFAULT NULL COMMENT '来源单据ID',
  `status` tinyint NULL DEFAULT 0 COMMENT '0草稿/1确认/2已转生产订单',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_mps_no`(`mps_no` ASC) USING BTREE,
  INDEX `idx_mps_material`(`material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '主生产计划MPS' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_mps
-- ----------------------------
INSERT INTO `pr_mps` VALUES (1, 'MPS202610001', 1, 500.0000, '2026-10-12', '2026-10-20', 'SO', 1, 2, '2026-10-31 11:53:32');
INSERT INTO `pr_mps` VALUES (2, 'MPS202610002', 2, 300.0000, '2026-10-18', '2026-10-26', 'SO', 2, 2, '2026-10-31 11:53:32');
INSERT INTO `pr_mps` VALUES (3, 'MPS202610003', 1, 200.0000, '2026-11-01', '2026-11-10', 'STOCK', NULL, 2, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for pr_mrp
-- ----------------------------
DROP TABLE IF EXISTS `pr_mrp`;
CREATE TABLE `pr_mrp`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `mrp_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'MRP运算号',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `gross_requirement` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '毛需求',
  `on_hand_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '现有库存',
  `on_order_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '在途量',
  `safety_stock` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '安全库存',
  `net_requirement` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '净需求',
  `suggest_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '建议类型：BUY采购/MAKE自制',
  `suggest_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '建议数量',
  `suggest_date` date NULL DEFAULT NULL COMMENT '建议日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0未处理/1已转单',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_mrp_material`(`material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = 'MRP运算结果' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_mrp
-- ----------------------------
INSERT INTO `pr_mrp` VALUES (1, 'MRP202610001', 3, 2000.0000, 500.0000, 1000.0000, 500.0000, 1000.0000, 'BUY', 1000.0000, '2026-10-25', 1, '2026-10-31 11:53:32');
INSERT INTO `pr_mrp` VALUES (2, 'MRP202610002', 4, 1000.0000, 300.0000, 400.0000, 300.0000, 600.0000, 'BUY', 600.0000, '2026-10-25', 1, '2026-10-31 11:53:32');
INSERT INTO `pr_mrp` VALUES (3, 'MRP202610003', 1, 200.0000, 100.0000, 0.0000, 50.0000, 150.0000, 'MAKE', 150.0000, '2026-11-01', 0, '2026-10-31 11:53:32');
INSERT INTO `pr_mrp` VALUES (4, 'MRP202611020001', 1, 200.0000, 0.0000, 0.0000, 50.0000, 250.0000, 'MAKE', 250.0000, '2026-11-01', 0, '2026-11-02 09:50:27');
INSERT INTO `pr_mrp` VALUES (5, 'MRP202611020001', 3, 408.0000, 300.0000, 0.0000, 500.0000, 608.0000, 'BUY', 608.0000, '2026-11-01', 0, '2026-11-02 09:50:27');
INSERT INTO `pr_mrp` VALUES (6, 'MRP202611020001', 4, 202.0000, 400.0000, 0.0000, 300.0000, 102.0000, 'BUY', 102.0000, '2026-11-01', 0, '2026-11-02 09:50:27');

-- ----------------------------
-- Table structure for pr_order
-- ----------------------------
DROP TABLE IF EXISTS `pr_order`;
CREATE TABLE `pr_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '生产制令号',
  `material_id` bigint NOT NULL COMMENT '要生产的成品ID',
  `plan_qty` decimal(18, 4) NOT NULL COMMENT '计划生产数量',
  `completed_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '已完工入库数量',
  `start_date` date NULL DEFAULT NULL COMMENT '计划开工日',
  `end_date` date NULL DEFAULT NULL COMMENT '计划完工日',
  `status` tinyint NULL DEFAULT 0 COMMENT '0计划/1下达/2生产中/3完工/4关闭',
  `source_so_id` bigint NULL DEFAULT NULL COMMENT '来源销售订单ID',
  `source_mps_id` bigint NULL DEFAULT NULL COMMENT '来源MPS ID',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_pr_order_no`(`order_no` ASC) USING BTREE,
  INDEX `idx_pr_mat`(`material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '生产订单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_order
-- ----------------------------
INSERT INTO `pr_order` VALUES (1, 'PR202610001', 1, 500.0000, 500.0000, '2026-10-12', '2026-10-20', 3, 1, 1, '2026-10-31 11:53:32');
INSERT INTO `pr_order` VALUES (2, 'PR202610002', 2, 300.0000, 300.0000, '2026-10-18', '2026-10-26', 3, 2, 2, '2026-10-31 11:53:32');
INSERT INTO `pr_order` VALUES (3, 'PR202610003', 1, 200.0000, 0.0000, '2026-11-01', '2026-11-10', 1, NULL, 3, '2026-10-31 11:53:32');
INSERT INTO `pr_order` VALUES (4, 'MO202610310001', 1, 150.0000, 0.0000, '2026-10-31', '2026-10-31', 0, NULL, NULL, '2026-10-31 11:56:04');
INSERT INTO `pr_order` VALUES (5, 'MO202611010001', 2, 120.0000, 0.0000, '2026-11-01', '2026-11-08', 1, NULL, NULL, '2026-11-01 12:56:00');

-- ----------------------------
-- Table structure for pr_outsourcing
-- ----------------------------
DROP TABLE IF EXISTS `pr_outsourcing`;
CREATE TABLE `pr_outsourcing`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `outsource_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '委外订单号',
  `supplier_id` bigint NOT NULL COMMENT '加工商ID',
  `material_id` bigint NOT NULL COMMENT '委外成品ID',
  `plan_qty` decimal(18, 4) NOT NULL COMMENT '委外数量',
  `received_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '已回收数量',
  `start_date` date NULL DEFAULT NULL COMMENT '开始日期',
  `end_date` date NULL DEFAULT NULL COMMENT '结束日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0计划/1外发/2部分回收/3完成',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_outsource_no`(`outsource_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '委外订单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_outsourcing
-- ----------------------------
INSERT INTO `pr_outsourcing` VALUES (1, 'OS202610001', 2, 5, 500.0000, 300.0000, '2026-10-15', '2026-10-30', 2);
INSERT INTO `pr_outsourcing` VALUES (2, 'OS202610002', 3, 6, 1000.0000, 0.0000, '2026-11-01', '2026-11-15', 1);

-- ----------------------------
-- Table structure for pr_outsourcing_issue
-- ----------------------------
DROP TABLE IF EXISTS `pr_outsourcing_issue`;
CREATE TABLE `pr_outsourcing_issue`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `outsource_id` bigint NOT NULL COMMENT '委外订单ID',
  `material_id` bigint NOT NULL COMMENT '发料物料ID',
  `issue_qty` decimal(18, 4) NOT NULL COMMENT '发料数量',
  `issue_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_os_issue`(`outsource_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '委外发料' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_outsourcing_issue
-- ----------------------------
INSERT INTO `pr_outsourcing_issue` VALUES (1, 1, 3, 500.0000, '2026-10-15 09:00:00');
INSERT INTO `pr_outsourcing_issue` VALUES (2, 1, 4, 250.0000, '2026-10-15 09:10:00');
INSERT INTO `pr_outsourcing_issue` VALUES (3, 2, 6, 1000.0000, '2026-11-01 09:00:00');

-- ----------------------------
-- Table structure for pr_report
-- ----------------------------
DROP TABLE IF EXISTS `pr_report`;
CREATE TABLE `pr_report`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `pr_order_id` bigint NOT NULL COMMENT '生产订单ID',
  `process_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '工序名称',
  `report_qty` decimal(18, 4) NOT NULL COMMENT '报工数量',
  `good_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '良品数量',
  `bad_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '不良品数量',
  `work_hours` decimal(10, 2) NULL DEFAULT 0.00 COMMENT '工时',
  `report_time` datetime NULL DEFAULT CURRENT_TIMESTAMP COMMENT '报工时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_report_pr`(`pr_order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '生产报工' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pr_report
-- ----------------------------
INSERT INTO `pr_report` VALUES (1, 1, 'SMT贴片', 500.0000, 495.0000, 5.0000, 40.00, '2026-10-16 17:00:00');
INSERT INTO `pr_report` VALUES (2, 1, '组装', 495.0000, 490.0000, 5.0000, 35.00, '2026-10-18 17:00:00');
INSERT INTO `pr_report` VALUES (3, 2, 'SMT贴片', 300.0000, 298.0000, 2.0000, 24.00, '2026-10-22 17:00:00');
INSERT INTO `pr_report` VALUES (4, 2, '组装', 298.0000, 295.0000, 3.0000, 20.00, '2026-10-24 17:00:00');

-- ----------------------------
-- Table structure for rpt_kpi
-- ----------------------------
DROP TABLE IF EXISTS `rpt_kpi`;
CREATE TABLE `rpt_kpi`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `kpi_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '指标编码',
  `kpi_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '指标名称',
  `kpi_value` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '指标值',
  `kpi_unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '单位',
  `calc_date` date NULL DEFAULT NULL COMMENT '统计日期',
  `update_time` datetime NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_kpi_code_date`(`kpi_code` ASC, `calc_date` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '看板指标' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rpt_kpi
-- ----------------------------
INSERT INTO `rpt_kpi` VALUES (1, 'KPI001', '本月销售额', 195000.0000, '元', '2026-10-31', '2026-10-31 11:53:32');
INSERT INTO `rpt_kpi` VALUES (2, 'KPI002', '本月采购额', 192100.0000, '元', '2026-10-31', '2026-10-31 11:53:32');
INSERT INTO `rpt_kpi` VALUES (3, 'KPI003', '库存周转率', 4.5000, '次', '2026-10-31', '2026-10-31 11:53:32');
INSERT INTO `rpt_kpi` VALUES (4, 'KPI004', '生产达成率', 98.5000, '%', '2026-10-31', '2026-10-31 11:53:32');
INSERT INTO `rpt_kpi` VALUES (5, 'KPI005', '应收未收金额', 150000.0000, '元', '2026-10-31', '2026-10-31 11:53:32');
INSERT INTO `rpt_kpi` VALUES (6, 'STOCK_ALERT', 'M001 低于安全库存', 0.0000, 'qty', '2026-11-01', '2026-11-01 11:10:29');
INSERT INTO `rpt_kpi` VALUES (7, 'STOCK_ALERT', 'M001 低于安全库存', 5.0000, 'qty', '2026-11-02', '2026-11-02 11:27:54');

-- ----------------------------
-- Table structure for rpt_report
-- ----------------------------
DROP TABLE IF EXISTS `rpt_report`;
CREATE TABLE `rpt_report`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `report_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '报表编码',
  `report_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '报表名称',
  `report_type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '类型：TABLE/CHART',
  `sql_text` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL COMMENT '取数SQL',
  `status` tinyint NULL DEFAULT 1 COMMENT '1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_report_code`(`report_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '报表定义' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rpt_report
-- ----------------------------
INSERT INTO `rpt_report` VALUES (1, 'RPT001', '销售订单明细表', 'TABLE', 'SELECT o.order_no, c.customer_name, o.order_date, o.total_amount FROM so_order o JOIN md_customer c ON o.customer_id=c.id', 1, '2026-10-31 11:53:32');
INSERT INTO `rpt_report` VALUES (2, 'RPT002', '采购订单汇总表', 'TABLE', 'SELECT o.order_no, s.supplier_name, o.order_date, o.total_amount FROM po_order o JOIN md_supplier s ON o.supplier_id=s.id', 1, '2026-10-31 11:53:32');
INSERT INTO `rpt_report` VALUES (3, 'RPT003', '库存台账表', 'TABLE', 'SELECT m.material_code, m.material_name, w.warehouse_name, s.current_qty FROM inv_stock s JOIN md_material m ON s.material_id=m.id JOIN md_warehouse w ON s.warehouse_id=w.id', 1, '2026-10-31 11:53:32');
INSERT INTO `rpt_report` VALUES (4, 'RPT004', '应收账龄分析', 'CHART', 'SELECT c.customer_name, r.amount, r.received_amount, r.due_date FROM fin_receivable r JOIN md_customer c ON r.customer_id=c.id', 1, '2026-10-31 11:53:32');

-- ----------------------------
-- Table structure for so_customer_credit
-- ----------------------------
DROP TABLE IF EXISTS `so_customer_credit`;
CREATE TABLE `so_customer_credit`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `credit_limit` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '信用额度',
  `used_amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '已用额度',
  `status` tinyint NULL DEFAULT 1 COMMENT '1正常/0冻结',
  `update_time` datetime NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_credit_customer`(`customer_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '客户信用' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of so_customer_credit
-- ----------------------------
INSERT INTO `so_customer_credit` VALUES (1, 1, 5000.00, 135000.00, 1, '2026-11-02 11:19:09', 1);
INSERT INTO `so_customer_credit` VALUES (2, 2, 300000.00, 45000.00, 1, '2026-10-31 11:53:32', 0);
INSERT INTO `so_customer_credit` VALUES (3, 3, 200000.00, 30000.00, 1, '2026-10-31 11:53:32', 0);
INSERT INTO `so_customer_credit` VALUES (4, 4, 400000.00, 0.00, 1, '2026-10-31 11:53:32', 0);
INSERT INTO `so_customer_credit` VALUES (5, 5, 150000.00, 0.00, 1, '2026-10-31 11:53:32', 0);

-- ----------------------------
-- Table structure for so_delivery
-- ----------------------------
DROP TABLE IF EXISTS `so_delivery`;
CREATE TABLE `so_delivery`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `delivery_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '发货单号',
  `order_id` bigint NOT NULL COMMENT '销售订单ID',
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `delivery_date` date NOT NULL COMMENT '发货日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0待发/1已发/2已签收',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_delivery_no`(`delivery_no` ASC) USING BTREE,
  INDEX `idx_delivery_order`(`order_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '发货单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of so_delivery
-- ----------------------------
INSERT INTO `so_delivery` VALUES (1, 'DEL202610001', 1, 1, '2026-10-12', 2, '2026-10-31 11:53:32');
INSERT INTO `so_delivery` VALUES (2, 'DEL202610002', 2, 2, '2026-10-18', 2, '2026-10-31 11:53:32');
INSERT INTO `so_delivery` VALUES (3, 'DEL202610003', 3, 3, '2026-10-28', 1, '2026-10-31 11:53:32');
INSERT INTO `so_delivery` VALUES (4, 'DN202611010001', 4, 1, '2026-11-01', 1, '2026-11-01 11:10:29');

-- ----------------------------
-- Table structure for so_delivery_detail
-- ----------------------------
DROP TABLE IF EXISTS `so_delivery_detail`;
CREATE TABLE `so_delivery_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `delivery_id` bigint NOT NULL COMMENT '发货单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '发货数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_dd_delivery`(`delivery_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '发货明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of so_delivery_detail
-- ----------------------------
INSERT INTO `so_delivery_detail` VALUES (1, 1, 1, 20.0000);
INSERT INTO `so_delivery_detail` VALUES (2, 1, 2, 30.0000);
INSERT INTO `so_delivery_detail` VALUES (3, 2, 2, 30.0000);
INSERT INTO `so_delivery_detail` VALUES (4, 3, 1, 40.0000);
INSERT INTO `so_delivery_detail` VALUES (5, 4, 1, 100.0000);

-- ----------------------------
-- Table structure for so_delivery_track
-- ----------------------------
DROP TABLE IF EXISTS `so_delivery_track`;
CREATE TABLE `so_delivery_track`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `delivery_id` bigint NOT NULL COMMENT '发货单ID',
  `carrier` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '承运商',
  `track_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '运单号',
  `track_status` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '状态：已揽收/运输中/派送中/已签收/异常',
  `location` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '当前位置',
  `remark` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `track_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_track_delivery`(`delivery_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '发货物流跟踪' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of so_delivery_track
-- ----------------------------
INSERT INTO `so_delivery_track` VALUES (1, 1, '顺丰速运', 'SF1234567890', '已签收', '北京市朝阳区', '客户已签收', '2026-10-14 15:30:00');
INSERT INTO `so_delivery_track` VALUES (2, 2, '中通快递', 'ZT9876543210', '已签收', '上海市浦东新区', '客户已签收', '2026-10-20 11:00:00');
INSERT INTO `so_delivery_track` VALUES (3, 3, '圆通速递', 'YT1122334455', '运输中', '广州市中转中心', '预计次日达', '2026-10-29 09:00:00');

-- ----------------------------
-- Table structure for so_order
-- ----------------------------
DROP TABLE IF EXISTS `so_order`;
CREATE TABLE `so_order`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '销售订单号',
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `order_date` date NOT NULL COMMENT '订单日期',
  `delivery_date` date NULL DEFAULT NULL COMMENT '要求交货日期',
  `status` tinyint NULL DEFAULT 0 COMMENT '0新建/1已确认/2已发货/3已完成',
  `total_amount` decimal(18, 2) NULL DEFAULT 0.00,
  `created_at` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_so_order_no`(`order_no` ASC) USING BTREE,
  INDEX `idx_so_customer`(`customer_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '销售订单主表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of so_order
-- ----------------------------
INSERT INTO `so_order` VALUES (1, 'SO202610001', 1, '2026-10-01', '2026-10-15', 3, 120000.00, '2026-10-31 11:53:32');
INSERT INTO `so_order` VALUES (2, 'SO202610002', 2, '2026-10-05', '2026-10-20', 3, 45000.00, '2026-10-31 11:53:32');
INSERT INTO `so_order` VALUES (3, 'SO202610003', 3, '2026-10-15', '2026-10-30', 2, 30000.00, '2026-10-31 11:53:32');
INSERT INTO `so_order` VALUES (4, 'SO202611010001', 1, '2026-11-01', '2026-11-03', 1, 15000.00, '2026-11-01 11:10:02');

-- ----------------------------
-- Table structure for so_order_detail
-- ----------------------------
DROP TABLE IF EXISTS `so_order_detail`;
CREATE TABLE `so_order_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `order_id` bigint NOT NULL COMMENT '销售订单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '销售数量',
  `unit_price` decimal(18, 6) NOT NULL COMMENT '单价',
  `shipped_qty` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '已发货数量',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_sod_order`(`order_id` ASC) USING BTREE,
  INDEX `idx_sod_material`(`material_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '销售订单明细' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of so_order_detail
-- ----------------------------
INSERT INTO `so_order_detail` VALUES (1, 1, 1, 20.0000, 4000.000000, 20.0000);
INSERT INTO `so_order_detail` VALUES (2, 1, 2, 30.0000, 1333.333333, 30.0000);
INSERT INTO `so_order_detail` VALUES (3, 2, 2, 30.0000, 1500.000000, 30.0000);
INSERT INTO `so_order_detail` VALUES (4, 3, 1, 40.0000, 750.000000, 40.0000);
INSERT INTO `so_order_detail` VALUES (5, 4, 1, 150.0000, 100.000000, 100.0000);

-- ----------------------------
-- Table structure for so_quote
-- ----------------------------
DROP TABLE IF EXISTS `so_quote`;
CREATE TABLE `so_quote`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `quote_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '报价单号',
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `quote_date` date NOT NULL COMMENT '报价日期',
  `valid_date` date NULL DEFAULT NULL COMMENT '报价有效期',
  `price_strategy` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'STANDARD' COMMENT '价格策略：STANDARD标准/PREFER最低价/VIP折扣/CUSTOM协议价',
  `discount_rate` decimal(6, 4) NULL DEFAULT 1.0000 COMMENT '折扣率（1=不打折）',
  `total_amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '报价总额',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态：0草稿/1已生效/2已转订单/3已失效',
  `order_id` bigint NULL DEFAULT NULL COMMENT '转生成的销售订单ID',
  `remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `audit_status` tinyint NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_quote_no`(`quote_no` ASC) USING BTREE,
  INDEX `idx_quote_customer`(`customer_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '销售报价单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of so_quote
-- ----------------------------
INSERT INTO `so_quote` VALUES (1, 'QT202610001', 1, '2026-09-20', '2026-10-20', 'VIP折扣', 0.9500, 120000.00, 2, 1, 'VIP客户九五折', '2026-10-31 11:53:32', 0);
INSERT INTO `so_quote` VALUES (2, 'QT202610002', 2, '2026-09-25', '2026-10-25', 'STANDARD', 1.0000, 45000.00, 2, 2, '标准报价', '2026-10-31 11:53:32', 0);
INSERT INTO `so_quote` VALUES (3, 'QT202610003', 3, '2026-10-10', '2026-11-10', 'CUSTOM', 0.9800, 30000.00, 1, 3, '协议价', '2026-10-31 11:53:32', 0);
INSERT INTO `so_quote` VALUES (4, 'QT202611020001', 1, '2026-11-02', NULL, 'STANDARD', 1.0000, 20.00, 0, NULL, '', '2026-11-02 11:19:09', 1);

-- ----------------------------
-- Table structure for so_quote_detail
-- ----------------------------
DROP TABLE IF EXISTS `so_quote_detail`;
CREATE TABLE `so_quote_detail`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `quote_id` bigint NOT NULL COMMENT '报价单ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '数量',
  `unit_price` decimal(18, 6) NOT NULL COMMENT '单价',
  `discount_rate` decimal(6, 4) NULL DEFAULT 1.0000 COMMENT '行折扣率',
  `amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '行金额',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_qd_quote`(`quote_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '销售报价单明细' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of so_quote_detail
-- ----------------------------
INSERT INTO `so_quote_detail` VALUES (1, 1, 1, 20.0000, 4000.000000, 0.9500, 76000.00);
INSERT INTO `so_quote_detail` VALUES (2, 1, 2, 30.0000, 1403.508772, 1.0000, 42105.26);
INSERT INTO `so_quote_detail` VALUES (3, 2, 2, 30.0000, 1500.000000, 1.0000, 45000.00);
INSERT INTO `so_quote_detail` VALUES (4, 3, 1, 40.0000, 765.306122, 1.0000, 30612.24);
INSERT INTO `so_quote_detail` VALUES (5, 0, 1, 2.0000, 10.000000, 1.0000, 20.00);

-- ----------------------------
-- Table structure for so_return
-- ----------------------------
DROP TABLE IF EXISTS `so_return`;
CREATE TABLE `so_return`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `return_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '退货单号',
  `delivery_id` bigint NULL DEFAULT NULL COMMENT '来源发货单ID',
  `customer_id` bigint NOT NULL COMMENT '客户ID',
  `material_id` bigint NOT NULL COMMENT '物料ID',
  `quantity` decimal(18, 4) NOT NULL COMMENT '退货数量',
  `return_date` date NOT NULL COMMENT '退货日期',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_so_return_no`(`return_no` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '销售退货单' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of so_return
-- ----------------------------
INSERT INTO `so_return` VALUES (1, 'SR202610001', 1, 1, 1, 2.0000, '2026-10-16');
INSERT INTO `so_return` VALUES (2, 'SR202610002', 2, 2, 2, 1.0000, '2026-10-22');

-- ----------------------------
-- Table structure for sys_approval
-- ----------------------------
DROP TABLE IF EXISTS `sys_approval`;
CREATE TABLE `sys_approval`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `approval_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批单号',
  `biz_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单据类型：SO/PO/PR/VOUCHER/PO_REQ/OTHER',
  `biz_id` bigint NULL DEFAULT NULL COMMENT '业务单据ID',
  `bill_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '业务单据号（冗余，便于查看）',
  `title` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '审批标题',
  `amount` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '金额',
  `applicant_id` bigint NULL DEFAULT NULL COMMENT '申请人ID',
  `applicant_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '申请人姓名',
  `status` tinyint NOT NULL DEFAULT 0 COMMENT '状态：0草稿/1审批中/2已通过/3已驳回',
  `current_node` int NOT NULL DEFAULT 0 COMMENT '当前审批节点序号',
  `total_nodes` int NOT NULL DEFAULT 1 COMMENT '审批节点总数',
  `remark` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '备注',
  `submit_time` datetime NULL DEFAULT NULL COMMENT '提交时间',
  `finish_time` datetime NULL DEFAULT NULL COMMENT '审批完成时间',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `node_roles` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_approval_no`(`approval_no` ASC) USING BTREE,
  INDEX `idx_appr_status`(`status` ASC) USING BTREE,
  INDEX `idx_appr_biz`(`biz_type` ASC, `biz_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 29 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '审批单' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sys_approval
-- ----------------------------
INSERT INTO `sys_approval` VALUES (1, 'APV202610001', 'SO', 1, 'SO202610001', '销售订单审批-北京华信', 120000.00, 1, '张伟', 2, 2, 2, 'VIP客户订单', '2026-10-01 10:00:00', '2026-10-01 15:00:00', '2026-10-31 11:53:32', NULL);
INSERT INTO `sys_approval` VALUES (2, 'APV202610002', 'PO', 1, 'PO202610001', '采购订单审批-华强电子', 113000.00, 2, '李娜', 2, 2, 2, '原材料采购', '2026-10-05 09:00:00', '2026-10-05 14:00:00', '2026-10-31 11:53:32', NULL);
INSERT INTO `sys_approval` VALUES (3, 'APV202610003', 'PR', 3, 'PR202610003', '生产订单审批-智能控制器', 200.00, 3, '王强', 3, 2, 2, '待生产部经理审批', '2026-10-31 09:00:00', '2026-11-02 08:42:11', '2026-10-31 11:53:32', NULL);
INSERT INTO `sys_approval` VALUES (4, 'AP-HR-LEAVE-0001', 'HR_LEAVE', NULL, 'LV-2026-0001', '赵敏 年假申请', NULL, 7, '赵敏', 2, 2, 2, '已通过', '2026-10-31 21:56:01', '2026-10-31 21:56:01', '2026-10-31 21:56:01', NULL);
INSERT INTO `sys_approval` VALUES (5, 'AP-HR-OT-0001', 'HR_OVERTIME', NULL, 'OT-2026-0001', '郑浩 加班申请', NULL, 11, '郑浩', 2, 2, 2, '已通过', '2026-10-31 21:56:01', '2026-10-31 21:56:01', '2026-10-31 21:56:01', NULL);
INSERT INTO `sys_approval` VALUES (6, 'AP-HR-EMP-0001', 'HR_EMP', NULL, 'EM-2026-0001', '郑浩 入职手续', NULL, 11, '郑浩', 2, 2, 2, '已通过', '2026-10-31 21:56:01', '2026-10-31 21:56:01', '2026-10-31 21:56:01', NULL);
INSERT INTO `sys_approval` VALUES (7, 'AP-HR-RECRUIT-0001', 'HR_RECRUIT', NULL, 'RC-2026-0001', '制造一部操作工招聘需求', NULL, 5, '陈刚', 2, 2, 2, '已通过', '2026-10-31 21:56:01', '2026-10-31 21:56:01', '2026-10-31 21:56:01', NULL);
INSERT INTO `sys_approval` VALUES (8, 'AP202611020003', 'PO', 1, 'TEST-PO-1', '测试采购审批', 100.00, NULL, NULL, 1, 0, 2, NULL, '2026-11-02 10:12:09', NULL, '2026-11-02 10:12:09', 'deptManager,financeDirector');
INSERT INTO `sys_approval` VALUES (9, 'AP202611020004', 'REQUISITION', NULL, NULL, '采购请购审批 ', 0.00, NULL, NULL, 1, 0, 1, '新建请购单自动送审', '2026-11-02 11:09:28', NULL, '2026-11-02 11:09:27', 'admin');
INSERT INTO `sys_approval` VALUES (10, 'AP202611020005', 'REQUISITION', NULL, NULL, '采购请购审批 ', 0.00, NULL, NULL, 1, 0, 1, '新建请购单自动送审', '2026-11-02 11:11:37', NULL, '2026-11-02 11:11:37', 'admin');
INSERT INTO `sys_approval` VALUES (11, 'AP202611020006', 'REQUISITION', 4, 'REQ202611020001', '采购请购审批 REQ202611020001', 0.00, NULL, NULL, 2, 1, 1, '新建请购单自动送审', '2026-11-02 11:12:11', '2026-11-02 11:12:11', '2026-11-02 11:12:11', 'admin');
INSERT INTO `sys_approval` VALUES (12, 'AP202611020007', 'RECEIPT', 1, 'RN-test', 'RECEIPT RN-test', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:12:43', '2026-11-02 11:12:43', '2026-11-02 11:12:43', 'admin');
INSERT INTO `sys_approval` VALUES (13, 'AP202611020008', 'PO_CHANGE', 1, 'PO-test', 'PO_CHANGE PO-test', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:12:43', '2026-11-02 11:12:43', '2026-11-02 11:12:43', 'admin');
INSERT INTO `sys_approval` VALUES (14, 'AP202611020009', 'REQUISITION', 4, 'REQ202611020001', 't', 0.00, NULL, NULL, 3, 1, 1, 't', '2026-11-02 11:12:43', '2026-11-02 11:12:43', '2026-11-02 11:12:43', 'admin');
INSERT INTO `sys_approval` VALUES (15, 'AP202611020010', 'QUOTE', NULL, NULL, '销售报价审批 ', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:17:40', '2026-11-02 11:17:40', '2026-11-02 11:17:40', 'admin');
INSERT INTO `sys_approval` VALUES (16, 'AP202611020011', 'CREDIT', NULL, '', '客户信用额度审批 ', 5000.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:17:40', '2026-11-02 11:17:40', '2026-11-02 11:17:40', 'admin');
INSERT INTO `sys_approval` VALUES (17, 'AP202611020012', 'QUOTE', 4, 'QT202611020001', '销售报价审批 QT202611020001', 20.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:19:09', '2026-11-02 11:19:09', '2026-11-02 11:19:09', 'admin');
INSERT INTO `sys_approval` VALUES (18, 'AP202611020013', 'CREDIT', 1, '1', '客户信用额度审批 1', 5000.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:19:09', '2026-11-02 11:19:09', '2026-11-02 11:19:09', 'admin');
INSERT INTO `sys_approval` VALUES (19, 'AP202611020014', 'VOUCHER', 6, 'V202611020002', '记账凭证审批 V202611020002', 100.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:23:45', '2026-11-02 11:23:45', '2026-11-02 11:23:45', 'admin');
INSERT INTO `sys_approval` VALUES (20, 'AP202611020015', 'FIN_RECEIPT', 1, NULL, '收款单审批 ', 100.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:23:45', '2026-11-02 11:23:45', '2026-11-02 11:23:45', 'admin');
INSERT INTO `sys_approval` VALUES (21, 'AP202611020016', 'FIN_PAYMENT', 18268, NULL, '付款单审批 ', 80.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:23:46', '2026-11-02 11:23:46', '2026-11-02 11:23:46', 'admin');
INSERT INTO `sys_approval` VALUES (22, 'AP202611020017', 'IN_ORDER', 4, 'IN202611020001', '入库单审批 IN202611020001', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:27:55', '2026-11-02 11:27:55', '2026-11-02 11:27:54', 'admin');
INSERT INTO `sys_approval` VALUES (23, 'AP202611020018', 'OUT_ORDER', 4, 'OUT202611020001', '出库单审批 OUT202611020001', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:27:55', '2026-11-02 11:27:55', '2026-11-02 11:27:55', 'admin');
INSERT INTO `sys_approval` VALUES (24, 'AP202611020019', 'TRANSFER', NULL, NULL, '调拨单审批 ', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:27:55', '2026-11-02 11:27:55', '2026-11-02 11:27:55', 'admin');
INSERT INTO `sys_approval` VALUES (25, 'AP202611020020', 'TRANSFER', NULL, NULL, '调拨单审批 ', 0.00, NULL, NULL, 2, 1, 1, 't', '2026-11-02 11:31:00', '2026-11-02 11:31:00', '2026-11-02 11:31:00', 'admin');
INSERT INTO `sys_approval` VALUES (26, 'AP202611020021', 'CHECK', 3, 'CK202611020001', '库存盘点审批 CK202611020001', 0.00, NULL, NULL, 1, 0, 1, '盘盈盘亏调整送审，审批通过后自动调整库存', '2026-11-02 11:47:17', NULL, '2026-11-02 11:47:17', 'admin');
INSERT INTO `sys_approval` VALUES (27, 'AP202611020022', 'CHECK', 4, 'CK202611020002', '库存盘点审批 CK202611020002', 0.00, NULL, NULL, 1, 0, 1, '盘盈盘亏调整送审，审批通过后自动调整库存', '2026-11-02 11:47:57', NULL, '2026-11-02 11:47:57', 'admin');
INSERT INTO `sys_approval` VALUES (28, 'AP202611020023', 'PO', 5, 'PO202611020002', '采购订单审批 PO202611020002', 5000.00, NULL, NULL, 1, 0, 2, '新建采购订单自动送审', '2026-11-02 14:40:07', NULL, '2026-11-02 14:40:07', 'deptManager,financeDirector');

-- ----------------------------
-- Table structure for sys_approval_log
-- ----------------------------
DROP TABLE IF EXISTS `sys_approval_log`;
CREATE TABLE `sys_approval_log`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `approval_id` bigint NOT NULL COMMENT '审批单ID',
  `node_no` int NOT NULL DEFAULT 0 COMMENT '节点序号，0=提交',
  `action` tinyint NOT NULL COMMENT '动作：1提交/2通过/3驳回/4撤回',
  `approver_id` bigint NULL DEFAULT NULL COMMENT '审批人ID',
  `approver_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批人姓名',
  `opinion` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '审批意见',
  `oper_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_alog_approval`(`approval_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 50 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '审批记录' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sys_approval_log
-- ----------------------------
INSERT INTO `sys_approval_log` VALUES (1, 1, 0, 1, 1, '张伟', '提交审批', '2026-10-01 10:00:00');
INSERT INTO `sys_approval_log` VALUES (2, 1, 1, 2, 4, '赵经理', '同意', '2026-10-01 12:00:00');
INSERT INTO `sys_approval_log` VALUES (3, 1, 2, 2, 5, '钱总监', '同意，优先安排', '2026-10-01 15:00:00');
INSERT INTO `sys_approval_log` VALUES (4, 2, 0, 1, 2, '李娜', '提交审批', '2026-10-05 09:00:00');
INSERT INTO `sys_approval_log` VALUES (5, 2, 1, 2, 4, '赵经理', '同意', '2026-10-05 11:00:00');
INSERT INTO `sys_approval_log` VALUES (6, 2, 2, 2, 5, '钱总监', '同意', '2026-10-05 14:00:00');
INSERT INTO `sys_approval_log` VALUES (7, 3, 0, 1, 3, '王强', '提交审批', '2026-10-31 09:00:00');
INSERT INTO `sys_approval_log` VALUES (8, 4, 0, 1, NULL, '赵敏', '提交年假申请', '2026-10-31 21:56:01');
INSERT INTO `sys_approval_log` VALUES (9, 4, 1, 2, 2, '李娜', '同意，已安排工作交接', '2026-10-31 21:56:01');
INSERT INTO `sys_approval_log` VALUES (10, 5, 1, 2, 2, '李娜', '同意加班', '2026-10-31 21:56:01');
INSERT INTO `sys_approval_log` VALUES (11, 6, 1, 2, 2, '李娜', '入职材料齐全', '2026-10-31 21:56:01');
INSERT INTO `sys_approval_log` VALUES (12, 7, 1, 2, 5, '陈刚', '同意招聘', '2026-10-31 21:56:01');
INSERT INTO `sys_approval_log` VALUES (13, 3, 2, 3, NULL, '审批人', '', '2026-11-02 08:42:11');
INSERT INTO `sys_approval_log` VALUES (14, 8, 0, 1, NULL, '申请人', NULL, '2026-11-02 10:12:09');
INSERT INTO `sys_approval_log` VALUES (15, 9, 0, 1, NULL, '申请人', '新建请购单自动送审', '2026-11-02 11:09:28');
INSERT INTO `sys_approval_log` VALUES (16, 10, 0, 1, NULL, '申请人', '新建请购单自动送审', '2026-11-02 11:11:37');
INSERT INTO `sys_approval_log` VALUES (17, 11, 0, 1, NULL, '申请人', '新建请购单自动送审', '2026-11-02 11:12:11');
INSERT INTO `sys_approval_log` VALUES (18, 11, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:12:11');
INSERT INTO `sys_approval_log` VALUES (19, 12, 0, 1, NULL, '申请人', 't', '2026-11-02 11:12:43');
INSERT INTO `sys_approval_log` VALUES (20, 12, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:12:43');
INSERT INTO `sys_approval_log` VALUES (21, 13, 0, 1, NULL, '申请人', 't', '2026-11-02 11:12:43');
INSERT INTO `sys_approval_log` VALUES (22, 13, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:12:43');
INSERT INTO `sys_approval_log` VALUES (23, 14, 0, 1, NULL, '申请人', 't', '2026-11-02 11:12:43');
INSERT INTO `sys_approval_log` VALUES (24, 14, 1, 3, NULL, 'admin', 'no', '2026-11-02 11:12:43');
INSERT INTO `sys_approval_log` VALUES (25, 15, 0, 1, NULL, '申请人', 't', '2026-11-02 11:17:40');
INSERT INTO `sys_approval_log` VALUES (26, 15, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:17:40');
INSERT INTO `sys_approval_log` VALUES (27, 16, 0, 1, NULL, '申请人', 't', '2026-11-02 11:17:40');
INSERT INTO `sys_approval_log` VALUES (28, 16, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:17:40');
INSERT INTO `sys_approval_log` VALUES (29, 17, 0, 1, NULL, '申请人', 't', '2026-11-02 11:19:09');
INSERT INTO `sys_approval_log` VALUES (30, 17, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:19:09');
INSERT INTO `sys_approval_log` VALUES (31, 18, 0, 1, NULL, '申请人', 't', '2026-11-02 11:19:09');
INSERT INTO `sys_approval_log` VALUES (32, 18, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:19:09');
INSERT INTO `sys_approval_log` VALUES (33, 19, 0, 1, NULL, '申请人', 't', '2026-11-02 11:23:45');
INSERT INTO `sys_approval_log` VALUES (34, 19, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:23:45');
INSERT INTO `sys_approval_log` VALUES (35, 20, 0, 1, NULL, '申请人', 't', '2026-11-02 11:23:45');
INSERT INTO `sys_approval_log` VALUES (36, 20, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:23:45');
INSERT INTO `sys_approval_log` VALUES (37, 21, 0, 1, NULL, '申请人', 't', '2026-11-02 11:23:46');
INSERT INTO `sys_approval_log` VALUES (38, 21, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:23:46');
INSERT INTO `sys_approval_log` VALUES (39, 22, 0, 1, NULL, '申请人', 't', '2026-11-02 11:27:55');
INSERT INTO `sys_approval_log` VALUES (40, 22, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:27:55');
INSERT INTO `sys_approval_log` VALUES (41, 23, 0, 1, NULL, '申请人', 't', '2026-11-02 11:27:55');
INSERT INTO `sys_approval_log` VALUES (42, 23, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:27:55');
INSERT INTO `sys_approval_log` VALUES (43, 24, 0, 1, NULL, '申请人', 't', '2026-11-02 11:27:55');
INSERT INTO `sys_approval_log` VALUES (44, 24, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:27:55');
INSERT INTO `sys_approval_log` VALUES (45, 25, 0, 1, NULL, '申请人', 't', '2026-11-02 11:31:00');
INSERT INTO `sys_approval_log` VALUES (46, 25, 1, 2, NULL, 'admin', 'ok', '2026-11-02 11:31:00');
INSERT INTO `sys_approval_log` VALUES (47, 26, 0, 1, NULL, '申请人', '盘盈盘亏调整送审，审批通过后自动调整库存', '2026-11-02 11:47:17');
INSERT INTO `sys_approval_log` VALUES (48, 27, 0, 1, NULL, '申请人', '盘盈盘亏调整送审，审批通过后自动调整库存', '2026-11-02 11:47:57');
INSERT INTO `sys_approval_log` VALUES (49, 28, 0, 1, NULL, '申请人', '新建采购订单自动送审', '2026-11-02 14:40:07');

-- ----------------------------
-- Table structure for sys_approval_rule
-- ----------------------------
DROP TABLE IF EXISTS `sys_approval_rule`;
CREATE TABLE `sys_approval_rule`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `biz_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `min_amount` decimal(18, 2) NOT NULL DEFAULT 0.00,
  `max_amount` decimal(18, 2) NULL DEFAULT NULL,
  `level` int NOT NULL DEFAULT 1,
  `node_roles` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'admin',
  `enabled` tinyint NULL DEFAULT 1,
  `remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_rule_biz`(`biz_type` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_approval_rule
-- ----------------------------
INSERT INTO `sys_approval_rule` VALUES (2, 'PO', 0.00, NULL, 2, 'deptManager,financeDirector', 1, NULL, '2026-11-02 10:12:08');

-- ----------------------------
-- Table structure for sys_bill_change
-- ----------------------------
DROP TABLE IF EXISTS `sys_bill_change`;
CREATE TABLE `sys_bill_change`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `bill_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '单据类型：SO/PO',
  `bill_id` bigint NOT NULL COMMENT '单据ID',
  `bill_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '单据号',
  `change_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '变更类型：QTY数量/PRICE价格/DATE交期/ADD新增行/DEL删除行',
  `before_value` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '变更前',
  `after_value` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '变更后',
  `reason` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '变更原因',
  `oper_user` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '操作人',
  `oper_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_change_bill`(`bill_type` ASC, `bill_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '单据变更记录' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of sys_bill_change
-- ----------------------------
INSERT INTO `sys_bill_change` VALUES (1, 'SO', 1, 'SO202610001', 'QTY', '18', '20', '客户追加订单数量', '张伟', '2026-10-02 10:00:00');
INSERT INTO `sys_bill_change` VALUES (2, 'SO', 2, 'SO202610002', 'DATE', '2026-10-18', '2026-10-20', '客户要求延期交货', '李娜', '2026-10-06 09:00:00');
INSERT INTO `sys_bill_change` VALUES (3, 'PO', 1, 'PO202610001', 'PRICE', '86.00', '85.00', '供应商降价', '李娜', '2026-10-05 16:00:00');
INSERT INTO `sys_bill_change` VALUES (4, 'PO', 2, 'PO202610002', 'QTY', '400', '500', '增加采购量', '李娜', '2026-10-09 10:00:00');

-- ----------------------------
-- Table structure for sys_dept_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_dept_role`;
CREATE TABLE `sys_dept_role`  (
  `id` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '主键（GUID，NewSequentialGuid）',
  `department_id` bigint NOT NULL DEFAULT 0 COMMENT '部门ID',
  `role_id` bigint NOT NULL DEFAULT 0 COMMENT '角色ID（sys_role.id）',
  `is_ban` tinyint NOT NULL DEFAULT 0 COMMENT '是否禁用：1禁用/0正常',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_dept_role`(`department_id` ASC, `role_id` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '部门角色关联（RBAC）' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dept_role
-- ----------------------------
INSERT INTO `sys_dept_role` VALUES ('a1b2c3d4e5f64789a0b1c2d3e4f50001', 1, 1, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role` VALUES ('a1b2c3d4e5f64789a0b1c2d3e4f50002', 2, 2, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role` VALUES ('a1b2c3d4e5f64789a0b1c2d3e4f50003', 3, 3, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role` VALUES ('DR-HR', 0, 5, 0, '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role` VALUES ('DR-MES', 0, 6, 0, '2026-10-31 23:29:22');

-- ----------------------------
-- Table structure for sys_dept_role_menu
-- ----------------------------
DROP TABLE IF EXISTS `sys_dept_role_menu`;
CREATE TABLE `sys_dept_role_menu`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '主键',
  `dept_role_id` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '部门角色ID（sys_dept_role.id）',
  `menu_id` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '菜单ID（sys_menu.id）',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_dept_role_id`(`dept_role_id` ASC) USING BTREE,
  INDEX `idx_menu_id`(`menu_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 46 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '部门角色菜单关联（RBAC）' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_dept_role_menu
-- ----------------------------
INSERT INTO `sys_dept_role_menu` VALUES (1, 'a1b2c3d4e5f64789a0b1c2d3e4f50001', 'm0000000000000000000000000000001', '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role_menu` VALUES (2, 'a1b2c3d4e5f64789a0b1c2d3e4f50001', 'm0000000000000000000000000000002', '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role_menu` VALUES (3, 'a1b2c3d4e5f64789a0b1c2d3e4f50002', 'm0000000000000000000000000000003', '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role_menu` VALUES (4, 'a1b2c3d4e5f64789a0b1c2d3e4f50003', 'm0000000000000000000000000000004', '2026-10-31 11:53:32');
INSERT INTO `sys_dept_role_menu` VALUES (5, 'DR-HR', 'HR-attendance', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (6, 'DR-HR', 'HR-candidate', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (7, 'DR-HR', 'HR-contract', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (8, 'DR-HR', 'HR-department', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (9, 'DR-HR', 'HR-employee', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (10, 'DR-HR', 'HR-employee-change', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (11, 'DR-HR', 'HR-employee-salary', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (12, 'DR-HR', 'HR-interview', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (13, 'DR-HR', 'HR-leave', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (14, 'DR-HR', 'HR-offer', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (15, 'DR-HR', 'HR-overtime', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (16, 'DR-HR', 'HR-perf-detail', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (17, 'DR-HR', 'HR-perf-indicator', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (18, 'DR-HR', 'HR-perf-plan', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (19, 'DR-HR', 'HR-perf-result', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (20, 'DR-HR', 'HR-position', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (21, 'DR-HR', 'HR-position-quota', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (22, 'DR-HR', 'HR-recruit-plan', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (23, 'DR-HR', 'HR-salary', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (24, 'DR-HR', 'HR-salary-detail', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (25, 'DR-HR', 'HR-salary-item', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (26, 'DR-HR', 'HR-schedule', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (27, 'DR-HR', 'HR-shift', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (28, 'DR-HR', 'HR-social-rule', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (29, 'DR-HR', 'HR-tax-bracket', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (30, 'DR-HR', 'HR-train-expense', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (31, 'DR-HR', 'HR-train-need', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (32, 'DR-HR', 'HR-train-plan', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (33, 'DR-HR', 'HR-train-record', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (34, 'DR-HR', 'HR-welfare', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (36, 'DR-MES', 'MES-material-issue', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (37, 'DR-MES', 'MES-production-order', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (38, 'DR-MES', 'MES-work-order', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (39, 'DR-MES', 'MES-work-report', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (40, 'DR-MES', 'MES-workstation', '2026-10-31 23:29:22');
INSERT INTO `sys_dept_role_menu` VALUES (43, 'DR-MES', 'MES-quality-check', '2026-10-31 23:48:31');
INSERT INTO `sys_dept_role_menu` VALUES (44, 'DR-MES', 'MES-routing', '2026-10-31 23:48:31');
INSERT INTO `sys_dept_role_menu` VALUES (45, 'DR-MES', 'MES-routing-step', '2026-10-31 23:48:31');

-- ----------------------------
-- Table structure for sys_ex_log
-- ----------------------------
DROP TABLE IF EXISTS `sys_ex_log`;
CREATE TABLE `sys_ex_log`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `request_path` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `message` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `stack_trace` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_ex_log
-- ----------------------------
INSERT INTO `sys_ex_log` VALUES (1, '/api/so/order/create', 'NullPointerException: customer_id is null', 'java.lang.NullPointerException\n	at com.erp.so.SoOrderService.create(SoOrderService.java:45)', '2026-10-31 11:53:32');
INSERT INTO `sys_ex_log` VALUES (2, '/api/inv/stock/query', 'SQLTimeoutException: query timeout', 'java.sql.SQLTimeoutException\n	at com.erp.inv.StockMapper.query(StockMapper.java:88)', '2026-10-31 11:53:32');
INSERT INTO `sys_ex_log` VALUES (3, '/api/v1/mes/biz/work-order/sync-from-pr', 'Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.', 'System.InvalidOperationException: Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.\r\n   at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.ThrowHelperUnableToResolveService(Type type, Type requiredBy)\r\n   at lambda_method1726(Closure, IServiceProvider, Object[])\r\n   at Microsoft.AspNetCore.Mvc.Controllers.ControllerFactoryProvider.<>c__DisplayClass6_0.<CreateControllerFactory>g__CreateController|0(ControllerContext controllerContext)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 12:56:00');
INSERT INTO `sys_ex_log` VALUES (4, '/api/v1/mes/biz/work-order/sync-from-pr', 'Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.', 'System.InvalidOperationException: Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.\r\n   at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.ThrowHelperUnableToResolveService(Type type, Type requiredBy)\r\n   at lambda_method1726(Closure, IServiceProvider, Object[])\r\n   at Microsoft.AspNetCore.Mvc.Controllers.ControllerFactoryProvider.<>c__DisplayClass6_0.<CreateControllerFactory>g__CreateController|0(ControllerContext controllerContext)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 12:56:00');
INSERT INTO `sys_ex_log` VALUES (5, '/api/v1/mes/biz/work-order/sync-from-pr', 'Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.', 'System.InvalidOperationException: Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.\r\n   at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.ThrowHelperUnableToResolveService(Type type, Type requiredBy)\r\n   at lambda_method1685(Closure, IServiceProvider, Object[])\r\n   at Microsoft.AspNetCore.Mvc.Controllers.ControllerFactoryProvider.<>c__DisplayClass6_0.<CreateControllerFactory>g__CreateController|0(ControllerContext controllerContext)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:40:58');
INSERT INTO `sys_ex_log` VALUES (6, '/api/v1/mes/biz/work-order/sync-from-pr', 'Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.', 'System.InvalidOperationException: Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.\r\n   at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.ThrowHelperUnableToResolveService(Type type, Type requiredBy)\r\n   at lambda_method1685(Closure, IServiceProvider, Object[])\r\n   at Microsoft.AspNetCore.Mvc.Controllers.ControllerFactoryProvider.<>c__DisplayClass6_0.<CreateControllerFactory>g__CreateController|0(ControllerContext controllerContext)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:41:01');
INSERT INTO `sys_ex_log` VALUES (7, '/api/v1/mes/biz/work-order/sync-from-pr', 'Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.', 'System.InvalidOperationException: Unable to resolve service for type \'Demo.Service.Interface.IMesService\' while attempting to activate \'Demo.Api.Admin.Controllers.MesBizController\'.\r\n   at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.ThrowHelperUnableToResolveService(Type type, Type requiredBy)\r\n   at lambda_method1685(Closure, IServiceProvider, Object[])\r\n   at Microsoft.AspNetCore.Mvc.Controllers.ControllerFactoryProvider.<>c__DisplayClass6_0.<CreateControllerFactory>g__CreateController|0(ControllerContext controllerContext)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()\r\n--- End of stack trace from previous location ---\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:49:55');
INSERT INTO `sys_ex_log` VALUES (8, '/api/v1/mes/biz/work-order/999/dispatch', '工单不存在', 'Demo.Infrastructure.Dto.Biz_Exception: 工单不存在\r\n   at Demo.Service.Insance.MesService.DispatchWorkOrderAsync(Int64 workOrderId, String operatorName) in D:\\API\\ERP\\Demo\\Demo.Service.Insance\\MesService.cs:line 42\r\n   at Demo.Api.Admin.Controllers.MesBizController.Dispatch(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Controllers\\MesBizController.cs:line 29\r\n   at lambda_method372(Closure, Object)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:55:28');
INSERT INTO `sys_ex_log` VALUES (9, '/api/v1/mes/biz/production-order/999/issue', '生产任务不存在', 'Demo.Infrastructure.Dto.Biz_Exception: 生产任务不存在\r\n   at Demo.Service.Insance.MesService.IssueMaterialAsync(Int64 productionOrderId, String operatorName) in D:\\API\\ERP\\Demo\\Demo.Service.Insance\\MesService.cs:line 89\r\n   at Demo.Api.Admin.Controllers.MesBizController.Issue(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Controllers\\MesBizController.cs:line 59\r\n   at lambda_method443(Closure, Object)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:55:28');
INSERT INTO `sys_ex_log` VALUES (10, '/api/v1/mes/biz/production-order/999/report', '生产任务不存在', 'Demo.Infrastructure.Dto.Biz_Exception: 生产任务不存在\r\n   at Demo.Service.Insance.MesService.ReportWorkAsync(Int64 productionOrderId, String operatorName) in D:\\API\\ERP\\Demo\\Demo.Service.Insance\\MesService.cs:line 131\r\n   at Demo.Api.Admin.Controllers.MesBizController.Report(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Controllers\\MesBizController.cs:line 69\r\n   at lambda_method456(Closure, Object)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:55:28');
INSERT INTO `sys_ex_log` VALUES (11, '/api/v1/mes/biz/work-report/999/qc', '报工单不存在', 'Demo.Infrastructure.Dto.Biz_Exception: 报工单不存在\r\n   at Demo.Service.Insance.MesService.QualityCheckAsync(Int64 workReportId, String operatorName) in D:\\API\\ERP\\Demo\\Demo.Service.Insance\\MesService.cs:line 174\r\n   at Demo.Api.Admin.Controllers.MesBizController.QualityCheck(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Controllers\\MesBizController.cs:line 79\r\n   at lambda_method464(Closure, Object)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:55:29');
INSERT INTO `sys_ex_log` VALUES (12, '/api/v1/mes/biz/work-order/999/sync-pr', '工单不存在', 'Demo.Infrastructure.Dto.Biz_Exception: 工单不存在\r\n   at Demo.Service.Insance.MesService.SyncWorkOrderToPrAsync(Int64 workOrderId, String operatorName) in D:\\API\\ERP\\Demo\\Demo.Service.Insance\\MesService.cs:line 241\r\n   at Demo.Api.Admin.Controllers.MesBizController.SyncToPr(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Controllers\\MesBizController.cs:line 49\r\n   at lambda_method477(Closure, Object)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-01 13:55:29');
INSERT INTO `sys_ex_log` VALUES (13, '/api/v1/approval/rule/1', '规则不存在', 'Demo.Infrastructure.Dto.Biz_Exception: 规则不存在\r\n   at Demo.Service.Instance.Approval_Service.DeleteRuleAsync(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Service.Insance\\Approval_Service.cs:line 296\r\n   at Demo.Api.Admin.Controllers.ApprovalController.DeleteRule(Int64 id) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Controllers\\ApprovalController.cs:line 80\r\n   at lambda_method645(Closure, Object)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\r\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\r\n   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)\r\n   at Demo.Api.Admin.Middles.Exception_MiddleWare.InvokeAsync(HttpContext context, IConfiguration configuration, I_Log_Service log) in D:\\API\\ERP\\Demo\\Demo.Api.Admin\\Middles\\Exception_MiddleWare.cs:line 23', '2026-11-02 10:12:09');

-- ----------------------------
-- Table structure for sys_menu
-- ----------------------------
DROP TABLE IF EXISTS `sys_menu`;
CREATE TABLE `sys_menu`  (
  `id` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '主键（GUID，NewSequentialGuid）',
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '菜单名称',
  `path` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '路由/权限路径（如 /sys/menu，对应 Permission_Attribute 的 key）',
  `icon` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '图标',
  `type` tinyint NOT NULL DEFAULT 2 COMMENT '类型：1目录/2菜单/3按钮',
  `sort` int NOT NULL DEFAULT 0 COMMENT '排序',
  `is_ban` tinyint NOT NULL DEFAULT 0 COMMENT '是否禁用：1禁用/0正常',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_path`(`path` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '系统菜单（RBAC）' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_menu
-- ----------------------------
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000001', '主数据', '/md', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000002', '销售', '/so', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000003', '采购', '/po', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000004', '库存', '/inv', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000005', '生产', '/pr', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000006', '财务', '/fin', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000007', '决策', '/rpt', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000008', '系统', '/sys', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('10000000000000000000000000000009', '审批流', '/sys/approval', NULL, 2, 0, 0, '2026-10-31 09:08:44');
INSERT INTO `sys_menu` VALUES ('1716b17b0f914838ad853e29567f0f6b', '销售', '/so', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('21e466817aa54e8fbe60af5c077c8743', '生产', '/pr', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('3821609e93094c1fb38ee2acb00beaf1', '系统', '/sys', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('8ab9fbd0644a4738b286251b0a290658', '采购', '/po', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('a3446338fa3f4125a469b6191e279396', '主数据', '/md', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('c6f7c75484614af68c2205f1d488bd5e', '库存', '/inv', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('fdf549b200aa40238200a944ab39ae61', '财务', '/fin', NULL, 2, 0, 0, '2026-10-30 19:32:45');
INSERT INTO `sys_menu` VALUES ('HR-attendance', '考勤打卡', '/hr/attendance', NULL, 3, 9, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-candidate', '候选人库', '/hr/candidate', NULL, 3, 20, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-contract', '劳动合同', '/hr/contract', NULL, 3, 6, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-department', '部门管理', '/hr/department', NULL, 3, 1, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-employee', '员工档案', '/hr/employee', NULL, 3, 4, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-employee-change', '人事异动', '/hr/employee-change', NULL, 3, 5, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-employee-salary', '薪资档案', '/hr/employee-salary', NULL, 3, 13, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-interview', '面试记录', '/hr/interview', NULL, 3, 21, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-leave', '请假管理', '/hr/leave', NULL, 3, 10, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-offer', '录用Offer', '/hr/offer', NULL, 3, 22, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-overtime', '加班管理', '/hr/overtime', NULL, 3, 11, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-perf-detail', '考核明细', '/hr/perf-detail', NULL, 3, 26, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-perf-indicator', '考核指标', '/hr/perf-indicator', NULL, 3, 23, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-perf-plan', '考核计划', '/hr/perf-plan', NULL, 3, 24, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-perf-result', '考核结果', '/hr/perf-result', NULL, 3, 25, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-position', '岗位体系', '/hr/position', NULL, 3, 2, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-position-quota', '岗位编制', '/hr/position-quota', NULL, 3, 3, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-recruit-plan', '招聘需求', '/hr/recruit-plan', NULL, 3, 19, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-salary', '薪资核算', '/hr/salary', NULL, 3, 14, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-salary-detail', '薪资明细', '/hr/salary-detail', NULL, 3, 15, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-salary-item', '薪资项目', '/hr/salary-item', NULL, 3, 12, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-schedule', '排班计划', '/hr/schedule', NULL, 3, 8, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-shift', '排班班次', '/hr/shift', NULL, 3, 7, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-social-rule', '社保规则', '/hr/social-rule', NULL, 3, 16, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-tax-bracket', '个税税率', '/hr/tax-bracket', NULL, 3, 17, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-train-expense', '培训费用', '/hr/train-expense', NULL, 3, 30, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-train-need', '培训需求', '/hr/train-need', NULL, 3, 27, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-train-plan', '培训计划', '/hr/train-plan', NULL, 3, 28, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-train-record', '培训记录', '/hr/train-record', NULL, 3, 29, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('HR-welfare', '员工福利', '/hr/welfare', NULL, 3, 18, 0, '2026-10-31 22:49:43');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000001', '销售管理', '/so', 'shop', 1, 1, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000002', '销售订单', '/so/order', 'file-text', 2, 2, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000003', '采购管理', '/po', 'shopping-cart', 1, 3, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000004', '采购订单', '/po/order', 'file-text', 2, 4, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000005', '库存管理', '/inv', 'database', 1, 5, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000006', '生产管理', '/pr', 'tool', 1, 6, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000007', '财务管理', '/fin', 'account-book', 1, 7, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000008', '报表管理', '/rpt', 'bar-chart', 1, 8, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('m0000000000000000000000000000009', '系统管理', '/sys', 'setting', 1, 9, 0, '2026-10-31 11:53:32');
INSERT INTO `sys_menu` VALUES ('MES-material-issue', '领料管理', '/mes/material-issue', NULL, 3, 4, 0, '2026-10-31 23:28:26');
INSERT INTO `sys_menu` VALUES ('MES-production-order', '生产任务', '/mes/production-order', NULL, 3, 3, 0, '2026-10-31 23:28:26');
INSERT INTO `sys_menu` VALUES ('MES-quality-check', '质量检验', '/mes/quality-check', NULL, 3, 8, 0, '2026-10-31 23:48:31');
INSERT INTO `sys_menu` VALUES ('MES-routing', '工艺路线', '/mes/routing', NULL, 3, 6, 0, '2026-10-31 23:48:31');
INSERT INTO `sys_menu` VALUES ('MES-routing-step', '工艺工序', '/mes/routing-step', NULL, 3, 7, 0, '2026-10-31 23:48:31');
INSERT INTO `sys_menu` VALUES ('MES-work-order', '工单管理', '/mes/work-order', NULL, 3, 1, 0, '2026-10-31 23:28:26');
INSERT INTO `sys_menu` VALUES ('MES-work-report', '报工管理', '/mes/work-report', NULL, 3, 5, 0, '2026-10-31 23:28:26');
INSERT INTO `sys_menu` VALUES ('MES-workstation', '工位管理', '/mes/workstation', NULL, 3, 2, 0, '2026-10-31 23:28:26');

-- ----------------------------
-- Table structure for sys_oper_log
-- ----------------------------
DROP TABLE IF EXISTS `sys_oper_log`;
CREATE TABLE `sys_oper_log`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `user_id` bigint NULL DEFAULT NULL COMMENT '操作人ID',
  `module` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '模块',
  `action` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '操作类型',
  `detail` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '操作详情',
  `oper_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_oper_time`(`oper_time` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 188 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '操作日志' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_oper_log
-- ----------------------------
INSERT INTO `sys_oper_log` VALUES (1, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 09:28:43');
INSERT INTO `sys_oper_log` VALUES (2, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 10:39:31');
INSERT INTO `sys_oper_log` VALUES (3, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 10:40:12');
INSERT INTO `sys_oper_log` VALUES (4, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 10:43:12');
INSERT INTO `sys_oper_log` VALUES (5, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 10:43:12');
INSERT INTO `sys_oper_log` VALUES (6, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 10:51:35');
INSERT INTO `sys_oper_log` VALUES (7, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 10:51:57');
INSERT INTO `sys_oper_log` VALUES (8, NULL, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 11:20:02');
INSERT INTO `sys_oper_log` VALUES (9, 1, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 11:36:11');
INSERT INTO `sys_oper_log` VALUES (10, 1, '销售订单', '新增', '新增销售订单SO202610001', '2026-10-01 09:30:00');
INSERT INTO `sys_oper_log` VALUES (11, 2, '采购订单', '审核', '审核采购订单PO202610001', '2026-10-05 14:00:00');
INSERT INTO `sys_oper_log` VALUES (12, 3, '生产订单', '下达', '下达生产订单PR202610001', '2026-10-12 08:00:00');
INSERT INTO `sys_oper_log` VALUES (13, 1, '库存管理', '入库', '采购入库IN202610001', '2026-10-10 09:00:00');
INSERT INTO `sys_oper_log` VALUES (14, 2, '财务管理', '付款', '付款单PAYF202610001核销应付单PAY202610001', '2026-10-18 10:00:00');
INSERT INTO `sys_oper_log` VALUES (15, 1, '系统', '登录', '/api/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-10-31 11:54:55');
INSERT INTO `sys_oper_log` VALUES (16, 1, '生产', '创建工单', '/api/pr {\"materialId\":1,\"planQty\":150,\"startDate\":\"2026-10-31\",\"endDate\":\"2026-10-31\"}', '2026-10-31 11:56:04');
INSERT INTO `sys_oper_log` VALUES (17, NULL, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 10:22:41');
INSERT INTO `sys_oper_log` VALUES (18, NULL, '系统', '登录', '/api/v1/auth/login {\r\n    \"password\":  ******\"123456\",\r\n    \"username\":  \"admin\"\r\n}', '2026-11-01 11:01:12');
INSERT INTO `sys_oper_log` VALUES (19, 1, '销售', '创建订单', '/api/v1/so {\"customerId\":1,\"orderDate\":\"2026-11-01\",\"deliveryDate\":\"2026-11-03\",\"lines\":[{\"materialId\":1,\"quantity\":150,\"unitPrice\":100}]}', '2026-11-01 11:10:02');
INSERT INTO `sys_oper_log` VALUES (20, 1, '销售', '确认订单', '/api/v1/so/4/confirm ', '2026-11-01 11:10:06');
INSERT INTO `sys_oper_log` VALUES (21, NULL, '库存预警', 'ALERT', 'M001 仓库1 现存量0.0000 安全库存50.0000', '2026-11-01 11:10:30');
INSERT INTO `sys_oper_log` VALUES (22, 1, '销售', '发货', '/api/v1/so/deliver {\"orderId\":4,\"warehouseId\":1,\"deliveryDate\":\"2026-11-01\",\"lines\":[{\"materialId\":1,\"quantity\":100}]}', '2026-11-01 11:10:30');
INSERT INTO `sys_oper_log` VALUES (23, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 11:58:18');
INSERT INTO `sys_oper_log` VALUES (24, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 12:20:39');
INSERT INTO `sys_oper_log` VALUES (25, NULL, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 12:22:51');
INSERT INTO `sys_oper_log` VALUES (26, NULL, '系统', '登录', '/api/v1/Auth/login {\r\n    \"password\":  ******\"123456\",\r\n    \"username\":  \"admin\"\r\n}', '2026-11-01 12:54:10');
INSERT INTO `sys_oper_log` VALUES (27, NULL, '系统', '登录', '/api/v1/Auth/login {\r\n    \"password\":  ******\"123456\",\r\n    \"username\":  \"admin\"\r\n}', '2026-11-01 12:54:20');
INSERT INTO `sys_oper_log` VALUES (28, 1, '数据维护', '新增', '/api/v1/mes/workstation {\r\n    \"remark\":  \"???????\",\r\n    \"code\":  \"WS-001\",\r\n    \"name\":  \"????\",\r\n    \"line\":  \"???A\"\r\n}', '2026-11-01 12:55:42');
INSERT INTO `sys_oper_log` VALUES (29, 1, '数据维护', '新增', '/api/v1/mes/workstation {\r\n    \"remark\":  \"??????\",\r\n    \"code\":  \"WS-002\",\r\n    \"name\":  \"?????\",\r\n    \"line\":  \"???A\"\r\n}', '2026-11-01 12:55:42');
INSERT INTO `sys_oper_log` VALUES (30, 1, '数据维护', '新增', '/api/v1/mes/workstation {\r\n    \"remark\":  \"???????\",\r\n    \"code\":  \"WS-003\",\r\n    \"name\":  \"?????\",\r\n    \"line\":  \"???B\"\r\n}', '2026-11-01 12:55:42');
INSERT INTO `sys_oper_log` VALUES (31, 1, '数据维护', '新增', '/api/v1/mes/routing {\r\n    \"productId\":  1,\r\n    \"status\":  1,\r\n    \"routingCode\":  \"RT-CTRL-V1\",\r\n    \"version\":  \"V1.0\",\r\n    \"remark\":  \"?????????\"\r\n}', '2026-11-01 12:55:42');
INSERT INTO `sys_oper_log` VALUES (32, 1, '数据维护', '新增', '/api/v1/mes/routing {\r\n    \"productId\":  2,\r\n    \"status\":  1,\r\n    \"routingCode\":  \"RT-PWR-V1\",\r\n    \"version\":  \"V1.0\",\r\n    \"remark\":  \"????????\"\r\n}', '2026-11-01 12:55:42');
INSERT INTO `sys_oper_log` VALUES (33, 1, '数据维护', '新增', '/api/v1/mes/routing-step {\r\n    \"stepNo\":  10,\r\n    \"stepName\":  \"??\",\r\n    \"stdHours\":  1.5,\r\n    \"routingId\":  1,\r\n    \"workstationId\":  1,\r\n    \"remark\":  \"?????????\"\r\n}', '2026-11-01 12:55:42');
INSERT INTO `sys_oper_log` VALUES (34, 1, '数据维护', '新增', '/api/v1/mes/routing-step {\r\n    \"stepNo\":  20,\r\n    \"stepName\":  \"????\",\r\n    \"stdHours\":  2,\r\n    \"routingId\":  1,\r\n    \"workstationId\":  2,\r\n    \"remark\":  \"48????\"\r\n}', '2026-11-01 12:55:43');
INSERT INTO `sys_oper_log` VALUES (35, 1, '数据维护', '新增', '/api/v1/mes/routing-step {\r\n    \"stepNo\":  30,\r\n    \"stepName\":  \"????\",\r\n    \"stdHours\":  0.5,\r\n    \"routingId\":  1,\r\n    \"workstationId\":  3,\r\n    \"remark\":  \"?????????\"\r\n}', '2026-11-01 12:55:43');
INSERT INTO `sys_oper_log` VALUES (36, 1, '数据维护', '新增', '/api/v1/mes/routing-step {\r\n    \"stepNo\":  10,\r\n    \"stepName\":  \"????\",\r\n    \"stdHours\":  1,\r\n    \"routingId\":  2,\r\n    \"workstationId\":  1,\r\n    \"remark\":  \"????????\"\r\n}', '2026-11-01 12:55:43');
INSERT INTO `sys_oper_log` VALUES (37, 1, '数据维护', '新增', '/api/v1/mes/routing-step {\r\n    \"stepNo\":  20,\r\n    \"stepName\":  \"????\",\r\n    \"stdHours\":  0.8,\r\n    \"routingId\":  2,\r\n    \"workstationId\":  3,\r\n    \"remark\":  \"??????\"\r\n}', '2026-11-01 12:55:43');
INSERT INTO `sys_oper_log` VALUES (38, 1, '生产', '创建工单', '/api/v1/pr {\r\n    \"endDate\":  \"2026-11-08\",\r\n    \"startDate\":  \"2026-11-01\",\r\n    \"materialId\":  2,\r\n    \"planQty\":  120\r\n}', '2026-11-01 12:56:00');
INSERT INTO `sys_oper_log` VALUES (39, 1, '生产', '下达', '/api/v1/pr/5/release ', '2026-11-01 12:56:00');
INSERT INTO `sys_oper_log` VALUES (40, 1, '数据维护', '新增', '/api/v1/mes/work-order {\r\n    \"source\":  2,\r\n    \"planEnd\":  \"2026-11-10\",\r\n    \"orderNo\":  \"PR202610003\",\r\n    \"planStart\":  \"2026-11-01\",\r\n    \"doneQty\":  0,\r\n    \"productId\":  1,\r\n    \"remark\":  \"?????????\",\r\n    \"status\":  0,\r\n    \"planQty\":  200,\r\n    \"priority\":  1,\r\n    \"prOrderId\":  3\r\n}', '2026-11-01 12:57:32');
INSERT INTO `sys_oper_log` VALUES (41, 1, '数据维护', '新增', '/api/v1/mes/work-order {\r\n    \"source\":  2,\r\n    \"planEnd\":  \"2026-11-08\",\r\n    \"orderNo\":  \"MO202611010001\",\r\n    \"planStart\":  \"2026-11-01\",\r\n    \"doneQty\":  0,\r\n    \"productId\":  2,\r\n    \"remark\":  \"?????????\",\r\n    \"status\":  0,\r\n    \"planQty\":  120,\r\n    \"priority\":  2,\r\n    \"prOrderId\":  5\r\n}', '2026-11-01 12:57:32');
INSERT INTO `sys_oper_log` VALUES (42, 1, '数据维护', '新增', '/api/v1/mes/production-order {\r\n    \"doneQty\":  0,\r\n    \"startTime\":  \"2026-11-01T08:00:00\",\r\n    \"workOrderId\":  1,\r\n    \"workstationId\":  1,\r\n    \"status\":  1,\r\n    \"remark\":  \"?????????\",\r\n    \"planQty\":  200,\r\n    \"prodNo\":  \"PO202611010001\"\r\n}', '2026-11-01 12:57:32');
INSERT INTO `sys_oper_log` VALUES (43, 1, '数据维护', '新增', '/api/v1/mes/production-order {\r\n    \"doneQty\":  0,\r\n    \"planQty\":  120,\r\n    \"prodNo\":  \"PO202611010002\",\r\n    \"workstationId\":  1,\r\n    \"workOrderId\":  2,\r\n    \"status\":  0,\r\n    \"remark\":  \"?????????\"\r\n}', '2026-11-01 12:57:32');
INSERT INTO `sys_oper_log` VALUES (44, 1, '数据维护', '新增', '/api/v1/mes/work-report {\r\n    \"badQty\":  2,\r\n    \"reportNo\":  \"WR202611010001\",\r\n    \"productionOrderId\":  1,\r\n    \"workHours\":  24,\r\n    \"goodQty\":  180,\r\n    \"status\":  1,\r\n    \"reportTime\":  \"2026-11-01T16:00:00\"\r\n}', '2026-11-01 12:57:54');
INSERT INTO `sys_oper_log` VALUES (45, 1, '数据维护', '新增', '/api/v1/mes/work-report {\r\n    \"badQty\":  0,\r\n    \"reportNo\":  \"WR202611010002\",\r\n    \"productionOrderId\":  2,\r\n    \"workHours\":  0,\r\n    \"goodQty\":  0,\r\n    \"status\":  0,\r\n    \"reportTime\":  \"2026-11-01T08:00:00\"\r\n}', '2026-11-01 12:57:54');
INSERT INTO `sys_oper_log` VALUES (46, 1, '数据维护', '新增', '/api/v1/mes/quality-check {\r\n    \"passQty\":  180,\r\n    \"productionOrderId\":  1,\r\n    \"checkNo\":  \"QC202611010001\",\r\n    \"failQty\":  2,\r\n    \"remark\":  \"??:2?????\",\r\n    \"checkType\":  3,\r\n    \"qty\":  182,\r\n    \"checkTime\":  \"2026-11-01T17:00:00\",\r\n    \"result\":  2,\r\n    \"workReportId\":  1\r\n}', '2026-11-01 12:57:54');
INSERT INTO `sys_oper_log` VALUES (47, 1, '数据维护', '修改', '/api/v1/mes/work-order/1 {\r\n    \"source\":  2,\r\n    \"planEnd\":  \"2026-11-10\",\r\n    \"id\":  1,\r\n    \"planStart\":  \"2026-11-01\",\r\n    \"doneQty\":  180,\r\n    \"productId\":  1,\r\n    \"remark\":  \"???,??180\",\r\n    \"status\":  1,\r\n    \"orderNo\":  \"PR202610003\",\r\n    \"planQty\":  200,\r\n    \"priority\":  1,\r\n    \"prOrderId\":  3\r\n}', '2026-11-01 12:57:54');
INSERT INTO `sys_oper_log` VALUES (48, 1, '数据维护', '修改', '/api/v1/mes/production-order/1 {\r\n    \"remark\":  \"?????\",\r\n    \"id\":  1,\r\n    \"doneQty\":  180,\r\n    \"prodNo\":  \"PO202611010001\",\r\n    \"workstationId\":  1,\r\n    \"workOrderId\":  1,\r\n    \"status\":  1,\r\n    \"planQty\":  200,\r\n    \"startTime\":  \"2026-11-01T08:00:00\"\r\n}', '2026-11-01 12:57:54');
INSERT INTO `sys_oper_log` VALUES (49, 1, '数据维护', '新增', '/api/v1/mes/material-issue {\"issueNo\":\"MI202611010001\",\"workOrderId\":1,\"materialId\":3,\"qty\":204,\"status\":1,\"issueTime\":\"2026-11-01T09:00:00\",\"remark\":\"控制芯片：计划200x2x1.02\"}', '2026-11-01 12:58:30');
INSERT INTO `sys_oper_log` VALUES (50, 1, '数据维护', '新增', '/api/v1/mes/material-issue {\"issueNo\":\"MI202611010002\",\"workOrderId\":1,\"materialId\":4,\"qty\":202,\"status\":1,\"issueTime\":\"2026-11-01T09:05:00\",\"remark\":\"电路板：计划200x1x1.01\"}', '2026-11-01 12:58:44');
INSERT INTO `sys_oper_log` VALUES (51, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 13:09:22');
INSERT INTO `sys_oper_log` VALUES (52, 1, '数据维护', '修改', '/api/v1/mes/workstation/1 {\"remark\":\"\",\"code\":\"WS-001\",\"name\":\"装配一区\",\"id\":1,\"line\":\"装配线A\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (53, 1, '数据维护', '修改', '/api/v1/mes/workstation/2 {\"remark\":\"\",\"code\":\"WS-002\",\"name\":\"老化测试区\",\"id\":2,\"line\":\"装配线A\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (54, 1, '数据维护', '修改', '/api/v1/mes/workstation/3 {\"remark\":\"\",\"code\":\"WS-003\",\"name\":\"检测包装区\",\"id\":3,\"line\":\"装配线B\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (55, 1, '数据维护', '修改', '/api/v1/mes/work-order/1 {\"source\":2,\"planEnd\":\"2026-11-10\",\"id\":1,\"planStart\":\"2026-11-01\",\"doneQty\":180,\"productId\":1,\"remark\":\"\",\"status\":1,\"orderNo\":\"PR202610003\",\"planQty\":200,\"priority\":1,\"prOrderId\":3}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (56, 1, '数据维护', '修改', '/api/v1/mes/work-order/2 {\"source\":2,\"planEnd\":\"2026-11-08\",\"id\":2,\"planStart\":\"2026-11-01\",\"doneQty\":0,\"productId\":2,\"remark\":\"\",\"status\":0,\"orderNo\":\"MO202611010001\",\"planQty\":120,\"priority\":2,\"prOrderId\":5}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (57, 1, '数据维护', '修改', '/api/v1/mes/production-order/1 {\"remark\":\"\",\"id\":1,\"doneQty\":180,\"prodNo\":\"PO202611010001\",\"workstationId\":1,\"workOrderId\":1,\"status\":1,\"planQty\":200,\"startTime\":\"2026-11-01T08:00:00\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (58, 1, '数据维护', '修改', '/api/v1/mes/production-order/2 {\"doneQty\":0,\"prodNo\":\"PO202611010002\",\"id\":2,\"workOrderId\":2,\"workstationId\":1,\"status\":0,\"remark\":\"\",\"planQty\":120}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (59, 1, '数据维护', '修改', '/api/v1/mes/material-issue/1 {\"issueNo\":\"MI202611010001\",\"id\":1,\"materialId\":3,\"workOrderId\":1,\"remark\":\"\",\"status\":1,\"issueTime\":\"2026-11-01T09:00:00\",\"qty\":204}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (60, 1, '数据维护', '修改', '/api/v1/mes/material-issue/2 {\"issueNo\":\"MI202611010002\",\"id\":2,\"materialId\":4,\"workOrderId\":1,\"remark\":\"\",\"status\":1,\"issueTime\":\"2026-11-01T09:05:00\",\"qty\":202}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (61, 1, '数据维护', '修改', '/api/v1/mes/routing/1 {\"id\":1,\"productId\":1,\"remark\":\"\",\"routingCode\":\"RT-CTRL-V1\",\"status\":1,\"version\":\"V1.0\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (62, 1, '数据维护', '修改', '/api/v1/mes/routing/2 {\"id\":2,\"productId\":2,\"remark\":\"\",\"routingCode\":\"RT-PWR-V1\",\"status\":1,\"version\":\"V1.0\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (63, 1, '数据维护', '修改', '/api/v1/mes/routing-step/1 {\"id\":1,\"stepNo\":10,\"stepName\":\"装配\",\"stdHours\":1.5,\"routingId\":1,\"workstationId\":1,\"remark\":\"\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (64, 1, '数据维护', '修改', '/api/v1/mes/routing-step/2 {\"id\":2,\"stepNo\":20,\"stepName\":\"老化测试\",\"stdHours\":2,\"routingId\":1,\"workstationId\":2,\"remark\":\"\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (65, 1, '数据维护', '修改', '/api/v1/mes/routing-step/3 {\"id\":3,\"stepNo\":30,\"stepName\":\"检测包装\",\"stdHours\":0.5,\"routingId\":1,\"workstationId\":3,\"remark\":\"\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (66, 1, '数据维护', '修改', '/api/v1/mes/routing-step/4 {\"id\":4,\"stepNo\":10,\"stepName\":\"模块组装\",\"stdHours\":1,\"routingId\":2,\"workstationId\":1,\"remark\":\"\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (67, 1, '数据维护', '修改', '/api/v1/mes/routing-step/5 {\"id\":5,\"stepNo\":20,\"stepName\":\"成品检测\",\"stdHours\":0.8,\"routingId\":2,\"workstationId\":3,\"remark\":\"\"}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (68, 1, '数据维护', '修改', '/api/v1/mes/quality-check/1 {\"passQty\":180,\"productionOrderId\":1,\"id\":1,\"checkNo\":\"QC202611010001\",\"failQty\":2,\"remark\":\"\",\"checkType\":3,\"qty\":182,\"checkTime\":\"2026-11-01T17:00:00\",\"result\":2,\"workReportId\":1}', '2026-11-01 13:28:35');
INSERT INTO `sys_oper_log` VALUES (69, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 13:35:25');
INSERT INTO `sys_oper_log` VALUES (70, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 13:35:33');
INSERT INTO `sys_oper_log` VALUES (71, 1, '数据维护', '新增', '/api/v1/mes/material-issue {\"issueTime\":\"2026-11-01T10:30:00\",\"materialId\":3,\"qty\":371,\"issueNo\":\"MI202611010003\",\"remark\":\"chip 120x3x1.03\",\"workOrderId\":2,\"status\":1}', '2026-11-01 13:38:56');
INSERT INTO `sys_oper_log` VALUES (72, 1, '数据维护', '新增', '/api/v1/mes/material-issue {\"issueTime\":\"2026-11-01T10:35:00\",\"materialId\":4,\"qty\":184,\"issueNo\":\"MI202611010004\",\"remark\":\"pcb 120x1.5x1.02\",\"workOrderId\":2,\"status\":1}', '2026-11-01 13:38:56');
INSERT INTO `sys_oper_log` VALUES (73, 1, '数据维护', '新增', '/api/v1/mes/quality-check {\"passQty\":50,\"productionOrderId\":1,\"checkNo\":\"QC202611010002\",\"failQty\":0,\"remark\":\"FAI 50 ok\",\"checkType\":1,\"qty\":50,\"checkTime\":\"2026-11-01T10:00:00\",\"result\":2,\"workReportId\":1}', '2026-11-01 13:38:56');
INSERT INTO `sys_oper_log` VALUES (74, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 13:50:25');
INSERT INTO `sys_oper_log` VALUES (75, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 13:55:19');
INSERT INTO `sys_oper_log` VALUES (76, 1, 'MES', '同步生产订单为工单', '/api/v1/mes/biz/work-order/sync-from-pr {}', '2026-11-01 13:55:20');
INSERT INTO `sys_oper_log` VALUES (77, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 13:55:28');
INSERT INTO `sys_oper_log` VALUES (78, 1, 'MES', '工单下发生产', '/api/v1/mes/biz/work-order/999/dispatch {}', '2026-11-01 13:55:28');
INSERT INTO `sys_oper_log` VALUES (79, 1, 'MES', '生产领料', '/api/v1/mes/biz/production-order/999/issue {}', '2026-11-01 13:55:28');
INSERT INTO `sys_oper_log` VALUES (80, 1, 'MES', '生产报工', '/api/v1/mes/biz/production-order/999/report {}', '2026-11-01 13:55:28');
INSERT INTO `sys_oper_log` VALUES (81, 1, 'MES', '质量检验', '/api/v1/mes/biz/work-report/999/qc {}', '2026-11-01 13:55:28');
INSERT INTO `sys_oper_log` VALUES (82, 1, 'MES', '回写生产订单状态', '/api/v1/mes/biz/work-order/999/sync-pr {}', '2026-11-01 13:55:29');
INSERT INTO `sys_oper_log` VALUES (83, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 14:18:01');
INSERT INTO `sys_oper_log` VALUES (84, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 14:22:01');
INSERT INTO `sys_oper_log` VALUES (85, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 14:35:21');
INSERT INTO `sys_oper_log` VALUES (86, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 14:56:26');
INSERT INTO `sys_oper_log` VALUES (87, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 14:57:59');
INSERT INTO `sys_oper_log` VALUES (88, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 15:24:28');
INSERT INTO `sys_oper_log` VALUES (89, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 15:27:25');
INSERT INTO `sys_oper_log` VALUES (90, NULL, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 15:42:11');
INSERT INTO `sys_oper_log` VALUES (91, 1, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 16:45:20');
INSERT INTO `sys_oper_log` VALUES (92, 1, 'HR', '处理员工异动', '/api/v1/hr/biz/employee-change/5/process ', '2026-11-01 17:51:03');
INSERT INTO `sys_oper_log` VALUES (93, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 17:59:45');
INSERT INTO `sys_oper_log` VALUES (94, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-01 18:00:32');
INSERT INTO `sys_oper_log` VALUES (95, NULL, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 08:39:16');
INSERT INTO `sys_oper_log` VALUES (96, 1, '审批流', '审批处理', '/api/v1/approval/3/handle {\"action\":3,\"opinion\":\"\"}', '2026-11-02 08:42:12');
INSERT INTO `sys_oper_log` VALUES (97, 1, '采购', '创建订单', '/api/v1/po {\"supplierId\":3,\"orderDate\":\"2026-11-02\",\"lines\":[{\"materialId\":6,\"quantity\":100,\"unitPrice\":300}]}', '2026-11-02 08:46:10');
INSERT INTO `sys_oper_log` VALUES (98, 1, '采购', '审核', '/api/v1/po/4/approve ', '2026-11-02 08:46:18');
INSERT INTO `sys_oper_log` VALUES (99, 1, '主数据', '保存', '/api/v1/md/customer {\"id\":1,\"customerCode\":\"CUST001\",\"customerName\":\"北京华信科技有限公司\",\"contactPerson\":\"张伟\",\"phone\":\"13800138001\",\"address\":\"北京市朝阳区建国路88号\",\"status\":0,\"createTime\":\"2026-10-31T11:53:32\"}', '2026-11-02 08:48:36');
INSERT INTO `sys_oper_log` VALUES (100, 1, '主数据', '保存', '/api/v1/md/customer {\"id\":1,\"customerCode\":\"CUST001\",\"customerName\":\"北京华信科技有限公司\",\"contactPerson\":\"张伟\",\"phone\":\"13800138001\",\"address\":\"北京市朝阳区建国路88号\",\"status\":1,\"createTime\":\"2026-10-31T11:53:32\"}', '2026-11-02 08:48:40');
INSERT INTO `sys_oper_log` VALUES (101, 1, '财务', '录凭证', '/api/v1/fin/voucher {\"voucherDate\":\"2026-11-02\",\"sourceModule\":\"手工\",\"entries\":[{\"accountCode\":\"\",\"direction\":\"DR\",\"amount\":10000,\"memo\":\"\"},{\"accountCode\":\"\",\"direction\":\"CR\",\"amount\":10000,\"memo\":\"\"}]}', '2026-11-02 08:55:12');
INSERT INTO `sys_oper_log` VALUES (102, 1, 'MES', '同步生产订单为工单', '/api/v1/mes/biz/work-order/sync-from-pr {}', '2026-11-02 08:57:44');
INSERT INTO `sys_oper_log` VALUES (103, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 09:05:45');
INSERT INTO `sys_oper_log` VALUES (104, NULL, '系统', '登录', '/api/v1/auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 09:30:59');
INSERT INTO `sys_oper_log` VALUES (105, 1, '财务', '成本核算', '/api/v1/fin/cost {\"prOrderId\":1,\"laborCost\":1,\"overheadCost\":0}', '2026-11-02 09:42:37');
INSERT INTO `sys_oper_log` VALUES (106, 1, '财务', '成本核算', '/api/v1/fin/cost {\"prOrderId\":1,\"laborCost\":0,\"overheadCost\":0}', '2026-11-02 09:42:49');
INSERT INTO `sys_oper_log` VALUES (107, 1, '生产', 'MRP运算', '/api/v1/pr/mrp ', '2026-11-02 09:50:27');
INSERT INTO `sys_oper_log` VALUES (108, 1, '生产', 'MRP运算', '/api/v1/pr/mrp ', '2026-11-02 09:50:35');
INSERT INTO `sys_oper_log` VALUES (109, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:08:34');
INSERT INTO `sys_oper_log` VALUES (110, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:09:19');
INSERT INTO `sys_oper_log` VALUES (111, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:09:57');
INSERT INTO `sys_oper_log` VALUES (112, 1, '审批流', '保存审批规则', '/api/v1/approval/rule {\"bizType\":\"PO\",\"minAmount\":0,\"maxAmount\":null,\"level\":2,\"nodeRoles\":\"deptManager,financeDirector\",\"enabled\":1,\"remark\":\"测试\"}', '2026-11-02 10:09:57');
INSERT INTO `sys_oper_log` VALUES (113, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:10:22');
INSERT INTO `sys_oper_log` VALUES (114, 1, '审批流', '删除审批规则', '/api/v1/approval/rule/1 ', '2026-11-02 10:10:22');
INSERT INTO `sys_oper_log` VALUES (115, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:12:08');
INSERT INTO `sys_oper_log` VALUES (116, 1, '审批流', '保存审批规则', '/api/v1/approval/rule {\"bizType\":\"PO\",\"minAmount\":0,\"maxAmount\":null,\"level\":2,\"nodeRoles\":\"deptManager,financeDirector\",\"enabled\":1}', '2026-11-02 10:12:09');
INSERT INTO `sys_oper_log` VALUES (117, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"PO\",\"bizId\":1,\"billNo\":\"TEST-PO-1\",\"title\":\"测试采购审批\",\"amount\":100,\"totalNodes\":1}', '2026-11-02 10:12:09');
INSERT INTO `sys_oper_log` VALUES (118, 1, '审批流', '删除审批规则', '/api/v1/approval/rule/1 ', '2026-11-02 10:12:09');
INSERT INTO `sys_oper_log` VALUES (119, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:30:42');
INSERT INTO `sys_oper_log` VALUES (120, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:32:34');
INSERT INTO `sys_oper_log` VALUES (121, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 10:39:36');
INSERT INTO `sys_oper_log` VALUES (122, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:09:27');
INSERT INTO `sys_oper_log` VALUES (123, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"REQUISITION\",\"billNo\":null,\"amount\":null,\"bizId\":null,\"remark\":\"新建请购单自动送审\",\"totalNodes\":1,\"title\":\"采购请购审批 \"}', '2026-11-02 11:09:28');
INSERT INTO `sys_oper_log` VALUES (124, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:11:10');
INSERT INTO `sys_oper_log` VALUES (125, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:11:37');
INSERT INTO `sys_oper_log` VALUES (126, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"REQUISITION\",\"billNo\":null,\"amount\":null,\"bizId\":null,\"remark\":\"新建请购单自动送审\",\"totalNodes\":1,\"title\":\"采购请购审批 \"}', '2026-11-02 11:11:37');
INSERT INTO `sys_oper_log` VALUES (127, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:12:11');
INSERT INTO `sys_oper_log` VALUES (128, 1, '采购', '新建请购单', '/api/v1/po/requisition {\"remark\":\"auto\",\"lines\":[{\"materialId\":1,\"reqQty\":5}],\"deptName\":\"测试部\",\"reqDate\":\"2026-11-02\"}', '2026-11-02 11:12:11');
INSERT INTO `sys_oper_log` VALUES (129, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"REQUISITION\",\"billNo\":\"REQ202611020001\",\"amount\":null,\"bizId\":4,\"remark\":\"新建请购单自动送审\",\"totalNodes\":1,\"title\":\"采购请购审批 REQ202611020001\"}', '2026-11-02 11:12:11');
INSERT INTO `sys_oper_log` VALUES (130, 1, '审批流', '审批处理', '/api/v1/approval/11/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:12:11');
INSERT INTO `sys_oper_log` VALUES (131, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (132, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"RECEIPT\",\"billNo\":\"RN-test\",\"amount\":null,\"bizId\":1,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"RECEIPT RN-test\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (133, 1, '审批流', '审批处理', '/api/v1/approval/12/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (134, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"PO_CHANGE\",\"billNo\":\"PO-test\",\"amount\":null,\"bizId\":1,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"PO_CHANGE PO-test\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (135, 1, '审批流', '审批处理', '/api/v1/approval/13/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (136, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"REQUISITION\",\"billNo\":\"REQ202611020001\",\"amount\":null,\"bizId\":4,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"t\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (137, 1, '审批流', '审批处理', '/api/v1/approval/14/handle {\"action\":3,\"opinion\":\"no\"}', '2026-11-02 11:12:43');
INSERT INTO `sys_oper_log` VALUES (138, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:17:39');
INSERT INTO `sys_oper_log` VALUES (139, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"QUOTE\",\"billNo\":null,\"amount\":null,\"bizId\":null,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"销售报价审批 \"}', '2026-11-02 11:17:40');
INSERT INTO `sys_oper_log` VALUES (140, 1, '审批流', '审批处理', '/api/v1/approval/15/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:17:40');
INSERT INTO `sys_oper_log` VALUES (141, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"CREDIT\",\"billNo\":\"\",\"amount\":5000,\"bizId\":null,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"客户信用额度审批 \"}', '2026-11-02 11:17:40');
INSERT INTO `sys_oper_log` VALUES (142, 1, '审批流', '审批处理', '/api/v1/approval/16/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:17:40');
INSERT INTO `sys_oper_log` VALUES (143, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (144, 1, '销售', '新建报价', '/api/v1/so/quote {\"customerId\":1,\"validDate\":null,\"discountRate\":1,\"remark\":\"\",\"lines\":[{\"unitPrice\":10,\"quantity\":2,\"discountRate\":1,\"materialId\":1}],\"priceStrategy\":\"STANDARD\",\"quoteDate\":\"2026-11-02\"}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (145, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"QUOTE\",\"billNo\":\"QT202611020001\",\"amount\":20,\"bizId\":4,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"销售报价审批 QT202611020001\"}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (146, 1, '审批流', '审批处理', '/api/v1/approval/17/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (147, 1, '销售', '维护信用额度', '/api/v1/so/credit {\"creditLimit\":5000,\"status\":1,\"customerId\":1}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (148, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"CREDIT\",\"billNo\":\"1\",\"amount\":5000,\"bizId\":1,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"客户信用额度审批 1\"}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (149, 1, '审批流', '审批处理', '/api/v1/approval/18/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:19:09');
INSERT INTO `sys_oper_log` VALUES (150, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:23:44');
INSERT INTO `sys_oper_log` VALUES (151, 1, '财务', '录凭证', '/api/v1/fin/voucher {\"entries\":[{\"amount\":100,\"memo\":\"t\",\"direction\":\"DR\",\"accountCode\":\"1001\"},{\"amount\":100,\"memo\":\"t\",\"direction\":\"CR\",\"accountCode\":\"1002\"}],\"voucherDate\":\"2026-11-02\",\"sourceModule\":\"手工\"}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (152, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"VOUCHER\",\"billNo\":\"V202611020002\",\"amount\":100,\"bizId\":6,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"记账凭证审批 V202611020002\"}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (153, 1, '审批流', '审批处理', '/api/v1/approval/19/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (154, 1, '财务', '收款核销', '/api/v1/fin/receipt {\"amount\":100,\"customerId\":1,\"receivableId\":1,\"receiptDate\":\"2026-11-02\"}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (155, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"FIN_RECEIPT\",\"billNo\":null,\"amount\":100,\"bizId\":1,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"收款单审批 \"}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (156, 1, '审批流', '审批处理', '/api/v1/approval/20/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (157, 1, '财务', '付款核销', '/api/v1/fin/payment {\"amount\":80,\"supplierId\":1,\"paymentDate\":\"2026-11-02\",\"payableId\":1}', '2026-11-02 11:23:45');
INSERT INTO `sys_oper_log` VALUES (158, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"FIN_PAYMENT\",\"billNo\":null,\"amount\":80,\"bizId\":18268,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"付款单审批 \"}', '2026-11-02 11:23:46');
INSERT INTO `sys_oper_log` VALUES (159, 1, '审批流', '审批处理', '/api/v1/approval/21/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:23:46');
INSERT INTO `sys_oper_log` VALUES (160, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:27:53');
INSERT INTO `sys_oper_log` VALUES (161, 1, '库存', '入库', '/api/v1/inv/in {\"inType\":\"OTHER\",\"lines\":[{\"materialId\":1,\"quantity\":5}],\"warehouseId\":1,\"sourceNo\":null,\"inDate\":\"2026-11-02\"}', '2026-11-02 11:27:54');
INSERT INTO `sys_oper_log` VALUES (162, NULL, '库存预警', 'ALERT', 'M001 仓库1 现存量5.0000 安全库存50.0000', '2026-11-02 11:27:54');
INSERT INTO `sys_oper_log` VALUES (163, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"IN_ORDER\",\"billNo\":\"IN202611020001\",\"bizId\":4,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"入库单审批 IN202611020001\"}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (164, 1, '审批流', '审批处理', '/api/v1/approval/22/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (165, 1, '库存', '出库', '/api/v1/inv/out {\"outDate\":\"2026-11-02\",\"warehouseId\":1,\"sourceNo\":null,\"outType\":\"OTHER\",\"lines\":[{\"materialId\":1,\"quantity\":2}]}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (166, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"OUT_ORDER\",\"billNo\":\"OUT202611020001\",\"bizId\":4,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"出库单审批 OUT202611020001\"}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (167, 1, '审批流', '审批处理', '/api/v1/approval/23/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (168, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"TRANSFER\",\"billNo\":null,\"bizId\":null,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"调拨单审批 \"}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (169, 1, '审批流', '审批处理', '/api/v1/approval/24/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:27:55');
INSERT INTO `sys_oper_log` VALUES (170, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:30:26');
INSERT INTO `sys_oper_log` VALUES (171, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:31:00');
INSERT INTO `sys_oper_log` VALUES (172, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"TRANSFER\",\"billNo\":null,\"bizId\":null,\"remark\":\"t\",\"totalNodes\":1,\"title\":\"调拨单审批 \"}', '2026-11-02 11:31:00');
INSERT INTO `sys_oper_log` VALUES (173, 1, '审批流', '审批处理', '/api/v1/approval/25/handle {\"action\":2,\"opinion\":\"ok\"}', '2026-11-02 11:31:00');
INSERT INTO `sys_oper_log` VALUES (174, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:45:15');
INSERT INTO `sys_oper_log` VALUES (175, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:45:25');
INSERT INTO `sys_oper_log` VALUES (176, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:45:54');
INSERT INTO `sys_oper_log` VALUES (177, 1, '库存', '创建盘点单', '/api/v1/inv/check {\"warehouseId\":1,\"checkDate\":\"2026-11-02\",\"remark\":\"\"}', '2026-11-02 11:47:14');
INSERT INTO `sys_oper_log` VALUES (178, 1, '库存', '录入盘点结果', '/api/v1/inv/check/finish {\"checkId\":3,\"lines\":[{\"materialId\":1,\"actualQty\":0},{\"materialId\":2,\"actualQty\":0}]}', '2026-11-02 11:47:17');
INSERT INTO `sys_oper_log` VALUES (179, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"CHECK\",\"bizId\":3,\"billNo\":\"CK202611020001\",\"title\":\"库存盘点审批 CK202611020001\",\"amount\":null,\"totalNodes\":1,\"remark\":\"盘盈盘亏调整送审，审批通过后自动调整库存\"}', '2026-11-02 11:47:17');
INSERT INTO `sys_oper_log` VALUES (180, 1, '库存', '创建盘点单', '/api/v1/inv/check {\"warehouseId\":1,\"checkDate\":\"2026-11-02\",\"remark\":\"\"}', '2026-11-02 11:47:54');
INSERT INTO `sys_oper_log` VALUES (181, 1, '库存', '录入盘点结果', '/api/v1/inv/check/finish {\"checkId\":4,\"lines\":[{\"materialId\":1,\"actualQty\":3},{\"materialId\":2,\"actualQty\":200}]}', '2026-11-02 11:47:57');
INSERT INTO `sys_oper_log` VALUES (182, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"CHECK\",\"bizId\":4,\"billNo\":\"CK202611020002\",\"title\":\"库存盘点审批 CK202611020002\",\"amount\":null,\"totalNodes\":1,\"remark\":\"盘盈盘亏调整送审，审批通过后自动调整库存\"}', '2026-11-02 11:47:57');
INSERT INTO `sys_oper_log` VALUES (183, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 11:58:26');
INSERT INTO `sys_oper_log` VALUES (184, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 14:11:38');
INSERT INTO `sys_oper_log` VALUES (185, NULL, '系统', '登录', '/api/v1/Auth/login {\"username\":\"admin\",\"password\":\"1******\"}', '2026-11-02 14:11:51');
INSERT INTO `sys_oper_log` VALUES (186, 1, '采购', '创建订单', '/api/v1/po {\"supplierId\":2,\"orderDate\":\"2026-11-02\",\"lines\":[{\"materialId\":4,\"quantity\":100,\"unitPrice\":50}]}', '2026-11-02 14:40:07');
INSERT INTO `sys_oper_log` VALUES (187, 1, '审批流', '提交审批', '/api/v1/approval/submit {\"bizType\":\"PO\",\"bizId\":5,\"billNo\":\"PO202611020002\",\"title\":\"采购订单审批 PO202611020002\",\"amount\":5000,\"totalNodes\":1,\"remark\":\"新建采购订单自动送审\"}', '2026-11-02 14:40:07');

-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE `sys_role`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '角色ID',
  `role_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色编码',
  `role_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '角色名称',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_role_code`(`role_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '系统角色' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_role
-- ----------------------------
INSERT INTO `sys_role` VALUES (1, 'admin', '管理员', 1);
INSERT INTO `sys_role` VALUES (5, 'hr', '人事专员', 1);
INSERT INTO `sys_role` VALUES (6, 'mes', '生产专员', 1);

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user`  (
  `id` bigint NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '登录名',
  `password` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '密码（加密存储）',
  `real_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '真实姓名',
  `status` tinyint NULL DEFAULT 1 COMMENT '状态：1启用/0停用',
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_username`(`username` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '系统用户' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO `sys_user` VALUES (1, 'admin', '$argon2id$v=19$m=19456,t=2,p=1$xLZSPot9fLgwM4Xt87xRNw$D5YMLZ/qiXbBDlBM1/tBoAdCvNXpv5R3ad7QBI1l+ME', '管理员', 1, '2026-10-30 19:32:45');

-- ----------------------------
-- Table structure for sys_user_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_user_role`;
CREATE TABLE `sys_user_role`  (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `user_id` bigint NOT NULL COMMENT '用户ID',
  `role_id` bigint NOT NULL COMMENT '角色ID',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_user_role`(`user_id` ASC, `role_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '用户角色关联' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of sys_user_role
-- ----------------------------
INSERT INTO `sys_user_role` VALUES (1, 1, 1);

SET FOREIGN_KEY_CHECKS = 1;
