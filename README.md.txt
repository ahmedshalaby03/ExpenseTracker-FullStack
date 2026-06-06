<div align="center">

<br/>

```
███████╗██╗  ██╗██████╗ ███████╗███╗   ██╗███████╗███████╗
██╔════╝╚██╗██╔╝██╔══██╗██╔════╝████╗  ██║██╔════╝██╔════╝
█████╗   ╚███╔╝ ██████╔╝█████╗  ██╔██╗ ██║███████╗█████╗  
██╔══╝   ██╔██╗ ██╔═══╝ ██╔══╝  ██║╚██╗██║╚════██║██╔══╝  
███████╗██╔╝ ██╗██║     ███████╗██║ ╚████║███████║███████╗
╚══════╝╚═╝  ╚═╝╚═╝     ╚══════╝╚═╝  ╚═══╝╚══════╝╚══════╝
                                                            
████████╗██████╗  █████╗  ██████╗██╗  ██╗███████╗██████╗  
╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝██╔══██╗ 
   ██║   ██████╔╝███████║██║     █████╔╝ █████╗  ██████╔╝ 
   ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗ 
   ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗███████╗██║  ██║ 
   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝
```

### **Full-Stack Expense Management System**
*Track smarter. Spend wiser. Save better.*

<br/>

![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)

</div>

---

## 📌 Overview

**Expense Tracker** is a production-ready, full-stack web application designed to give individuals full control over their personal finances. Built on **Angular** for a dynamic frontend experience and **ASP.NET Core Web API** with **Onion Architecture** on the backend — secured with **JWT Authentication** and powered by **SQL Server**.

Whether you want to track your daily coffee runs or analyze monthly spending patterns — this app has you covered.

---

## 🏗️ Architecture

```
ExpenseTracker-FullStack/
├── backend/
│   └── ExpenseTracker/
│       ├── ExpenseTracker.Api            # Presentation Layer  → Controllers, Middleware
│       ├── ExpenseTracker.Application    # Application Layer  → Services, DTOs, Interfaces
│       ├── ExpenseTracker.Domain         # Domain Layer       → Entities, Enums
│       └── ExpenseTracker.Infrastructure # Infrastructure     → EF Core, Repos, Identity
│
└── frontend/
    └── Angular/
        ├── src/app/
        │   ├── core/         # Guards, Interceptors, Services
        │   ├── features/     # Feature Modules (Auth, Dashboard, etc.)
        │   └── shared/       # Shared Components & Pipes
        └── environments/
```

> Follows **Onion Architecture** on the backend to enforce separation of concerns and keep the domain model independent of infrastructure.

---

## ⚡ Tech Stack

| Layer | Technology |
|---|---|
| **Frontend Framework** | Angular 17+ |
| **Language** | TypeScript |
| **Styling** | Bootstrap 5 + Custom CSS |
| **Backend Framework** | ASP.NET Core Web API (.NET 8) |
| **ORM** | Entity Framework Core |
| **Database** | Microsoft SQL Server |
| **Authentication** | ASP.NET Identity + JWT Bearer Tokens |
| **Architecture** | Onion Architecture (Clean Architecture) |

---

## ✨ Features

### 🔐 Auth & Security
- User **Registration** and **Login**
- Secure **JWT Authentication** with token refresh
- Protected routes via **Angular Auth Guards**
- HTTP **Interceptors** for automatic token attachment

### 💸 Core Functionality
- ➕ Add, ✏️ Edit, 🗑️ Delete **Transactions** (Income & Expenses)
- 🗂️ **Category Management** — create and organize your own categories
- 📅 Filter transactions by **date range**, category, or type

### 📊 Analytics & Insights
- 📈 **Dashboard** with visual spending overview
- 📋 **Reports** — monthly and category-based breakdowns
- 💡 Quick stats: total income, total expenses, net balance

### 👤 User Experience
- ⚙️ **Profile Management** — update name, email, password
- 🎨 **Preferences** — user-specific settings
- 📱 **Fully Responsive** UI — works on all screen sizes

---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB)
- [Node.js](https://nodejs.org/) (v18+)
- [Angular CLI](https://angular.io/cli) — `npm install -g @angular/cli`

---

### 🖥️ Backend Setup

```bash
# 1. Navigate to the backend directory
cd backend/ExpenseTracker

# 2. Restore NuGet packages
dotnet restore

# 3. Update appsettings.json with your SQL Server connection string
#    (see Configuration section below)

# 4. Apply database migrations
dotnet ef database update \
  --project ExpenseTracker.Infrastructure \
  --startup-project ExpenseTracker.Api

# 5. Run the API
dotnet run --project ExpenseTracker.Api
```

✅ API will be running at: **`https://localhost:7092`**  
📄 Swagger UI: **`https://localhost:7092/swagger`**

---

### 🌐 Frontend Setup

```bash
# 1. Navigate to the frontend directory
cd frontend/Angular

# 2. Install dependencies
npm install

# 3. Start the development server
ng serve
```

✅ App will be running at: **`http://localhost:4200`**

---

## ⚙️ Configuration

### Backend — `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ExpenseTrackerDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JWT": {
    "Key": "YOUR_SUPER_SECRET_KEY_HERE_MIN_32_CHARS",
    "Issuer": "ExpenseTrackerApi",
    "Audience": "ExpenseTrackerClient",
    "ExpiryInDays": 7
  }
}
```

### Frontend — `environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7092/api'
};
```

---

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Login and receive JWT token |
| `GET` | `/api/transactions` | Get all user transactions |
| `POST` | `/api/transactions` | Create a new transaction |
| `PUT` | `/api/transactions/{id}` | Update a transaction |
| `DELETE` | `/api/transactions/{id}` | Delete a transaction |
| `GET` | `/api/categories` | Get all categories |
| `POST` | `/api/categories` | Create a new category |
| `GET` | `/api/dashboard` | Get dashboard summary data |
| `GET` | `/api/reports` | Get financial reports |
| `GET` | `/api/profile` | Get user profile |
| `PUT` | `/api/profile` | Update user profile |

> 🔒 All endpoints (except auth) require a valid **Bearer Token** in the `Authorization` header.

---

## 🔄 How It Works

```
[User] → [Angular Frontend]
              │
              │  HTTP Request + JWT Token
              ▼
      [ASP.NET Core API]
              │
        ┌─────┴─────┐
        │           │
  [Application]  [Identity]
   Services &    Auth Layer
    DTOs          (JWT)
        │
        ▼
  [Infrastructure]
   EF Core + Repos
        │
        ▼
  [SQL Server DB]
```

---

## 📦 Building for Production

### Backend
```bash
dotnet publish ExpenseTracker.Api -c Release -o ./publish
```

### Frontend
```bash
ng build --configuration production
```
Output will be in the `dist/` folder — ready to deploy to any static hosting (Nginx, IIS, Vercel, etc.).

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!

1. Fork the repository
2. Create your feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<div align="center">

**Built with ❤️ using Angular & ASP.NET Core**

*If this project helped you, give it a ⭐ — it means a lot!*

</div>