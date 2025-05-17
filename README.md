# Event Booking System API

This is the backend API for the Event Booking System, built with ASP.NET Core 8.0 and Entity Framework Core.

## Prerequisites

Before you begin, ensure you have the following installed:
- .NET 8.0 SDK
- SQL Server (Express or Developer edition)
- Git
- Visual Studio 2022 or Visual Studio Code with C# extensions

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/yourusername/Event-Booking-System.git
cd Event-Booking-System/Event-Booking-System-API
```

2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EventBookingSystem;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Open a terminal in the API project directory and run the following commands:
```bash
# Install Entity Framework Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Apply database migrations
dotnet ef database update
```

4. Start the API:
```bash
dotnet run
```

The API will be available at `https://localhost:7054`

## Project Structure

```
Event-Booking-System-API/
├── Controllers/           # API endpoints
├── EventService/         # Business logic and DTOs
├── DB-Layer/            # Database context and entities
│   ├── Entities/        # Entity models
│   └── Persistence/     # DbContext and configurations
└── Program.cs           # Application entry point
```

## API Endpoints

### Authentication
- POST `/api/Auth/Register` - Register a new user
- POST `/api/Auth/Login` - Login user
- POST `/api/Auth/RefreshToken` - Refresh JWT token

### Events
- GET `/api/Event/GetAllEvents` - Get all events
- GET `/api/Event/GetEventById/{id}` - Get event by ID
- POST `/api/Event/CreateEvent` - Create new event
- PUT `/api/Event/UpdateEvent` - Update event
- DELETE `/api/Event/DeleteEvent/{id}` - Delete event
- POST `/api/Event/SoftDeleteEvent/{id}` - Soft delete event

### Categories
- GET `/api/Category/GetAllCategories` - Get all categories
- POST `/api/Category/CreateCategory` - Create new category
- PUT `/api/Category/UpdateCategory` - Update category
- DELETE `/api/Category/DeleteCategory/{id}` - Delete category
- POST `/api/Category/SoftDeleteCategory/{id}` - Soft delete category

### Bookings
- GET `/api/Booking/GetUserBookings` - Get user's bookings
- POST `/api/Booking/CreateBooking` - Create new booking
- DELETE `/api/Booking/CancelBooking/{id}` - Cancel booking

## Features

- JWT Authentication
- Role-based authorization
- Entity Framework Core with SQL Server
- Repository pattern
- Unit of Work pattern
- Soft delete functionality
- CORS configuration
- Swagger documentation

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- AutoMapper
- Swagger/OpenAPI

## Development

1. Create a new branch for your feature
2. Make your changes
3. Add migrations if needed:
```bash
dotnet ef migrations add YourMigrationName
dotnet ef database update
```
4. Submit a pull request

## License

This project is licensed under the MIT License. 