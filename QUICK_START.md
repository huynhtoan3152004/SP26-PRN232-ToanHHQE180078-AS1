# ============================================
# QUICK START GUIDE
# ============================================

## 🚀 Hướng dẫn chạy nhanh

### Bước 1: Start Docker Desktop
- Mở Docker Desktop (nếu chưa chạy)
- Đợi Docker Desktop khởi động xong (icon Docker ở system tray sáng)

### Bước 2: Chạy script tự động
```powershell
cd "d:\WorkSpace\Ki8\PRN232\HuynhHuuToan_ SE1856_A01_BE"
.\setup-docker-sql.ps1
```

Script sẽ tự động:
1. ✅ Kiểm tra Docker Desktop
2. ✅ Start/Create SQL Server container
3. ✅ Copy SQL script vào container
4. ✅ Chạy SQL script (tạo tables + seed data)
5. ✅ Verify data
6. ✅ Start API

### Bước 3: Test API
Sau khi API chạy, mở browser:
- **Swagger UI**: `https://localhost:7xxx/swagger`
- **Health Check**: `GET /api/Health`

### Các endpoint để test:
```
GET /api/Health              # Kiểm tra kết nối DB + số lượng data
GET /api/Category            # Lấy tất cả categories
GET /api/Category/1          # Chi tiết category (full includes)
GET /api/NewsArticle         # Lấy tất cả news articles
GET /api/NewsArticle/NEWS001 # Chi tiết news article (full includes)
```

---

## 🐳 Lệnh Docker thủ công (nếu cần)

### Start container
```powershell
docker start sqlserver_funews
```

### Stop container
```powershell
docker stop sqlserver_funews
```

### View logs
```powershell
docker logs sqlserver_funews
```

### Connect to SQL Server
```powershell
docker exec -it sqlserver_funews /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315"
```

### Run SQL script manually
```powershell
docker cp "docker/init-db/FUNewsManagementSystem.sql" sqlserver_funews:/tmp/
docker exec -it sqlserver_funews /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -i /tmp/FUNewsManagementSystem.sql
```

---

## 🔧 Troubleshooting

### Lỗi: Docker Desktop không chạy
**Giải pháp**: Mở Docker Desktop và đợi khởi động xong

### Lỗi: Container đã tồn tại
**Giải pháp**: 
```powershell
docker start sqlserver_funews
```

### Lỗi: Port 1433 đã được sử dụng
**Giải pháp**: 
```powershell
# Kiểm tra process đang dùng port 1433
netstat -ano | findstr :1433

# Kill process (thay <PID> bằng Process ID)
taskkill /PID <PID> /F
```

### Lỗi: SQL script không chạy
**Giải pháp**: Chạy script theo từng phần:
```powershell
# 1. Create database
docker exec -it sqlserver_funews /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -Q "CREATE DATABASE FUNewsManagementSystem"

# 2. Run full script
docker exec -it sqlserver_funews /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Zzdigimon@315" -i /tmp/FUNewsManagementSystem.sql
```

---

## 📊 Expected Data after Seed

| Table | Records |
|-------|---------|
| Category | 8 |
| Tag | 8 |
| SystemAccount | 5 |
| NewsArticle | 10 |
| NewsTag | 23 |

---

## ✅ Checklist để test đầy đủ

- [ ] Health check returns "healthy"
- [ ] GET /api/Category returns 8 categories
- [ ] GET /api/Category/1 includes Parent, Children, NewsArticleCount
- [ ] GET /api/NewsArticle returns 10 articles
- [ ] GET /api/NewsArticle/NEWS001 includes Category, CreatedBy, UpdatedBy, Tags
- [ ] Search works: `/api/Category?searchTerm=tech`
- [ ] Sort works: `/api/Category?sortBy=CategoryName&sortOrder=asc`
- [ ] Paging works: `/api/Category?pageNumber=1&pageSize=5`
- [ ] Filter works: `/api/NewsArticle?categoryID=4&newsStatus=true`

---

## 🎯 Quick Commands

```powershell
# Setup everything
.\setup-docker-sql.ps1

# Just start API (if DB already setup)
cd "HuynhHuuToan_ SE1856_A01_BE"
dotnet run

# Build project
dotnet build

# Check Docker containers
docker ps

# Restart SQL Server
docker restart sqlserver_funews
```
