# 📘 Employee Management System (EMS) API

A **.NET 8 Web API** project implementing **CRUD operations** on Employees using:

* **Entity Framework Core (Database-First approach)**
* Clean Architecture patterns: Repository, Unit of Work, Service Layer
* DTOs, AutoMapper, FluentValidation
* Global Error Handling & Logging with Serilog
* Swagger (OpenAPI) with XML Comments
* Pagination & Postman Collection for API usability

This project is structured as a **portfolio-ready solution** to demonstrate backend development and architectural best practices in interviews.

---

## 🛠 Tech Stack

* **.NET 8 / C# 8.0**
* **Entity Framework Core (DB-First with SQLite)**
* **Repository + Unit of Work Pattern**
* **Dependency Injection**
* **AutoMapper**
* **FluentValidation**
* **Serilog** (console, file, SQL Server sinks)
* **Swagger / OpenAPI (Swashbuckle)**
* **xUnit** (unit testing with InMemory provider)

---

## 🚀 How to Run

1. Clone the repo:

   ```bash
   git clone https://github.com/deepak-khare2015/EmployeeManagementSystem.git
   cd EmployeeManagementSystem
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run EF Core DB-First scaffolding (if regenerating from DB):

   ```bash
   dotnet ef dbcontext scaffold "Data Source=ems.db" Microsoft.EntityFrameworkCore.Sqlite -o Models
   ```

4. Run the API:

   ```bash
   dotnet run --project EmployeeManagement.API
   ```

5. Open Swagger UI:

   ```
   https://localhost:5125/
   ```

---

## 📂 Final Project Architecture

```plaintext
EmployeeManagementSystem/
│
├── EmployeeManagement.API/                     # API Layer (Presentation)
│   ├── Controllers/
│   │   ├── EmployeeController.cs               # CRUD endpoints (XML docs + Serilog)
│   │   └── ManagerController.cs
│   ├── Middleware/
│   │   ├── ExceptionMiddleware.cs              # Global error handling
│   │   └── RequestResponseLoggingMiddleware.cs # Request/response logging
│   ├── Docs/
│   │   ├── EMS-Postman-Collection.json         # Postman exported collection
│   │   └── swagger-ui.png                      # Swagger UI screenshot
│   ├── Logs/                                   # Serilog rolling logs
│   ├── appsettings.json                        # Config (connection string, logging, etc.)
│   ├── appsettings.Development.json
│   ├── appsettings.Production.json
│   ├── EmployeeAPI.http                        # Test requests (VS HTTP client)
│   └── Program.cs                              # Startup (Swagger, DI, Logging, Middleware)
│
├── EmployeeManagement.Application/             # Application Layer (Business Logic)
│   ├── DTOs/
│   │   ├── EmployeeCreateDTO.cs
│   │   ├── EmployeeDTO.cs
│   │   └── EmployeeUpdateDTO.cs
│   ├── Exceptions/
│   │   └── NotFoundException.cs
│   ├── Interface/
│   │   ├── IEmployeeRepository.cs
│   │   ├── IEmployeeService.cs
│   │   ├── IGenericRepository.cs
│   │   ├── IManagerRepository.cs
│   │   └── IUnitOfWork.cs
│   ├── Mapping/
│   │   └── EmployeeProfile.cs                  # AutoMapper profile
│   └── Services/
│       └── EmployeeService.cs                  # Business logic
│
├── EmployeeManagement.Domain/                  # Domain Layer (Entities)
│   └── Entities/
│       ├── Employee.cs                         # EF Core DB-First entity
│       └── Manager.cs
│
├── EmployeeManagement.Infrastructure/          # Infrastructure Layer (Data Access)
│   ├── Data/
│   │   └── AppDbContext.cs                     # EF Core DbContext
│   ├── Repositories/
│   │   ├── GenericRepository.cs
│   │   ├── EmployeeRepository.cs
│   │   ├── ManagerRepository.cs
│   │   └── UnitOfWork.cs
│   └── DependencyInjection.cs                  # DI setup for repositories & UoW
│
└── Tests/                                      # Test Layer
    └── RepositoryTests.cs                      # xUnit tests (InMemory DB)
```

---

## 🖼 Architecture Diagram

```plaintext
                ┌───────────────────────────┐
                │       Presentation        │
                │   (EmployeeManagement.API)│
                │  Controllers + Middleware │
                └─────────────┬─────────────┘
                              │
                              ▼
                ┌───────────────────────────┐
                │     Application Layer     │
                │ DTOs | Services | Mapping │
                │ Interfaces (IEmployeeSvc) │
                └─────────────┬─────────────┘
                              │
                              ▼
                ┌───────────────────────────┐
                │        Domain Layer       │
                │   Entities (Employee.cs)  │
                └─────────────┬─────────────┘
                              │
                              ▼
                ┌───────────────────────────┐
                │     Infrastructure Layer  │
                │ Repositories | UnitOfWork │
                │   AppDbContext (EF Core)  │
                └─────────────┬─────────────┘
                              │
                              ▼
                ┌───────────────────────────┐
                │        Database (MSSQL)   │
                └───────────────────────────┘
```

---

## 📖 Week-by-Week Progress

### ✅ Week 1 — Repository Pattern

* Scaffolded **EF Core DB-First** models (`Employee.cs`).
* Implemented `IRepository<T>` + `Repository<T>` for CRUD.
* Added `IEmployeeRepository` for employee-specific queries.
* Created **unit test skeleton** with xUnit + InMemory provider.

---

### ✅ Week 2 — Unit of Work

* Added `IUnitOfWork` + `UnitOfWork`.
* Centralized transaction commits.
  💬 **Talking Point:** Scoped lifetime ensures consistency per request.

---

### ✅ Week 3 — Service Layer + DI

* Added `IEmployeeService` + `EmployeeService`.
* Controllers now delegate to service layer.
  💬 **Talking Point:** Thin controllers, SRP, DI best practices.

---

### ✅ Week 4 — DTOs + AutoMapper + Validation

* Added `EmployeeDto`, `EmployeeCreateDto`, `EmployeeUpdateDto`.
* Configured AutoMapper mapping profiles.
* Added FluentValidation rules for Create/Update.
  💬 **Talking Point:** DTOs secure contracts, validators ensure integrity.

---

### ✅ Week 5 — Global Error Handling + Logging

* Implemented **ExceptionMiddleware** for consistent error responses.
* Added **Serilog** with console/file sinks.
* Structured logging in controllers with `Log.Information`, `Log.Warning`, `Log.Error`.
  💬 **Talking Point:** Logs are structured and correlation-ready.

---

### ✅ Week 6 — Swagger, Pagination, Documentation

* Enabled **Swagger UI at root** (`/`).
* Configured **XML comments** → Swagger shows endpoint descriptions, parameters, sample requests/responses.
* Implemented **Pagination** (`PagedResult<T>`).
* Exported **Postman Collection**.
* Added **Swagger screenshot** for documentation.

📸 Swagger Screenshot:

![Swagger Screenshot](EmployeeManagement.API/Docs/swagger-ui.png)

---

## 🔹 Sample Requests & Responses

### 1. GET All Employees (Paginated)

```
GET /api/Employee?pageNumber=1&pageSize=2
```

**Response (200 OK):**

```json
{
  "items": [
    { "id": 1, "name": "John Doe", "position": "Manager", "salary": 85000 },
    { "id": 2, "name": "Jane Smith", "position": "Developer", "salary": 60000 }
  ],
  "totalRecords": 42,
  "pageNumber": 1,
  "pageSize": 2
}
```

---

### 2. GET Employee by Id

```
GET /api/Employee/1
```

**Response (200 OK):**

```json
{
  "id": 1,
  "name": "John Doe",
  "position": "Manager",
  "salary": 85000
}
```

**Response (404 Not Found):**

```json
{
  "error": "Employee with id 99 not found",
  "statusCode": 404,
  "traceId": "00-abc123xyz"
}
```

---

### 3. POST Create Employee

```
POST /api/Employee
```

**Request:**

```json
{
  "name": "Deepak Khare",
  "position": "Software Engineer",
  "salary": 75000
}
```

**Response (201 Created):**

```json
{
  "id": 43,
  "name": "Deepak Khare",
  "position": "Software Engineer",
  "salary": 75000
}
```

---

### 4. PUT Update Employee

```
PUT /api/Employee/2
```

**Request:**

```json
{
  "id": 2,
  "name": "Amith",
  "position": "Senior Developer",
  "salary": 70000
}
```

**Response (200 OK):**

```json
{
  "id": 2,
  "name": "Amith",
  "position": "Senior Developer",
  "salary": 70000
}
```

**Response (404 Not Found):**

```json
{
  "error": "Employee with id 2 not found",
  "statusCode": 404,
  "traceId": "00-abc123xyz"
}
```

---

### 5. DELETE Employee

```
DELETE /api/Employee/43
```

**Response (204 No Content):**

```
(no body returned)
```

**Response (400 Bad Request):**

```json
{
  "error": "Invalid employee id",
  "statusCode": 400,
  "traceId": "00-abc123xyz"
}
```

---

## 📁 Postman Collection

Located under `/EmployeeManagement.API/Docs`:

* [EMS-Postman-Collection.json](EmployeeManagement.API/Docs/EMS-Postman-Collection.json)

👉 Import this into Postman → instantly test all endpoints.

---
