## 📝 Description (JwtAuthRefreshTokenAPI)

This project is a basic **ASP.NET Core Web API** that implements **JWT authentication** with:

* ✅ Login and registration
* ✅ Role-based authorization (`User`, `Admin`, etc.)
* ✅ Refresh token support
* ✅ Token generation using a custom `JwtHelper`
* ✅ Clean folder structure with services, DTOs, and controllers

It’s meant to be a **starter template** for any future projects where I need secure login, user roles, and token refresh features.

---

### 🔧 Features

* User registration with role assignment
* Login returns access + refresh tokens
* Secure protected routes using `[Authorize]`
* Auto-expiring access token with refresh token flow
* Token claims include username, role, JTI
* All logic separated into services and helpers

---

### 🛠️ Stack Used

* ASP.NET Core Web API
* JWT (`System.IdentityModel.Tokens.Jwt`)
* Entity Framework Core + SQL Server
* Swagger (OpenAPI)
* C#

---
### 📘 How It Works

This project is a basic JWT authentication system built with ASP.NET Core Web
API. When a user registers, their details (including role) are saved in the database. 
During login, if the credentials match, the server generates an access token (with a short lifespan) and a 
refresh token (with a longer lifespan). The access token includes the user's name and role in its claims,
and it's used to access protected API routes. If the access token expires, the client can send the refresh
token to get a new access token without logging in again. Role-based access is handled using `[Authorize(Roles = "RoleName")]`, and 
all token logic is managed using a helper class to keep the code reusable and clean.


