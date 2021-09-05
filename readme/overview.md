# 288 Group Developer Test

## Overview

### Architecture

A microservice architecture has been used for this application, rather than a monolithic layer application where each concern would be housed inside a main project.

- Docker and Docker-Compose have been used to take the concern of managing dependencies away from the developer. Additionally, each service can be scaled independently, and dynamically based on load-balancers, when using the microservice pattern - which lends itself well to scaling up or down in relation to user demand.

#### Modules

5 modules are available in the `/backend/src/` folder and are each concerned with managing a database entity.

- Auth
- Basket
- Product
- BasketProduct
- Discount

Rather than splitting the code into `Controllers` or `Services` folders, I thought it would be better to group software items into a parent folder to increase the cohesion of those items and make it easier for future maintainers to navigate and build up a mental model.

A `RegisterXModule.cs` can be found in the folders to handle registering interfaces and classes with the DI container (Dependency Inversion Principle). In this way, the modules are isolated concerns and can be picked up and put down into another project.

---

### Design Patterns

#### Command Pattern

The command pattern has been used to implement the Single Responsibility principle, reduce controller code, and isolate the functionality tested within unit tests.

- The Basket module further splits commands between Actions (modifies the state) and Queries (queries the state without modifying it). When aiming for clean architecture, these distinctions help to create more deterministic code and also adopt functional programming principles.

#### Repository Pattern

The BaseRepository class can be found in `/backend/src/Common/` and serves as a layer of indirection over the EntityFramework layer to implement CRUD functionality. Each module inherits from this class and specifies its own entity type via Generics. By virtue of the Dependency Inversion principle, the BaseRepository could be swapped for an implementation that writes to disk in JSON (for example) without requiring modification to the controller logic.

#### Factory Pattern

The factory pattern has been used in the `/backend/tests` folder to reduce the code require to produce test data. Also extensions have been used as factories, such as `BasketExtensions.cs` where a productId can be supplied to produce a BasketProductDTO for the BasketProduct link table.

#### Facade Pattern

The basket controller is the most complicated controller and so it depends on several commands rather than defining the logic itself, which wouldn't be reusable in other contexts. A `BasketControllerFacade` has exists to make it more convenient and isolated to add or remove the controller's dependencies rather than clogging up it's constructor.

---

### Unit Testing

Unit tests can be found in the `/backend/tests/ folder.

- Every command used within the application so far are covered by unit tests.
- Only had time to write unit tests for one controller, but showed example using Moq to mock interfaces with it.

#### Notes

I initially struggled to setup Moq to mock the Entity Framework layer because I couldn't satisfy the API of an `IQueryableIncludeable`. And from researching, mocking Entity Framework seemed like a convoluted process where the functions on the DbContext would be reimplemented to add, remove, or query items from an internal List. I then discovered EntityFramework offered an InMemory provider which essentially provided the same thing for free.

The Unit Tests for the commands and services have been written using the InMemory provider. I'm not happy with the `InitAndGetDbContext()` areas within these tests because all the dependencies are 'newed' up instead of mocking their interface. However, on Sunday when the majority of the work was done, I had another attempt at using Moq and was successful. An example of writing Unit Tests with mocked interfaces can be found in `/backend/tests/Modules/BasketControllerTests.cs`. This is also the only example of a controller unit test, I didn't have time to write the tests for the other controllers but they mostly contain boilerplate logic.

I am happy with the tests themselves and feel each action, query, and service has an appropriate test suite to catch early insights into regression bugs.

---

### Security

- The application uses JWT Token authentication to lock down routes, but any authenticated user can perform actions on any resource. That is to say, authorisation has not been implemented and I would use the Identity Claims system to define roles and also create middleware, such as `IsOwner` or `IsAdmin` to prevent modification of resources that a user does not own or does not have access to.
- Connection Strings and Keys are defined in a `dev.env` file which is loaded into the ASP context with Docker-Compose's `env_file` setting. So no sensitive data is hardcoded into the code. This allows the `.env` file to be swapped out with an offline file in a production environment to keep access secure.
