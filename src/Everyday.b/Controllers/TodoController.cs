using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Models;
using Everyday.b.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Everyday.b.Controllers
{
    [Route("api/[controller]/[Action]")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly TodoManager _todoManager;

        public TodoController(TodoManager todoManager)
        {
            _todoManager = todoManager;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok("ok");
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] TodoItem item)
        {
            var result = await _todoManager.AddItemAsync(User.Identity.Name, item);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok();
        }

    }
}
