# 🧑‍💼 Human Resources Management System (HRMS)

**HRMS** is a full-featured **Human Resources Management System** built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.  
It provides a complete solution for managing employees, attendance, payroll, and leave requests —  
designed with a **Clean Architecture** using **Repository Pattern**, **Unit of Work**, and **Service Layer** principles for maintainability and scalability.

---

## 🚀 Main Features

- 👥 **Employee Management** — Add, edit, and deactivate employees.  
- 🏢 **Department & Job Titles** — Organize the company hierarchy.  
- 🕒 **Attendance Tracking** — Record employee check-in/check-out.  
- 💰 **Payroll System** — Generate payslips with allowances and deductions.  
- 🧾 **Salary Components** — Manage all allowances and deductions flexibly.  
- 📄 **Document Management** — Upload and manage employee-related files (contracts, certificates, etc.).  
- 🏖️ **Leave Requests** — Submit and approve/reject leave requests.  
- 🔐 **User Authentication & Roles** — Built on **ASP.NET Identity** with custom user profiles.

---

## 🧩 Architecture Overview

The project follows **Clean Layered MVC Architecture**:

Controllers → Services (Business Logic) → Unit of Work → Repositories → DbContext → SQL Server

markdown
Copy code

Each entity has:
- A **Model** representing its database structure.
- A **Repository** for database operations.
- A **Service** handling business logic.
- A **Controller + Views** for user interaction.

---

## ⚙️ Technologies Used

- **ASP.NET Core 8 MVC**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Identity**
- **Bootstrap 5** (UI styling)
- **LINQ & Async Programming**
- **Generic Repository Pattern**
- **Unit of Work Pattern**
- **Dependency Injection (DI)**
- **C# 12**

---

## 📁 Project Structure

HRMS/
├── Controllers/ → MVC Controllers (EmployeeController, DepartmentController, etc.)
├── Models/ → Entity Models
├── Views/ → Razor Views (UI Pages)
│ ├── Employee/
│ ├── Department/
│ ├── Shared/
│ └── ...
├── Interfaces/ → Repository & Service Interfaces
├── Repositories/ → Repository Implementations
├── Services/
│ ├── Interfaces/ → Service Interfaces
│ └── Implementation/ → Service Implementations
├── DependencyInjection/ → Service registration (DI setup)
├── Data/ → ApplicationDbContext
├── wwwroot/ → Static files (CSS, JS, images)
├── appsettings.json → App configuration
└── Program.cs → App startup

yaml
Copy code

---

## 🧰 How to Run Locally

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/<your-username>/HRMS.git
cd HRMS
2️⃣ Update the Database Connection
In appsettings.json, update your SQL Server connection string:

json
Copy code
"ConnectionStrings": {
  "Connection": "Server=YOUR_SERVER;Database=HRMS_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}
3️⃣ Apply Migrations
bash
Copy code
dotnet ef database update
4️⃣ Run the Application
bash
Copy code
dotnet run
or open the solution in Visual Studio → press F5 to run.

🧠 Design Principles
Separation of Concerns (SoC)

Single Responsibility Principle (SRP)

Dependency Injection (DI)

Clean and Scalable Architecture

Async/Await for better performance

💡 Future Improvements
Role-based dashboards (Admin / HR / Employee)

Dynamic reporting and analytics

Email notifications (for leaves, payslips, etc.)

Frontend UI enhancement with React or Blazor

Audit logging and activity tracking

