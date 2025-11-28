# Library Management System

Система управління бібліотекою з мікросервісною архітектурою.

## Архітектура

Проект складається з:
- **BorrowingService** - сервіс для управління читачами та позиками
- **CatalogService** - сервіс для управління каталогом книг
- **Frontend** - React + Vite фронтенд-додаток

## Передумови

1. **.NET 8 SDK** або новіша версія
2. **Node.js** (версія 18 або новіша) та **npm**
3. **PostgreSQL** база даних
4. **Visual Studio** або **VS Code** (опціонально)

## Покрокова інструкція запуску

### Крок 1: Налаштування бази даних

1. Створіть дві бази даних в PostgreSQL:
   ```sql
   CREATE DATABASE BorrowingDb;
   CREATE DATABASE CatalogDb;
   ```
   Або використовуйте назви з `appsettings.json`:
   - `catalog_service` для CatalogService
   - Перевірте назву бази в `BorrowingService/BorrowingService.Api/appsettings.json`

2. Оновіть рядки підключення в файлах:
   - `BorrowingService/BorrowingService.Api/appsettings.json`
   - `CatalogService/CatalogService.Api/appsettings.json`

3. Застосуйте міграції для CatalogService:
   ```bash
   cd CatalogService/CatalogService.Api
   dotnet ef database update --project ../CatalogService.Dal
   ```
   Це створить структуру таблиць та seed дані (автори, жанри, книги).

### Крок 2: Встановлення залежностей

1. Відкрийте термінал в корені проекту
2. Встановіть залежності для обох сервісів:
   ```bash
   cd BorrowingService/BorrowingService.Api
   dotnet restore
   
   cd ../../CatalogService/CatalogService.Api
   dotnet restore
   ```

3. Встановіть залежності для фронтенду:
   ```bash
   cd ../../Frontend
   npm install
   ```

### Крок 3: Запуск всіх сервісів (бекенд + фронтенд)

#### Варіант 1: Використання скрипту (рекомендовано)

**Windows (PowerShell):**
```powershell
.\start-all.ps1
```
або
```powershell
.\start-backend.ps1
```

**Windows (CMD):**
```cmd
start-all.bat
```
або
```cmd
start-backend.bat
```

**Linux/Mac:**
```bash
chmod +x start-all.sh
./start-all.sh
```
або
```bash
chmod +x start-backend.sh
./start-backend.sh
```

Скрипти запустять всі сервіси в окремих вікнах:
- **BorrowingService**: http://localhost:5017
- **CatalogService**: http://localhost:5180
- **Frontend**: http://localhost:5173

#### Варіант 2: Ручний запуск

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
npm run dev
```

#### Варіант 3: Запуск через Visual Studio

1. Відкрийте `LibrarySystem.sln` в Visual Studio
2. Клікніть правою кнопкою на solution → **Properties**
3. Оберіть **Multiple startup projects**
4. Встановіть **Start** для:
   - `BorrowingService.Api`
   - `CatalogService.Api`
5. Натисніть **F5** або **Start**
6. В окремому терміналі запустіть фронтенд:
   ```bash
   cd Frontend
   npm run dev
   ```

> **Примітка:** Якщо використовуєте скрипти (`start-all.*` або `start-backend.*`), фронтенд запускається автоматично, і окремий крок 4 не потрібен.

### Крок 4: Перевірка роботи

1. Відкрийте браузер і перейдіть на: `http://localhost:5173`
2. Перевірте навігацію між вкладками:
   - **Readers** - список читачів
   - **Borrowings** - список позик
   - **Каталог** - управління каталогом книг
   - **Add Reader** - додавання читача
   - **Issue Book** - видача книги

3. Перевірте Swagger документацію API:
   - BorrowingService: http://localhost:5017/swagger
   - CatalogService: http://localhost:5180/swagger

## Порти сервісів

| Сервіс | HTTP | HTTPS |
|--------|------|-------|
| BorrowingService | 5017 | 7147 |
| CatalogService | 5180 | 7102 |
| Frontend | 5173 | - |

## Структура проекту

```
LibrarySystem/
├── BorrowingService/
│   ├── BorrowingService.Api/       # API контролери
│   ├── BorrowingService.Bll/       # Бізнес-логіка
│   ├── BorrowingService.Dal/       # Доступ до даних
│   └── BorrowingService.Domain/    # Доменні моделі
├── CatalogService/
│   ├── CatalogService.Api/         # API контролери
│   ├── CatalogService.Bll/         # Бізнес-логіка
│   ├── CatalogService.Dal/         # Доступ до даних
│   └── CatalogService.Domain/      # Доменні моделі
├── Frontend/                        # React додаток
└── start-backend.*                 # Скрипти запуску
```

## Розв'язання проблем

### Помилка "address already in use"

Якщо порт зайнятий, виконайте:

**Windows:**
```powershell
# Знайти процес на порту 5017
netstat -ano | findstr :5017

# Завершити процес (замініть PID на знайдений)
taskkill /PID <PID> /F
```

**Linux/Mac:**
```bash
# Знайти процес на порту 5017
lsof -i :5017

# Завершити процес
kill -9 <PID>
```

### Помилки підключення до бази даних

1. Перевірте, що PostgreSQL запущений
2. Перевірте рядок підключення в `appsettings.json`
3. Переконайтеся, що бази даних створені
4. Застосуйте міграції (якщо вони є):
   ```bash
   cd BorrowingService/BorrowingService.Api
   dotnet ef database update
   ```

### Помилки на фронтенді

1. Перевірте, що обидва бекенд-сервіси запущені
2. Перевірте налаштування proxy в `Frontend/vite.config.ts`
3. Очистіть кеш:
   ```bash
   cd Frontend
   rm -rf node_modules
   npm install
   ```

## Зупинка сервісів

Якщо використовували скрипти:
- Просто закрийте вікна терміналів, де запущені сервіси

Якщо запускали вручну:
- Натисніть `Ctrl+C` в кожному терміналі

Якщо запускали через Visual Studio:
- Натисніть `Shift+F5` або кнопку Stop

## Розробка

### Створення міграцій (якщо потрібно)

```bash
# BorrowingService
cd BorrowingService/BorrowingService.Api
dotnet ef migrations add MigrationName --project ../BorrowingService.Dal

# CatalogService
cd CatalogService/CatalogService.Api
dotnet ef migrations add MigrationName --project ../CatalogService.Dal
```

### Запуск тестів

```bash
# BorrowingService тести
cd BorrowingService/BorrowingService.Tests
dotnet test

# CatalogService тести
cd CatalogService/CatalogService.Tests
dotnet test
```

## Додаткова інформація

- API документація доступна через Swagger UI на `/swagger` кожного сервісу
- Логи зберігаються в папці `Logs/` кожного API проекту
- Для production використовуйте окрему конфігурацію з `appsettings.Production.json`

