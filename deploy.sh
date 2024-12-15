#!/bin/bash

# Exit the script if any command fails
set -e

# Variables
FRONTEND_DIR="./Client"
FRONTEND_BUILD_DIR="$FRONTEND_DIR/dist"
BACKEND_BUILD_DIR="./out"
REMOTE_CLIENT_DIR="budgie:/var/www/budgieclient"
REMOTE_API_DIR="budgie:/var/www/budgieapi"

echo "=== Starting Deployment ==="

# Step 1: Build Frontend
echo "=== Building Frontend ==="
npm --prefix $FRONTEND_DIR install
npm --prefix $FRONTEND_DIR run build

# Step 2: Build Backend
echo "=== Building Backend ==="
dotnet publish Budgie.API/Budgie.API.csproj -c Release -o $BACKEND_BUILD_DIR

# Step 3: Transfer Frontend Files to Server
echo "=== Transferring Frontend Files ==="
rsync -avz $FRONTEND_BUILD_DIR/ $REMOTE_CLIENT_DIR

# Step 4: Transfer Backend Files to Server
echo "=== Transferring Backend Files ==="
echo "rsync -avz $BACKEND_BUILD_DIR/ $REMOTE_API_DIR"
rsync -avz $BACKEND_BUILD_DIR/ $REMOTE_API_DIR
rsync -avz Budgie.API/Data/ $REMOTE_API_DIR/Data

# Step 6: Reload API
echo "=== Reloading API ==="
ssh budgie "sudo /bin/systemctl enable budgieapi"
ssh budgie "sudo /bin/systemctl start budgieapi"
ssh budgie "sudo /bin/systemctl restart budgieapi"

# Step 7: Reload Nginx
echo "=== Reloading Nginx ==="
ssh budgie "sudo /bin/systemctl reload nginx"

echo "=== Deployment Complete ==="
