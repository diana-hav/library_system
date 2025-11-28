#!/bin/bash

echo "Starting All Services..."
echo ""

echo "Starting BorrowingService on http://localhost:5017..."
cd BorrowingService/BorrowingService.Api
dotnet run &
BORROWING_PID=$!
cd ../..

sleep 3

echo "Starting CatalogService on http://localhost:5180..."
cd CatalogService/CatalogService.Api
dotnet run &
CATALOG_PID=$!
cd ../..

sleep 3

echo "Starting Frontend on http://localhost:5173..."
cd Frontend
npm run dev &
FRONTEND_PID=$!
cd ..

echo ""
echo "All services are starting..."
echo "BorrowingService: http://localhost:5017 (PID: $BORROWING_PID)"
echo "CatalogService: http://localhost:5180 (PID: $CATALOG_PID)"
echo "Frontend: http://localhost:5173 (PID: $FRONTEND_PID)"
echo ""
echo "Press Ctrl+C to stop all services..."

# Wait for Ctrl+C
trap "kill $BORROWING_PID $CATALOG_PID $FRONTEND_PID; exit" INT TERM
wait




