# 🛍️ DeveloperStore

**Modular e-commerce backend** built with **ASP.NET Core** and **CQRS** architecture.  
Handles everything from **carts** and **sales** to **product management** and **user workflows**.

---

## 📦 Project Structure

```plaintext
DeveloperStore/
├── src/
|   ├── Auth
│   ├── Products
│   ├── Carts
│   ├── Sales
|   ├── User
│   └── Shared
├── tests/
│   ├── Auth.Tests
│   ├── Products.Tests
│   ├── Carts.Tests
│   ├── Sales.Tests
│   ├── User.Tests
│   └── Shared.Tests
├── DeveloperStore.sln
├── DeveloperStore.postman_collection.json
└── README.md
```

---

## 🔧 Technologies Used

| Layer              | Technology                         |
|-------------------|-------------------------------------|
| **Backend**        | ASP.NET Core (.NET 8)              |
| **Architecture**   | CQRS with MediatR & ErrorOr        |
| **ORM**            | EF Core + PostgreSQL               |
| **Validation**     | FluentValidation                   |
| **Mapping**        | AutoMapper                         |
| **API Docs**       | Swagger                            |
| **Testing**        | xUnit + FluentAssertions           |

---

## ✅ Features

- Modular APIs for **Carts**, **Sales**, **Products**, **Auth** and **User**
- Custom validation pipeline with `ValidationBehavior`
- Global exception handling via middleware
- Rich error formatting: `ValidationError`, `InternalServerError`
- AutoMapper setup for DTO ↔ Entity mapping
- Included Postman collection for local API testing

---

## 🚀 Running Locally

### 🧱 Build and Run

```bash
dotnet build DeveloperStore.sln
dotnet run --project src/Auth/DeveloperStore.Auth.Api
dotnet run --project src/Users/DeveloperStore.Users.Api
dotnet run --project src/Carts/DeveloperStore.Carts.Api
dotnet run --project src/Products/DeveloperStore.Products.Api
dotnet run --project src/Sales/DeveloperStore.Sales.Api
```

Ensure your PostgreSQL connection string is set in each `appsettings.Development.json`.

---

### 📬 Postman Collection

To test the API:

1. Open Postman → *Import* → Select `DeveloperStore.postman_collection.json`
2. Use available endpoints:
   - `GET /api/carts/{id}`
   - `POST /api/carts`
   - `POST /api/sales`
   - `GET /api/products/{id}`

---

### 🧪 Testing

All unit tests are located in the `tests/` directory and follow the MediatR command/handler structure.

```bash
dotnet test DeveloperStore.sln
```

Assertions are built using **FluentAssertions** and **ErrorOr**.

---

## 📘 Notes

- Dates must follow the format `yyyy-MM-dd` (normalized to UTC)
- Cart quantity must be between **1–20** items per product
- Business rules (e.g., discounts) are enforced during the **SaleCommand**, not at cart-level

---

## 👨‍💻 Author

Developed by **Guilherme Campos**  

---