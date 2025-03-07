# BlogNet

A blogging service implemented using **.NET 8** and **Clean Architecture**. It leverages modern technologies and patterns such as **JWT** for authentication, **CQRS** for separating read and write operations, and **AutoMapper** for object mapping.

## Features

- **User authentication & authorization using JWT**
- **CQRS pattern for handling commands & queries using MediatR**
- **AutoMapper for mapping**
- **Relational database support (SQL Server)**
- **UnitOfWork & Generic Repository**
- **Model Tracking**

## Technologies Used

- **.NET 8**
- **ASP.NET Core**
- **Entity Framework Core**
- **JWT (JSON Web Tokens)**
- **SQL Server**
- **Swagger for API documentation**

## Project Architecture

This project follows **Clean Architecture**, which consists of the following layers:

1. **Presentation Layer**: Contains controllers and view models responsible for handling client requests.
2. **Application Layer**: Implements business logic through commands and queries.
3. **Domain Layer**: Contains domain models and core business logic.
4. **Infrastructure Layer**: Handles data access (e.g., Entity Framework Core) and other infrastructure services.

## Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

- **.NET 8 SDK**
- **SQL Server**
- **Visual Studio 2022 یا Visual Studio کد**

### Setup Instructions

1. **Clone the repository**
2. **Enter your ConnectionString in appsetting.json**

   ```bash
   Update-Database
4. **Start Project**:
   
