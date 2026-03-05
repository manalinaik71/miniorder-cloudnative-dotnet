#build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Order.Api.csproj ./
RUN dotnet restore ./Order.Api.csproj

COPY . .
RUN dotnet publish Order.Api.csproj -c Release -o /app/publish

#runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS = http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet","Order.Api.dll" ]
