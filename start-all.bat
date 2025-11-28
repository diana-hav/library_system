@echo off
echo Starting All Services...
echo.

echo Starting BorrowingService on http://localhost:5017...
start "BorrowingService" cmd /k "cd BorrowingService\BorrowingService.Api && dotnet run"

timeout /t 3 /nobreak >nul

echo Starting CatalogService on http://localhost:5180...
start "CatalogService" cmd /k "cd CatalogService\CatalogService.Api && dotnet run"

timeout /t 3 /nobreak >nul

echo Starting Frontend on http://localhost:5173...
start "Frontend" cmd /k "cd Frontend && npm run dev"

echo.
echo All services are starting...
echo BorrowingService: http://localhost:5017
echo CatalogService: http://localhost:5180
echo Frontend: http://localhost:5173
echo.
echo Press any key to close this window (services will continue running)...
pause >nul




