using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CqrsExtensions;

namespace SampleWebApplication
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsers, IReadOnlyList<User>>
    {
        private readonly UsersRepository _usersRepository;

        public GetUsersQueryHandler(UsersRepository usersRepository) => _usersRepository = usersRepository;

        public Task<IReadOnlyList<User>> Handle(GetUsers query, CancellationToken cancellationToken)
        {
            return Task.FromResult(_usersRepository.GetUsers);
        }
    }
}