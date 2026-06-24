# daily-cosmic-observer
DailyCosmicObserver (DCO): an upcoming full-stack web application designed to act as a digital astronomy journal that automatically fetches data from the **NASA Astronomy Picture of the Day (APOD) API**, allowing users to browse the APOD events, write personal logs, and organize those logs using custom tags.

## Status
Work in progress (Phase 1)

## Project Roadmap
The development of the DCO project is divided into two distinct phases:
- **Phase 1: RESTful Backend API:** building a robust `CosmicObserver API` with N-Tier architecture using .NET 10 to handle external API integrations, data management and business logic,
- **Phase 2: Frontend Web Client:** developing the UI to consume the `CosmicObserver API`, visualize the data (including cosmic events images), and provide a journal-like experience for the user.

## Backend Tech Stack (Phase 1)
- **Framework:** .NET 10 (ASP.NET Core Web API)
- **Language:** C# 14
- **Database:** SQLite
- **ORM:** Entity Framework Core (Code-First)
- **Architecture:** Clean N-Layer REST API
- **Design Tools:** dbdiagram.io (Database Schema Architecture)

## Progress & Milestones
DCO's Phase 1 is under development. Here is a list of what has been accomplished so far:

### 1. Database Schema
The database schema was designed using the `dbdiagram.io`. The schema consists of:
- a many-to-one relationship between `cosmic_logs` and `cosmic_events`,
- a many-to-many relationship between `cosmic_logs` and `cosmic_tags` (utilizing `log_tags` join table).

### 2. The Data Layer (EF Core)
- **Domain Models:** designed and implemented pure data containers (`CosmicEvent`, `CosmicLog`, `CosmicTag`) with strict `required` modifiers for null-safety.
- **Database Schema:** translated the initial `dbdiagram` schema into a local SQLite database using EF Core Code-First migrations.
- **Relational Mapping:** configured one-to-many and many-to-many relationships (including auto-generated join tables for `Logs` and `Tags`).

### 3. External API Integration (NASA APOD)
- **Secure HTTP Pipeline:** wired up a typed `HttpClient` utilizing Dependency Injection to communicate with NASA's servers.
- **Data Decoupling (DTOs):** implemented the `NasaApodResponse` Data Transfer Object to safely deserialize NASA's JSON into C# properties.
- **API Security:** integrated the .NET Secret Manager (`NasaApiOptions`) to ensure private API keys remain securely out of source control.
- **Routing:** established the initial `ApodController` exposing a `[HttpGet]` endpoint (`api/Apod`) to serve formatted daily picture data.

### 4. Data Pipeline (APOD to EF Core)
- **Fetching Pipelines:** Updated the data layer to support retrieving individual daily records or bulk collections across specific date ranges (`IEnumerable<NasaApodResponse>`).
- **Automated Database Mapping:** Configured the pipeline to automatically translate and map incoming NASA DTOs into permanent `CosmicEvent` database entities.
- **Advanced Data Integrity:** Implemented bulk duplicate-checking mechanism. By utilizing Entity Framework queries and **LINQ**, the system safely filters and saves incoming array responses.
- **Routing:** Updated the `ApodController` to handle both standard `[HttpGet]` requests for specific dates and a dedicated `[HttpGet("range")]` endpoint for bulk data synchronization.

### 5. Data Engine & Endpoints (`CosmicTags`)
- Implemented `CreateTag` and `TagResponse` DTOs for `CosmicTags` HTTP payloads.
- Built `CosmicTagService` to handle database interactions, utilizing Entity Framework's **Change Tracker** for optimized updates.
- Established `TagController` with a complete set of CRUD endpoints (`GET`, `POST`, `PUT`, `DELETE`) and HTTP status code handling.
- Added validation logic to prevent duplicate tag creation and naming collisions.

### 6. Data Engine & Endpoints (`CosmicLogs`)
- Implemented `CreateLog` and `LogResponse` DTOs for `CosmicLogs` payloads.
- Built `CosmicLogService` to handle database interactions, including logic to synchronize **Many-to-Many** tag relationship without duplicates.
- Established `LogController` with all CRUD endpoints (`GET`, `POST`, `PUT`, `DELETE`) including `[FromQuery]` endpoints for multi-category and tag filtering.
- Created `LogMappingExtensions` static class to adhere to the **DRY** principle, translating DTO mappings directly into SQL queries.

### 7. Data Engine & Endpoints (`CosmicEvents`)
- Implemented `EventResponse` DTO for `CosmicEvents` payloads.
- Updated `CosmicEventService` with asynchronous tasks for getting and deleting events.
- Established `EventController` with CRUD endpoints (`GET`, `DELETE`) including endpoints for specific date and date range event filtering.
- Created `EventMappingExtensions` static class for **Cosmic Events** data mapping:
	- Implemented `ToEventResponseExpression` static expression for Entity -> DTO mapping.
	- Implemented `ToEventEntity` static extension method for DTO -> Entity mapping.

### 8. Centralization of Cosmic Events Routing
- Updated `EventController` to act as the single entry point for frontend `CosmicEvent` requests, injecting `INasaApodService` to coordinate directly between the local database and the external **APOD API**.
- Implemented routing logic that checks the database first: if a specific date or fragments of a date range are missing, the controller automatically triggers a background fetch from NASA and saves the missing entries.
- Updated `NasaApodResponse` DTO to prevent deserialization crashes when NASA returns embedded video links (`.mp4`) instead of a standard image URL.

### 9. Cache-Aside Pattern Integration (`CosmicEvents`)
- **Memory Allocation:** Registered .NET's built-in `IMemoryCache` to reduce database load and external API calls.
- **Service Integration:** Implemented the Cache-Aside pattern across `CosmicEventService` **read tasks** (`GetEventByIdAsync`, `GetEventByDateAsync`, and `GetEventsRangeAsync`).
- **Memory Protection:** Configured `SlidingExpiration` (30 minutes) and `AbsoluteExpirationRelativeToNow` (24 hours) policies, while implementing conditional **Zero-Time** to prevent caching incomplete date ranges or null database queries.

### 10. Tag Data Sanitization Pipeline (`CosmicTags`)
- Created `StringSanitizationExtensions` class for string manipulation.
- Implemented `ToSanitizedString` static method with culture-sensitive lowercasing, whitespace trimming, and LINQ pipeline to filter out special characters.
- Integrated the sanitization inside the `CreateTagAsync` and `UpdateTagAsync`.


## TODOs
- Implement custom result pattern and global exception handling.
- XML documentation.
- Refactor `CosmicEvent.ImageUrl` property to `MediaUrl` (including EF Core Migration).
- Fully test the backend pipeline using Swagger / `http` files before initiating Phase 2.