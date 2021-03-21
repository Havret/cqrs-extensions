using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApplication
{
    public class UsersRepository
    {
        private readonly ConcurrentDictionary<int, User> _users = new();

        public UsersRepository()
        {
            AddUser(new User { Id = 1, FirstName = "Robert", LastName = "Martin" });
            AddUser(new User { Id = 2, FirstName = "Martin", LastName = "Fowler" });
        }

        public IReadOnlyList<User> GetUsers => _users.Values.ToList();
        public void AddUser(User user) => _users.AddOrUpdate(user.Id, user, (_, _) => user);
    }
}