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
- **Database: Postgres**
  - I am working with a Postgres DB for my current project at work, but I have also had experience with MySQL. 
- **Authentication / Authorization: JWT Bearer Token**: I kept it simple for now. This bearer is valid for an hour and doesn't have an issuer source. I'd like to add a much more complex auth system as I continue to develop this app (refreshing tokens automatically, potentially an external auth service, tetc.)
- **Testing Strategy: /tests folder**: This folder contains 2 test projects to test out Decidr.Api and Decidr.Operations. In the future, I want to add tests around the infrastructure, including integration tests to test out persisting and retrieving data (i.e. Postgres in a local container). I'd also like to add front-end tests.

These decisions were made to balance scalability, maintainability, and developer productivity.

### Repository Structure
Here is the folder structure of my repo. I'll explain why I organized it this way.

#### `Decidr.Api`
This is the ASP.NET Core Web API. In this project I've created controllers for each Decidr endpoint, included adding security for these endpoints (requiring valid Bearer token).
The bearer token contains the information of the logged-in user's user id.
<br>
To help out with Authorization, I created an attribute `RequiresUserContextAttribute` that I put on each controller that defined
APIs that required a logged-in user. This attribute looks for the user id from the JWT token and fetches that user from the DB. We then have access to
the logged-in user's information for the rest of the request.

#### `Decidr.Operations`
The business logic layer. This is where I added the logic to process requests. Importantly, this project has a folder called `Infrastructure`. `Infrastructure` contains generic interfaces with methods to grab data from external sources (the db, third party APIs, etc).
<br>
This allows for the operation layer to be completely independent from infrastructure implementation. For example, if I ever needed to update the third party API to fetch movie information from, the operation project wouldn't need to change. All I'd need to do is update the implementation of that interface, which would live in the `Decidr.Infrastructure` project.

#### `Decidr.Infrastructure`
Gets data from external sources (Postgres, third party APIs, etc.). It has `Decidr.Operations` as a project reference to have access to the interfaces mentioned above. This project then implements those interfaces.
<br>
This project sets up EF Core and handles querying / persisting to Postgres.
<br>
Right now, we are fetching movie information from this third party movie API (`TheMovieDb`).

#### `decidr-web`
The Angular app. I'm not yet familiar with the best practices, but I put each component in its own folder and have a `shared` folder for shared interfaces (i.e. `Set`).

---

## Next Steps
While the app works, it's still in the early stages. Here are some things I'd like to do:

- **Funcationality:** Here are some features I'd like to add:
  - Support 'deleting' sets (soft deletes)
  - Add more ways to create movie sets. Have the user select a category (comedies), search for a movie term, Best Picture category, etc.
  - Add other types of sets. I'd like to add a section for 'restaurants' and query the Yelp API to allow users to vote on where to eat.
  - Add support to 'get more' cards for each set. Right now we only get a fixed amount of movies for each set. I want to add a way to request more.
  - Add about & settings pages
- Infrastructure improvements:
  - An important improvement would be to remove any sensitive tokens / JWT key from the app settings. I only added it there for local development, but it doesn't belong there. It belongs in some sort of secrets manager (Vault, AWS Secrets Manager, etc).
  - I'd like to fully deploy this app and host on AWS. 
- **Error Handling:** Improve consistency in handling failures  
- **Testing Coverage:** Increase unit/integration test coverage  
- **Performance:** Optimize database queries and API response times  


---
