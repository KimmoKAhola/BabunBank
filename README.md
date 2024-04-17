# Babun Bank MVC Project

# Babun Bank MVC Project

MVC stands for Model, View, Controller. We have used this design pattern to separate the logic of our application into
distinct layers:

- Models: Used to handle data and business logic.
- Views: Used to handle graphical user interface objects and presentation.
- Controllers: Acts as an interface between Model and View components.

The main features of our MVC project include user authentication, account management, and transaction management.

## `CustomerFactory` Class

The `CustomerFactory` class is a static factory class in the main MVC bank project. It serves to create `Customer`
objects from `SignUpCustomerModel` data provided during a customer's signup process. To perform this conversion, it
utilizes `AutoMapper`, a popular object-object mapper in .NET.

The `Create` method of this class accepts a `SignUpCustomerModel` object and an `IMapper` instance as parameters.
The `IMapper` is used to map the data from the `SignUpCustomerModel` to a new `Customer` object.

### `Create` Method

The `Create` method in the `CustomerFactory` class works to generate a `Customer` instance from the
provided `SignUpCustomerModel` object:

- `model`: This is the model captured during signup. It is of the type `SignUpCustomerModel`.
- `mapper`: This `IMapper` instance is used to convert the `SignUpCustomerModel` into a `Customer` object.

This method returns a `Customer` object mapped from the `SignUpCustomerModel` object. If any error occurs during the
conversion, it throws an exception.

**Note:** Currently, the creation of an account for the customer, the disposition, and the owner details are not handled
in this method.

It is important to be aware that the `CustomerFactory` class is a part of the larger Babun Bank MVC software
solution.```

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

## Data Visualization Service Class

# `DataVisualizationService` Class

This class is located within the `DetectMoneyLaundering.Services` namespace in the Console Application of the solution.

The `DataVisualizationService` class is a static class that is utilized for creating and managing various forms of visual data representations, namely plots and charts, based on transactional data. This is primarily geared towards distinguishing between suspicious and normal transactions.

The following methods are provided:

1.  `CreateIndividualPlot(InspectAccountModel model)`

    This method is used to create visual representations for individual account models. It creates plots for suspicious transactions, normal transactions, and an accompanying pie chart for percentage distribution of the transactions.

2.  `CreateSuspiciousTransactionsPlot(Transaction[] transactions, int length, string customerName)`

    This private method generates plots for suspicious transactions. It takes an array of `Transaction` objects, their number, and the `CustomerName` as arguments.

3.  `CreateNormalTransactionsPlot(Transaction[] transactions, int length, string customerName)`

    Similar to the previous method, this private method generates plots for normal transactions.

4.  `CreatePieChart(double percentageOfSuspiciousTransactions, string customerName)`

    This private method creates a pie chart showing the percentage distribution of suspicious and normal transactions.

5.  `CreateGeneralPlot()`

    This public method creates a general plot with default settings.

We utilize the ScottPlot library for graphical visualization of the transaction data. The suspicious transactions are marked with a red color, while normal ones are marked with a blue color.

The plots created are saved as `.png` images within the `wwwroot/images/moneylaundering/` directory.

> Note: You will need to have the ScottPlot library installed in your solution to use this class.
> 
> # `MoneyLaunderingService` Class

This class is located within the `DetectMoneyLaundering.Services` namespace in the Console Application of the solution.

The `MoneyLaunderingService` class serves to investigate a particular account's transactions for potential money laundering evidence. Data from these investigations can be visualized using the associated `DataVisualizationService`.

The following methods are provided:

1. `GetAccount(int id)`

   This method, given an account ID, fetches the associated `Account` object from the database, along with its related `Transactions` and `Dispositions`. The function returns a `Task<Account?>`, which signifies the asynchronous nature of the operation.

2. `InspectAccount(int id)`

   This method takes an account ID as its input and processes the associated account's transactions. It creates an `InspectAccountModel` object, where normal transactions and transactions over the limit (15000 in this case) are categorized. This model is then passed to `DataVisualizationService.CreateIndividualPlot(result)` to enable creating a visual representation of the transactions.

> Note: The `DataAccessLibrary` and `DetectMoneyLaundering.Models` namespaces are being used in this class. Be sure to have these dependencies correctly set up in your project.

> Note 2: To use this class, please make sure you have a functional `BankAppDataContext` instance, which is a necessity for the functioning of methods within this class.