# miniorder-cloudnative-dotnet
## Day 1 : Run Catalog API in docker (Code spaces)

### Build
docker build -t catalog-api -f src/Catalog.Api/Dockerfile .

### Run
docker run --rm -e ASPNETCORE_ENVIRONMENT=Development -p 8080:8080 catalog-api

### open
/swagger
