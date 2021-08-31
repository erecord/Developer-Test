# Day 1

Set up docker compose environment for dotnet and mssql services.

- Played around with `dotnet new` and researched `webapp` and `webapi` type projects.
- Tried to scaffold Identity into `webapi` project but it seemed that implementing a JWT flow was easier because Identity wanted to use Views and I wanted the backend service to be a pure RESTful API and use a frontend service for the presentation layer.

# Day 2

- Created JWTService and configured JWT Bearer authorisation middleware
- Created StoreDbContext and AuthController to Register and Authenticate users
- Added Product and Basket models and controller
  - Initial thoughts were to have the Basket concern as a cookie on client machine, but I thought it may be harder to write tests and it splits the Shopping Cart concern across two services (backend/frontend).
  - Many Baskets can have Many Products so a link table is required to resolve Many to Many relationships. Entity Framework handles updating the database schema nicely, but I'm having difficulties adding Products to a Basket via a RESTful client. Additionally, there appears a circular reference when viewing a Basket's data (Basket has Products, and the Products have Baskets - had to use Newtonsoft for ReferenceLoopHandling)
  - I really wanted to just store a list of integers on the Basket model which would each be Foreign Keys to the ProductIds but this isn't possible unless I do a hack of encoding/decoding the list into a delineated string.
