# Швидкий старт

## Мінімальні кроки для запуску

### 1. Налаштування БД
```sql
CREATE DATABASE BorrowingDb;
CREATE DATABASE CatalogDb;
-- або використайте назву з appsettings.json: catalog_service
```

Застосуйте міграції для CatalogService:
```bash
cd CatalogService/CatalogService.Api
dotnet ef database update --project ../CatalogService.Dal
```

### 2. Запуск всіх сервісів (бекенд + фронтенд)

**Простий спосіб - скрипт (рекомендовано):**
```powershell
# Windows PowerShell
.\start-all.ps1
```

```cmd
REM Windows CMD
start-all.bat
```

```bash
# Linux/Mac
chmod +x start-all.sh
./start-all.sh
```

**Або вручну в трьох терміналах:**

**Термінал 1 - BorrowingService:**
```bash
cd BorrowingService/BorrowingService.Api
dotnet run
```

**Термінал 2 - CatalogService:**
```bash
cd CatalogService/CatalogService.Api
dotnet run
```

**Термінал 3 - Frontend:**
```bash
cd Frontend
npm install  # якщо ще не встановлено
npm run dev
```

### 3. Відкрити браузер
http://localhost:5173

## Перевірка

- ✅ BorrowingService: http://localhost:5017/swagger
- ✅ CatalogService: http://localhost:5180/swagger
- ✅ Frontend: http://localhost:5173

## Зупинка

**Якщо використовували скрипт:**
- Закрийте вікна терміналів, де запущені сервіси

**Якщо запускали вручну:**
- `Ctrl+C` в кожному терміналі

