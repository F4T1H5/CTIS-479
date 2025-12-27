# CTIS479 MVC Project

## Acknowledgements
* Special thanks to my instructor, **Çağıl Alsaç**, for his guidance.
* This project was built using the [ProductsMVC](https://github.com/cagilalsac/ProductsMVC) repository as a primary reference and architectural template.

## Project Overview
This project is an ASP.NET Core MVC web application designed to manage a database of movies, directors, and genres. It implements a layered architecture (Core, App, MVC) and features role-based authentication, session management for favorites, and full CRUD capabilities.

## Features

### 🎬 Movie Management
- **CRUD Operations:** Create, Read, Update, and Delete movies.
- **Rich Data:** Movies include details like Revenue, Release Date, and associated Director.
- **Categorization:** Many-to-Many relationship support for assigning multiple **Genres** to a single movie.
- **Director Management:** Manage director profiles and link them to movies.

### 🔐 Authentication & Security
- **Cookie Authentication:** Secure login and logout functionality with sliding expiration.
- **Role-Based Access Control (RBAC):**
  - **Admins:** Have full access to create, edit, and delete content.
  - **Users:** Can view lists and details, but are restricted from administrative actions.
- **Data Protection:** CSRF protection using Anti-Forgery tokens.

### ⭐ User Features
- **Favorites List:** Session-based storage allowing users to add movies to a temporary "Favorites" list.
- **User Accounts:** Registration and profile management.

## Architecture
The solution follows a clean, layered architecture:

1.  **CORE:** Contains base abstract classes, interfaces, and common utilities.
2.  **APP:** Contains the Business Logic Layer (BLL), including:
    - **Domain Entities:** (`Movie`, `Director`, `Genre`, `User`, etc.)
    - **Services:** (`MovieService`, `DirectorService`, `UserService`) implementing logic separate from the UI.
    - **Data Access:** Entity Framework Core configuration and Migrations.
3.  **MVC:** The Presentation Layer containing Controllers and Views (Razor).

## Technology Stack
- **Framework:** .NET 8 (ASP.NET Core MVC)
- **Database:** SQLite (configured in `Program.cs`)
- **ORM:** Entity Framework Core
- **Frontend:** HTML5, CSS3, Bootstrap, jQuery
- **IDEs:** Visual Studio / VS Code

## Getting Started

### Prerequisites
- .NET SDK (Version 8.0)
- Visual Studio 2022
