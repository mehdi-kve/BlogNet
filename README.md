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
## Endpoints

### Account
- `POST /api/Account/identify/create` - Create a new account.
- `POST /api/Account/identify/login` - Authenticate and log in a user.
- `POST /api/Account/identify/refresh-token` - Refresh authentication token.
- `POST /api/Account/identify/role/create` - Create a new user role.
- `GET /api/Account/identify/role/list` - Get a list of all roles.
- `POST /setting` - Setting up an administration.
- `GET /api/Account/identify/users-with-roles` - Get a list of users with their assigned roles.
- `POST /api/Account/identity/change-role` - Change a user's role.
  
### Post
- `GET /api/Post/GetAll` - Get a list of all posts.
- `GET /api/Post/GetById/{id}` - Get details of a specific post by ID.
- `POST /api/Post/Create` - Create a new post.
- `PUT /api/Post/Update/{id}` - Update post details.
- `DELETE /api/Post/Delete/{id}` - Delete a post.

### Post Category
- `GET /api/PostCategory/GetAll` - Get a list of all post categories.
- `POST /api/PostCategory/Create` - Create a new category.
- `PUT /api/PostCategory/Update/{id}` - Update category details.
- `DELETE /api/PostCategory/Delete/{id}` - Delete a category.

### Comment
- `GET /api/Comment/GetByPost/{postId}` - Get all comments for a specific post.
- `POST /api/Comment/Create` - Create a new comment.
- `PUT /api/Comment/Update/{id}` - Update an existing comment.
- `DELETE /api/Comment/Delete/{id}` - Delete a comment.

### Like
- `GET /api/Like/GetByPost/{postId}` - Get all likes for a specific post.
- `POST /api/Like/Create/{postId}` - Add a like to a post.
- `DELETE /api/Like/Delete/{postId}` - Remove a like from a post.




## Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

- **.NET 8 SDK**
- **SQL Server**
- **Visual Studio 2022 یا Visual Studio**

### Setup Instructions

1. **Clone the repository**
2. **Enter your ConnectionString in appsetting.json**

   ```bash
   Update-Database
4. **Start Project**:
   
