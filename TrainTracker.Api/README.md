TrainTracker API 🚆

Backend API for a real-time train departure tracking application built with ASP.NET Core.

This API integrates with the public iRail API to provide live departure data, train stop information, delay handling, and station search functionality for the frontend application.

✨ Features
Real-time train departure board
Station search with autocomplete
Dynamic train stop tracking
Train delay and cancellation status handling
Upcoming stops calculation
In-memory caching for improved performance
RESTful API architecture
External railway API integration (iRail)
🛠 Tech Stack
ASP.NET Core Web API
C#
MemoryCache
HttpClient
REST API
JSON Serialization
DTO Mapping Layer
📡 API Endpoints
Get live departures for a station
GET /api/liveboard/{station}

Example:

GET /api/liveboard/Gent-Sint-Pieters

Returns:

Upcoming departures
Delay information
Platform details
Train status
Upcoming stops
Search stations
GET /api/stations?query=gen

Returns matching Belgian train stations based on user input.

⚡ Performance Optimization

The application uses in-memory caching to reduce external API calls and improve response times.

Implemented caching strategies:

Liveboard caching
Vehicle/stops caching
Sliding expiration
Absolute expiration
🧠 Architecture

The backend follows a service-based architecture:

Controllers → Services → Mapping Layer → External API
Main Components
Controllers

Handle HTTP requests and responses.

Services

Contain business logic, API integration, caching, and train status calculations.

Mapping Layer

Transforms external API responses into optimized DTOs for frontend consumption.

External API

The application consumes real-time railway data from the iRail public API.

🌍 External API

This project uses the public iRail API:

iRail API

🚀 Run Locally

Clone the repository:

git clone https://github.com/adelalrafiq/TrainTracker.git

Navigate to the project folder:

cd TrainTracker.Api

Run the application:

dotnet run

Default local URL:

https://localhost:5000
🔗 Related Projects
Frontend Repository

https://github.com/adelalrafiq/train-tracker-ui.git

Live Demo

https://train-tracker-ui-fdfc.vercel.app/liveboard

## ⚠️ Important

The backend API is deployed on Render's free hosting plan.  
If the application has been inactive for some time, the first request may be slower due to cold start initialization.

👨‍💻 Author

**Adela Alrafiq** - Full Stack Developer
GitHub: https://github.com/adelalrafiq