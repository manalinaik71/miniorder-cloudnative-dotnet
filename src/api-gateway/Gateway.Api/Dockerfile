# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Gateway.Api.csproj ./
RUN dotnet restore ./Gateway.Api.csproj

COPY . ./
RUN dotnet publish ./Gateway.Api.csproj -c Release -o /app/publish

#runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet","Gateway.Api.dll" ]
