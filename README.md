# Disaster Prediction and Alert System API

This project is a .NET-based API for predicting potential disaster risks and sending alerts to affected communities. It utilizes real-time environmental data to assess risks for various disaster types such as floods, earthquakes, and wildfires.

## Project Structure

The project follows Clean Architecture principles and is divided into four main layers:

- `Domain`: Contains the core entities and business models.
- `Application`: Contains business logic, interfaces for repositories and services.
- `Infrastructure`: Manages data access, database connections, and external service integrations.
- `API`: Contains API controllers and handles HTTP requests/respons

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
   
# Input Data
## Regions
- **Region ID**: Unique identifier for each region.
- **Location Coordinates**: Latitude and longitude of the region.
- **Disaster Types**: List of disaster types to monitor (e.g., flood, wildfire, earthquake).

## Alert Settings
- **Region ID**: Identifier for the region.
- **Disaster Type**: Type of disaster (must match one monitored by the region).
- **Threshold Score**: Risk score threshold that triggers an alert for this disaster type.

# Data Sources and Risk Calculation

- External data is fetched from APIs such as OpenWeather or Earthquake data sources (like USGS) to obtain real-time data, including rainfall levels, seismic activity, and temperature.
- Risk calculation is based on the following criteria:
  - **Flood Risk**: Based on rainfall data in mm, e.g., rainfall over 50mm has a high risk.
  - **Earthquake Risk**: Based on seismic activity data, e.g., a magnitude over 5.0 has a high risk.
  - **Wildfire Risk**: Based on temperature and humidity data, e.g., high temperatures with low humidity increase the risk.
- Calculated risk scores are compared against configured thresholds to determine if an alert should be triggered.

# Output

## Disaster Risk Report
For each region, the report contains:
- Region ID
- Disaster Type
- Risk Score
- Risk Level: Low, Medium, or High based on the risk score.
- Alert Triggered: True/False, indicating whether an alert has been triggered.

## Alert Data
When an alert is sent, it includes:
- Region ID
- Disaster Type
- Risk Level
- Alert Message: Detailed message including the reason for the alert.
- Timestamp: Time of alert generation.

# Special Features

## Data Caching with Redis
- Environmental data and calculated risk scores are stored in Redis with a 15-minute expiration.
- This caching mechanism avoids redundant API calls and improves performance.

## Azure Deployment
- The API is deployed on Azure.
- Live link: [Insert Azure deployment URL here]

# Error Handling

The API handles various error scenarios, including:
- Failed external API calls
- Missing data from external sources
- Regions with no available data

Appropriate error messages and HTTP status codes are returned in these cases.

# Messaging API Integration
Alerts are sent via a messaging API (e.g., Twilio, SendGrid) to notify people in high-risk regions. This ensures timely communication of potential disasters to affected communities.

# Logging

The system implements comprehensive logging to track:
- API usage
- Alerts generated
- External API calls
- Error occurrences

This logging facilitates auditing, debugging, and system performance monitoring.

# Performance Considerations

- Redis caching is implemented to minimize redundant external API calls and improve response times.
- The system is designed to handle real-time data processing efficiently, ensuring quick risk assessments and alert generation.
- [Add any other performance optimizations implemented in the system]

# Add Region
Allows users to add regions with specific location coordinates and types of disasters they want to monitor.

- **URL**: `/api/regions`
- **Method**: POST
- **Request Body**:
  ```json
  {
    "regionID": "R1",
    "latitude": 34.0522,
    "longitude": -118.2437,
    "disasterTypes": ["Flood", "Earthquake", "Wildfire"]
  }
  ```
- **Response**: 201 Created with the created region data

# Configure Alert Settings
Allows users to configure alert settings for each region, including thresholds for disaster risk scores.

- **URL**: `/api/alert-settings`
- **Method**: POST
- **Request Body**:
  ```json
  {
    "regionID": "R1",
    "disasterType": "Flood",
    "thresholdScore": 0.7
  }
  ```
- **Response**: 201 Created with the created alert setting data

# Get Disaster Risks

Triggers the disaster risk assessment, fetching real-time environmental data for all regions and calculating risk scores.

- **URL**: `/api/disaster-risks`
- **Method**: GET
- **Response**: 200 OK with an array of disaster risks
  ```json
  [
    {
      "regionID": "R1",
      "disasterType": "Flood",
      "riskScore": 0.8,
      "riskLevel": "High",
      "alertTriggered": true
    }
  ]
  ```
  
# Send Alert
Sends an alert for regions identified as high-risk and stores the alert in the database.

- **URL**: `/api/alerts/send`
- **Method**: POST
- **Request Body**:
  ```json
  {
    "regionID": "R1",
    "disasterType": "Flood",
    "riskLevel": "High",
    "alertMessage": "Heavy rainfall detected. Flood risk is high."
  }
- **Response**: 201 Created with the sent alert data

# Get Recent Alerts

Returns a list of recent alerts for each region, stored in a database.

- **URL**: `/api/alerts`
- **Method**: GET
- **Response**: 200 OK with an array of recent alerts
  ```json
  [
    {
      "regionID": "R1",
      "disasterType": "Flood",
      "riskLevel": "High",
      "alertMessage": "Heavy rainfall detected. Flood risk is high.",
      "timestamp": "2023-05-20T14:30:00Z"
    }
  ]
  ```

