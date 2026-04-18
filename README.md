# WEX Assessment

Project - Wex Purchase Transactions 
Store and Retrieve Purchase Transactions Infos

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/) 

# Architecture  Tools

- Clean Architecture
- DDD
- TDD

# Tools
- Mediatr
- EntityFramework
- Redis
- Postgresql
- Polly
- Xunit
- NSubstitue
- Serilog
- Swagger

---

## locally

1. Clone:

```bash
git clone 
cd src/
```
2.
```bash
dotnet restore
dotnet build
```
3.
```bash
dotnet run --project Wex.TransactionManager.Api
```

## docker
1.
```bash
docker-compose build
```
2.
```bash
docker-compose up
```


## create transactions
```bash
curl -X 'POST' \
  'https://localhost:7097/api/Transaction' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "amount": 0,
  "description": "string",
  "transactionDate": "2026-04-18T23:42:51.542Z"
}'
```

## retrieve
```bash
curl -X 'GET' \
  'https://localhost:7097/api/Transaction/d32791fd-b808-432c-9a0d-37c5e538dedc?currency=Brazil-Real' \
  -H 'accept: text/plain'
```


Project Structure

Wex.TransactionManager.Api — Business API

Wex.TransactionManager.Application — Application services

Wex.TransactionManager.Domain — Entities and Business rules

Wex.TransactionManager.Infrastructure — Repository implementations, Database access, and External clients

Wex.TransactionManagement.E2ETests — Automated Testing
Wex.TransactionManagement.IntegrationTests - Integration Testing
Wex.TransactionManagement.UnitTests - Unit Testing