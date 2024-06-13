# ApbdTask11

# Animal Clinic API

## Introduction
This project is an API for managing an animal clinic, allowing for operations on animals, employees, and visits.

## Setup
1. Clone the repository.
2. Navigate to the project directory.
3. Ensure you have Docker installed and running.
4. Update the `appsettings.json` file with your database configuration.

## Running the Application
1. Build and run the application using your IDE or the command line:
    ```sh
    dotnet build
    dotnet run
    ```
2. Use tools like Postman or Swagger UI to test the endpoints.

## Endpoints
### Animals
- `GET /api/animals` - Get all animals.
- `GET /api/animals/{id}` - Get a specific animal by ID.
- `POST /api/animals` - Create a new animal.
- `PUT /api/animals/{id}` - Update an existing animal.
- `DELETE /api/animals/{id}` - Delete an animal.

### Visits
- `GET /api/visits` - Get all visits.
- `GET /api/visits/{id}` - Get a specific visit by ID.
- `POST /api/visits` - Create a new visit.
- `PUT /api/visits/{id}` - Update an existing visit.
- `DELETE /api/visits/{id}` - Delete a visit.

##Task 12
Endpoints
Authentication
-POST /api/auth/register - Register a new user by providing a username and password.
-POST /api/auth/login - Login with a username and password, return an access token and a refresh token.
-POST /api/auth/refresh - Obtain a new access token based on the refresh token.
Animals
-GET /api/animals - Get all animals. (Requires authentication)
-GET /api/animals/{id} - Get a specific animal by ID. (Requires authentication)
-POST /api/animals - Create a new animal. (Requires authentication)
-PUT /api/animals/{id} - Update an existing animal. (Requires authentication)
-DELETE /api/animals/{id} - Delete an animal. (Requires admin role)
Visits
-GET /api/visits - Get all visits. (Requires authentication)
-GET /api/visits/{id} - Get a specific visit by ID. (Requires authentication)
-POST /api/visits - Create a new visit. (Requires authentication)
-PUT /api/visits/{id} - Update an existing visit. (Requires authentication)
-DELETE /api/visits/{id} - Delete a visit. (Requires admin role)
Middleware
ExceptionHandlingMiddleware - Handles exceptions on a global level, ensuring consistent error responses.
Notes
In case of client errors, the API specifies why the client receives the error.
Data validation is crucial for all endpoints.
The principles of REST, SOLID, DRY, YAGNI, and KISS are followed.
The code is divided into separate layers, avoiding the Repository pattern unless proven necessary.
Endpoints use CancellationToken for async operations.


## License
This project is licensed under the MIT License.
