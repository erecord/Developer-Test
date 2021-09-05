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

# Day 3

- Replaced Entity Framework auto generated link-table with an explicit link-table and added this to DbContext.
- Created UpdateBasketDTO which contains a Basket model and a `List<Int>`.
  - Added or Removed entries from the BasketProduct link-table based on the ints in the `List<Int>`.
  - This allows me to supply a list of Foreign Keys (of Products) and the controller has logic to add or remove link-table entries appropriately - really happy with this :D.

# Day 4 (Evening)

- Learnt more about xUnit and Moq
- Created generic BaseRepository which inherits from a new IRepository to eventually make unit testing and mocking possible.
  - Now all the CRUD logic is the BaseRepository and I just inherit from it and supply the DbContext and the DbSet<T>.

# Day 5 (Evening)

- Added Repository for other entities and refactored their controllers to use the repository instead of depending on DbContext directly.
- Added StoreBackend.Tests.csproj and researched xUint, Moq, FakeItEasy etc.

# Day 6 (Evening)

- Spent awhile trying to mock the DbContext layer. The solutions I found online all seemed to mock the CRUD operations and then add the entities to an internal List instead of talking to a database. However, intellisense complained about a return type of TEntity instead of the expected EntityEntry<TEntity> which I couldn't figure out.
- I then found the InMemory Database Provider for EntityFramework which seemingly achieves the same thing as trying to mock the DbContext and add it to a list in the background. The concern is that the tests only prove the InMemory Database Provider functions correctly and show nothing about the production environment: SqlServer Database Provider. But I trust that EntityFramework behaves mostly equivalent between providers (when ignoring InMemory vs. Relational Database differences).
- Created tests for AddProductIdsToBasketCommand and RemoveProductIdsFromBasketCommand

# Day 7

- Added tests for
  - QueryProductIdsInBasketCommand
  - QueryProductsInBasketCommand
  - QueryTotalCostOfBasketCommand
  - CalculateDiscountedPriceCommand
  - BasketDiscountService
- Added Test Entity Factories to cut down repeated code when setting up a test's DB InMemory context

# Day 8

- Added SetDiscountCode, GetBasketTotalCost, and GetBasketTotalCostAfterDiscount routes to the BasketController
