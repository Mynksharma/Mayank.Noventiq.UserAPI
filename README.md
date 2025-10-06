# 🌐 Mayank.Noventiq.UserAPI

A **secure and multilingual ASP.NET Core Web API** for **User and Role Management** with **JWT Authentication** and **Refresh Token** support.

---

## 🚀 Overview

This project provides a complete backend solution for managing **users**, **roles**, and **authentication** with **localized (Hindi & English)** responses.

It supports:
- Role-based access control (RBAC)
- JWT access tokens (expire in 15 minutes)
- Refresh tokens (stored in DB, expire in 7 days)
- Multilingual responses (English 🇺🇸 & Hindi 🇮🇳)

---

## 🧩 Tech Stack

- **Framework:** ASP.NET Core 8.0  
- **Database:** SQL Server  
- **ORM:** Entity Framework Core  
- **Authentication:** JWT (JSON Web Token)  
- **Documentation:** Swagger UI  
- **Localization:** Hindi & English via `Accept-Language` header  

---

## 🔐 Authentication & Authorization

| Endpoint Type | Description |
|----------------|-------------|
| **Public** | No authentication required (e.g., `/login`, `/register`) |
| **Authenticated User** | Requires valid JWT token |
| **Admin Only** | Requires user with Admin role |

- **Unauthorized requests →** `403 Forbidden`  
- **Successful requests →** `200 OK`  
- **Other codes →** Based on API result (`400`, `404`, etc.)

---

## 🔁 JWT Token Logic

### Access Token
- Valid for **15 minutes** — used for secured API calls.

### Refresh Token
- Valid for **7 days**, stored in the database, and used to generate a new access token when it expires.

---

## 🌏 Multilingual Support

API supports **English (en-US)** and **Hindi (hi-IN)** responses.  
Set the desired language using the `Accept-Language` header.

| Language | Header Example |
|-----------|----------------|
| English | `Accept-Language: en-US` |
| Hindi | `Accept-Language: hi-IN` |


**Response:**
```json
{
  "message": "भूमिकाएँ सफलतापूर्वक प्राप्त की गईं",
  "response": { ... }
}
```

## 📚 API Endpoints

---

### 🧾 Auth

| Method | Endpoint | Description |
|---------|-----------|-------------|
| `POST` | `/api/Auth/login` | User login and receive JWT + Refresh token |
| `POST` | `/api/Auth/register` | Register a new user |
| `POST` | `/api/Auth/refresh` | Generate a new access token using refresh token (Authenticated users) |

---

### 🧑‍💼 Roles (Admin Access Required)

| Method | Endpoint | Description |
|---------|-----------|-------------|
| `GET` | `/api/Roles/GetAllRoles` | Fetch all roles |
| `GET` | `/api/Roles/GetRoleById/{id}` | Get role by ID |
| `POST` | `/api/Roles/CreateRole` | Create a new role (Admin only) |
| `PUT` | `/api/Roles/UpdateRole/{id}` | Update role by ID (Admin only) |
| `DELETE` | `/api/Roles/DeleteRole/{id}` | Delete role by ID (Admin only) |

---

### 👥 Users

| Method | Endpoint | Description |
|---------|-----------|-------------|
| `POST` | `/api/User/CreateUser` | Create a new user (Admin only) |
| `GET` | `/api/User/GetAllUsers` | Retrieve all users (Admin only) |
| `PUT` | `/api/User/UpdateUser/{id}` | Update user details (Admin only) |
| `DELETE` | `/api/User/DeleteUser/{id}` | Delete user (Admin only) |
| `GET` | `/api/User/GetUserById/{id}` | Get user details by ID (Admin only) |
| `GET` | `/api/User/GetUserRoleById/{id}` | Get role of specific user (Admin only) |

---

## 🧭 How to Access APIs via Swagger

1. **Run the project.**  
2. **Open Swagger UI:**  
   👉 `https://localhost:7085/swagger/index.html`  
3. **Seed roles/users (recommended):**  
- Run the **attached SQL scripts** to insert sample **roles** and **users**.  
- Check your DB to find the valid `roleId` for **Admin** (and other roles).  
4. **Register a user:**  
- Expand **`POST /api/Auth/register`**.  
- Provide **email**, **password**, and a valid **`roleId`** (Admin roleId for admin users, others for normal users).  
- Submit and confirm registration succeeded.  
5. **Login to get tokens:**  
- Expand **`POST /api/Auth/login`**.  
- Enter your registered **email** & **password** and **Execute**.  
- Copy the returned **access token** (and keep the **refresh token**).  
6. **Authorize in Swagger:**  
- Click the **Authorize** button (top-right lock icon).  
- In the input box, type your token as:  
  ```
  Bearer <Your JWT access token>
  ```
  **Example:** `Bearer -wdqwedwe.....`  
- Click **Authorize**, then **Close**.  
7. **Set language (optional):**  
- For any request, add **`Accept-Language`** header:  
  `en-US` → English or `hi-IN` → Hindi.  
8. **Call endpoints according to your role:**  
- **Admin** can call all endpoints (Create/Update/Delete).  
- **Authenticated** users can call allowed GET endpoints.  
- **Public** endpoints (**login**, **register**, **refresh**) require no token.  
9. **When access token expires (~15 min):**  
- Use **`POST /api/Auth/refresh`** with your refresh token to get a new access token.

---

## 🔁 JWT Token Logic

- **Access Token** → Valid for **15 minutes** — used for secured API calls.  
- **Refresh Token** → Valid for **7 days**, stored in the database, used to renew expired access tokens.

---

---

## ⚙️ Response Codes

| Code | Meaning |
|------|----------|
| `200` | Success |
| `400` | Bad Request |
| `401` | Unauthorized |
| `403` | Forbidden (Role/Access denied) |
| `404` | Not Found |
| `500` | Internal Server Error |

---

## 🗄️ Database Setup (Running `Noventiq_SQL.sql`)

To set up the database for this project, follow the steps below:

1. **Open SQL Server Management Studio (SSMS)** or any SQL client tool of your choice.
2. **Execute attached Novefntiq_SQL.sql file.**
3. **Used DB server name-(localdb)\MSSQLLocalDB, if you have different server, make changes in ConnectionString inside appsettings.json.**
---

## 📸 Screenshots

| Swagger Overview | Hindi Response Example |
|------------------|-------------------------|
| *Add Swagger Screenshot Here* | *Add Hindi Response Screenshot Here* |


📸 **Example from Swagger:**

