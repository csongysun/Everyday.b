using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Common;
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
        public async Task<ActionResult> Add([FromBody] TodoItem item)
        {
            if (item == null || !ModelState.IsValid)
                return BadRequest(new[] {ErrorDescriber.ModelNotValid});
            var result = await _todoManager.AddItemAsync(User.Identity.Name, item);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("{itemId}")]
        public async Task<ActionResult> Delete(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                return BadRequest(new[] { ErrorDescriber.ModelNotValid });
            }
            var userId = User.Identity.Name;
            var result = await _todoManager.DeleteItemAsync(itemId, userId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] TodoItemModel model)
        {
            if (string.IsNullOrEmpty(model?.Id) || !ModelState.IsValid )
                return BadRequest(new[] { ErrorDescriber.ModelNotValid });
            var userId = User.Identity.Name;
            var result = await _todoManager.UpdateItemAsync(model, userId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Today()
        {
            var result = await _todoManager.GetTodayItems(User.Identity.Name, DateTime.Today);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult> All()
        {
            var result = await _todoManager.GetAllItems(User.Identity.Name);
            return Ok(result);
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult> Check(string itemId)
        {
            var userId = User.Identity.Name;
            var result = await _todoManager.Check(itemId, userId);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return NoContent();
        }

    }
}
