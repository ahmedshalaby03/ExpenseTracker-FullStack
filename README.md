<div align="center">

<br/>

```text
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

# Expense Tracker

### Full-Stack Expense Management System

Track smarter. Spend wiser. Save better.

<br/>

![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge\&logo=angular\&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge\&logo=microsoftsqlserver\&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge\&logo=typescript\&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge\&logo=bootstrap\&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge\&logo=jsonwebtokens\&logoColor=white)

</div>

---

## Live Demo

| Service      | Link                                                    |
| ------------ | ------------------------------------------------------- |
| Frontend App | https://expense-track.runasp.net                        |
| API Swagger  | https://expense-track-api.runasp.net/swagger/index.html |
| API Base URL | https://expense-track-api.runasp.net/api                |

> The project is deployed on **MonsterASP.NET** using separate hosted services for the Angular frontend and the ASP.NET Core Web API.

---

## Overview

**Expense Tracker** is a full-stack web application designed to help users manage their personal finances through income and expense tracking, category management, dashboard insights, reports, and profile customization.

The frontend is built with **Angular**, while the backend is built using **ASP.NET Core Web API** following **Onion Architecture**. Authentication is handled using **ASP.NET Identity** and **JWT Bearer Tokens**, with data stored in **SQL Server**.

---

## Architecture

```text
ExpenseTracker-FullStack/
├── Backend/
│   └── ExpenseTracker/
│       ├── ExpenseTracker.Api
│       │   └── Controllers, Program.cs, API configuration
│       │
│       ├── ExpenseTracker.Application
│       │   └── DTOs, Interfaces, Application Services
│       │
│       ├── ExpenseTracker.Domain
│       │   └── Entities, Enums, Core Domain Models
│       │
│       └── ExpenseTracker.Infrastructure
│           └── EF Core, Identity, Database Context, Services
│
└── Frontend/
    └── Angular/
        └── ExpenseTracker.Client/
            ├── src/app/core
            │   └── Services, Models, Interceptors
            │
            ├── src/app/features
            │   └── Auth, Dashboard, Categories, Transactions, Profile, Reports
            │
            ├── src/app/shared
            │   └── Shared UI Components
            │
            └── src/environments
                └── API environment configuration
```

The backend follows **Onion Architecture**, which keeps the domain and application logic independent from infrastructure concerns such as databases, authentication, and external services.

---

## Tech Stack

| Layer          | Technology                    |
| -------------- | ----------------------------- |
| Frontend       | Angular                       |
| Language       | TypeScript                    |
| Styling        | Bootstrap Icons + Custom CSS  |
| Backend        | ASP.NET Core Web API          |
| Runtime        | .NET 10                       |
| Database       | SQL Server                    |
| ORM            | Entity Framework Core         |
| Authentication | ASP.NET Identity + JWT Bearer |
| Architecture   | Onion Architecture            |
| Hosting        | MonsterASP.NET                |
| SSL            | Let's Encrypt HTTPS           |

---

## Features

### Authentication & Security

* User registration
* User login
* JWT authentication
* Protected API endpoints
* Angular route protection
* HTTP interceptor for attaching bearer tokens
* ASP.NET Identity password hashing

### Transactions

* Add income and expense transactions
* Update existing transactions
* Delete transactions
* Filter transactions by type, category, and date
* Track payment methods

### Categories

* Create custom income and expense categories
* Update category name, icon, color, and type
* Delete categories
* User-specific categories

### Dashboard

* Total income
* Total expenses
* Net balance
* Recent transactions
* Expenses by category
* Monthly income vs expenses

### Reports

* Financial summary reports
* Category-based reports
* Monthly breakdowns
* User-specific analytics

### Profile Management

* Update profile information
* Upload profile avatar
* Update preferences
* Change password
* Manage notification settings
* Set preferred currency

### Responsive UI

* Modern dark-themed interface
* Mobile-friendly layout
* Works across desktop, tablet, and mobile screens

---

## API Endpoints

### Auth

| Method | Endpoint             | Description                 |
| ------ | -------------------- | --------------------------- |
| POST   | `/api/Auth/register` | Register a new user         |
| POST   | `/api/Auth/login`    | Login and receive JWT token |

### Categories

| Method | Endpoint               | Description         |
| ------ | ---------------------- | ------------------- |
| GET    | `/api/Categories`      | Get user categories |
| GET    | `/api/Categories/{id}` | Get category by id  |
| POST   | `/api/Categories`      | Create category     |
| PUT    | `/api/Categories/{id}` | Update category     |
| DELETE | `/api/Categories/{id}` | Delete category     |

### Transactions

| Method | Endpoint                 | Description           |
| ------ | ------------------------ | --------------------- |
| GET    | `/api/Transactions`      | Get user transactions |
| GET    | `/api/Transactions/{id}` | Get transaction by id |
| POST   | `/api/Transactions`      | Create transaction    |
| PUT    | `/api/Transactions/{id}` | Update transaction    |
| DELETE | `/api/Transactions/{id}` | Delete transaction    |

### Dashboard

| Method | Endpoint                                | Description                         |
| ------ | --------------------------------------- | ----------------------------------- |
| GET    | `/api/Dashboard/summary`                | Get dashboard summary               |
| GET    | `/api/Dashboard/recent-transactions`    | Get recent transactions             |
| GET    | `/api/Dashboard/expenses-by-category`   | Get expenses by category            |
| GET    | `/api/Dashboard/monthly-income-expense` | Get monthly income and expense data |

### Profile

| Method | Endpoint                       | Description             |
| ------ | ------------------------------ | ----------------------- |
| GET    | `/api/Profile`                 | Get user profile        |
| PUT    | `/api/Profile`                 | Update profile          |
| PUT    | `/api/Profile/preferences`     | Update user preferences |
| PUT    | `/api/Profile/change-password` | Change password         |
| POST   | `/api/Profile/avatar`          | Upload profile avatar   |

> All endpoints except authentication require a valid JWT token in the `Authorization` header.

```text
Authorization: Bearer YOUR_TOKEN_HERE
```

---

## How It Works

```text
[User]
   |
   v
[Angular Frontend]
   |
   | HTTP Requests + JWT Token
   v
[ASP.NET Core Web API]
   |
   v
[Application Services]
   |
   v
[Infrastructure Layer]
   |
   v
[SQL Server Database]
```

---

## Getting Started Locally

### Prerequisites

Make sure you have installed:

* .NET SDK
* SQL Server or SQL Server LocalDB
* Node.js
* Angular CLI

```bash
npm install -g @angular/cli
```

---

## Backend Setup

Navigate to the backend solution folder:

```bash
cd Backend/ExpenseTracker
```

Restore packages:

```bash
dotnet restore
```

Update your local connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "expensetracker": "Server=(localdb)\\MSSQLLocalDB;Database=ExpenseTrackerDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "THIS_IS_A_SUPER_SECRET_KEY_FOR_EXPENSE_TRACKER_PROJECT_123456",
    "Issuer": "ExpenseTrackerIssuer",
    "Audience": "ExpenseTrackerAudience"
  }
}
```

Apply migrations:

```bash
dotnet ef database update --project ExpenseTracker.Infrastructure --startup-project ExpenseTracker.Api
```

Run the API:

```bash
dotnet run --project ExpenseTracker.Api
```

Local Swagger:

```text
http://localhost:5139/swagger/index.html
```

---

## Frontend Setup

Navigate to the Angular project:

```bash
cd Frontend/Angular/ExpenseTracker.Client
```

Install dependencies:

```bash
npm install
```

Development environment configuration:

```ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5139/api'
};
```

Run Angular:

```bash
ng serve
```

Local frontend:

```text
http://localhost:4200
```

---

## Production Configuration

### Angular Production Environment

`src/environments/environment.ts`

```ts
export const environment = {
  production: true,
  apiUrl: 'https://expense-track-api.runasp.net/api'
};
```

### ASP.NET Core Production Connection

`appsettings.Production.json`

```json
{
  "ConnectionStrings": {
    "expensetracker": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "ExpenseTrackerIssuer",
    "Audience": "ExpenseTrackerAudience"
  },
  "AllowedHosts": "*"
}
```

---

## Deployment

The project is deployed on **MonsterASP.NET** using two separate services:

| Part             | Hosting                               |
| ---------------- | ------------------------------------- |
| Angular Frontend | Static website on MonsterASP.NET      |
| ASP.NET Core API | WebDeploy to MonsterASP.NET           |
| Database         | SQL Server database on MonsterASP.NET |
| SSL              | Let's Encrypt HTTPS certificate       |

---

### Backend Deployment

The backend API was deployed to MonsterASP.NET using **WebDeploy** from Visual Studio.

Production API:

```text
https://expense-track-api.runasp.net
```

Swagger:

```text
https://expense-track-api.runasp.net/swagger/index.html
```

Main backend deployment steps:

```text
1. Create a new ASP.NET website on MonsterASP.NET
2. Create a SQL Server database
3. Add production connection string in appsettings.Production.json
4. Configure CORS for the Angular domain
5. Publish ExpenseTracker.Api using WebDeploy
6. Enable HTTPS using Let's Encrypt
7. Test API using Swagger
```

CORS configuration example:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://expense-track.runasp.net",
                "https://expense-track.runasp.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

---

### Frontend Deployment

The Angular frontend was built using:

```bash
ng build --configuration production
```

Build output:

```text
dist/ExpenseTracker.Client/browser
```

The following files were uploaded to the MonsterASP.NET website `/wwwroot` folder:

```text
index.html
main-*.js
styles-*.css
favicon.ico
media/
web.config
```

Production frontend:

```text
https://expense-track.runasp.net
```

---

## Angular IIS Rewrite Configuration

To support Angular routing on IIS/MonsterASP.NET, a `web.config` file is added to the production build output:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

This prevents 404 errors when refreshing Angular routes like:

```text
/auth/login
/dashboard
/profile
/transactions
```

---

## Build for Production

### Backend

```bash
dotnet publish ExpenseTracker.Api -c Release -o ./publish
```

### Frontend

```bash
ng build --configuration production
```

---

## Important Notes

* The backend uses JWT authentication.
* Passwords are stored securely using ASP.NET Identity hashing.
* The frontend communicates with the hosted API using HTTPS.
* The SQL Server database is hosted on MonsterASP.NET.
* Angular routing requires `web.config` when deployed to IIS-based hosting.
* The `.well-known` folder should not be deleted because it is used for SSL certificate validation.

---

## Future Improvements

* Add password reset by email
* Add recurring transactions
* Add budget alerts
* Add multi-currency support
* Add admin dashboard

---

## Author

**Ahmed Saeed Shalaby**

Full-Stack .NET Developer
ASP.NET Core Web API | Angular | SQL Server | Entity Framework Core

---

<div align="center">

Built with passion using **Angular** and **ASP.NET Core**

If this project helped you, give it a star.

</div>
