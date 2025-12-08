using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Domain.Models;
using SorokChatServer.Domain.Repositories;
using SorokChatServer.Mapper;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres.Repositories;

public class UsersRepository : BaseRepository<User, UserEntity>, IUsersRepository
{
    public UsersRepository(PostgresContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<Result<User, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await Items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Value.Equals(email), cancellationToken);
        if (user is null) return new Error(NotFoundError, HttpStatusCode.BadRequest);
        return Mapper.Map<User>(user);
    }

    protected override User Merge(User old, User current)
    {
        return Mapper.Map<User>(current);
    }
}