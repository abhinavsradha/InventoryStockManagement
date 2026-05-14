# API Documentation

Base URL:

```text
http://localhost:5148
```

## Create Product

Creates a product with variants and variant options.

```http
POST /api/products
Content-Type: application/json
```

### Request Body

```json
{
  "name": "Shirt",
  "productCode": "SHIRT-001",
  "hsnCode": "6109",
  "isFavourite": false,
  "createdUser": null,
  "variants": [
    {
      "name": "Size",
      "options": ["S", "M", "L"]
    },
    {
      "name": "Color",
      "options": ["Red", "Blue", "Black"]
    }
  ]
}
```

### Success Response

```http
201 Created
```

### Error Responses

```http
400 Bad Request
409 Conflict
500 Internal Server Error
```

## List Products

Returns paginated product list with variants and stock.

```http
GET /api/products?pageNumber=1&pageSize=10&search=shirt
```

### Query Parameters

| Name | Type | Required | Description |
| --- | --- | --- | --- |
| pageNumber | int | No | Page number. Default is 1. |
| pageSize | int | No | Page size. Maximum is 50. |
| search | string | No | Search by product name, product code, or HSN code. |

### Success Response

```http
200 OK
```

```json
{
  "items": [
    {
      "id": "00000000-0000-0000-0000-000000000000",
      "productCode": "SHIRT-001",
      "productName": "Shirt",
      "hsnCode": "6109",
      "totalStock": 10,
      "isFavourite": false,
      "active": true,
      "createdDate": "2026-05-13T00:00:00+00:00",
      "variants": [
        {
          "name": "Size",
          "options": ["S", "M", "L"]
        }
      ]
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1
}
```

## Add Stock

Adds stock through purchase transaction.

```http
POST /api/products/stock/purchase
Content-Type: application/json
```

### Request Body

```json
{
  "productId": "00000000-0000-0000-0000-000000000000",
  "quantity": 20,
  "notes": "Opening purchase"
}
```

### Success Response

```http
204 No Content
```

### Error Responses

```http
400 Bad Request
404 Not Found
```

## Remove Stock

Removes stock through sale transaction.

```http
POST /api/products/stock/sale
Content-Type: application/json
```

### Request Body

```json
{
  "productId": "00000000-0000-0000-0000-000000000000",
  "quantity": 5,
  "notes": "Customer sale"
}
```

### Success Response

```http
204 No Content
```

### Error Responses

```http
400 Bad Request
404 Not Found
409 Conflict
```

`409 Conflict` is returned when sale quantity is greater than available stock.

