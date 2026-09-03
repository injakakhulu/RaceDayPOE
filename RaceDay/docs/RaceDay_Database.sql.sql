/* ============================================================
   RaceDay Database
   Programming 2B - Part 1
   ============================================================ */

-- Create Database
CREATE DATABASE RaceDay;
GO

USE RaceDay;
GO

/* ============================================================
   1. USERS
   Stores both Organisers and Participants
   ============================================================ */

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO


/* ============================================================
   2. EVENTS
   Stores events created by Organisers
   ============================================================ */

CREATE TABLE Events
(
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserId INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(1000) NULL,
    EventDate DATE NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    EventType NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Events_Users
        FOREIGN KEY (OrganiserId)
        REFERENCES Users(UserId)
);
GO


/* ============================================================
   3. CATEGORIES
   Categories belonging to an event
   ============================================================ */

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    MaximumParticipants INT NOT NULL,

    CONSTRAINT FK_Categories_Events
        FOREIGN KEY (EventId)
        REFERENCES Events(EventId),

    CONSTRAINT CK_Categories_Distance
        CHECK (Distance > 0),

    CONSTRAINT CK_Categories_EntryFee
        CHECK (EntryFee >= 0),

    CONSTRAINT CK_Categories_MaxParticipants
        CHECK (MaximumParticipants > 0)
);
GO


/* ============================================================
   4. ENROLMENTS
   Participants entering event categories
   ============================================================ */

CREATE TABLE Enrolments
(
    EnrolmentId INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(30) NOT NULL DEFAULT 'Registered',

    CONSTRAINT FK_Enrolments_Users
        FOREIGN KEY (ParticipantId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Enrolments_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT CK_Enrolments_Status
        CHECK (Status IN ('Registered', 'Completed', 'Cancelled')),

    CONSTRAINT UQ_Enrolments_Participant_Category
        UNIQUE (ParticipantId, CategoryId)
);
GO


/* ============================================================
   5. RESULTS
   Results captured for participants
   ============================================================ */

CREATE TABLE Results
(
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentId INT NOT NULL,
    FinishTime TIME NOT NULL,
    Position INT NOT NULL,
    ResultStatus NVARCHAR(30) NOT NULL DEFAULT 'Official',
    CapturedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Results_Enrolments
        FOREIGN KEY (EnrolmentId)
        REFERENCES Enrolments(EnrolmentId),

    CONSTRAINT UQ_Results_Enrolment
        UNIQUE (EnrolmentId),

    CONSTRAINT CK_Results_Position
        CHECK (Position > 0),

    CONSTRAINT CK_Results_Status
        CHECK (ResultStatus IN ('Official', 'Pending', 'Disqualified'))
);
GO


/* ============================================================
   6. EVENT IMAGES
   Stores Azure Blob Storage image URLs
   ============================================================ */

CREATE TABLE EventImages
(
    EventImageId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_EventImages_Events
        FOREIGN KEY (EventId)
        REFERENCES Events(EventId)
);
GO


/* ============================================================
   SEED DATA
   ============================================================ */

/* -------------------------
   Users
   2 Organisers
   4 Participants
   ------------------------- */

INSERT INTO Users
    (FirstName, LastName, Email, PasswordHash, Role)
VALUES
    ('Thabo', 'Mokoena', 'thabo@raceday.co.za', 'HASHED_PASSWORD_1', 'Organiser'),
    ('Lerato', 'Dlamini', 'lerato@raceday.co.za', 'HASHED_PASSWORD_2', 'Organiser'),
    ('Sipho', 'Nkosi', 'sipho@example.com', 'HASHED_PASSWORD_3', 'Participant'),
    ('Ayanda', 'Mthembu', 'ayanda@example.com', 'HASHED_PASSWORD_4', 'Participant'),
    ('Jordan', 'Smith', 'jordan@example.com', 'HASHED_PASSWORD_5', 'Participant'),
    ('Nomsa', 'Khumalo', 'nomsa@example.com', 'HASHED_PASSWORD_6', 'Participant');
GO


/* -------------------------
   Events
   3 Events
   ------------------------- */

INSERT INTO Events
    (OrganiserId, EventName, Description, EventDate, Location, EventType)
VALUES
    (
        1,
        'Cape Town City Run',
        'Annual road running event through Cape Town.',
        '2026-11-15',
        'Cape Town',
        'Running'
    ),
    (
        1,
        'Johannesburg Cycle Challenge',
        'Road cycling event for different experience levels.',
        '2026-12-05',
        'Johannesburg',
        'Cycling'
    ),
    (
        2,
        'Durban Coastal Walk',
        'Community walking event along the Durban coastline.',
        '2027-01-10',
        'Durban',
        'Walking'
    );
GO


/* -------------------------
   Categories
   Multiple categories
   for each event
   ------------------------- */

INSERT INTO Categories
    (EventId, CategoryName, Distance, EntryFee, MaximumParticipants)
VALUES
    (1, '5 KM Run', 5.00, 100.00, 500),
    (1, '10 KM Run', 10.00, 150.00, 500),
    (1, '21 KM Half Marathon', 21.10, 250.00, 300),

    (2, '20 KM Cycle', 20.00, 200.00, 300),
    (2, '50 KM Cycle', 50.00, 350.00, 250),
    (2, '100 KM Cycle', 100.00, 500.00, 150),

    (3, '5 KM Walk', 5.00, 80.00, 400),
    (3, '10 KM Walk', 10.00, 120.00, 400);
GO


/* -------------------------
   Enrolments
   ------------------------- */

INSERT INTO Enrolments
    (ParticipantId, CategoryId, EnrolmentDate, Status)
VALUES
    (3, 1, '2026-09-01', 'Registered'),
    (4, 2, '2026-09-01', 'Registered'),
    (5, 3, '2026-09-02', 'Completed'),
    (6, 4, '2026-09-02', 'Registered'),
    (3, 5, '2026-09-02', 'Completed'),
    (4, 7, '2026-09-03', 'Registered');
GO


/* -------------------------
   Results
   Only completed
   enrolments have results
   ------------------------- */

INSERT INTO Results
    (EnrolmentId, FinishTime, Position, ResultStatus)
VALUES
    (3, '01:48:32', 12, 'Official'),
    (5, '02:15:45', 25, 'Official');
GO


/* -------------------------
   Event Images
   ImageUrl represents the
   URL that will later point
   to Azure Blob Storage
   ------------------------- */

INSERT INTO EventImages
    (EventId, ImageUrl)
VALUES
    (1, 'https://racedaystorage.blob.core.windows.net/events/cape-town-city-run.jpg'),
    (2, 'https://racedaystorage.blob.core.windows.net/events/johannesburg-cycle.jpg'),
    (3, 'https://racedaystorage.blob.core.windows.net/events/durban-coastal-walk.jpg');
GO


/* ============================================================
   VERIFY DATABASE
   ============================================================ */

SELECT * FROM Users;
SELECT * FROM Events;
SELECT * FROM Categories;
SELECT * FROM Enrolments;
SELECT * FROM Results;
SELECT * FROM EventImages;
GO