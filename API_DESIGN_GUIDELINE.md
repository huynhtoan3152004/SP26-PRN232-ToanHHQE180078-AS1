# 📘 API Design Guideline - PRN232 Team Standard

> **Version**: 1.0  
> **Last Updated**: 2026-02-02

---

## 1. Architecture & Project Structure

### 1.1 Kiến trúc 3-Layer
```
PRN232.[ProjectName].API        → Controllers (Presentation Layer)
PRN232.[ProjectName].Services   → Business Logic (Service Layer)
PRN232.[ProjectName].Repo       → Data Access (Repository Layer)
```

### 1.2 Trách nhiệm từng Layer

| Layer | Trách nhiệm | KHÔNG được làm |
|-------|-------------|----------------|
| **Controller** | Nhận request, Validate input, Gọi service, Trả response | Chứa business logic |
| **Service** | Xử lý toàn bộ business logic, Mapping giữa các model | Truy xuất DB trực tiếp |
| **Repository** | Truy xuất dữ liệu từ DB | Chứa nghiệp vụ |

---

## 2. Data Model Specification

### 2.1 Bắt buộc sử dụng 4 loại Model

| Model Type | Layer | Mô Tả |
|------------|-------|-------|
| **Entity Model** | Repository | Ánh xạ trực tiếp với DB (EF Core) |
| **Business Model** | Service | Xử lý nghiệp vụ nội bộ |
| **Request Model** | API | Nhận dữ liệu từ client |
| **Response Model** | API | Trả dữ liệu cho client |

### 2.2 Quy tắc KHÔNG được vi phạm

- ❌ **KHÔNG** trả Entity Model trực tiếp trong API response
- ❌ **KHÔNG** dùng Request/Response Model trong Service hoặc Repository
- ❌ **KHÔNG** để lộ cấu trúc DB hoặc chi tiết triển khai nội bộ

---

## 3. RESTful API Design & Naming Convention

### 3.1 Endpoint Pattern
- Theo hướng **resource-based**
- URL sử dụng **danh từ số nhiều**, không dùng động từ

```
✅ Đúng: /api/products, /api/products/{id}
❌ Sai:  /api/getProducts, /api/createProduct
```

### 3.2 HTTP Methods

| Method | Mục đích | Ví dụ |
|--------|----------|-------|
| `GET` | Lấy dữ liệu | `GET /api/products` |
| `POST` | Tạo mới | `POST /api/products` |
| `PUT` | Cập nhật toàn bộ | `PUT /api/products/{id}` |
| `DELETE` | Xóa | `DELETE /api/products/{id}` |

### 3.3 Query Parameters Convention
- Tên theo **camelCase**
- Phản ánh rõ ý nghĩa nghiệp vụ

---

## 4. GET Collection Resource (List API)

### 4.1 Endpoint
```
GET /api/{resources}
```

### 4.2 Query Parameters - Team Standard

| Parameter | Type | Mô Tả | Mặc định |
|-----------|------|-------|----------|
| `searchTerm` | `string` | Tìm kiếm full-text trên các field chính | - |
| `sortBy` | `string` | Tên field để sort | field chính của entity |
| `sortOrder` | `string` | `asc` hoặc `desc` | `asc` |
| `page` | `int` | Số trang hiện tại | `1` |
| `pageSize` | `int` | Số item mỗi trang | `10` |
| `fields` | `string` | Chọn fields cần trả về (comma-separated) | - |

### 4.3 Pagination Limits - Team Decision

| Setting | Giá trị |
|---------|---------|
| Default Page | `1` |
| Default Page Size | `10` |
| **Max Page Size** | `100` |
| Min Page Size | `1` |

> ⚠️ Nếu `pageSize > 100`, tự động giới hạn về `100`

### 4.4 Filter Parameters - Team Decision

#### a) Khi nào dùng Single ID vs Multiple IDs?

| Trường hợp | Format | Ví dụ |
|------------|--------|-------|
| Filter theo **1 giá trị** | `{field}={value}` | `?categoryId=3` |
| Filter theo **nhiều giá trị** | `{field}={v1},{v2},...` | `?categoryId=3,5,7` |

> 📌 **Team Decision**: Hỗ trợ cả 2 format. Backend tự detect dựa vào comma.

#### b) Khi nào dùng Enum vs Text?

| Dùng Enum | Dùng Text |
|-----------|-----------|
| Trạng thái cố định (Active/Inactive) | Tìm kiếm tự do |
| Role (Admin/Staff/User) | Mô tả, tiêu đề |
| Status (Pending/Approved/Rejected) | Địa chỉ, comments |

```
# Enum - dùng string value
?status=Active
?role=Admin

# Text - dùng searchTerm
?searchTerm=laptop
```

#### c) Date Range Filter

```
?createdDateFrom=2026-01-01&createdDateTo=2026-01-31
```

### 4.5 Response với Pagination Metadata

```json
{
  "success": true,
  "message": "Retrieved successfully",
  "data": {
    "items": [...],
    "page": 1,
    "pageSize": 10,
    "totalItems": 156,
    "totalPages": 16
  },
  "errors": null
}
```

### 4.6 Full Example
```http
GET /api/products?searchTerm=laptop
    &categoryId=3,5
    &status=Active
    &sortBy=createdDate
    &sortOrder=desc
    &page=1
    &pageSize=20
    &fields=id,name,price
```

---

## 5. GET Resource by ID

### 5.1 Endpoint
```
GET /api/{resources}/{id}
```

### 5.2 Team Decision

| Quy tắc | Chi tiết |
|---------|----------|
| **Số lượng ID** | Chỉ lấy theo **1 ID duy nhất** |
| **Trả về** | Đầy đủ thông tin liên quan của resource |
| **Độ sâu dữ liệu** | Tối đa **1 cấp** - không đệ quy vô hạn |
| **Circular reference** | Không được gây circular reference |

### 5.3 Response Example
```json
{
  "success": true,
  "message": "Category retrieved successfully",
  "data": {
    "categoryId": 5,
    "categoryName": "Technology",
    "categoryDescription": "Tech articles",
    "isActive": true,
    "parentCategoryId": 1,
    "parentCategoryName": "News",
    "childrenCount": 3,
    "newsArticleCount": 42
  },
  "errors": null
}
```

### 5.4 Khi Resource Không Tồn Tại
```json
{
  "success": false,
  "message": "Category with ID 999 not found",
  "data": null,
  "errors": null
}
```
**HTTP Status**: `404 Not Found`

---

## 6. POST Create

### 6.1 Endpoint
```
POST /api/{resources}
```

### 6.2 Request Body
- Sử dụng **Request Model** (CreateDto)
- Không bao gồm ID (auto-generated)

```json
POST /api/categories
Content-Type: application/json

{
  "categoryName": "New Category",
  "categoryDescription": "Description",
  "parentCategoryId": 1,
  "isActive": true
}
```

### 6.3 Response
**HTTP Status**: `201 Created`

```json
{
  "success": true,
  "message": "Category created successfully",
  "data": {
    "categoryId": 15,
    "categoryName": "New Category",
    "categoryDescription": "Description",
    "parentCategoryId": 1,
    "isActive": true
  },
  "errors": null
}
```

---

## 7. PUT Update

### 7.1 Endpoint
```
PUT /api/{resources}/{id}
```

### 7.2 Request Body
- Sử dụng **Request Model** (UpdateDto)
- Gửi **tất cả các field** cần cập nhật

```json
PUT /api/categories/5
Content-Type: application/json

{
  "categoryId": 5,
  "categoryName": "Updated Name",
  "categoryDescription": "Updated Description",
  "parentCategoryId": 1,
  "isActive": false
}
```

### 7.3 Response
**HTTP Status**: `200 OK` hoặc `204 No Content`

---

## 8. DELETE

### 8.1 Endpoint
```
DELETE /api/{resources}/{id}
```

### 8.2 Response
- Thành công: `204 No Content`
- Không tìm thấy: `404 Not Found`

---

## 9. Response Format Chuẩn

### 9.1 Wrapper Model (Bắt buộc)
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}
```

### 9.2 Success Response
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { ... },
  "errors": null
}
```

### 9.3 Error Response
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    "CategoryName is required",
    "CategoryName must be between 3 and 100 characters"
  ]
}
```

---

## 10. HTTP Status Codes

| Status Code | Tình huống |
|-------------|------------|
| `200 OK` | GET/PUT thành công |
| `201 Created` | POST tạo resource thành công |
| `204 No Content` | DELETE thành công |
| `400 Bad Request` | Validation fail, Invalid request |
| `401 Unauthorized` | Chưa đăng nhập / Token hết hạn |
| `403 Forbidden` | Không có quyền truy cập |
| `404 Not Found` | Resource không tồn tại |
| `500 Internal Server Error` | Lỗi server |

---

## 11. Authentication & Authorization

### 11.1 JWT Authentication (Bắt buộc)
- Login API trả về **access token**
- Các API bảo vệ yêu cầu header:

```
Authorization: Bearer <token>
```

---

## 12. Validation & Exception Handling

### 12.1 Validation
- Validate đầu vào qua **Request Model** (Data Annotations)
- Response lỗi theo format chuẩn với message rõ ràng

### 12.2 Exception Handling
- Sử dụng **Global Exception Handler**
- ❌ Không try-catch tràn lan trong controller
- ❌ Không expose stack trace hoặc thông tin nội bộ

---

## 📋 Evaluation Checklist

| # | Tiêu chí | ✓ |
|---|----------|---|
| 1 | Đúng kiến trúc 3-layer và naming project | ☐ |
| 2 | Sử dụng đúng 4 loại model | ☐ |
| 3 | RESTful endpoint và query string hợp lệ | ☐ |
| 4 | Có search, filter, sort, paging, selection | ☐ |
| 5 | GET LIST có pagination metadata (page, pageSize, totalItems, totalPages) | ☐ |
| 6 | Response format chuẩn (success, message, data, errors) | ☐ |
| 7 | Có JWT authentication | ☐ |
| 8 | Có global exception handling | ☐ |
| 9 | Không leak Entity hoặc DB detail | ☐ |

---

## 📋 Quick Reference

```
┌─────────────────────────────────────────────────────────────────┐
│                    API DESIGN QUICK REFERENCE                    │
├─────────────────────────────────────────────────────────────────┤
│ GET Collection:   GET /api/{resources}                          │
│ GET By ID:        GET /api/{resources}/{id}                     │
│ Create:           POST /api/{resources}                         │
│ Update:           PUT /api/{resources}/{id}                     │
│ Delete:           DELETE /api/{resources}/{id}                  │
├─────────────────────────────────────────────────────────────────┤
│ QUERY PARAMS:                                                   │
│ • searchTerm      - Full-text search                            │
│ • {field}         - Filter (single: =3, multi: =3,5,7)          │
│ • sortBy          - Sort field                                  │
│ • sortOrder       - asc/desc (default: asc)                     │
│ • page            - Page number (default: 1)                    │
│ • pageSize        - Items per page (default: 10, max: 100)      │
│ • fields          - Select specific fields                      │
├─────────────────────────────────────────────────────────────────┤
│ RESPONSE FORMAT:                                                │
│ { success, message, data, errors }                              │
├─────────────────────────────────────────────────────────────────┤
│ PAGINATION METADATA:                                            │
│ { page, pageSize, totalItems, totalPages }                      │
└─────────────────────────────────────────────────────────────────┘
```
