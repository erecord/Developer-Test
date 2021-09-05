#!/bin/bash

docker exec -it -w /source/src store_backend dotnet ef migrations add InitialMigration 
docker exec -it -w /source/src store_backend dotnet ef database update