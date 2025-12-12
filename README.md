# K2TeamProject

A console application for managing school data, including students, courses, teachers, enrollments, grades, and scheduling.

## Features

- Student overview and reporting
- Active and upcoming course listings
- Grade statistics and period-based reporting
- Enrollment and registration management
- Teacher and course administration
- Scheduling support

## Technologies

- C#
- Entity Framework Core (Code-First & Database-First)
- SQL Server

## Getting Started

1. **Clone the repository:**
2. **Configure the database:**
- Update `appsettings.json` with your SQL Server connection string.

3. **Apply migrations:**
- Apply migrations for both code-first and database-first contexts. **Update-Database -context DatabaseFirstContext** **Update-Database -context CodeFirstContext**

4. **Run the application:**
- Run the application with F5

## Project Structure

- `Models/` - Entity and view models
- `UI/` - Console UI menus and methods
- `Migrations/` - Entity Framework migrations
- `Data/` - Data context and service classes
