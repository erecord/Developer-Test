# 288 Group Developer Test

## If I Had More Time

### Frontend Service

- Add a frontend
  - A frontend service can be added to the docker environment and consume the backend API
  - Instances of the frontend service could be scaled up or down dynamically easily depending on user traffic due to the microservice architecture.
  - I have built frontends in vanilla HTML & CSS, React, and Vue, but I would pick Blazor because it fits into the dotnet ecosystem, supports component based views (like React and Vue), and has an interoperability layer for JS. Additionally, PWA is supported out the box which means one mobile responsive codebase can target Desktop, Tablet, and Mobile (across Windows, Mac, Linux, iOS and Android), and bypasses Google Play or the App Store which would otherwise reduce transaction income streams by 30%.

### DevOps

- Set up a production docker environment
  - Use Nginx as a reverse proxy to act as the sole public port and hide the other services internally
  - Use Nginx and LetsEncrypt to obtain an SSL certificate
  - Host the production environment on a linux server

### Backend

- Add unit tests for the remaining controllers
  - Look into refactoring the unit tests for the commands and services to use mocking instead of using the InMemory provider
- Add authorisation via Claims and Middleware to protect resources and manage data modification
- Add translation system instead of hardcoding english

#### Entities

- Check if the discount has expired when it's requested to be used
- Add constraints so that a user can only have one basket
- Add support for product quantities within a basket (can only have one of each at the moment)
- Expand the Product schema to include URL for image, and quantity left in stock
