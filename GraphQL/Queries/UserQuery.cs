using GraphQLDemo.Data;
using GraphQLDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.GraphQL.Queries
{
    public class UserQuery
    {
        public async Task<List<User>> GetUsers([Service] IDbContextFactory<AppDbContext> dbContextFactory)
        {
            await using var context = dbContextFactory.CreateDbContext();
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(int id, [Service] IDbContextFactory<AppDbContext> dbContextFactory)
        {
            await using var context = dbContextFactory.CreateDbContext();
            return await context.Users.FindAsync(id);
        }
    }
}
