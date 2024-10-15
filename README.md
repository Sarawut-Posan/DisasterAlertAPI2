# Disaster Prediction and Alert System API

This project is a .NET-based API for predicting potential disaster risks and sending alerts to affected communities. It utilizes real-time environmental data to assess risks for various disaster types such as floods, earthquakes, and wildfires.

## Project Structure

The project follows Clean Architecture principles and is divided into four main layers:

- `DisasterAlertAPI.Domain`: Contains the core entities and business models.
- `DisasterAlertAPI.Application`: Contains business logic, interfaces for repositories and services.
- `DisasterAlertAPI.Infrastructure`: Manages data access, database connections, and external service integrations.
- `DisasterAlertAPI.API`: Contains API controllers and handles HTTP requests/responses.

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL database
- OpenWeather API key (for weather data)

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/DisasterAlertAPI.git
   cd DisasterAlertAPI
   ```

2. Set up the database connection string in `src/DisasterAlertAPI.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=disasteralert;Username=yourusername;Password=yourpassword"
   }
   ```

3. Set up the OpenWeather API key in `src/DisasterAlertAPI.API/appsettings.json`:
   ```json
   "OpenWeatherApiKey": "your_api_key_here"
   ```

4. Run database migrations:
   ```
   cd src/DisasterAlertAPI.API
   dotnet ef database update
   ```

5. Run the application:
   ```
   dotnet run
   ```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

## API Endpoints

- `POST /api/regions`: Add a new region to monitor
- `POST /api/alert-settings`: Configure alert settings for a region
- `GET /api/disaster-risks`: Get current disaster risks for all monitored regions
- `POST /api/alerts/send`: Send an alert for a high-risk region
- `GET /api/alerts`: Get recent alerts

## Running Tests

To run the unit tests:

```
dotnet test
```

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.