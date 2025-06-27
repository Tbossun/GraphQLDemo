# GraphQLDemo

A simple GraphQL API for managing users, built with .NET and HotChocolate.

## Features
- GraphQL endpoint for User CRUD operations
- Schema-first approach with `.graphql` files
- Input validation

## Setup
1. Clone the repository
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run database migrations:
   ```bash
   dotnet ef database update
   ```
4. Start the server:
   ```bash
   dotnet run
   ```
5. Access GraphQL Playground at `http://localhost:{port}/graphql` (your configured port)

## Schema Overview
See `GraphQL/Schemas/user.graphql` for the full schema definition.

### User Type
```
type User {
  id: ID!
  name: String!
  email: String!
}
```

### Queries
- `users`: Get all users
- `user(id: ID!)`: Get a user by ID

### Mutations
- `createUser(name: String!, email: String!): User!`
- `updateUser(id: ID!, name: String, email: String): User!`
- `deleteUser(id: ID!): Boolean!`

## Example Queries

### Get All Users
```graphql
query {
  users {
    id
    name
    email
  }
}
```

### Get User by ID
```graphql
query {
  user(id: "1") {
    id
    name
    email
  }
}
```

### Create User
```graphql
mutation {
  createUser(name: "Alice", email: "alice@example.com") {
    id
    name
    email
  }
}
```

### Update User
```graphql
mutation {
  updateUser(id: "1", name: "Alice Smith") {
    id
    name
    email
  }
}
```

### Delete User
```graphql
mutation {
  deleteUser(id: "1")
}
```

## License
MIT 