# Inventory & Stock Management System

ASP.NET Core / Blazor machine test project for managing products, variants, sub-variants, purchase stock, and sale stock.

## Technology Stack

- .NET 8
- ASP.NET Core Blazor Web App
- ASP.NET Core Minimal APIs
- Entity Framework Core 8
- Microsoft SQL Server LocalDB
- Bootstrap

## Features

- Create product with variants and options
- Product listing with pagination and search
- Add stock through purchase entry
- Remove stock through sale entry
- Prevent sale when stock is insufficient
- SQL Server database integration
- REST API endpoints
- Server-side validation and error handling
- Logging through ASP.NET Core logging

## Project Structure

```text
InventoryStockManagement
|-- Components/Pages
|   |-- Home.razor
|   |-- ProductCreate.razor
|   |-- Products.razor
|   |-- Stock.razor
|-- Data
|   |-- InventoryDbContext.cs
|-- DatabaseScripts
|   |-- CreateDatabase.sql
|-- Docs
|   |-- API.md
|-- Dtos
|   |-- ProductDtos.cs
|-- Endpoints
|   |-- ProductEndpoints.cs
|-- Models
|   |-- Product.cs
|   |-- ProductVariant.cs
|   |-- ProductVariantOption.cs
|   |-- StockTransaction.cs
|-- Services
|   |-- ProductService.cs
|-- Program.cs
|-- appsettings.json
```

## Database

The application uses SQL Server LocalDB by default.

Database name:

```text
InventoryStockManagementDb
```

Connection string location:

```text
appsettings.json
```

```json
"InventoryConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryStockManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

The database is created automatically when the app starts because `Program.cs` calls `Database.EnsureCreated()`.

You can also manually create the database using:

```text
DatabaseScripts/CreateDatabase.sql
```

## Setup Instructions

1. Open `InventoryStockManagement.csproj` in Visual Studio 2022.
2. Confirm .NET 8 SDK is installed.
3. Restore NuGet packages if Visual Studio asks.
4. Make sure SQL Server LocalDB is installed.
5. Run the project using the `http` profile.
6. Open:

```text
http://localhost:5148
```

## Running From Command Line

```bash
dotnet restore
dotnet build
dotnet run
```

## Main Screens

- Dashboard: `/`
- Product List: `/products`
- Create Product: `/products/create`
- Stock Add/Remove: `/stock`

## API Documentation

API details are available in:

```text
Docs/API.md
```

## Submission Contents

This folder contains:

- Complete source code
- SQL database script
- README setup instructions
- API documentation

