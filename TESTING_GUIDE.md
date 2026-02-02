# Fields & Expand API Testing Guide

## 📚 Tổng quan

API Category giờ đã hỗ trợ 2 tính năng nâng cao:
- **Fields**: Chọn các trường cần trả về → giảm payload
- **Expand**: Eager loading các navigation properties → giảm N+1 queries

---

## 🧪 Test Cases

### ✅ Test 1: Standard Query (không Fields, không Expand)

```http
GET /api/Category?PageNumber=1&PageSize=5
```

**Response:**
```json
{
  "items": [
    {
      "categoryID": 1,
      "categoryName": "Technology",
      "categoryDescription": "All about technology...",
      "parentCategoryID": null,
      "isActive": true
    }
  ],
  "totalItems": 40,
  "pageNumber": 1,
  "pageSize": 5,
  "totalPages": 8,
  "hasPrevious": false,
  "hasNext": true
}
```

---

### ✅ Test 2: Fields Projection - Chỉ lấy ID và Name

```http
GET /api/Category?Fields=CategoryID,CategoryName&PageSize=10
```

**Response:**
```json
{
  "items": [
    {
      "CategoryID": 1,
      "CategoryName": "Technology"
    },
    {
      "CategoryID": 2,
      "CategoryName": "Business"
    }
  ],
  "totalItems": 40,
  "pageNumber": 1,
  "pageSize": 10
}
```

**Lợi ích:** Payload giảm ~60% so với full DTO

---

### ✅ Test 3: Expand ParentCategory

```http
GET /api/Category?ParentCategoryID=1&Expand=ParentCategory&PageSize=5
```

**Kết quả:** Eager load parent category, giảm N+1 queries

---

### ✅ Test 4: Kết hợp Fields + Filter + Search

```http
GET /api/Category?SearchTerm=AI&IsActive=true&Fields=CategoryID,CategoryName,ParentCategoryID&PageSize=20
```

**Response:**
```json
{
  "items": [
    {
      "CategoryID": 6,
      "CategoryName": "Artificial Intelligence",
      "ParentCategoryID": 1
    }
  ]
}
```

---

### ✅ Test 5: Lấy tất cả children của Technology

```http
GET /api/Category?ParentCategoryID=1&SortBy=CategoryName&SortOrder=asc
```

**Response:** Danh sách AI, Web Dev, Mobile Apps, Cybersecurity, Cloud, DevOps

---

### ✅ Test 6: Filter Inactive Categories

```http
GET /api/Category?IsActive=false
```

**Response:** Chỉ trả về "Archived Tech" và "Deprecated Category"

---

### ✅ Test 7: Search + Sort + Page

```http
GET /api/Category?SearchTerm=development&SortBy=CategoryName&SortOrder=desc&PageNumber=1&PageSize=5
```

---

### ✅ Test 8: Expand Children

```http
GET /api/Category?ParentCategoryID=&Expand=Children&PageSize=5
```

**Kết quả:** Root categories với children được load sẵn

---

### ✅ Test 9: Fields với nhiều trường

```http
GET /api/Category?Fields=CategoryID,CategoryName,IsActive&IsActive=true&PageSize=100
```

---

### ✅ Test 10: Hierarchical Query (3 levels)

```http
GET /api/Category?ParentCategoryID=6&Expand=ParentCategory
```

**Kết quả:** 
- Level 3: NLP, Computer Vision, Robotics
- ParentCategory loaded: Artificial Intelligence

---

## 📊 Performance Comparison

| Scenario | Without Fields | With Fields | Reduction |
|----------|----------------|-------------|-----------|
| 100 categories | ~50KB | ~20KB | 60% |
| 1000 categories | ~500KB | ~200KB | 60% |

| Scenario | Without Expand | With Expand | Queries |
|----------|----------------|-------------|---------|
| Get 10 categories + parents | 11 queries | 1 query | 90% faster |
| Get 100 categories + children | 101 queries | 1 query | 99% faster |

---

## 🔧 Advanced Use Cases

### UC1: Admin Dashboard - Category Tree
```http
GET /api/Category?Fields=CategoryID,CategoryName,ParentCategoryID&IsActive=true&PageSize=1000
```

### UC2: Dropdown List
```http
GET /api/Category?Fields=CategoryID,CategoryName&IsActive=true&SortBy=CategoryName&PageSize=100
```

### UC3: Category Management Page
```http
GET /api/Category?Expand=ParentCategory,Children&PageSize=20
```

### UC4: Mobile App (bandwidth sensitive)
```http
GET /api/Category?Fields=CategoryID,CategoryName,IsActive&PageSize=50
```

---

## 🐛 Error Cases

### ❌ Invalid Field Name
```http
GET /api/Category?Fields=InvalidField,CategoryName
```
**Response:** Chỉ trả về CategoryName, bỏ qua InvalidField

### ❌ Invalid Expand
```http
GET /api/Category?Expand=NonExistentRelation
```
**Response:** Bỏ qua expand không hợp lệ

---

## 📝 Valid Values

### Fields (CategoryResponseDto properties)
- `CategoryID`
- `CategoryName`
- `CategoryDescription`
- `ParentCategoryID`
- `IsActive`

### Expand (Navigation properties)
- `ParentCategory` hoặc `Parent`
- `Children` hoặc `InverseParentCategory`
- `NewsArticles`

### SortBy
- `CategoryName` (default)
- `CreatedDate`
- `CategoryID`

### SortOrder
- `asc` (default)
- `desc`

---

## 🚀 Next Steps

Áp dụng pattern tương tự cho:
- ✅ TagService
- ✅ NewsArticleService (thêm filter: Tags, Status, DateRange)
- ✅ SystemAccountService (thêm filter: Multi-role)
