# API Testing - Category Endpoints

Base URL: `http://localhost:5024`

---

## 📝 1. CREATE Categories (POST)

### 1.1 Create Root Category - Technology
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Technology",
  "categoryDescription": "All about technology and innovations",
  "parentCategoryID": null,
  "isActive": true
}
```

### 1.2 Create Root Category - Business
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Business",
  "categoryDescription": "Business news and finance",
  "parentCategoryID": null,
  "isActive": true
}
```

### 1.3 Create Sub-category - AI (under Technology ID=1)
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Artificial Intelligence",
  "categoryDescription": "AI, Machine Learning, and Deep Learning",
  "parentCategoryID": 1,
  "isActive": true
}
```

### 1.4 Create Sub-category - Web Development
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Web Development",
  "categoryDescription": "Frontend and Backend technologies",
  "parentCategoryID": 1,
  "isActive": true
}
```

### 1.5 Create Sub-category - Startups (under Business ID=2)
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Startups",
  "categoryDescription": "Startup ecosystem and funding",
  "parentCategoryID": 2,
  "isActive": true
}
```

### 1.6 Create Inactive Category
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Archived Tech",
  "categoryDescription": "Old technology news",
  "parentCategoryID": 1,
  "isActive": false
}
```

---

## 🔍 2. GET All Categories (with Filters)

### 2.1 Standard Query - Get All Active
```http
GET http://localhost:5024/api/Category?IsActive=true&PageSize=10
```

### 2.2 Fields Projection - Only ID and Name
```http
GET http://localhost:5024/api/Category?Fields=CategoryID,CategoryName&PageSize=20
```

### 2.3 Search by Name
```http
GET http://localhost:5024/api/Category?SearchTerm=technology&PageSize=10
```

### 2.4 Get Children of Category (ParentCategoryID=1)
```http
GET http://localhost:5024/api/Category?ParentCategoryID=1&SortBy=CategoryName&SortOrder=asc
```

### 2.5 Get Root Categories Only
```http
GET http://localhost:5024/api/Category?ParentCategoryID=null&PageSize=20
```

### 2.6 Combine: Search + Filter + Fields
```http
GET http://localhost:5024/api/Category?SearchTerm=AI&IsActive=true&Fields=CategoryID,CategoryName,ParentCategoryID
```

### 2.7 Expand ParentCategory
```http
GET http://localhost:5024/api/Category?ParentCategoryID=1&Expand=ParentCategory
```

### 2.8 Sort Descending
```http
GET http://localhost:5024/api/Category?SortBy=CategoryName&SortOrder=desc&PageSize=15
```

### 2.9 Get Inactive Only
```http
GET http://localhost:5024/api/Category?IsActive=false
```

### 2.10 Pagination - Page 2
```http
GET http://localhost:5024/api/Category?PageNumber=2&PageSize=5
```

---

## 🔎 3. GET by ID

### 3.1 Get Technology Category (ID=1)
```http
GET http://localhost:5024/api/Category/1
```

### 3.2 Get AI Category (ID=3)
```http
GET http://localhost:5024/api/Category/3
```

### 3.3 Get Non-existent Category
```http
GET http://localhost:5024/api/Category/9999
```
**Expected:** 404 Not Found

---

## ✏️ 4. UPDATE Category

### 4.1 Update Technology Category
```http
PUT http://localhost:5024/api/Category/1
Content-Type: application/json

{
  "categoryID": 1,
  "categoryName": "Technology & Innovation",
  "categoryDescription": "All about technology, innovations, and digital transformation",
  "parentCategoryID": null,
  "isActive": true
}
```

### 4.2 Move Category to Different Parent
```http
PUT http://localhost:5024/api/Category/3
Content-Type: application/json

{
  "categoryID": 3,
  "categoryName": "Artificial Intelligence",
  "categoryDescription": "AI, ML, and Neural Networks",
  "parentCategoryID": 2,
  "isActive": true
}
```

### 4.3 Deactivate Category
```http
PUT http://localhost:5024/api/Category/6
Content-Type: application/json

{
  "categoryID": 6,
  "categoryName": "Archived Tech",
  "categoryDescription": "No longer active",
  "parentCategoryID": 1,
  "isActive": false
}
```

### 4.4 Update with ID Mismatch (Error)
```http
PUT http://localhost:5024/api/Category/1
Content-Type: application/json

{
  "categoryID": 999,
  "categoryName": "Wrong ID",
  "categoryDescription": "Test",
  "parentCategoryID": null,
  "isActive": true
}
```
**Expected:** 400 Bad Request - ID mismatch

---

## ❌ 5. DELETE Category

### 5.1 Delete Archived Category
```http
DELETE http://localhost:5024/api/Category/6
```

### 5.2 Delete Non-existent Category
```http
DELETE http://localhost:5024/api/Category/9999
```
**Expected:** 404 Not Found

---

## 📊 6. Advanced Testing Scenarios

### 6.1 Bulk Create - All Root Categories
```json
// POST multiple times with these:
[
  {
    "categoryName": "Sports",
    "categoryDescription": "Sports news and updates",
    "parentCategoryID": null,
    "isActive": true
  },
  {
    "categoryName": "Entertainment",
    "categoryDescription": "Movies and music",
    "parentCategoryID": null,
    "isActive": true
  },
  {
    "categoryName": "Health",
    "categoryDescription": "Health and wellness",
    "parentCategoryID": null,
    "isActive": true
  }
]
```

### 6.2 Create 3-Level Hierarchy

**Level 1:**
```json
{
  "categoryName": "Technology",
  "categoryDescription": "Tech root",
  "parentCategoryID": null,
  "isActive": true
}
```
Response: `{ "categoryID": 1, ... }`

**Level 2:**
```json
{
  "categoryName": "Artificial Intelligence",
  "categoryDescription": "AI parent",
  "parentCategoryID": 1,
  "isActive": true
}
```
Response: `{ "categoryID": 10, ... }`

**Level 3:**
```json
{
  "categoryName": "Natural Language Processing",
  "categoryDescription": "NLP sub-category",
  "parentCategoryID": 10,
  "isActive": true
}
```

---

## 🧪 7. Test Edge Cases

### 7.1 Empty Name (Validation Error)
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "",
  "categoryDescription": "Test",
  "parentCategoryID": null,
  "isActive": true
}
```
**Expected:** 400 Bad Request

### 7.2 Invalid Parent ID
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Test",
  "categoryDescription": "Test",
  "parentCategoryID": 99999,
  "isActive": true
}
```
**Expected:** May fail with foreign key constraint

### 7.3 Null Description (Should be allowed)
```http
POST http://localhost:5024/api/Category
Content-Type: application/json

{
  "categoryName": "Minimal Category",
  "categoryDescription": null,
  "parentCategoryID": null,
  "isActive": true
}
```

---

## 📝 Expected Responses

### Success Response (POST/GET by ID)
```json
{
  "categoryID": 1,
  "categoryName": "Technology",
  "categoryDescription": "All about technology...",
  "parentCategoryID": null,
  "isActive": true
}
```

### Paged Response (GET with pagination)
```json
{
  "items": [
    {
      "categoryID": 1,
      "categoryName": "Technology",
      "categoryDescription": "...",
      "parentCategoryID": null,
      "isActive": true
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalItems": 25,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true
}
```

### Fields Response (with Fields parameter)
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
  "pageNumber": 1,
  "pageSize": 10,
  "totalItems": 25
}
```

### Error Response (404)
```json
{
  "message": "Category with ID 9999 not found"
}
```

### Error Response (400)
```json
{
  "message": "ID mismatch"
}
```
