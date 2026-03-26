**How to Run ?**

.Net version used is .NET 8.
Open in Visual Studio 2022 or later.
Rebuild all projects.
Debug start VehicleTelemetry.API for web server to start.
Point Browser to https://localhost:7077/swagger/index.html to test APIs through swagger GUI.

**API**
1.  **Insert telemetry data**
 ```bash
	    curl -X 'POST' \
	      'https://localhost:7077/api/v1/Telemetry' \
	      -H 'accept: */*' \
	      -H 'Content-Type: application/json' \
	      -d '{
			      "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
			      "engineRpm": 10000,
			      "fuelLevelPercentage": 100,
			      "latitude": 90,
			      "longitude": 180
		       }'
```    
Server response: 200 OK
Response headers:
 ```bash
	 api-supported-versions: 1.0 
	 content-length: 0  
	 date: Thu,26 Mar 2026 13:10:41 GMT 
	 server: Kestrel
```
2. **Read last telemetry data**
```bash
curl -X 'GET' \
  'https://localhost:7077/api/v1/telemetry/3fa85f64-5717-4562-b3fc-2c963f66afa6/latest' \
  -H 'accept: */*'
```
Server response: 200 OK
Response headers:
```bash
	api-supported-versions: 1.0 
	content-type: application/json;
	charset=utf-8  date: Thu,26 Mar 2026 19:36:32 GMT 
	server: Kestre
```
Response body:
```json
{
  "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "timestamp": "2026-03-26T20:26:56.8831013+01:00",
  "engineRpm": 10000,
  "fuelLevelPercentage": 100,
  "latitude": 90,
  "longitude": 180
}
```
3.  **Background service**
Web server console will print every second last 5 telemetry data.
```bash
info: VehicleTelemetry.Workers.CloudSend[0]
      Sending data to cloud for Device 3fa85f64-5717-4562-b3fc-2c963f66afa6: Timestamp=03/26/2026 20:26:56 +01:00, EngineRpm=10000, FuelLevelPercentage=100, Latitude=90.0, Longitude=180.0
info: VehicleTelemetry.Workers.CloudSend[0]
      Sending data to cloud for Device 3fa85f64-5717-4562-b3fc-2c963f66afa6: Timestamp=03/26/2026 20:25:50 +01:00, EngineRpm=10000, FuelLevelPercentage=100, Latitude=90.0, Longitude=180.0
info: VehicleTelemetry.Workers.CloudSend[0]
      Sending data to cloud for Device 3fa85f64-5717-4562-b3fc-2c963f66afa6: Timestamp=03/26/2026 18:13:41 +01:00, EngineRpm=10000, FuelLevelPercentage=100, Latitude=90.0, Longitude=180.0
info: VehicleTelemetry.Workers.CloudSend[0]
      Sending data to cloud for Device 7932fbc2-f96c-482b-9727-f7faaf9af3ca: Timestamp=03/26/2026 17:17:50 +01:00, EngineRpm=10000, FuelLevelPercentage=100, Latitude=90.0, Longitude=180.0
info: VehicleTelemetry.Workers.CloudSend[0]
      Sending data to cloud for Device 3fa85f64-5717-4562-b3fc-2c963f66afa6: Timestamp=03/26/2026 14:10:41 +01:00, EngineRpm=9002, FuelLevelPercentage=100, Latitude=90.0, Longitude=180.0
info: VehicleTelemetry.Workers.CloudSend[0]
```

4. **Run Unit Tests for Telemetry data**
On Visual Studio menu click Test->Run All tests

**Folder structure**

```
    VehicleTelemetry.Api/
    ├── Controllers/                      # API endpoints 
    ├── Data/                             # DbContext and SQLite database
    ├── DTOs/                             # Data Transfer Objects for request/response models
    ├── Migrations/                       # EF Core migrations
    ├── Interfaces/                       # Interfaces for Repositories and Services
    ├── Repositories/                     # Data Acess layer through Repository pattern
    ├── Services/                         # Service layer for business logic
    ├── Model/                            # TelemetryData model
    ├── Workers/                          # Background service logic
    ├── Program.cs                        # App entry point & configuration
    ├── SlugifyParameterTransformer.cs    # Url reformatting
    └── appsettings.json                  # Configuration settings

    VehicleTelemetry.Tests/               # NUnit tests
    └── TelemetryServiceTests.cs          # Tests for all service methods with mock data

```

**Architectural Decisions**

Applied priciples of Clean code & separations of concerns with multiple layers. Used DI when applicable (for DBContext, Services, Repositories, IHostedService). 
Data logging with Microsoft.Extensions.Logging. SQLite database is build around model first principle from TelemetryData model. Extensivly using async/await.
Data flow:

```
	API entry point (Controllers)->Services->Repositories->DBContext->SQLite database
```

Data is orderd by CreatedAt DateTime atribut which is added in TelemetryData model. SQLite database has problem with ordering by DateTimeOffset so new type is added 
to ensure data is ordered by EF Core/SQL. 

