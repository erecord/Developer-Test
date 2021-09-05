# 288 Group Developer Test

## How to start the application

Prerequisite: Install Docker on your operating system, either on a Linux OS or using WSL2 on Windows.

1. In a terminal, navigate to the repository's root folder.
2. Run `./dev.sh up -d`
3. Wait for the script to complete
4. The backend API is now running on `localhost:5000/api`
5. With the docker services still running, in the root folder, run `./initial-bootstrap.sh` to perform the initial database migration
