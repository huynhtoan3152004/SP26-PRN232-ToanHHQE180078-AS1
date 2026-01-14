# 🎯 HƯỚNG DẪN SỬ DỤNG - FU NEWS MANAGEMENT SYSTEM

## ✅ SETUP HOÀN TẤT - CHECKLIST

### 1. **Generic Repository Pattern** ✅
- ✅ `IGenericRepository<T>` - Interface chung
- ✅ `GenericRepository<T>` - Implementation
- ✅ `Query()`, `FindByIdAsync()`, `AddAsync()`, `Update()`, `Remove()`

### 2. **Unit of Work Pattern** ✅  
- ✅ `IUnitOfWork` - Interface quản lý repositories
- ✅ `UnitOfWork` - Implementation với lazy loading
- ✅ `SaveChangesAsync()` - Transaction management

### 3. **Naming Convention** ✅
- ✅ PascalCase: Classes, Methods, Properties
- ✅ camelCase: parameters, local variables  
- ✅ Interface prefix: `I` (IUnitOfWork, ICategoryService)
- ✅ Async suffix cho async methods
- ✅ Dto suffix cho Data Transfer Objects

### 4. **GetId ĐẦY ĐỦ THÔNG TIN** ✅

#### Category GetById trả về:
```json
{
  "categoryID": 1,
  "categoryName": "Technology",
  "parentCategory": { "categoryID": null, "categoryName": "..." },
  "children": [ {...}, {...} ],
  "newsArticleCount": 15
}
```

#### NewsArticle GetById trả về:
```json
{
  "newsArticleID": 1,
  "newsTitle": "...",
  "newsContent": "...",
  "category": { "categoryID": 1, "categoryName": "Technology" },
  "createdBy": { "accountID": 1, "accountName": "John", "accountEmail": "john@example.com" },
  "updatedBy": { "accountID": 2, "accountName": "Jane", "accountEmail": "jane@example.com" },
  "tags": [
    { "tagID": 1, "tagName": "AI" },
    { "tagID": 2, "tagName": "Machine Learning" }
  ]
}
```

### 5. **GetList với SEARCH, SORT, PAGING, SELECTION, EXPANSION** ✅

#### 🔍 **SEARCH** (tìm kiếm theo text)
```
GET /api/Category?searchTerm=technology
GET /api/NewsArticle?searchTerm=covid
```

#### 🔀 **SORT** (sắp xếp)
```
GET /api/Category?sortBy=CategoryName&sortOrder=asc
GET /api/NewsArticle?sortBy=CreatedDate&sortOrder=desc
```

#### 📄 **PAGING** (phân trang)
```
GET /api/Category?pageNumber=1&pageSize=10
GET /api/Tag?pageNumber=2&pageSize=20
```

#### 🎯 **SELECTION** (filter theo fields cụ thể)
```
# Category - filter theo ParentCategoryID và IsActive
GET /api/Category?parentCategoryID=1&isActive=true

# NewsArticle - filter theo CategoryID, Status, CreatedBy
GET /api/NewsArticle?categoryID=2&newsStatus=true&createdByID=1

# NewsArticle - filter theo Date Range
GET /api/NewsArticle?createdDateFrom=2024-01-01&createdDateTo=2024-12-31

# SystemAccount - filter theo Role
GET /api/SystemAccount?accountRole=1
```

#### 🔗 **EXPANSION** (Include navigation properties)
**GetById tự động expand đầy đủ thông tin:**
- Category: Include Parent + Children + NewsArticleCount
- NewsArticle: Include Category + CreatedBy + UpdatedBy + Tags
- Tag: Include NewsArticleCount
- SystemAccount: Include CreatedNewsCount + UpdatedNewsCount

### 6. **KẾT HỢP TẤT CẢ** ✅
```
GET /api/NewsArticle?searchTerm=covid&categoryID=1&newsStatus=true&sortBy=CreatedDate&sortOrder=desc&pageNumber=1&pageSize=10&createdDateFrom=2024-01-01
```

---

## 📡 API ENDPOINTS

### **Category API**
```
GET    /api/Category              - Danh sách (search, sort, paging, filter)
GET    /api/Category/{id}         - Chi tiết (full Parent, Children, NewsCount)
POST   /api/Category              - Tạo mới
PUT    /api/Category/{id}         - Cập nhật
DELETE /api/Category/{id}         - Xóa
```

### **Tag API**
```
GET    /api/Tag                   - Danh sách
GET    /api/Tag/{id}              - Chi tiết (NewsArticleCount)
POST   /api/Tag                   - Tạo mới
PUT    /api/Tag/{id}              - Cập nhật
DELETE /api/Tag/{id}              - Xóa
```

### **SystemAccount API**
```
GET    /api/SystemAccount         - Danh sách (filter by role)
GET    /api/SystemAccount/{id}    - Chi tiết (CreatedNewsCount, UpdatedNewsCount)
POST   /api/SystemAccount         - Tạo mới
PUT    /api/SystemAccount/{id}    - Cập nhật
DELETE /api/SystemAccount/{id}    - Xóa
```

### **NewsArticle API**
```
GET    /api/NewsArticle           - Danh sách (search, sort, paging, filter)
GET    /api/NewsArticle/{id}      - Chi tiết (Category, CreatedBy, UpdatedBy, Tags)
POST   /api/NewsArticle           - Tạo mới (kèm TagIDs)
PUT    /api/NewsArticle/{id}      - Cập nhật (kèm TagIDs)
DELETE /api/NewsArticle/{id}      - Xóa
```

---

## 🚀 CÁCH CHẠY

### 1. **Chạy API**
```bash
cd "HuynhHuuToan_ SE1856_A01_BE"
dotnet run
```
→ API chạy tại: `http://localhost:5024`  
→ Swagger UI: `http://localhost:5024/swagger`

### 2. **Test với Swagger UI**
1. Mở `http://localhost:5024/swagger`
2. Chọn endpoint muốn test (vd: `GET /api/Category`)
3. Click **"Try it out"**
4. Điền parameters (searchTerm, sortBy, pageNumber...)
5. Click **"Execute"**
6. Xem Response Body & Response Code

### 3. **Test với cURL**
```bash
# Lấy danh sách Category (search + sort + paging)
curl -X GET "http://localhost:5024/api/Category?searchTerm=tech&sortBy=CategoryName&sortOrder=asc&pageNumber=1&pageSize=10"

# Lấy chi tiết Category (full includes)
curl -X GET "http://localhost:5024/api/Category/1"

# Tạo mới Category
curl -X POST "http://localhost:5024/api/Category" \
  -H "Content-Type: application/json" \
  -d '{"categoryName":"Technology","categoryDescription":"Tech news","isActive":true}'

# Lấy NewsArticle với nhiều filter
curl -X GET "http://localhost:5024/api/NewsArticle?categoryID=1&newsStatus=true&sortBy=CreatedDate&sortOrder=desc&pageNumber=1&pageSize=10"
```

---

## 📦 CẤU TRÚC PROJECT

```
├── HuynhHuuToan_ SE1856_A01_BE/              # 🌐 API Layer
│   ├── Controllers/
│   │   ├── CategoryController.cs             # CRUD endpoints
│   │   ├── TagController.cs
│   │   ├── SystemAccountController.cs
│   │   └── NewsArticleController.cs
│   └── Program.cs                            # DI Configuration
│
├── HuynhHuuToan_ SE1856_A01_Service/         # 💼 Business Logic Layer
│   ├── Common/
│   │   ├── PagedResult.cs                    # Generic paging result
│   │   └── BaseQueryParams.cs                # Base class cho query params
│   ├── DTOs/
│   │   ├── Category/CategoryDtos.cs          # Create, Update, Response, Detail
│   │   ├── Tag/TagDtos.cs
│   │   ├── SystemAccount/SystemAccountDtos.cs
│   │   └── NewsArticle/NewsArticleDtos.cs
│   ├── QueryParams/
│   │   ├── CategoryQueryParams.cs            # Search, Sort, Paging, Filter
│   │   ├── TagQueryParams.cs
│   │   ├── SystemAccountQueryParams.cs
│   │   └── NewsArticleQueryParams.cs
│   ├── Extensions/
│   │   └── QueryExtensions.cs                # ApplyPaging, ApplySorting
│   └── Services/
│       ├── ICategoryService.cs               # Interface
│       ├── CategoryService.cs                # Implementation với UoW
│       ├── ITagService.cs
│       ├── TagService.cs
│       ├── ISystemAccountService.cs
│       ├── SystemAccountService.cs
│       ├── INewsArticleService.cs
│       └── NewsArticleService.cs
│
└── HuynhHuuToan_ SE1856_A01_Repository/      # 🗄️ Data Access Layer
    ├── Models/
    │   ├── Entities/                         # Entity classes
    │   │   ├── Category.cs
    │   │   ├── Tag.cs
    │   │   ├── SystemAccount.cs
    │   │   └── NewsArticle.cs
    │   └── Data/
    │       └── FUNewsManagementSystemContext.cs  # DbContext
    ├── Repositories/
    │   ├── IGenericRepository.cs             # Generic repository interface
    │   └── GenericRepository.cs              # Generic repository implementation
    └── UnitOfWork/
        ├── IUnitOfWork.cs                    # UoW interface
        └── UnitOfWork.cs                     # UoW implementation
```

---

## 🎓 GIẢI THÍCH CHO NGƯỜI MỚI HỌC

### 1. **Generic Repository là gì?**
Thay vì viết riêng Repository cho từng entity:
```csharp
// ❌ KHÔNG DÙng: Phải viết 4 repository giống nhau
public class CategoryRepository { ... }
public class TagRepository { ... }
public class SystemAccountRepository { ... }
public class NewsArticleRepository { ... }

// ✅ DÙNG: Chỉ 1 Generic Repository cho tất cả
public class GenericRepository<T> where T : class { ... }
```

### 2. **Unit of Work là gì?**
Quản lý tất cả repositories và transactions ở 1 chỗ:
```csharp
// ❌ KHÔNG DÙNG: SaveChanges nhiều lần, không có transaction
_context.Categories.Add(category);
_context.SaveChanges();
_context.Tags.Add(tag);
_context.SaveChanges();

// ✅ DÙNG: UnitOfWork quản lý transaction
_unitOfWork.Categories.Add(category);
_unitOfWork.Tags.Add(tag);
_unitOfWork.SaveChangesAsync(); // Chỉ save 1 lần
```

### 3. **Query Pipeline hoạt động như thế nào?**
```csharp
// Bước 1: Lấy IQueryable
var query = _unitOfWork.Categories.Query();

// Bước 2: Apply Search
query = query.Where(c => c.CategoryName.Contains(searchTerm));

// Bước 3: Apply Filter
query = query.Where(c => c.IsActive == true);

// Bước 4: Count (trước khi paging)
var totalItems = await query.CountAsync();

// Bước 5: Apply Sort
query = query.OrderBy(c => c.CategoryName);

// Bước 6: Apply Paging
query = query.Skip(0).Take(10);

// Bước 7: Execute query (ToListAsync)
var items = await query.ToListAsync();
```

### 4. **Tại sao GetById phải Include đầy đủ?**
```csharp
// ❌ KHÔNG DÙNG: Chỉ trả ID, không có thông tin related
{
  "categoryID": 1,
  "categoryName": "Technology",
  "parentCategoryID": 5  // ← Chỉ có ID, không biết tên Parent là gì
}

// ✅ DÙNG: Include Parent để lấy đầy đủ thông tin
{
  "categoryID": 1,
  "categoryName": "Technology",
  "parentCategory": {
    "categoryID": 5,
    "categoryName": "Main Categories"  // ← Đầy đủ thông tin Parent
  }
}
```

---

## 🐛 TROUBLESHOOTING

### Lỗi: "Unable to connect to database"
```bash
# Kiểm tra SQL Server đang chạy
# Cập nhật connection string trong appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=FUNewsManagementSystem;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=True;"
  }
}
```

### Lỗi: "Build failed"
```bash
# Clean và rebuild
dotnet clean
dotnet build
```

### Lỗi: "Port 5024 already in use"
```bash
# Tìm và kill process đang dùng port
netstat -ano | findstr :5024
taskkill /PID <PID> /F
```

---

## 📝 GIT COMMANDS

### Push lên GitHub
```bash
# Chạy script tự động
.\git-setup.ps1

# Hoặc thủ công:
git init
git add .
git commit -m "Initial commit: Full CRUD with Generic Repository and Unit of Work"
git branch -M main
git remote add origin https://github.com/huynhtoan3152004/SP26-PRN232-ToanHHQE180078-AS1.git
git push -u origin main
```

---

## ✅ LAB2 REQUIREMENTS CHECKLIST

- ✅ **Generic Repository Pattern** - IGenericRepository<T>, GenericRepository<T>
- ✅ **Unit of Work Pattern** - IUnitOfWork, UnitOfWork
- ✅ **Naming Convention chuẩn** - PascalCase, camelCase, Interface prefix
- ✅ **GetById đầy đủ thông tin** - Include Parent, Children, Tags, Category, CreatedBy, UpdatedBy
- ✅ **GetList có Search** - searchTerm parameter
- ✅ **GetList có Sort** - sortBy, sortOrder parameters
- ✅ **GetList có Paging** - pageNumber, pageSize, PagedResult<T>
- ✅ **GetList có Selection** - Filter by specific fields (categoryID, newsStatus, isActive...)
- ✅ **GetList có Expansion** - Include navigation properties in GetById
- ✅ **Clean code structure** - 3 layers (API, Service, Repository)
- ✅ **Dễ hiểu cho người mới** - Comments tiếng Việt, giải thích rõ ràng

---

## 👨‍💻 AUTHOR
**Huỳnh Hữu Toàn**
- Student ID: QE180078
- Class: SE1856
- Email: toanhhqe180078@fpt.edu.vn
- GitHub: https://github.com/huynhtoan3152004/SP26-PRN232-ToanHHQE180078-AS1
