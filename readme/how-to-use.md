# 288 Group Developer Test

## Use postman to interact with the backend

1. Install [Postman](https://www.postman.com/downloads)
2. From the root folder, import `ASP Store Backend.postman_collection.json` into Postman
3. The routes require authentication, so create a user and sign in using the 'Auth' folder.
   - The JWT Token will be automatically stored and sent in your subsequent request headers

## Set up data

1. Use Postman to create 2 products and a discount code
2. Use Postman to create a basket

## Use the API

1. Add products to the basket using the `Update Basket` route

- The `"productIds": []` is an array of foreign keys for the Product entities. Add or remove ints from the array and submit the request to add or remove products from the basket.

2. Use the `Get Basket Total Cost` route to retrieve the basket's total cost
3. Use the `Set Discount Code` route to assign a discount to the basket
4. Use the `Get Basket Total After Discount` route to retrieve the basket's total cost after discount

## Notes

- Only the `Get Basket` route (/api/basket/1) will be populated with the products and discount data to reduce the payload of the `Get All` baskets. This pattern would be used for other entities if more development took place.
- If for some reason the API stops responding, in the root folder, run `./restart_backend.sh` to attempt to resolve the issue.
