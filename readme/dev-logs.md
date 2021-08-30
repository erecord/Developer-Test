# Day 1
Set up docker compose environment for dotnet and mssql services.
- Played around with `dotnet new` and researched `webapp` and `webapi` type projects.
- Tried to scaffold Identity into `webapi` project but it seemed that implementing JWT flow was easier

# Day 2
- Created JWTService and configured JWT Bearer authorisation middleware
- Created StoreDbContext and AuthController to Register and Authenticate users 