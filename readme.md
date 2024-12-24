**NatroCase**
- This project performs domain control, and after logging in, we can add the domains we check to our favorites.
- Since the front-end is not mandatory, it was written as an API.
- PostgreSQL was used as the database.
- The patterns used are CQS, DDD, and MediatR.

**How to Run Project**
- docker run --name natro-case-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=123456 -e POSTGRES_DB=natro -p 5432:5432 -d postgres
- run project on your ide


**To create a new write migration**
- cd NatroCase.Infrastructure
- dotnet ef migrations add -o ../NatroCase.Infrastructure/Persistence/Migrations/ -c NatroCaseDbContext -s ../NatroCase.Api/NatroCase.Api.csproj [message]