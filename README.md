# Presentation link
https://youtu.be/hoGw8N9S6L0?si=VIs_SgN004MDMYyi

# RaceDayPOE
Project Overview

RaceDay is a sports event management system designed to support the management and participation of running, walking and cycling events in South Africa.

The system will have two main user roles:

Organiser: Create and manage events, categories, enrolments and participant results.
Participant: Register for events, view their enrolments and track their results.

This repository contains the planning and database work completed for Part 1 of the Programming 2B POE.

Part 1

Part 1 focuses on planning the system before development of the RESTful API begins.

The following items have been completed:

Entity Relationship Diagram (ERD)
API Endpoint Plan
SQL Server database script
Database structure, relationships and sample data
GitHub repository and version control
////////////////////////////////////////
# Repository Structure
RaceDay/
(inside RaceDay Foler) 
docs/
(inside docs Folder)
-RaceDay_ERD.png
-API_Endpoint_Plan.pdf
-RaceDay_Database.sql
/////////////////////////////////////////////////
# Database

The RaceDay database is designed using SQL Server.

The main entities are:

Users
Events
Categories
Enrolments
Results
EventImages

The database script creates the RaceDay database, tables, primary keys, foreign keys, constraints and sample data.

The SQL script was tested using SQL Server Management Studio (SSMS).

API Endpoint Plan

The API Endpoint Plan documents the RESTful API that will be developed in Part 2.

It identifies:

HTTP methods
API routes
Endpoint descriptions
Required user roles
Request bodies
Expected responses
Relevant HTTP status codes

No REST API implementation is included in Part 1. The endpoint plan serves as the blueprint for the API development in Part 2.

ERD

The ERD represents the database structure and relationships between the RaceDay entities.

The ERD and SQL database script are designed to correspond with each other.
