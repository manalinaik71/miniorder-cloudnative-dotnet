# miniorder-cloudnative-dotnet
A hands-on portfolio project to practice **cloud-native .NET** basics: Docker, Docker Compose, and (later) microservices, integration testing, Azure, and Kubernetes.

**Tech stack:** .NET 8, ASP.NET Core Web API, Docker, Docker Compose, PostgreSQL, EF Core

---
## Day 1 - Run Catalog API in Docker (Codespaces)

### Build
docker build -t catalog-api -f src/Catalog.Api/Dockerfile .

### Run
docker run --rm -e ASPNETCORE_ENVIRONMENT=Development -p 8080:8080 catalog-api

### Open
/swagger

---

## Day 2 - Docker Compose (API + PostgreSQL)

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

## Day 3 - Microservices + OpenAPI

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
### Day 3–4  - Microservices + PostgreSQL + Docker Compose + Swagger

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
### Day 4 - Gateway (YARP)

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
```
---
### Day 5 - Integration Testing

### Overview

The goal was to verify that APIs work correctly when called through real HTTP endpoints, instead of testing only individual methods in isolation.

This day covers:

- ASP.NET Core Integration Testing
- `WebApplicationFactory<Program>`
- Testing API endpoints using `HttpClient`
- Verifying end-to-end request/response flow



### What was implemented

A separate integration test project was added for testing API behavior.



### Test cases covered

#### 1. Catalog API — GET endpoint
Verified that the Catalog API returns product details correctly for a valid product ID.

#### 2. Order API — POST endpoint
Verified that a new order can be created successfully.

#### 3. Order API — GET by ID endpoint
Verified that the created order can be fetched correctly using its generated order ID.

#### 4. Service-to-service validation flow
While creating an order, the Order API checks whether the product exists in the Catalog API before saving the order.  
This validates the real integration behavior between both services.




### Technologies used

- ASP.NET Core
- xUnit
- `Microsoft.AspNetCore.Mvc.Testing`
- `WebApplicationFactory<Program>`
- `HttpClient`
- FluentAssertions


### Project structure

Example structure:

```bash
src/
  catalog-service/
    Catalog.Api/
  order-service/
    Order.Api/
tests/
  Order.Api.IntegrationTests/

```  
---
### Day 6 - Deployment Notes (Azure Container Apps)

As part of Azure learning, I deployed the microservices application to Azure Container Apps.
The deployed components included:

1. Gateway App

2. Catalog API

3. Catalog DB container

### The deployment process involved:

1. building Docker images

2. pushing images to Azure Container Registry

3. deploying them to Azure Container Apps

4. configuring secrets and environment variables

5. checking application logs for troubleshooting

6. understanding revisions after updates

### This hands-on exercise helped me understand how cloud-native microservices can be hosted and managed in Azure without manually managing servers or Kubernetes clusters.

---


### Day 7 - Kubernetes Hands-on Proof

I deployed the microservices application in a Kubernetes playground using Deployments and Services.

### Verified components
- catalog-api
- catalog-db
- order-api
- order-db
- gateway-api

### Verification commands used
```bash
kubectl get pods
kubectl get svc
kubectl get svc gateway-api
curl http://localhost:30080
```
### Screenshot proof

![Kubernetes Hands-on Proof](k8s-proof.png)

---

