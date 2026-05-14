Inventory Stock Management

This is a .NET machine test project for product inventory and stock management.

The project is developed using ASP.NET Core Blazor, Entity Framework Core and SQL Server LocalDB.

Technologies used:

.NET 8
ASP.NET Core Blazor
Entity Framework Core
SQL Server LocalDB
Bootstrap

How to run:

1. Download or clone the project.
2. Open InventoryStockManagement.sln in Visual Studio 2022.
3. Restore NuGet packages if needed.
4. Run the project using the http profile.
5. Open the below URL in browser.

http://localhost:5148

Database:

Database name is InventoryStockManagementDb.

The connection string is available in appsettings.json.

The database will be created automatically when the application starts.

SQL script is also added in this folder:

DatabaseScripts/CreateDatabase.sql

Main pages:

/ - Dashboard
/products - Product list
/products/create - Create product
/stock - Add or remove stock

API endpoints:

POST /api/products
GET /api/products
POST /api/products/stock/purchase
POST /api/products/stock/sale

API details are added in:

Docs/API.md

Folder details:

Models - Entity classes
Data - DbContext
Dtos - Request and response models
Services - Business logic
Endpoints - API endpoints
Components/Pages - Blazor pages
DatabaseScripts - SQL script
Docs - API documentation

Notes:

Product can have multiple variants like Size and Color.
Each variant can have multiple options.
Purchase will increase stock.
Sale will decrease stock.
Sale is not allowed if stock is not enough.
Product list has pagination and search.

