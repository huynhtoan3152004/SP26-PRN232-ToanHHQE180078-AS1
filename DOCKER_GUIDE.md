# 🐳 Hướng Dẫn Chạy Docker SQL Server Từ Đầu Đến Cuối

## 📋 Mục Lục
1. [Chuẩn Bị](#chuẩn-bị)
2. [Bước 1: Start Docker Desktop](#bước-1-start-docker-desktop)
3. [Bước 2: Kiểm Tra Container](#bước-2-kiểm-tra-container)
4. [Bước 3: Start SQL Server Container](#bước-3-start-sql-server-container)
5. [Bước 4: Seed Database](#bước-4-seed-database)
6. [Bước 5: Start API](#bước-5-start-api)
7. [Bước 6: Test API](#bước-6-test-api)
8. [Troubleshooting](#troubleshooting)

---

## Chuẩn Bị

### Yêu Cầu
- ✅ Docker Desktop đã cài đặt
- ✅ .NET 8.0 SDK đã cài đặt
- ✅ SQL Server image: `mcr.microsoft.com/mssql/server:2022-latest`
- ✅ Container name: `sql2022`
- ✅ Port: `1433`

### Thông Tin Kết Nối
```
Server: localhost,1433
Database: FUNewsManagementSystem
Username: sa
Password: Zzdigimon@315
```

---

## Bước 1: Start Docker Desktop

### Cách 1: Mở Docker Desktop Thủ Công
1. Tìm Docker Desktop trong Start Menu
2. Click để mở
3. Đợi Docker Desktop khởi động (icon Docker ở system tray sáng màu xanh)
4. Verify: Click vào icon Docker, chọn "Docker Desktop is running"

### Cách 2: Start Docker Desktop Từ PowerShell
```powershell
Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"
Start-Sleep -Seconds 30
```

### Kiểm Tra Docker Đã Chạy
```powershell
docker --version
docker ps
```

**Kết quả mong đợi:**
```
Docker version 20.x.x, build xxxxx
CONTAINER ID   IMAGE     COMMAND   CREATED   STATUS    PORTS     NAMES
```

---

## Bước 2: Kiểm Tra Container

### Xem Tất Cả Containers (kể cả stopped)
```powershell
docker ps -a
```

**Tìm container SQL Server:**
```powershell
docker ps -a | Select-String "sql"
```

**Kết quả mong đợi:**
```
3dc5e869461a   mcr.microsoft.com/mssql/server:2022-latest   ...   sql2022
```

### Nếu Container Chưa Tồn Tại - Tạo Mới
```powershell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Zzdigimon@315" `
   -p 1433:1433 --name sql2022 `
   -d mcr.microsoft.com/mssql/server:2022-latest
```

**Giải thích:**
- `-e "ACCEPT_EULA=Y"`: Chấp nhận license SQL Server
- `-e "MSSQL_SA_PASSWORD=..."`: Đặt password cho user sa
- `-p 1433:1433`: Map port 1433 từ container ra host
- `--name sql2022`: Đặt tên container
- `-d`: Chạy ở background

---

## Bước 3: Start SQL Server Container

### Kiểm Tra Status Container
```powershell
docker ps -a --filter "name=sql2022"
```

**Nếu STATUS = "Exited" → Start container:**
```powershell
docker start sql2022
```

**Verify container đã chạy:**
```powershell
docker ps --filter "name=sql2022"
```

**Kết quả mong đợi:**
```
CONTAINER ID   IMAGE                                        STATUS          PORTS                    NAMES
3dc5e869461a   mcr.microsoft.com/mssql/server:2022-latest   Up 10 seconds   0.0.0.0:1433->1433/tcp   sql2022
```

### Đợi SQL Server Khởi Động Hoàn Toàn
```powershell
Start-Sleep -Seconds 10
Write-Host "✓ SQL Server is ready" -ForegroundColor Green
```

### Xem Logs Để Confirm
```powershell
docker logs sql2022 --tail 20
```

**Tìm dòng:**
```
SQL Server is now ready for client connections.
```

---

## Bước 4: Seed Database

### Bước 4.1: Copy SQL Script Vào Container
```powershell
cd "d:\WorkSpace\Ki8\PRN232\HuynhHuuToan_ SE1856_A01_BE"
docker cp "docker/init-db/FUNewsManagementSystem.sql" sql2022:/tmp/setup.sql
```

**Kết quả:**
```
Successfully copied 24.6kB to sql2022:/tmp/setup.sql
```

### Bước 4.2: Chạy SQL Script
```powershell
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -i /tmp/setup.sql
```

**Lưu ý:** 
- Dùng `/opt/mssql-tools18/bin/sqlcmd` (không phải `mssql-tools`)
- Thêm flag `-C` để trust server certificate

**Kết quả mong đợi:**
```
Changed database context to 'FUNewsManagementSystem'.
(8 rows affected)   <- Categories
(8 rows affected)   <- Tags
(5 rows affected)   <- SystemAccounts
(10 rows affected)  <- NewsArticles (nếu thành công)
```

### Bước 4.3: Verify Data
```powershell
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -Q "USE FUNewsManagementSystem; SELECT 'Categories' AS TableName, COUNT(*) AS RecordCount FROM Category UNION ALL SELECT 'Tags', COUNT(*) FROM Tag UNION ALL SELECT 'SystemAccounts', COUNT(*) FROM SystemAccount UNION ALL SELECT 'NewsArticles', COUNT(*) FROM NewsArticle;"
```

**Kết quả mong đợi:**
```
TableName       RecordCount
--------------- -----------
Categories      8
Tags            8
SystemAccounts  5
NewsArticles    10 (hoặc 0 nếu có lỗi IDENTITY_INSERT)
```

### Nếu NewsArticles = 0 - Fix Script Và Chạy Lại

**Kiểm tra NewsArticle ID type:**
```powershell
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -Q "USE FUNewsManagementSystem; SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NewsArticle' AND COLUMN_NAME = 'NewsArticleId';"
```

**Nếu DATA_TYPE = 'int' → Cần dùng số thay vì 'NEWS001'**

---

## Bước 5: Start API

### Bước 5.1: Navigate Đến Project Folder
```powershell
cd "d:\WorkSpace\Ki8\PRN232\HuynhHuuToan_ SE1856_A01_BE\HuynhHuuToan_ SE1856_A01_BE"
```

### Bước 5.2: Restore Dependencies (nếu cần)
```powershell
dotnet restore
```

### Bước 5.3: Build Project
```powershell
dotnet build
```

**Kết quả mong đợi:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Bước 5.4: Run API
```powershell
dotnet run
```

**Kết quả mong đợi:**
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5024
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Bước 5.5: Mở Swagger UI
**Trong browser, mở:**
```
http://localhost:5024/swagger
```

---

## Bước 6: Test API

### Test 1: Health Check
**Endpoint:** `GET /api/Health`

**Cách test trong Swagger:**
1. Expand endpoint `GET /api/Health`
2. Click "Try it out"
3. Click "Execute"

**Kết quả mong đợi:**
```json
{
  "status": "healthy",
  "database": "connected",
  "data": {
    "categories": 8,
    "tags": 8,
    "accounts": 5,
    "newsArticles": 10
  },
  "timestamp": "2026-01-14T...",
  "message": "Database has data"
}
```

### Test 2: Get All Categories
**Endpoint:** `GET /api/Category`

**Parameters:**
- `searchTerm`: (empty)
- `sortBy`: CategoryName
- `sortOrder`: asc
- `pageNumber`: 1
- `pageSize`: 10

**Kết quả mong đợi:**
```json
{
  "items": [
    {
      "categoryId": 1,
      "categoryName": "Technology",
      "categoryDesciption": "Latest in tech and innovation",
      "parentCategoryId": null,
      "newsArticleCount": 2
    },
    ...
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "totalRecords": 8
}
```

### Test 3: Get Category By ID (Full Details)
**Endpoint:** `GET /api/Category/1`

**Kết quả mong đợi:**
```json
{
  "categoryId": 1,
  "categoryName": "Technology",
  "categoryDesciption": "Latest in tech and innovation",
  "parentCategoryId": null,
  "parent": null,
  "children": [
    {
      "categoryId": 3,
      "categoryName": "AI & Machine Learning"
    },
    {
      "categoryId": 4,
      "categoryName": "Software Development"
    }
  ],
  "newsArticleCount": 2
}
```

### Test 4: Search Categories
**Endpoint:** `GET /api/Category?searchTerm=tech`

**Kết quả:** Trả về categories có chứa "tech" trong tên hoặc description

### Test 5: Get All Tags
**Endpoint:** `GET /api/Tag`

**Kết quả:** Trả về 8 tags

### Test 6: Create New Category (POST)
**Endpoint:** `POST /api/Category`

**Request Body:**
```json
{
  "categoryName": "Test Category",
  "categoryDesciption": "This is a test",
  "parentCategoryId": null
}
```

**Kết quả:** Status 201 Created + location header

### Test 7: Get All News Articles
**Endpoint:** `GET /api/NewsArticle`

**Kết quả:** Danh sách NewsArticles với Category, Tags

---

## 📊 Checklist Hoàn Thành

- [ ] Docker Desktop đã chạy
- [ ] Container sql2022 status = "Up"
- [ ] Database FUNewsManagementSystem đã tồn tại
- [ ] Categories: 8 records ✓
- [ ] Tags: 8 records ✓
- [ ] SystemAccounts: 5 records ✓
- [ ] NewsArticles: 10 records (hoặc tạo qua API)
- [ ] API đã chạy trên http://localhost:5024
- [ ] Swagger UI accessible
- [ ] Health check returns "healthy"
- [ ] GET /api/Category returns data
- [ ] GET /api/Category/1 includes parent/children

---

## Troubleshooting

### Lỗi 1: Docker Desktop Không Chạy
**Triệu chứng:**
```
error during connect: Get 'http://%2F%2F.%2Fpipe%2FdockerDesktopLinuxEngine/...
```

**Giải pháp:**
1. Mở Docker Desktop
2. Đợi icon Docker sáng xanh
3. Chạy lại lệnh

---

### Lỗi 2: Container Không Start
**Triệu chứng:**
```
docker start sql2022
Error response from daemon: driver failed...
```

**Giải pháp:**
```powershell
# Xóa container cũ
docker rm sql2022

# Tạo container mới
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Zzdigimon@315" `
   -p 1433:1433 --name sql2022 `
   -d mcr.microsoft.com/mssql/server:2022-latest
```

---

### Lỗi 3: Port 1433 Đã Được Sử Dụng
**Triệu chứng:**
```
Error starting userland proxy: listen tcp4 0.0.0.0:1433: bind: Only one usage...
```

**Giải pháp:**
```powershell
# Kiểm tra process đang dùng port 1433
netstat -ano | findstr :1433

# Kill process (thay <PID>)
taskkill /PID <PID> /F

# Hoặc stop container khác
docker stop $(docker ps -q --filter "publish=1433")
```

---

### Lỗi 4: SQL Script Không Chạy - "sqlcmd not found"
**Triệu chứng:**
```
exec: "/opt/mssql-tools/bin/sqlcmd": stat /opt/mssql-tools/bin/sqlcmd: no such file or directory
```

**Giải pháp:**
Dùng **mssql-tools18** thay vì mssql-tools:
```powershell
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -i /tmp/setup.sql
```

---

### Lỗi 5: IDENTITY_INSERT Error
**Triệu chứng:**
```
Cannot insert explicit value for identity column in table 'NewsArticle' when IDENTITY_INSERT is set to OFF
```

**Giải pháp:**
NewsArticle ID là INT IDENTITY, không thể insert giá trị cụ thể. Có 2 cách:

**Cách 1: Tạo NewsArticles qua API (Recommended)**
1. Mở Swagger
2. POST /api/NewsArticle
3. Điền thông tin (không cần NewsArticleId)

**Cách 2: Fix SQL Script**
```sql
-- Thay đổi NewsArticle ID từ NVARCHAR sang INT
-- Hoặc bật IDENTITY_INSERT
SET IDENTITY_INSERT NewsArticle ON;
-- INSERT statements
SET IDENTITY_INSERT NewsArticle OFF;
```

---

### Lỗi 6: API Connection Refused
**Triệu chứng:**
```
No connection could be made because the target machine actively refused it. (localhost:1433)
```

**Giải pháp:**
```powershell
# Kiểm tra SQL Server đang chạy
docker ps --filter "name=sql2022"

# Xem logs
docker logs sql2022 --tail 20

# Nếu cần restart
docker restart sql2022
Start-Sleep -Seconds 10
```

---

### Lỗi 7: Database Không Tồn Tại
**Triệu chứng:**
```
Cannot open database "FUNewsManagementSystem" requested by the login
```

**Giải pháp:**
```powershell
# Tạo database thủ công
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -Q "CREATE DATABASE FUNewsManagementSystem"

# Sau đó chạy lại seed script
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd `
   -S localhost -U sa -P "Zzdigimon@315" -C `
   -i /tmp/setup.sql
```

---

## 🎯 Script Tự Động Hoá (All-in-One)

**Tạo file `quick-start.ps1`:**
```powershell
# Quick Start - All Steps
Write-Host "=== Starting Docker SQL Server & API ===" -ForegroundColor Cyan

# 1. Start container
Write-Host "`n[1/4] Starting SQL Server..." -ForegroundColor Yellow
docker start sql2022
Start-Sleep -Seconds 10

# 2. Copy & Run SQL script
Write-Host "`n[2/4] Seeding database..." -ForegroundColor Yellow
docker cp "docker/init-db/FUNewsManagementSystem.sql" sql2022:/tmp/setup.sql
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -C -i /tmp/setup.sql

# 3. Verify data
Write-Host "`n[3/4] Verifying data..." -ForegroundColor Yellow
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -C -Q "USE FUNewsManagementSystem; SELECT 'Categories' AS TableName, COUNT(*) AS RecordCount FROM Category UNION ALL SELECT 'Tags', COUNT(*) FROM Tag UNION ALL SELECT 'SystemAccounts', COUNT(*) FROM SystemAccount UNION ALL SELECT 'NewsArticles', COUNT(*) FROM NewsArticle;"

# 4. Start API
Write-Host "`n[4/4] Starting API..." -ForegroundColor Yellow
cd "HuynhHuuToan_ SE1856_A01_BE"
Start-Process "http://localhost:5024/swagger"
dotnet run
```

**Chạy:**
```powershell
.\quick-start.ps1
```

---

## 📝 Summary Commands

```powershell
# Start Docker Desktop
Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"

# Start SQL Server
docker start sql2022

# Seed Database
docker cp "docker/init-db/FUNewsManagementSystem.sql" sql2022:/tmp/setup.sql
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -C -i /tmp/setup.sql

# Verify Data
docker exec sql2022 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -C -Q "USE FUNewsManagementSystem; SELECT COUNT(*) FROM Category;"

# Start API
cd "HuynhHuuToan_ SE1856_A01_BE"
dotnet run

# Open Swagger
start http://localhost:5024/swagger
```

---

## 🔗 Quick Links

- **Swagger UI**: http://localhost:5024/swagger
- **Health Check**: http://localhost:5024/api/Health
- **Categories**: http://localhost:5024/api/Category
- **Tags**: http://localhost:5024/api/Tag
- **System Accounts**: http://localhost:5024/api/SystemAccount
- **News Articles**: http://localhost:5024/api/NewsArticle

---

## 📚 Tài Liệu Liên Quan

- [README.md](README.md) - Project overview
- [SETUP_GUIDE.md](SETUP_GUIDE.md) - Detailed setup instructions
- [QUICK_START.md](QUICK_START.md) - Quick start guide

---

**Tác giả:** Huynh Huu Toan  
**Ngày cập nhật:** 2026-01-14  
**Version:** 1.0
