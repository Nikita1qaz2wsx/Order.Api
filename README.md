# Order.Api

## How to run

```bash
dotnet run --project Order.Api
```

API listens on `http://localhost:5135`.

Swagger UI: `http://localhost:5135/swagger`

---

## Example requests

**Create an order**
```http
POST http://localhost:5135/api/orders
Content-Type: application/json

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderItems": [
    { "id": "1fa11111-1111-1111-1111-111111111111", "name": "Widget A", "price": 9.99, "quantity": 2 },
    { "id": "2fa22222-2222-2222-2222-222222222222", "name": "Widget B", "price": 4.50, "quantity": 1 }
  ]
}
```

Response `201 Created`:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderItems": [...],
  "totalPrice": 24.48
}
```

**Get order by Id**
```http
GET http://localhost:5135/api/orders/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

Response `200 OK`:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderItems": [...],
  "totalPrice": 24.48
}
```

---

## Example error responses

**Not found — `404`**
```http
GET http://localhost:5135/api/orders/00000000-0000-0000-0000-000000000000
```
```json
{
  "message": "Order 00000000-0000-0000-0000-000000000000 not found."
}
```

**Unhandled exception — `500`**
```http
GET http://localhost:5135/api/orders/throw
```
```json
{
  "error": "An unexpected error occurred.",
  "traceId": "0HN8K2VQ1234:00000001",
}
```
