# ECommerceApp

A containerized .NET 8 layered Web API for e-commerce order and product management, featuring Redis caching, MassTransit/RabbitMQ messaging, PostgreSQL persistence, and Elasticsearch indexing.

---

## 🏛️ Architecture Layers

| Layer                   | Description                                                                                         |
|-------------------------|-----------------------------------------------------------------------------------------------------|
| **API Layer (Api)**           | Controllers: `OrdersController`, `ProductsController`                                          |
| **Application Layer**         | Services: `OrderService`, `ProductService`; DTOs; Interfaces                                   |
| **Infrastructure Layer**      | Caching (`RedisCacheService`); Messaging (MassTransit/RabbitMQ config & Consumers); HTTP Clients (`BalanceApiClient`); Logging (`Serilog`) |
| **Persistence Layer**         | EF Core `DbContext`; Migrations; Repositories                                                    |
| **Domain Layer**              | Entities; Value Objects                                                                           |
| **Shared Layer**              | Shared Models; Common EventBus implementation; Utilities                                          |


## 🚀 Tech Stack

- **Framework**: .NET 8 Web API, C#  
- **Architecture**: Clean/CQRS-inspired layered (Application, Domain, Infrastructure, Persistence)  
- **Messaging**: MassTransit + RabbitMQ (with quorum queues)  
- **Cache**: Redis via `ICacheService` / `RedisCacheService`  
- **Database**: PostgreSQL  
- **Search**: Elasticsearch  
- **Testing**: NUnit, Moq, FluentAssertions (integration tests)  
- **Containerization**: Docker & Docker Compose  

---
### Backend
- **.NET 8.0
- **Entity Framework Core
- **Mapster** - Object mapping
- **Serilog** - Structured logging

### Database & Message Broker
- **PostgreSQL
- **RabbitMQ
- **Entity Framework Migrations** 

### Infrastructure
- **Docker & Docker Compose** - Containerization ve orchestration
- **Swagger/OpenAPI** - API documentations

### Prerequisites
- Docker Desktop (v20.10+) — Container runtime for running services locally
- .NET 8.0 SDK — Software development kit for building and running the application

### Running with Docker

1. **Clone the repository:**
```bash
git clone https://github.com/<your-username>/ECommerceApp.git
cd ECommerceApp
```

2. **Start all services:**
```bash
docker-compose up -d
```

3. **Check service status:**
```bash
docker-compose ps
```

4. **Follow logs:**
```bash
docker-compose logs -f
```
## ⚙️ Application Configuration

| Key                                         | Value                                                                          |
|---------------------------------------------|--------------------------------------------------------------------------------|
| **ConnectionStrings:Postgres**              | `Host=localhost;Port=5432;Database=ecommerce;Username=postgres;Password=postgrespw` |
| **RedisSettings:ConnectionString**          | `localhost:6379`                                                               |
| **Messaging:Transport**                     | `RabbitMq`                                                                     |
| **RabbitMq:Host**                           | `localhost`                                                                    |
| **RabbitMq:UserName**                       | `guest`                                                                        |
| **RabbitMq:Password**                       | `guest`                                                                        |
| **RabbitMq:VirtualHost**                    | `/`                                                                            |
| **RabbitMq:Port**                           | `5672`                                                                         |
| **Serilog:Using**                           | `["Serilog.Sinks.Http"]`                                                       |
| **Serilog:MinimumLevel**                    | `Information`                                                                  |
| **Serilog:WriteTo:0:Name**                  | `Console`                                                                      |
| **Serilog:WriteTo:1:Name**                  | `DurableHttpUsingFileSizeRolledBuffers`                                        |
| **Serilog:WriteTo:1:Args:requestUri**        | `http://localhost:5044`                                                        |
| **ExternalServices:BalanceApiBaseUrl**      | `https://balance-management-pi44.onrender.com`                                 |

# From Rider or Visual Studio Code:
# - Start the Api project
# - Start the Consumer project

# Or from the command line:
cd Api && dotnet run
cd ../Consumer && dotnet run
