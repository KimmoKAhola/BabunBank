# Babun Bank MVC Project

This project has been created using MVC in C#. The result can be viewed here https://babunbank.azurewebsites.net/ and here https://babun-api.azurewebsites.net/swagger/index.html

View of the landing page
<img src="https://kimmostorage.blob.core.windows.net/test/babun-landing-page.png"/> 

View of the news page
<img src="https://kimmostorage.blob.core.windows.net/test/babun-blog-page.png" />

View of the accounts page
<img src="https://kimmostorage.blob.core.windows.net/test/babun-accounts-page.png" />

## Babun-API
An RESTful API service designed to handle various operations related to accounts and transactions. Utilizes automapper for object mapping, OpenAPI for versioning and documentation as well ass JWT for tokenization.
The result of the api can be viewed in the news section on the bank app.
Uses the bogus package for seeding data.

## DataAccessLibrary
A library that handles the data access layer. This library is in control of all data fetching and saving. Utilizes Entity Framework Core.
Patterns that have been used: repository pattern, service pattern and options pattern.

## DetectMoneyLaundering
A console application that can be used to find customers suspected of money laundering. The results of the money laundering can be saved as graphs by using the Scottplot package or saved as plain text to .txt files locally.


## Babun Bank
The main application. Showcases a fictional bank where users can use the bank with different results depending on their roles. Uses Microsoft Identity to handle login and user roles.
The bank is built using MVC in C# and uses several different patterns: factory pattern, service pattern, options pattern.

The bank utilizes automapper for object mapping, Fluent Validator and Microsoft's Data Annotations for server side validation, jquery for client-side validation.
