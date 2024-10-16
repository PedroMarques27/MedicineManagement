# Medicine Management Platform

This repository contains a technical challenge project provided by  **SmartMed** , where I developed a **Medicine Management Platform** using **.NET 8** for the backend, **Entity Framework Core (EF Core)** for database interaction, and a **MySQL** database. The platform manages users, medicines, and prescriptions efficiently through a clean architecture implementing the  **Repository Pattern** .

## Main Features

The Medicine Management Platform provides functionality to:

* **Manage Medicines** : CRUD operations for medicines, ensuring that quantities cannot be negative.
* **Manage Users** : CRUD operations for user data.
* **Manage Prescriptions** : Create, read, update, and delete prescriptions that associate users with a list of medicines.

## Table of Contents

* [Main Features](#mainfeatures)
* [Usage](#usage)
* [Project Structure](#project-structure)
  * [1. Database Project](#1-database-project)
    * [Entities](#entities)
    * [Repositories](#repositories)
  * [2. Process Project](#2-process-project)
    * [Providers](#providers)
    * [API Endpoints](#api-endpoints)
  * [3. Tests Project](#3-tests-project)
* [Technologies Used](#technologies-used)

---

## Usage

The full project has been containerized using Docker. To Run the project:

* Clone the repository
* Open CMD inside the Repository
* Run:

  ```
  docker-compose up --build
  ```
* This will create a Docker container for the mySQL database and another for the project itself. The API is exposed at port 8080
* Install dotnet EF Tools

  ```
  dotnet tool install --global dotnet-ef
  ```
* Inside the repository folder, run the Migrations:

  ```
  dotnet ef database update --project "Process"
  ```
* Refeer to the [Swagger.json](swagger.json) For More Information

## Project Structure

The solution is divided into three key projects, the Database Project, the Process Project and the Tests Project:

### 1. **Database Project**

* **Purpose** : This project manages the **Entity Framework Core** integration with the **MySQL** database and defines the database models and repositories.
* **Entities**:

  * **UserModel**

    * The `UserModel` represents a user entity in the system with the following properties:
      * `Name `: The name of the user.
      * `Email `(primary key): The unique email address of the user. This field serves as the primary identifier for the user.
      * `PrescriptionList `: A collection of prescriptions associated with the user. A user can have multiple prescriptions.
    * A UserModel has a one-to-many relationship with PrescriptionModel, meaning each user can have multiple prescriptions.
  * **PrescriptionModel**

    * The `PrescriptionModel` represents a prescription made by a user, containing:

      * `Id `(primary key): A unique identifier for each prescription.
      * `CreationDate `: The date when the prescription was created.
    * Prescriptions are automatically deleted when the associated user is removed (DeleteBehavior.Cascade).
  * **MedicineModel**

    * The `MedicineModel` represents a medicine, with the following attributes:
      * `Name `(string, required, primary key): The name of the medicine.
      * `Quantity `(int, required): The number of units existing for this medicine. It must be a positive value.
      * `CreationDate `(DateTime, required): The date when this medicine was registered in the database.
  * **PrescriptionMedicineModel**

    * A representation of the many-to-many relationship between MedicineModel and PrescriptionModel. When a Medicine is deleted, it is deleted from all the prescriptions. When a prescription is deleted, no medicine is deleted.
* **Repositories**

  The project follows the Repository Pattern, encapsulating the data access logic into specific repositories for each entity (`User`, `Prescription`, and `Medicine`).

  * **UserRepository** (for `UserModel`)

    * The `UserRepository` manages data access and operations related to users. It provides the following methods:
      * **GetAllUsers()** : Retrieves all users from the database, including their associated prescriptions and medicines.
      * **GetUserByIdAsync(string email)** : Fetches a specific user by their email, including their associated prescriptions and medicines.
      * **AddUserAsync(UserModel user)** : Adds a new user to the database.
      * **UpdateUserAsync(UserModel updatedUser)** : Updates an existing user’s data (Name Only).
      * **DeleteUserByIdAsync(string email)** : Deletes a user based on their email. This operation cascades to delete all associated prescriptions.
  * **PrescriptionRepository** (for `PrescriptionModel`)

    * The `PrescriptionRepository` handles data access for prescriptions. It offers the following methods:

      * **GetAllPrescriptions()** : Retrieves all prescriptions from the database, including their associated medicines.
      * **GetPrescriptionById(Guid id)** : Fetches a specific prescription by its unique identifier (`Id`), including its associated medicines.
      * **AddPrescriptionAsync(PrescriptionModel prescription)** : Adds a new prescription to the database.
      * **UpdatePrescriptionAsync(PrescriptionModel prescription)** : Updates an existing prescription’s data.
      * **DeletePrescriptionByIdAsync(Guid id)** : Deletes a prescription based on its `Id`.
      * **Exists(string name)** : True if a prescription exists in the database, else false.
  * **MedicineRepository** (for `MedicineModel`)

    * The `MedicineRepository` manages data access for medicines. It provides the following methods:

      * **GetAllMedicines()** : Retrieves all medicines from the database.
      * **GetMedicineByNameAsync(string name)** : Fetches a specific medicine by its name.
      * **AddMedicineAsync(MedicineModel medicine)** : Adds a new medicine to the database.
      * **UpdateMedicineAsync(MedicineModel medicine)** : Updates an existing medicine’s details (Quantity).
      * **DeleteMedicineByNameAsync(string name)** : Deletes a medicine based on its name.
      * **Exists(string name)** : True if a medicine exists in the database, else false.

### 2. **Process Project**

* **Purpose** : This project implements the **Web API** and contains the business logic, organized into controllers and providers.
* The Providers are responsible for mapping the repository operations with the data received from the controllers. They implement structure-mapping, logic under the CRUD operations (e.g. verifying that a user exists before updating it) and other business logic.
  Furthermore, all provider methods return a StatusResponseDTO. This entity enables the capture of errors returning them as a message. The **StatusResponseDTO** is as follows:
  * `Success(bool)`: If the operation was successfull (i.e. if the error message is empty)
  * `Data (object?)`: Possible Data resulting from the operation
  * `Error (string?)`: Error Detail Message
 * **API Endpoints**
    The project includes three main controllers: UserController, MedicinesController, and PrescriptionController. These controllers provide RESTful API endpoints to manage users, medicines, and prescriptions in the system.

    **User Controller**

    * The UserController manages operations related to users and their prescriptions. It provides the following endpoints:
      * **POST** `/api/user`
        **Description**: Adds a new user to the system (Can't Add Prescriptions or Medicine).
        **Body**: UserInputDto containing the user's name and email.
        **Response**: 201 Created with the user data, or 400 Bad Request if the input is invalid.
      * **DELETE** `/api/user/{email}`
        **Description**: Deletes a user based on their email (and their prescriptions).
        **Response**: 204 No Content if the user is successfully deleted, or 404 Not Found if the user does not exist.
      * **GET** `/api/user/{email}`
        **Description**: Retrieves a user by their email, including their prescriptions and medicines.
        **Response**: 200 OK with the user data, or 404 Not Found if the user is not found.
      * **GET `/api/user/all`**
        **Description**: Retrieves all users from the system, including their prescriptions and medicines.
        **Response**: 200 OK with a list of all users, or 404 Not Found if no users exist.
      * **PUT `/api/user/update/{email}`**
        **Description**: Updates an existing user’s information (Name).
        **Body**: UserInputDto with the updated user data.
        **Response**: 200 OK with the updated user data, or 404 Not Found if the user does not exist.
      * **POST** `/api/user/{email}/Prescription`
        **Description**: Creates a prescription for a user, containing a list of medicine names.
        **Body**: ICollection of medicine names.
        **Response**: 201 Created with the created prescription, or 400 Bad Request if the input is invalid.
      * **GET** `/api/user/{email}/Prescription`
        **Description**: Retrieves a user's prescriptions by their email, including the medicine data.
        **Response**: 200 OK with prescription data, or 404 Not Found if the user does not exist.

    **MedicinesController**

    * The MedicinesController provides CRUD operations for medicines. Updating or Creating Medicine with a negative quantity is disallowed by the system. **FluentValidation** ensures that the quantity value is a positive integer. It exposes the following endpoints:

      * **GET** `/api/medicines`
        **Description**: Retrieves all medicines in the system.
        **Response**: 200 OK with the list of all medicines, or 400 Bad Request if something goes wrong.
      * **GET** `/api/medicines/{name}`
        **Description**: Retrieves a specific medicine by its name.
        **Response**: 200 OK with the medicine data, or 404 Not Found if the medicine is not found.
      * **POST** `/api/medicines`
        **Description**: Adds a new medicine to the system.
        **Body**: Medicine object containing the name, quantity. CreationDate is given by the application.
        **Response**: 201 Created with the created medicine, or 400 Bad Request if the input is invalid.
      * **PUT** `/api/medicines/{name}`
        **Description**: Updates an existing medicine's data (Quantity).
        **Body**: Medicine object with the updated details.
        **Response**: 200 OK with the updated medicine data, or 400 Bad Request if the input is invalid.
      * **DELETE** `/api/medicines/{name} `
        **Description**: Deletes a medicine by its name.
        **Response**: 200 OK if the medicine is successfully deleted, or 404 Not Found if the medicine does not exist.

    **PrescriptionController**

    * The PrescriptionController manages prescriptions and provides the following endpoints:

      * **GET** `/api/prescriptions`
        **Description**: Retrieves all prescriptions in the system.
        **Response**: 200 OK with the list of all prescriptions, or 404 Not Found if no prescriptions are found.
      * **GET** `/api/prescriptions/{id}`
        **Description**: Retrieves a specific prescription by its ID.
        **Response**: 200 OK with the prescription data, or 404 Not Found if the prescription is not found.
      * **DELETE** `b`
        **Description**: Deletes a prescription by its ID.
        **Response**: 204 No Content if the prescription is successfully deleted, or 404 Not Found if the prescription does not exist.
      * **PUT** `/api/prescriptions/{id}`
        **Description**: Updates the list of medicines in a prescription.
        **Body**: ICollection of medicine names.
        **Response**: 200 OK with the updated prescription data, or 404 Not Found if the prescription does not exist.

For more information, refeer the [API Documentation (swagger.json)](swagger.json) file.

### 3. **Tests Project**

* **Purpose** : Contains all unit tests for the platform to ensure the correctness of business logic and controller functionality.
* **Tools** :
* `xUnit`: For unit testing.
* `Moq`: For mocking dependencies (e.g., repositories) to isolate test cases.
* **Tests** : 97 tests where implemented to ensure the maximum code coverage (94.7%). The tests are split into two folders: Process and Database, regarding the classes being tested. `Microsoft.EntityFrameworkCore.InMemory` package was used to correctly mock interactions between the system and the database.

## Technologies Used

* **.NET 8**
* **Entity Framework Core 8.0**
* **MySQL** : To store user, medicine, and prescription data.
* **Repository Pattern** : Ensures clean separation between data access and business logic.
* **Unit Testing** : xUnit and Moq for comprehensive test coverage.
* **DTOs (Data Transfer Objects)** : To transfer data efficiently and securely.
