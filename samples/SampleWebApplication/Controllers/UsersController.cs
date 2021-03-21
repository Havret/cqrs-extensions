using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CqrsExtensions;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICqrsDispatcher _cqrsDispatcher;

        public UsersController(ICqrsDispatcher cqrsDispatcher)
        {
            _cqrsDispatcher = cqrsDispatcher;
        }

        [HttpGet]
        public Task<IReadOnlyList<User>> Get()
        {
            return _cqrsDispatcher.Dispatch(new GetUsers(), ControllerContext.HttpContext.RequestAborted);
        }
    }
}