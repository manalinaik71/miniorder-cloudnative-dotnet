# miniorder-cloudnative-dotnet
A hands-on portfolio project to practice **cloud-native .NET** basics: Docker, Docker Compose, and (later) microservices, integration testing, Azure, and Kubernetes.

**Tech stack:** .NET 8, ASP.NET Core Web API, Docker, Docker Compose, PostgreSQL, EF Core

---
## Day 1 – Run Catalog API in Docker (Codespaces)

### Build
docker build -t catalog-api -f src/Catalog.Api/Dockerfile .

### Run
docker run --rm -e ASPNETCORE_ENVIRONMENT=Development -p 8080:8080 catalog-api

### Open
/swagger

---

## Day 2 – Docker Compose (API + PostgreSQL)

### Start
docker compose up --build

### Stop (keep DB data)
docker compose down

### Stop (delete DB data)
docker compose down -v

### Swagger
Open: /swagger

### Endpoints
POST /api/v1/products
GET  /api/v1/products

---

## Day 3 – Microservices + OpenAPI

### Existing Catalog API (http://localhost:8080)
- Swagger: /swagger

### Catalog Service (http://localhost:5001)
- Swagger: /swagger
- GET /api/v1/products
- GET /api/v1/products/{id}
- POST /api/v1/products

### Order Service (http://localhost:5002)
- Swagger: /swagger
- POST /api/v1/orders
- GET /api/v1/orders/{id}

### Run
docker compose up --build

---