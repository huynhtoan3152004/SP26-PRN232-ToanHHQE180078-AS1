# SP26-PRN232-ToanHHQE180078-AS1

## 📋 Giới thiệu
Dự án FU News Management System - Assignment 1

**Sinh viên:** Huỳnh Hữu Toàn - SE1856 - QE180078

## 🏗️ Kiến trúc 3 lớp (3-Layer Architecture)

```
├── HuynhHuuToan_ SE1856_A01_BE/           # API Layer (Controllers)
├── HuynhHuuToan_ SE1856_A01_Service/      # Business Logic Layer
│   ├── Common/                             # PagedResult, BaseQueryParams
│   ├── DTOs/                               # Data Transfer Objects
│   ├── QueryParams/                        # Search/Filter parameters
│   ├── Extensions/                         # Query Extensions
│   └── Services/                           # Service Interfaces & Implementations
└── HuynhHuuToan_ SE1856_A01_Repository/   # Data Access Layer
    ├── Models/
    │   ├── Entities/                       # Entity classes
    │   └── Data/                           # DbContext
    ├── Repositories/                       # Generic Repository
    └── UnitOfWork/                         # Unit of Work pattern
```

## ✨ Tính năng

### 1. **Generic Repository Pattern**
- IGenericRepository<T> interface
- Tái sử dụng code cho tất cả entities
- Query(), FindByIdAsync(), AddAsync(), Update(), Remove()

### 2. **Unit of Work Pattern**
- Quản lý transactions
- SaveChangesAsync() tập trung
- Đảm bảo data consistency

### 3. **Query Features (đầy đủ Lab2)**

#### ✅ **Search**
```
GET /api/Category?searchTerm=technology
```

#### ✅ **Sort**
```
GET /api/Category?sortBy=CategoryName&sortOrder=asc
```

#### ✅ **Paging**
```
GET /api/Category?pageNumber=1&pageSize=10
```

#### ✅ **Selection** (Filter theo fields)
```
GET /api/Category?isActive=true&parentCategoryID=1
GET /api/NewsArticle?categoryID=2&newsStatus=true&createdDateFrom=2024-01-01
```

#### ✅ **Expansion** (Include navigation properties)
- GetById luôn trả đầy đủ thông tin related entities
- Category: Parent, Children, NewsArticleCount
- NewsArticle: Category, CreatedBy, UpdatedBy, Tags

### 4. **CRUD Operations**
Tất cả 4 entities: Category, Tag, SystemAccount, NewsArticle

## 🗄️ Database Entities

1. **Category** (Danh mục tin tức)
2. **Tag** (Nhãn cho tin tức)
3. **SystemAccount** (Tài khoản hệ thống)
4. **NewsArticle** (Bài viết tin tức)

## 🚀 Cách chạy

### 1. Cài đặt
```bash
# Clone repository
git clone https://github.com/huynhtoan3152004/SP26-PRN232-ToanHHQE180078-AS1.git
cd SP26-PRN232-ToanHHQE180078-AS1
```

### 2. Cấu hình Database
Cập nhật `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=FUNewsManagementSystem;User Id=sa;Password=YourPassword;TrustServerCertificate=True;Encrypt=True;"
  }
}
```

### 3. Chạy migrations (nếu cần)
```bash
cd HuynhHuuToan_\ SE1856_A01_Repository
dotnet ef database update
```

### 4. Build & Run
```bash
cd ..
dotnet build
dotnet run --project "HuynhHuuToan_ SE1856_A01_BE"
```

### 5. Mở Swagger UI
```
https://localhost:7xxx/swagger
```

## 📡 API Endpoints

### Category
- `GET /api/Category` - Danh sách (search, sort, paging, filter)
- `GET /api/Category/{id}` - Chi tiết (full includes)
- `POST /api/Category` - Tạo mới
- `PUT /api/Category/{id}` - Cập nhật
- `DELETE /api/Category/{id}` - Xóa

### Tag
- `GET /api/Tag`
- `GET /api/Tag/{id}`
- `POST /api/Tag`
- `PUT /api/Tag/{id}`
- `DELETE /api/Tag/{id}`

### SystemAccount
- `GET /api/SystemAccount`
- `GET /api/SystemAccount/{id}`
- `POST /api/SystemAccount`
- `PUT /api/SystemAccount/{id}`
- `DELETE /api/SystemAccount/{id}`

### NewsArticle
- `GET /api/NewsArticle`
- `GET /api/NewsArticle/{id}`
- `POST /api/NewsArticle`
- `PUT /api/NewsArticle/{id}`
- `DELETE /api/NewsArticle/{id}`

## 📝 Query Parameters Examples

### Search + Sort + Paging
```
GET /api/Category?searchTerm=tech&sortBy=CategoryName&sortOrder=asc&pageNumber=1&pageSize=10
```

### Filter + Sort
```
GET /api/NewsArticle?categoryID=1&newsStatus=true&sortBy=CreatedDate&sortOrder=desc
```

### Date Range Filter
```
GET /api/NewsArticle?createdDateFrom=2024-01-01&createdDateTo=2024-12-31
```

## 🛠️ Technologies

- **.NET 8.0**
- **Entity Framework Core 8.0**
- **SQL Server**
- **Swagger/OpenAPI**
- **Generic Repository Pattern**
- **Unit of Work Pattern**

## 📦 Naming Convention

✅ Tuân thủ C# naming conventions:
- PascalCase: Classes, Methods, Properties
- camelCase: parameters, local variables
- Interface prefix: I (IUnitOfWork, ICategoryService)
- Async suffix cho async methods
- Dto suffix cho Data Transfer Objects

## 🎯 Lab2 Requirements Checklist

- ✅ Generic Repository pattern
- ✅ Unit of Work pattern
- ✅ Naming Convention chuẩn
- ✅ GetById đầy đủ thông tin (includes)
- ✅ GetList có Search
- ✅ GetList có Sort
- ✅ GetList có Paging
- ✅ GetList có Selection (filter)
- ✅ GetList có Expansion (includes)
- ✅ Clean code structure (3 layers)
- ✅ Dễ hiểu cho người mới học

## 👨‍💻 Author
**Huỳnh Hữu Toàn**
- Student ID: QE180078
- Class: SE1856
- Email: toanhhqe180078@fpt.edu.vn
