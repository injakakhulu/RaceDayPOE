# Project Overview

This branch contains the necessary requirements and implementation for Part 2 of the RaceDay Programming 2B POE. Part 1 requirements, including the ERD and SQL documentation, are available in the separate part one branch.

RaceDay is a platform designed to help South African running, walking and cycling event organisers manage events and participants. It allows organisers to manage events, categories and results while participants can browse events, enrol in events and view their results.


# Part 2 Features

**Authentication and Authorisation**
- User registration for Organiser and Participant roles.
- User login and logout.
- Session based authentication.
- Password hashing using PasswordHasher.
- Role based access control.
- Protection of endpoints against unauthorised access.
- Organisers can only manage their own events and related information.

**Event Management**
- View all events.
- View an individual event.
- Create events.
- Update existing events.
- Delete events.
- Automatically associate newly created events with the logged in organiser.

**Category Management**
- View categories.
- View categories belonging to a specific event.
- Create, update and delete categories.
- Validate that organisers manage categories belonging to their own events.

**Enrolment Management**
- Participants can enrol in events using available categories.
- Participants can view their own enrolments.
- Participants can view individual enrolments subject to authorisation.
- Participants can cancel their enrolments.
- The API validates event and category relationships.
- Duplicate enrolment in the same event is prevented.

**Results Management**
- Participants can view their own results.
- Users can view results for an event.
- Organisers can capture results for their event participants.
- Organisers can update and delete results.
- Duplicate results for the same enrolment are prevented.
- Result access is restricted according to the user's role and event ownership.

# Database

The application uses Microsoft SQL Server with Entity Framework Core Code First.
The development database is named RaceDay_Dev. Database tables are created through Entity Framework Core migrations.
The database includes the following main entities:
- Users
- Events
- Categories
- Enrolments
- Results

**The original RaceDay database has been kept separate from the development database.**

# API Testing

Swagger / OpenAPI is used to explore and test the API endpoints.
The API has been tested for registration, login, session management, role based access, event creation, category creation, enrolment and result management.

# Unit Testing

An xUnit test project named RaceDay.Tests has been created and linked to the API project.
The project uses Entity Framework Core InMemory to test controller functionality without requiring the development SQL Server database.

- The current unit tests cover:
- Retrieving events successfully.
- Retrieving an event inserted into the test database.
- Returning a not found response when an event does not exist.
- Retrieving categories successfully.
- Retrieving a category inserted into the test database.
- Returning an empty enrolment list when no enrolments exist.
- Returning an empty results list when no results exist.

**Current test status: 7 unit tests passing.**


# Part 1 Documentation

The part one branch contains the Part 1 requirements and documentation, including the ERD and SQL materials.
The current master branch contains the Part 2 API implementation, unit tests and continuous integration workflow.

# Demonstration Video


YouTube link: [Add your Part 2 demonstration video link here]

CI Screenshot
