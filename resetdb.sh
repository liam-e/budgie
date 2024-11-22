#!/bin/bash
echo "Removing existing migrations..."
rm Budgie.API/Migrations/*

echo "Adding new migration..."
dotnet ef migrations add InitialCreate --project Budgie.API/Budgie.API.csproj

echo "Updating database..."
dotnet ef database update --project Budgie.API/Budgie.API.csproj

echo "Starting application with dotnet watch..."
ASPNETCORE_ENVIRONMENT=Development dotnet watch run --launch-profile https --project Budgie.API/Budgie.API.csproj

