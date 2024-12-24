**NatroCase**

Natro Test Case

**How to Run Project**
- docker run --name natro-case-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=123456 -e POSTGRES_DB=natro -p 5432:5432 -d postgres
- run project on your ide


**To create a new write migration**
- cd NatroCase.Infrastructure
- dotnet ef migrations add -o ../NatroCase.Infrastructure/Persistence/Migrations/ -c NatroCaseDbContext -s ../NatroCase.Api/NatroCase.Api.csproj [message]