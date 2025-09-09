# Decidr App

---
Decidr is the Tinder for decisions. Time to see what plans you and your loved ones vote for anonymously.

---

## Introduction
This app is designed to help make deciding social plans easier.  
After spending countless hours not knowing what to pick on Netflix, what restaurant to go to,
what events to attend, I wanted to make an app to help me, my partner, and friends decide. Enter Decidr.

---

## Running Locally

Instructions on how to run this app locally.

*Please note that this app is in the beginning stages of development so it's not fully polished and
there are a lot of TODOs.*

### Requirements
Before running the app locally, make sure you have the following installed:

- **.NET SDK**: `9.0.203` or higher  
- **Node.js**: `v22.12.0` or higher  
- **npm**: `10.9.0` or higher  
- **Angular CLI**: install globally with  
  ```bash
  npm install -g @angular/cli
  ```
- **Docker Desktop**: required for running Postgres locally  
- **Postgres**: runs inside Docker (see setup steps below)
- **pgAdmin**: GUI to view the database schema and query the database

---

### Steps to Run

1. **Clone the repository**

1. **Start Postgres in Docker**
   Run the following command in the root folder to start a local Postgres container:
   ```bash
   docker-compose up -d
   ```
   - Postgres will now be available at `localhost:5432`.
   - If you'd like to view data and query the database, open up pgAdmin.
     - Right click on servers > Register > Server
     - Put in a name for this server (i.e. 'local').
     - Click on the 'Connection' tab.
       - Hostname: `localhost`
       - Port: `5432`
       - Username: `postgres`
       - Password: `postgres`
     - Save. You should now be connected to the database.

1. **Backend Setup (.NET)**
   ```bash
   dotnet restore
   dotnet build
   ```
   - Run migrations:
     ```bash
     cd Decidr.Infrastructure
     dotnet ef database update
     cd ../
     ```
   - Start the backend API (from root folder):
     ```bash
     cd Decidr.Api
     dotnet run --launch-profile https
     ```
   - The backend should now be running at [https://localhost:5001](https://localhost:5001).
   - Navigate to app's Swagger doc to view endpoints. Click [here](https://localhost:5001/swagger/index.html).
     - Note, all APIs except /login and /register require an auth bearer token. To test out authorized endpoints...
       - You will need to successfully call the /login endpoint with valid credentials.
       - Copy the token from the response.
       - In the top right corner of the Swagger doc, click 'Authorize', paste your token, and click 'Authorize'. You're now logged in and can call authorized endpoints.

1. **Frontend Setup (Angular)** - Start at root folder.
   ```bash
   cd decidr-web
   npm install
   ng serve
   ```
   - The frontend should now be running at [http://localhost:4200](http://localhost:4200).
     ```
---

## Technical Decisions
Some of the key technical decisions made for this project include:

- **Framework Choice: Angular**
  - I haven't worked with Angular before, so I wanted to give it a try! It was really fun learning about this framework.
  - I have worked with Ember JS before, as a full stack developer at Alarm.com, so I do have experience with a JS framework. 
  - There's a lot more I need to learn about Angular, and I'm sure the code can be cleaned up a bit and simplified! I'd also like to add unit tests to the front end to test out the components.
- **Database:** [Postgres/MongoDB/etc. and reasoning]  
- **Architecture:** [Monolith, Microservices, Layered Architecture, etc.]  
- **Authentication / Authorization:** [JWT, OAuth2, etc.]  
- **Testing Strategy:** [Unit tests, integration tests, mocking frameworks, etc.]  

These decisions were made to balance scalability, maintainability, and developer productivity.

---

## Next Steps
While the project is functional, there are areas for improvement:

- **Documentation:** Expand usage examples and API references  
- **Error Handling:** Improve consistency in handling failures  
- **Testing Coverage:** Increase unit/integration test coverage  
- **Performance:** Optimize database queries and API response times  
- **Dev Experience:** Add Docker support, CI/CD pipelines, or local dev tooling  

These improvements will make the project more robust, easier to maintain, and friendlier for new contributors.

---
