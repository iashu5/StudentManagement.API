# Student Management System API

A secure RESTful API for managing students with JWT authentication, built with ASP.NET Core 8.0 and ADO.NET with Stored Procedures.

## 🚀 Features

- **JWT Authentication** - Secure token-based authentication
- **CRUD Operations** - Complete student management (Create, Read, Update, Delete)
- **Stored Procedures** - All database operations use stored procedures
- **Global Exception Handling** - Centralized error handling middleware
- **Structured Logging** - Serilog for file and console logging
- **Swagger Documentation** - Interactive API documentation
- **CORS Enabled** - Cross-origin requests allowed for React frontend

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (2019 or later)
- [Git](https://git-scm.com/)

## 🗄️ Database Setup

### 1. Create Database and Stored Procedures

Execute the following SQL script in SQL Server Management Studio:

```sql
-- Create Database
CREATE DATABASE StudentManagementDB;
GO

USE StudentManagementDB;
GO

-- Create Students Table
CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Age INT NOT NULL,
    Course NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Get All Students
CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN
    SELECT Id, Name, Email, Age, Course, CreatedDate 
    FROM Students ORDER BY Id DESC;
END
GO

-- Get Student By ID
CREATE PROCEDURE sp_GetStudentById @Id INT
AS
BEGIN
    SELECT Id, Name, Email, Age, Course, CreatedDate 
    FROM Students WHERE Id = @Id;
END
GO

-- Create Student
CREATE PROCEDURE sp_CreateStudent
    @Name NVARCHAR(100), @Email NVARCHAR(100), @Age INT, @Course NVARCHAR(50)
AS
BEGIN
    INSERT INTO Students (Name, Email, Age, Course, CreatedDate)
    VALUES (@Name, @Email, @Age, @Course, GETDATE());
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- Update Student
CREATE PROCEDURE sp_UpdateStudent
    @Id INT, @Name NVARCHAR(100), @Email NVARCHAR(100), @Age INT, @Course NVARCHAR(50)
AS
BEGIN
    UPDATE Students SET Name=@Name, Email=@Email, Age=@Age, Course=@Course
    WHERE Id=@Id;
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- Delete Student
CREATE PROCEDURE sp_DeleteStudent @Id INT
AS
BEGIN
    DELETE FROM Students WHERE Id=@Id;
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- Check Email Exists
CREATE PROCEDURE sp_CheckEmailExists @Email NVARCHAR(100), @ExcludeId INT = NULL
AS
BEGIN
    IF @ExcludeId IS NULL
        SELECT COUNT(1) FROM Students WHERE Email=@Email;
    ELSE
        SELECT COUNT(1) FROM Students WHERE Email=@Email AND Id!=@ExcludeId;
END
GO

-------------------------------------

The API will be available at:

HTTPS: https://localhost:7096

HTTP: http://localhost:5075

Swagger UI: https://localhost:7096/swagger

📚 API Endpoints
Authentication (No token required)
Method	Endpoint	Description
POST	/api/Auth/login	User login
Student Management (Bearer token required)

Method	    Endpoint	        Description
GET	      /api/Students	        Get all students
GET	      /api/Students/{id}	Get student by ID
POST	 /api/Students	        Create new student
PUT	     /api/Students/{id}  	Update student
DELETE	/api/Students/{id}	    Delete student


🔐 Authentication
Default Credentials

Username: admin
Password: password


---------------------------------------------------------------------------------------

StudentManagementSystem.API/
├── Controllers/
│   ├── AuthController.cs          # Authentication endpoints
│   └── StudentsController.cs      # Student CRUD endpoints
├── Services/
│   ├── AuthService.cs              # JWT token generation
│   └── StudentService.cs           # Business logic
├── Middleware/
│   └── ExceptionMiddleware.cs      # Global error handling
├── Helpers/
│   └── DatabaseHelper.cs           # ADO.NET database operations
├── Models/
│   ├── Student.cs                  # Student entity
│   ├── StudentDto.cs               # Data transfer objects
│   └── ApiResponse.cs              # Standard API response
├── Program.cs                       # Application entry point
└── appsettings.json                 # Configuration


----------------------------------------------------------------------------------------------------------------------------

🛠️ Technologies Used
ASP.NET Core 8.0 - Web API framework

ADO.NET - Data access technology

SQL Server - Database

JWT - Authentication

Serilog - Logging

Swagger - API documentation



🔒 Security Features
JWT Bearer authentication

Password hashing (extendable)

Email uniqueness validation

SQL injection prevention via stored procedures

CORS policy for specific origins