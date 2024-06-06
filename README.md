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

## License
This project is licensed under the MIT License.
