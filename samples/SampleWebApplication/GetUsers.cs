using System.Collections.Generic;
using CqrsExtensions;

namespace SampleWebApplication
{
    public class GetUsers : IQuery<IReadOnlyList<User>>
    {
    }
}