# Inventory Stock Management

This is a small inventory project done for the .NET machine test.

The project is made with ASP.NET Core Blazor and SQL Server. It has product creation, product listing, variants, stock purchase, and stock sale.

## Technologies Used

- .NET 8
- ASP.NET Core Blazor
- Entity Framework Core
- SQL Server LocalDB
- Bootstrap

## How to Run

1. Download or clone the project.
2. Open `InventoryStockManagement.sln` in Visual Studio 2022.
3. Restore NuGet packages if it is not restored automatically.
4. Run the project with the `http` profile.
5. The app will open in browser.

Local URL:

```text
http://localhost:5148
```

## Database

I used SQL Server LocalDB for this project.

Database name:

```text
InventoryStockManagementDb
```

The connection string is in `appsettings.json`.

```json
"InventoryConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryStockManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

The database will be created automatically when the application starts.

I have also added the SQL script here:

```text
DatabaseScripts/CreateDatabase.sql
```

## Main Pages

- `/` - Dashboard
- `/products` - Product list
- `/products/create` - Create product
- `/stock` - Add or remove stock

## API Endpoints

Product APIs are added in `Endpoints/ProductEndpoints.cs`.

Main APIs:

```text
POST /api/products
GET /api/products
POST /api/products/stock/purchase
POST /api/products/stock/sale
```

More API details are written in:

```text
Docs/API.md
```

## Folder Details

```text
Models              - Entity classes
Data                - DbContext
Dtos                - Request and response models
Services            - Business logic
Endpoints           - API endpoints
Components/Pages    - Blazor pages
DatabaseScripts     - SQL script
Docs                - API documentation
```

## Notes

- Product can have multiple variants like Size and Color.
- Each variant can have multiple options.
- Purchase will increase stock.
- Sale will decrease stock.
- Sale is not allowed if stock is not enough.
- Product list has pagination and search.

