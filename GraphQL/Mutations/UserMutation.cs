using GraphQLDemo.Data;
using GraphQLDemo.GraphQL.Inputs;
using GraphQLDemo.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQLDemo.GraphQL.Mutations
{
    public class UserMutation
    {
        public async Task<User> CreateUser(
            UserInput input,
            [Service] IDbContextFactory<AppDbContext> dbContextFactory,
            [Service] IValidator<UserInput> validator)
        {
            var validationResult = await validator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                throw new GraphQLException(
                    validationResult.Errors.Select(e => new Error(e.ErrorMessage, "VALIDATION_ERROR")));
            }
            await using var context = dbContextFactory.CreateDbContext();
            var user = new User
            {
                Name = input.Name,
                Email = input.Email
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(
            int id, string? name, string? email,
            [Service] IDbContextFactory<AppDbContext> dbContextFactory,
            [Service] IValidator<UserInput> validator)
        {
            await using var context = dbContextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(id);
            if (user == null)
                throw new GraphQLException(new Error($"User with id {id} not found", "NOT_FOUND"));

            // Validate only if name or email is being updated
            var input = new UserInput
            {
                Name = name ?? user.Name,
                Email = email ?? user.Email
            };
            var validationResult = await validator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                throw new GraphQLException(
                    validationResult.Errors.Select(e => new Error(e.ErrorMessage, "VALIDATION_ERROR")));
            }
            if (!string.IsNullOrEmpty(name)) user.Name = name;
            if (!string.IsNullOrEmpty(email)) user.Email = email;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int id, [Service] IDbContextFactory<AppDbContext> dbContextFactory)
        {
            await using var context = dbContextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(id);
            if (user == null)
                throw new GraphQLException(new Error($"User with id {id} not found", "NOT_FOUND"));
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
