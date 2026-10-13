# Student Management API — ASP.NET Core 8 Web API

A secure REST API for managing student records, with JWT authentication, centralized error handling, structured logging and Swagger docs.
The React + TypeScript frontend lives in [student-management-ui](https://github.com/iashu5/student-management-ui).

## Features

- **JWT authentication**: login endpoint issues tokens, and the student endpoints are protected
- **Student CRUD**: create, read, update and delete through a service layer
- **Global exception middleware**: every error comes back in the same `ApiResponse` format
- **Structured logging** with Serilog (console + file sinks)
- **Swagger / OpenAPI** for exploring and testing the API
- **SQL Server** data access with `Microsoft.Data.SqlClient`

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 Web API |
| Auth | JWT Bearer |
| Data | SQL Server · Microsoft.Data.SqlClient |
| Logging | Serilog |
| Docs | Swashbuckle (Swagger) |

## Project structure

```
StudentManagement.API/
├── Controllers/   AuthController, StudentsController
├── Services/      AuthService, StudentService
├── Helpers/       DatabaseHelper
├── Middleware/    ExceptionMiddleware
├── Modals/        Student, StudentDto, ApiResponse
└── Program.cs     DI, JWT, Serilog, Swagger setup
```

## Running locally

1. Create a `StudentManagementDB` database in SQL Server.
2. Set the connection string and JWT key with user secrets, so they stay out of git:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True"
   dotnet user-secrets set "Jwt:Key" "<a-long-random-secret>"
   ```
3. Run `dotnet run` and open `/swagger`.
