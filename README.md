# About The Project

### Built With (Tech stack)

#### Front end
* [Angular](https://angular.io)
* [DaisyUI](https://daisyui.com)
* [Tailwind](https://tailwindcss.com)
* [Typescript](https://www.typescriptlang.org)


#### Back end
* [Dotnet](https://dotnet.microsoft.com)
* [Rest API](https://restfulapi.net)
* [Playwright](https://playwright.dev)
* [Fluent Assertions](https://fluentassertions.com/)
* [Dapper](https://github.com/DapperLib/Dapper)
* [Bogus](https://github.com/bchavez/Bogus)

#### Database
* [ElephantSQL](https://www.elephantsql.com)
* [PostgresSQL](https://www.postgresql.org)

## Box Factory
The boomer boss of the box factory wants to modernise by having an inhouse application to keep track of their products (boxes).

He wants employees to be able to add new boxes to their catalogue, edit existing, remove and find particular boxes based on searching and sorting preferences.

You, as the CTO, must decide what properties boxes have in the IT system. (In other words, there's no prebuilt database schema or starter app foundation to rely on)

### Strict minimal requirements:

- The client application must be built using Angular. ✅

- The backend must be built using a .NET Web API + Relational Database. ✅

- Communication between client and server should be done using HTTP. ✅

- There must be data validation on both the client and server side. ✅

- There must be at least 1 business entity and 1 table in the database. ✅

- There must be at least the following CRUD operations: ✅
  - Create a new box
  - Delete an existing box
  - Search boxes (Client side)
  - See all details for one given box on it's own page (get by ID)
  - Updating a box
- Any testing deemed relevant must be conducted in order to assure quality. This can be E2E testing using Playwright (SDK is free of choice, but I recommend .NET Playwright) or integration testing of API's (simply calling the API with an HTTP client in an NUnit test and making assertions). ✅

- You must have at least one workflow on Github Actions which automates building, running and testing of your application. ✅


## DB Creation script
```
DROP SCHEMA IF EXISTS box_factory CASCADE;
CREATE SCHEMA box_factory;
CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

create table box_factory.materials
(
    id            integer generated always as identity
        constraint materials_pk
            primary key,
    material_name varchar(256) not null
);

create table box_factory.box_inventory
(
    guid             uuid                     default uuid_generate_v4() not null
        constraint box_inventory_pk
            primary key,
    width            numeric,
    height           numeric,
    depth            numeric,
    location         varchar(256),
    description      text,
    datetime_created timestamp with time zone default CURRENT_TIMESTAMP,
    title            varchar(256)                                        not null,
    quantity         integer                  default 0                  not null,
    material_id      integer
        constraint box_inventory_materials_id_fk
            references box_factory.materials
);

```

## DB Insert script
```
-- Materials
INSERT INTO materials (material_name) VALUES ('Cardboard');
INSERT INTO materials (material_name) VALUES ('Kraft Paper');
INSERT INTO materials (material_name) VALUES ('Corrugated Fiberboard');
INSERT INTO materials (material_name) VALUES ('Wood');
INSERT INTO materials (material_name) VALUES ('Plastic');
INSERT INTO materials (material_name) VALUES ('Foam');
INSERT INTO materials (material_name) VALUES ('Metal');
INSERT INTO materials (material_name) VALUES ('Adhesive Tape');
INSERT INTO materials (material_name) VALUES ('Bubble Wrap');
INSERT INTO materials (material_name) VALUES ('Polyethylene');
```

```
-- Sample data for the box_inventory  adjust materialIds to match the materials table
INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (10, 5, 3, 'Storage Room A', 'Small box', 'Sample Box 1', 20, 1);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (15, 8, 5, 'Shelf 2', 'Medium box', 'Sample Box 2', 15, 2);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (20, 10, 6, 'Warehouse B', 'Large box', 'Sample Box 3', 10, 3);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (12, 6, 4, 'Storage Room C', 'Small box', 'Sample Box 4', 25, 4);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (18, 9, 7, 'Shelf 3', 'Medium box', 'Sample Box 5', 18, 5);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (24, 12, 8, 'Warehouse A', 'Large box', 'Sample Box 6', 12, 6);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (8, 4, 2, 'Storage Room D', 'Small box', 'Sample Box 7', 30, 7);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (14, 7, 5, 'Shelf 4', 'Medium box', 'Sample Box 8', 20, 8);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (22, 11, 7, 'Warehouse C', 'Large box', 'Sample Box 9', 15, 9);

INSERT INTO box_inventory (width, height, depth, location, description, title, quantity, material_id)
VALUES (10, 5, 3, 'Storage Room E', 'Small box', 'Sample Box 10', 24, 10);

```