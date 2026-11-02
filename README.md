# ERP 企业资源管理系统

> 基于 **.NET 9 (ASP.NET Core)** + **Vue 3** 的全栈 ERP 系统，覆盖人事、财务、库存、制造执行（MES）、采购、销售与审批流，含 102 张业务数据表与统一鉴权。

[![Backend](https://img.shields.io/badge/Backend-.NET%209.0-512BD4)](https://dotnet.microsoft.com/)
[![Frontend](https://img.shields.io/badge/Frontend-Vue%203%20%2B%20TypeScript-42b883)](https://vuejs.org/)
[![Database](https://img.shields.io/badge/Database-MySQL%20%2B%20EF%20Core-4479A1)](https://www.mysql.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](#license)

---

## 项目简介

本系统是一套面向中小制造企业的综合管理 ERP，采用**前后端分离**架构。后端基于 ASP.NET Core Web API，按接口、服务、仓储分层组织；前端使用 Vue 3 + TypeScript + Element Plus。系统覆盖从采购、库存、生产制造到销售、财务与人力资源的业务链路，并内置**工作流审批**与**数据看板**。

---

## 功能模块

系统共计 **102 张业务数据表**，划分为以下业务域：

| 模块 | 说明 |
| --- | --- |
| **人事管理 HR** | 员工、部门、岗位、合同、考勤、请假、加班、薪酬（社保 / 个税规则）、招聘、面试、绩效、培训 |
| **财务管理** | 应收 / 应付、收付款、成本、会计凭证与分录 |
| **库存管理** | 入库 / 出库、批次、库存、盘点、调拨、库存流水 |
| **主数据 MD** | 客户、物料、BOM 等基础数据 |
| **制造执行 MES** | 工单、生产订单、工艺路线、工位、报工、质量检验、领料 |
| **生产 / 采购 / 销售** | 生产计划、采购订单、销售订单 |
| **审批流** | 通用审批引擎与待办 |
| **看板与报表** | 经营数据可视化（ECharts）与统计报表 |
| **系统** | 统一 JWT 鉴权、操作日志 |

---

## 技术栈

### 后端（.NET 9）
- **框架**：ASP.NET Core Web API（多项目分层解决方案）
- **持久化**：Entity Framework Core 9 + MySQL（Pomelo 驱动）
- **认证授权**：JWT Bearer 认证 + 基于策略（Policy）的授权
- **接口文档**：Swagger / OpenAPI
- **跨域**：CORS 中间件
- **架构模式**：仓储模式（Repository）、服务层（Service）、依赖倒置（接口与实现分离）

### 前端（Vue 3）
- **核心**：Vue 3（`<script setup>`）+ TypeScript + Vite
- **UI**：Element Plus（中文语言包）+ @element-plus/icons-vue
- **状态管理**：Pinia
- **路由**：Vue Router 4
- **可视化**：ECharts（Dashboard 看板）
- **请求**：Axios（统一拦截、自动附带 Token、业务码处理）

---

## 系统架构（分层）

后端采用清晰分层，业务契约以接口定义，实现与契约解耦，便于测试与替换：

```
Demo/
├── Demo.Api.Admin/              # 接口层：控制器、Program、中间件、过滤器
├── Demo.Service.Interface/     # 服务接口定义（业务契约）
├── Demo.Service.Insance/       # 服务实现（业务逻辑）
├── Demo.Repository.Interface/  # 仓储接口
├── Demo.Repository.Instance/   # 仓储实现（数据访问）
├── Demo.Infrastructure.EFCore/ # EF Core 上下文与实体映射（Fluent API）
├── Demo.Infrastructure.Dto/    # 数据传输对象
├── Demo.Infrastructure.Attributes/ # 自定义特性
└── Demo.Infrastructure.Tools/  # 工具类（JWT 等）
```

前端结构：

```
Demoue/vite-project/
├── src/
│   ├── api/            # 接口请求封装
│   ├── components/     # 通用组件（ErpPagination、各模块 Crud）
│   ├── layout/         # 后台布局与侧边菜单
│   ├── router/         # 路由与菜单配置
│   ├── stores/         # Pinia 状态（auth、notify）
│   ├── utils/          # 请求拦截、审批提交
│   └── views/          # 业务页面（hr/finance/mes/sales/...）
└── vite.config.ts      # 构建与开发代理配置
```

---

## 环境要求

- **.NET SDK** 9.0 及以上
- **Node.js** 18+（建议 20 LTS）
- **MySQL** 8.0（或兼容版本），默认端口 3306
- （可选）Redis、RabbitMQ —— 仅当使用对应特性时所需

---

## 本地运行

### 1. 数据库初始化
在 MySQL 中创建数据库并导入初始化脚本（含 102 张表）：

```sql
CREATE DATABASE erp_db CHARACTER SET utf8mb4;
-- 使用客户端导入脚本
mysql -u root -p erp_db < SqlScript/erp_db.sql
```

### 2. 配置后端
复制配置模板并填入本机数据库连接与密钥（**请勿提交真实凭证**）：

```bash
cp Demo/Demo.Api.Admin/appsettings.example.json Demo/Demo.Api.Admin/appsettings.json
```

编辑 `appsettings.json`，修改 `ConnectionStrings:ErpDb` 与 `Jwt:SecretKey` 等。

### 3. 启动后端（默认 http://localhost:5290）
```bash
cd Demo/Demo.Api.Admin
dotnet restore
dotnet run
```
启动后访问 **Swagger 文档**：`http://localhost:5290/swagger`

### 4. 启动前端（默认 http://localhost:5173）
```bash
cd Demoue/vite-project
npm install
npm run dev
```
前端通过 Vite 代理将 `/api` 请求转发至后端 `http://localhost:5290`。浏览器打开 `http://localhost:5173` 即可访问管理后台。

---

## 使用 Docker Compose 部署

容器化编排三个服务：**MySQL（自动建库并导入 102 张表）→ 后端 API → 前端（Nginx）**。Redis 与 RabbitMQ 默认未启用，不影响系统运行。

### 1. 目录与新增文件
```
ERP/
├── docker-compose.yml              # 主编排（mysql + backend + frontend）
├── docker-compose.full.yml          # 可选组件叠加（redis + rabbitmq）
├── .env.example                     # 环境变量模板（已提交）
├── .env                             # 本地实际变量（已被 .gitignore 忽略）
├── Demo/
│   ├── Dockerfile                  # 后端镜像（sdk 构建 → aspnet 运行）
│   └── .dockerignore               # 排除 bin/obj 与含密钥的 appsettings
├── Demoue/vite-project/
│   ├── Dockerfile                  # 前端镜像（node 构建 → nginx 托管）
│   ├── nginx.conf                  # 反向代理 /api 到 backend
│   └── .dockerignore
└── SqlScript/erp_db.sql            # 挂载到 mysql 容器自动初始化
```

### 2. 环境变量配置
所有密码与密钥集中在根目录 `.env`，**不写入代码、不进镜像**：

```bash
cp .env.example .env
# 按需修改 MYSQL_ROOT_PASSWORD、JWT_SECRET_KEY 等
```

`.env` 已被 `.gitignore` 忽略，仅 `.env.example` 会提交到仓库。所有变量均带默认值，未配置时自动回退（如 `MYSQL_ROOT_PASSWORD:-erp_password`），因此即使缺少 `.env` 也能直接启动。

### 3. 构建并启动
```bash
# 在项目根目录执行（需已安装 Docker 与 Docker Compose v2）
docker compose up -d --build
```
首次启动会依次：拉取镜像 → 构建前后端 → 等待 MySQL 健康 → 启动后端（自动执行建表脚本）→ 启动前端。

### 4. 访问地址
- 前端管理后台：**http://localhost:5173**
- 后端 API / Swagger 文档：**http://localhost:8080**（Development 环境下 Swagger 挂在根路径）
> 端口可通过 `.env` 中的 `FRONTEND_PORT` / `BACKEND_PORT` / `MYSQL_PORT` 调整。

### 5. 数据初始化
`SqlScript/erp_db.sql` 通过挂载卷 `./SqlScript/erp_db.sql:/docker-entrypoint-initdb.d/` 在 **MySQL 数据卷首次为空时**自动导入，无需手动执行。若需重新初始化，删除数据卷后重启：
```bash
docker compose down -v && docker compose up -d --build
```

### 6. 常用运维命令
```bash
docker compose logs -f backend     # 查看后端日志
docker compose ps                  # 查看服务状态
docker compose down                # 停止并移除容器（保留数据卷）
docker compose down -v             # 停止并删除数据卷（清空数据库）
```

### 7. 可选组件：Redis 与 RabbitMQ
默认编排下，RabbitMQ 未配置即自动禁用、Redis 不阻断启动，系统可完整运行。启用后可得**异步库存预警**（RabbitMQ 消息总线 + Redis 幂等去重与快照缓存）：

```bash
docker compose -f docker-compose.yml -f docker-compose.full.yml up -d --build
```
- Redis：`localhost:6379`（AOF 持久化）
- RabbitMQ：`localhost:5672`，管理控制台 `http://localhost:15672`（guest/guest）

> 该文件**刻意不命名为** `docker-compose.override.yml` —— 后者会被 `docker compose up` 自动加载，将使可选组件变成强制启动；使用独立文件名并显式 `-f` 才能保持"按需启用"的语义。

### 8. 配置与安全说明
- **密钥注入**：`docker-compose.yml` 通过环境变量（如 `ConnectionStrings__ErpDb`、`Jwt__SecretKey`）覆盖镜像内 `appsettings.example.json` 的占位值，真实凭证不落盘、不进镜像；取值统一来自 `.env`。
- **镜像安全**：`.dockerignore` 已排除真实的 `appsettings.json` / `appsettings.Development.json`，避免数据库密码与 JWT 密钥被打包进镜像层。
- **生产部署**：将 `.env` 中的 `ASPNETCORE_ENVIRONMENT` 改为 `Production`（关闭 Swagger），并务必替换 `MYSQL_ROOT_PASSWORD` 与 `JWT_SECRET_KEY` 为强密钥；`.env` 请通过密钥管理或部署流水线下发，切勿提交。
- **健康检查**：MySQL、Redis、RabbitMQ 均配置了健康检查，后端按 `service_healthy` 条件启动，避免依赖未就绪导致的启动崩溃。

---

## 接口文档

后端已集成 Swagger / OpenAPI，运行后在以下地址查看全部接口：

```
http://localhost:5290/swagger
```

---

## 安全说明

- 仓库已将含真实凭证的 `appsettings.json` / `appsettings.Development.json` 加入 `.gitignore`，请以 `appsettings.example.json` 为模板自行配置。
- JWT 密钥、数据库密码等敏感信息请使用环境变量或密钥管理，避免硬编码到版本库。
- 示例代码中的默认凭证仅用于本地演示，**切勿在生产环境使用**。

---

## 目录总览

```
ERP/
├── Demo/                  # 后端解决方案（.NET 9 分层架构）
│   ├── Dockerfile         # 后端镜像构建
│   └── .dockerignore
├── Demoue/vite-project/  # 前端（Vue 3 + Vite + TypeScript）
│   ├── Dockerfile         # 前端镜像构建
│   ├── nginx.conf         # 反向代理配置
│   └── .dockerignore
├── SqlScript/             # 数据库初始化脚本（erp_db.sql）
├── docker-compose.yml     # 服务编排（mysql + backend + frontend）
├── docker-compose.full.yml # 可选组件叠加（redis + rabbitmq）
├── .env.example           # 环境变量模板（已提交）
├── .gitignore
└── README.md
```

---

## License

[MIT](LICENSE) —— 仅供学习与演示使用。
