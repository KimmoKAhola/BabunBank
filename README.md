# Babun Bank MVC Project

# Babun Bank MVC Project

MVC stands for Model, View, Controller. We have used this design pattern to separate the logic of our application into
distinct layers:

- Models: Used to handle data and business logic.
- Views: Used to handle graphical user interface objects and presentation.
- Controllers: Acts as an interface between Model and View components.

The main features of our MVC project include user authentication, account management, and transaction management.

## Data Access Library

In the Data Access Library, we have implemented the Repository pattern. It is used to create an abstraction layer
between the data access layer and the business logic layer of an application.

The main job of the Repository pattern is to hide the details of how exactly the data gets pulled from the database.

## API

The API project provides a RESTful service which allows bank transactions to be done remotely via HTTP requests.
Additionally, the `AdController` class (Version 2.0) is included to manage ads that can be used within blog posts. This
class offers
endpoints for creating, fetching, updating, and soft-deleting ads in the database. The class requires user authorization
via a JWT token for updating and deleting ads.

## `AdsController` Class

This class represents a controller for handling ads. The ads can be used at blog posts however you wish. It utilizes
an `ApiContext` instance for database context and an `IMapper` instance to convert between data transfer objects.

**Note:** This controller requires user API versioning matching "1.0".

Routes to this controller can be made at `[controller]`.

The following characteristics were originally registered:

- Produces response type: `401` (Unauthorized)
- Produces response type: `403` (Forbidden)
- Produces response type: `500` (Internal Server Error)
- Produces response type: `503` (Service Unavailable)

### Methods

#### `Get(int id)`

Fetches a singular ad by its database ID.

*Parameters:* `{id}` - The ID of the ad to fetch.

*Returns:* HTTP status code `200` (OK) with the ad object if found, `404` (Not Found) if the ad with the corresponding
ID doesn't exist.

*Api Version:* "1.0"

---

#### `Get()`

Obtain all database objects.

This function returns all available database objects, both deleted and non-deleted (since soft deletion is implemented).

*Returns:* HTTP status code `200` (Ok) with a list of all ads, or `400` (Bad Request) if no ads were found.

*Api Version:* "1.0"

---

#### `Create(CreateAdModel model)`

Creates a new ad, if the database limit of 500 rows hasn't been reached.

*Parameters:* `{model}` - A representation of the ad to create.

*Returns:* HTTP status code `200` (OK) with the created ad object if successful, '400' (Bad Request) if the limit is
reached and `500` (Internal Server Error) if the save to the database fails.

*Api Version:* "1.0"

---

#### `Update(int id, EditAdModel model)`

Updates an existing ad info, given its ID and a model of the new object.

*Parameters:*

- `{id}` - The id of the database object to update.
- `{model}` - Your model of the object to update.

*Returns:*

- `Bad Request` if id is incorrect.
- `Invalid Bad Request` if model state is incorrect.
- `Not Found` if the item does not exist.
- `Status code 500` if the database fails.

*Api Version:* "1.0"

---

#### `Delete(int id)`

Soft deletes an ad given its ID.

*Parameters:* `{id}` - The id of the ad to delete.

*Returns:*

- `BadRequest` if the minimum database amount of 100 has been reached.
- `NotFound` if the ad does not exist.
- `Status code 500` if the database operation fails.

*Api Version:* "1.0"

---

### Note

All `HttpPut`, `HttpDelete` requests require JWT token for authorization.

Key features of the `AdController` class:

- GET `<id>`: Fetches a singular ad by its database ID. Returns an HTTP status code 200 (OK) along with the ad object if
  found, or an HTTP status code 404 (Not Found) if the ad with the specified ID does not exist.
- GET: Obtains all database objects (both deleted and non-deleted, as soft delete is implemented).
- POST: Creates a new ad, subject to the database having less than 500 rows. Returns Ok(object), returns BadRequest if
  limit is reached, returns status code 500 if the save to database fails.
- PUT `id`: Updates an existing ad given its ID and a model of the new object. Returns BadRequest if ID is incorrect or
  if ModelState is invalid. Returns NotFound if the item does not exist. Returns status code 500 if the database
  operation fails.
- DELETE `id`: Soft deletes an ad given its ID. Returns BadRequest if the database minimum amount of 100 has been
  reached. Returns NotFound if the ad does not exist. Returns status code 500 if the database operation fails.

### TokenController Class

This class generates a token that is needed for authorization in the API.

**Api Version:** "1.0"

Routes to this controller can be made at `[controller]`.

#### Methods

--- 

##### `GenerateToken()`

Get a 10-minute token to access the v1 API. This will not work for v2.

The response of this API is a token returned as a simple string. The token lasts for a period of 10 minutes.

**Returns:**

- Status code `200` along with the token.

**Api Version:** "1.0"

The main goal of this service is to encapsulate the bank and advertisement operations inside a set of API endpoints,
ensuring the security and integrity of data is met, and a consistent and simple interface is provided to consumers.

## Console Application

The Console application is a command-line interface for the banking system, designed for system administrators. It uses
the Command pattern to define each operation as an object, simplifying the expansion of system capabilities.

Common tasks include creating and managing user accounts, updating system settings, and performing batch updates of bank
records.

## Prerequisites, Installation, and Other Sections...

Replace this with the rest of your README file as indicated in the previous message.

# Data Access Library

# API

# Console application
