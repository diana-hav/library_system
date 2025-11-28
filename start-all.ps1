# PowerShell script to start all services (backend + frontend)
Write-Host "Starting All Services..." -ForegroundColor Green
Write-Host ""

# Start BorrowingService
Write-Host "Starting BorrowingService on http://localhost:5017..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd BorrowingService\BorrowingService.Api; dotnet run" -WindowStyle Normal

Start-Sleep -Seconds 3

# Start CatalogService
Write-Host "Starting CatalogService on http://localhost:5180..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd CatalogService\CatalogService.Api; dotnet run" -WindowStyle Normal

Start-Sleep -Seconds 3

# Start Frontend
Write-Host "Starting Frontend on http://localhost:5173..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd Frontend; npm run dev" -WindowStyle Normal

Write-Host ""
Write-Host "All services are starting..." -ForegroundColor Green
Write-Host "BorrowingService: http://localhost:5017" -ForegroundColor Yellow
Write-Host "CatalogService: http://localhost:5180" -ForegroundColor Yellow
Write-Host "Frontend: http://localhost:5173" -ForegroundColor Yellow
Write-Host ""
Write-Host "Services are running in separate windows. Close those windows to stop the services." -ForegroundColor Magenta




