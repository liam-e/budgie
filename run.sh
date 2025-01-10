#!/bin/bash
python Budgie.API/Data/dates.py

echo "Starting application with dotnet watch..."
ASPNETCORE_ENVIRONMENT=Development dotnet watch run --launch-profile http --project Budgie.API/Budgie.API.csproj
