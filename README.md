# Shifts Logger

**Shifts Logger** is a two-part application: a **.NET Web API**
and an interactive **console app** for managing employee work shifts.

The project is built using **.NET 8**, **Entity Framework Core (Code-First)**, **SQL Server**, and a 
**Spectre.Console** UI for a polished terminal experience. It's designed to help you understand how 
to build and consume APIs, handle user input, and structure real-world apps.

---

## 🧩 Tech Stack

- [.NET 8 Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
  
- [Entity Framework Core (Code-First)](https://learn.microsoft.com/en-us/ef/core/)
  
- [SQL Server](https://www.microsoft.com/sql-server/)
  
- [Spectre.Console](https://spectreconsole.net/) (rich console UI)
  
- Swagger (auto-generated API testing UI)

---

## 📁 Project Structure

```
ShiftsLogger/
│
├── ShiftsLogger.API/           # Web API project (back-end)
│   ├── Controllers/            # API endpoints
│   ├── Data/                   # DbContext and configuration
│   ├── Models/                 # Entity models
│   ├── Migrations/             # EF Core migrations
│   ├── Services/               # Business logic
│   └── Program.cs              # Entry point
│
├── ShiftsLogger.ConsoleUI/     # Console app (front-end)
│   ├── Menus/                  # Menu navigation
│   ├── UI/                     # Display and input prompts
│   ├── Models/                 # Internal models
│   ├── ApiService.cs           # Handles API requests
│   └── Program.cs              # Entry point
```

---

## ✅ Features

- View, create, edit, and delete **Workers**
  
- View, create, edit, and delete **Shifts**
  
- Assign shifts to specific workers
  
- Filter shifts by worker or by ID
  
- All user input and validations handled via console
  
- Interactive menu-based navigation
  
- Full Swagger support for endpoint testing

---


## 🧠 Skills Practiced

- Building RESTful APIs with ASP.NET Core
  
- Separating concerns with services and controllers
  
- Using EF Core with the Code-First approach
  
- Rich terminal UI design with Spectre.Console
  
- Client-side validation and error handling
  
- API testing with Swagger/Postman
  

