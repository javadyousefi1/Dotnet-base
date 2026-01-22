# Dotnet Base - Clean Architecture

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                          Web.Api                            │
│                    (Controllers, Program)                    │
└─────────────────────────────────────────────────────────────┘
                            │
        ┌───────────────────┼───────────────────┐
        │                   │                   │
        ▼                   ▼                   ▼
┌──────────────┐   ┌──────────────┐   ┌──────────────┐
│Infrastructure│   │ Application  │   │ Persistence  │
│ (JWT, Redis) │───│(CQRS, MediatR)│───│(EF Core, DB) │
└──────────────┘   └──────────────┘   └──────────────┘
                            │
                            ▼
                    ┌──────────────┐
                    │    Domain    │
                    │(Entities,Enum)│
                    └──────────────┘
```

## Project Structure

```
src/
├── Domain/                    # Core domain entities and enums
│   ├── Entities/
│   │   └── User.cs
│   └── Enums/
│       └── UserRole.cs
│
├── Application/               # Use cases and abstractions
│   ├── Common/
│   │   ├── Abstractions/
│   │   │   ├── IOtpCache.cs
│   │   │   ├── IJwtProvider.cs
│   │   │   └── IUserRepository.cs
│   │   └── Models/
│   │       ├── JwtToken.cs
│   │       └── PaginatedResult.cs
│   ├── Authentication/
│   │   ├── GetOtp/
│   │   │   ├── GetOtpCommand.cs
│   │   │   ├── GetOtpCommandHandler.cs
│   │   │   └── GetOtpResult.cs
│   │   └── VerifyOtp/
│   │       ├── VerifyOtpCommand.cs
│   │       ├── VerifyOtpCommandHandler.cs
│   │       └── VerifyOtpResult.cs
│   └── Users/
│       └── GetAllUsers/
│           ├── GetAllUsersQuery.cs
│           ├── GetAllUsersQueryHandler.cs
│           └── UserDto.cs
│
├── Infrastructure/            # External services implementation
│   ├── Authentication/
│   │   ├── JwtProvider.cs
│   │   └── JwtSettings.cs
│   └── Caching/
│       └── RedisOtpCache.cs
│
├── Persistence/              # Database and EF Core
│   ├── Configurations/
│   │   └── UserConfiguration.cs
│   ├── Repositories/
│   │   └── UserRepository.cs
│   └── ApplicationDbContext.cs
│
└── Web.Api/                  # API endpoints
    ├── Controllers/
    │   ├── AuthController.cs
    │   └── UserController.cs
    └── Program.cs
```

## Features

### Authentication Flow (OTP → JWT)

1. **POST /auth/getOtp**
   - Generates 6-digit OTP
   - Stores in Redis (TTL: 2 minutes)
   - Returns success message

2. **POST /auth/verifyOtp**
   - Verifies OTP from Redis
   - Auto-registers user if not exists
   - Returns JWT token

### User Management

1. **GET /user/getAllUsers?page=1&pageSize=20**
   - Paginated user list
   - Requires JWT authentication

## Technology Stack

- **.NET 8.0**
- **PostgreSQL** (Database)
- **Redis** (OTP Cache)
- **EF Core** (ORM)
- **MediatR** (CQRS)
- **JWT** (Authentication)

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker and Docker Compose

### Running the Application

1. **Start PostgreSQL and Redis**
   ```bash
   docker-compose up -d
   ```

2. **Run Migrations**
   ```bash
   cd src/Web.Api
   dotnet ef migrations add InitialCreate --project ../Persistence
   dotnet ef database update --project ../Persistence
   ```

3. **Run the API**
   ```bash
   dotnet run --project src/Web.Api
   ```

4. **Access Swagger**
   ```
   https://localhost:5001/swagger
   ```

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dotnetbase;Username=postgres;Password=postgres",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "DotnetBaseApi",
    "Audience": "DotnetBaseClient",
    "ExpirationInMinutes": 1440
  }
}
```

## API Endpoints

### Authentication

#### Get OTP
```http
POST /auth/getOtp
Content-Type: application/json

{
  "phoneNumber": "+1234567890"
}
```

#### Verify OTP
```http
POST /auth/verifyOtp
Content-Type: application/json

{
  "phoneNumber": "+1234567890",
  "otp": "123456",
  "name": "John",
  "family": "Doe"
}
```

### Users

#### Get All Users
```http
GET /user/getAllUsers?page=1&pageSize=20
Authorization: Bearer {jwt-token}
```

## Domain Model

### User Entity
```csharp
- Id: Guid
- PhoneNumber: string
- Email: string?
- Name: string
- Family: string
- Roles: ICollection<UserRole>
- CreatedAt: DateTime
- UpdatedAt: DateTime?
```

### UserRole Enum
```csharp
- User = 0
- SuperAdmin = 1
```

## Architecture Principles

### Clean Architecture Rules

1. **Domain Layer**
   - NO external dependencies
   - Pure business entities and enums
   - NO EF, Redis, HTTP, JWT

2. **Application Layer**
   - Defines abstractions (interfaces)
   - Contains use cases (Commands/Queries)
   - Uses MediatR for CQRS
   - NO implementation of external services

3. **Infrastructure Layer**
   - Implements Application abstractions
   - JWT and Redis implementations
   - NO business logic

4. **Persistence Layer**
   - EF Core and database
   - Repository implementations
   - Entity configurations

5. **Web.Api Layer**
   - HTTP endpoints only
   - Maps requests to Commands/Queries
   - NO business logic

### Dependency Direction
```
Web.Api → Application → Domain
    ↓           ↓
Infrastructure  Persistence
```

## License

MIT
