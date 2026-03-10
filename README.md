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
## Day 3–4: Microservices + PostgreSQL + Docker Compose + Swagger

### What was built
- Two microservices:
  - **Catalog Service** (Products)
  - **Order Service** (Orders)
- Each service runs independently in Docker with its own Swagger/OpenAPI
- PostgreSQL added as real persistence (data survives restarts)
  - **catalog-db** for Catalog Service
  - **order-db** for Order Service
- EF Core + Npgsql used with migrations; tables are created automatically on startup via `Database.Migrate()`


### Architecture (local)
- catalog-service → http://localhost:5001
- order-service → http://localhost:5002
- catalog-db (Postgres) → localhost:5433 (for local access)
- order-db (Postgres) → localhost:5434 (for local access)

Inside Docker network, services connect using service DNS names:
- `catalog-db:5432`
- `order-db:5432`


### How to run
```bash
docker compose up --build
docker compose down

```
---
### Day 4 Progress — Gateway (YARP)

Implemented an API Gateway using YARP to provide a single public entry point for backend services.

### Completed
- Added `api-gateway` project using YARP
- Configured reverse proxy routes:
  - `/catalog/*` → `catalog-service`
  - `/orders/*` → `order-service`
- Added `/health` endpoint for gateway health check
- Verified successful routing for Catalog and Order APIs
- Tested gateway endpoints successfully in Postman

### Current Outcome
The system now supports API-driven architecture with a single gateway entry point in front of multiple backend services.

### Architecture
```text
Client / Postman
       |
       v
   API Gateway (YARP)
    /             \
   v               v
Catalog API      Order API
   |               |
   v               v
Catalog DB       Order DB