# RaceDay API - Part 2

**Student Name: Siyabonga Msimango  
**Student Number: ST10488360
**Module: PROG6212  
**Project: RaceDay - Event Management System (Part 2: RESTful API Development)

##  Project Overview
RaceDay is a full-stack web-based event management system designed for the South African road running, walking, and cycling community. 

This repository contains **Part 2** of the project: the RESTful API backend. It is built using **ASP.NET Core Web API** in C# and connects to a **SQL Server** database using **Entity Framework Core** (Code-First approach). The API handles all data operations, enforces role-based access (Organiser and Participant), and is fully documented using Swagger.

##  Technologies Used
*   Language: C#
*   Framework: ASP.NET Core Web API (.NET 8.0)
*   Database: SQL Server (LocalDB)
*   ORM: Entity Framework Core (Code-First)
*   Authentication: Session-based management & Password Hashing (SHA256)
*   Documentation: Swagger UI
*   Testing:** xUnit

##  Key Features
*   Role-Based Access Control:** Distinct permissions for `Organiser` and `Participant` roles enforced at the API level.
*   Authentication:** Secure user registration and login using hashed passwords and session management.
*   Event Management:** Organisers can create, update, and delete events and categories.
*   Enrollments:** Participants can browse events and enroll in specific categories.
*   Results:** Organisers can capture finish times and positions; Participants can view their own results.
*   Profile Management:** Both roles can view and update their own profile information.
*   Swagger Integration:** All endpoints are fully visible, documented, and testable directly from the browser.

##  Getting Started

# Prerequisites
Before you begin, ensure you have the following installed:
*   [Visual Studio 2022](https://visualstudio.microsoft.com/) (with ASP.NET and web development workload)
*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   SQL Server Express LocalDB (usually installed with Visual Studio)

# Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Siyabonga95-lab/RaceDay_API.git# RaceDay_API# RaceDay_API
